using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TrelloImport.Models;

namespace TrelloImport
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            String apiKey = configuration.GetSection("ApiKey").Value;
            String apiToken = configuration.GetSection("ApiToken").Value;
            String path = configuration.GetSection("Path").Value;
            String boardId = configuration.GetSection("BoardId").Value;

            path = path.Replace("{BoardId}",boardId);

            if (!path.Contains("key"))
            {
                path += !path.Contains("?") ? "?key=" : "&key=";
                path += apiKey;
            }

            if (!path.Contains("token"))
            {
                path += !path.Contains("?") ? "?token=" : "&token=";
                path += apiToken;
            }

            RunAsync(path).GetAwaiter().GetResult();
        }

        static async Task<Trello> GetTrelloAsync(string path)
        {
            Trello trello = null;
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                trello = await response.Content.ReadAsAsync<Trello>();
            }

            return trello;
        }

        static async Task RunAsync(string path)
        {
            client.BaseAddress = new Uri(path);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                Trello trello = await GetTrelloAsync(client.BaseAddress.AbsoluteUri);
                PrintJson(path, trello);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }

        private static void PrintJson(String path, Trello trello)
        {
            Console.WriteLine("*****Endpoint*****");
            Console.WriteLine(path);

            // Console.Write("\n*****Return*****");
            // Console.Write($"\nid: {trello.id}" +
            //               $"\nname:{trello.name}" +
            //               $"\nclosed:{trello.closed}" +
            //               $"\nurl:{trello.url}" +
            //               $"\nshortUrl:{trello.shortUrl}" +
            //               $"\nurl: {trello.url}");

            Console.Write("\n*****Return*****\n");
            foreach(PropertyDescriptor descriptor in TypeDescriptor.GetProperties(trello))
            {
                string name=descriptor.Name;
                object value=descriptor.GetValue(trello);
                Console.WriteLine("{0}: {1}",name,value);
            }
        }
    }
}
