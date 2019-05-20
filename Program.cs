using System;
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
            // Console.WriteLine("Hello World!");

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            String ApiKey = configuration.GetSection("ApiKey").Value;
            String ApiToken = configuration.GetSection("ApiToken").Value;
            
            String Path = "https://api.trello.com/1/members/me/";
            Path = Path + "?key="+ApiKey + "&token="+ApiToken;

            RunAsync(Path).GetAwaiter().GetResult();
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
                PrintJson(trello);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }

        private static void PrintJson(Trello trello)
        {            
            Console.WriteLine($"id: {trello.id}" +
                              $"\tname:{trello.name}" +
                              $"\tdesc:{trello.desc}" +
                              $"\tdescData:{trello.descData}" +
                              $"\tclosed:{trello.closed}" +
                              $"\tidOrganization:{trello.idOrganization}" +
                              $"\turl: {trello.url}");
        }
    }
}
