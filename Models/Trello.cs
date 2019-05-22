using System;
using System.Collections.Generic;
//using Newtonsoft.Json;

namespace TrelloImport.Models
{
    public class Trello{
      //[JsonProperty(PropertyName = "id")]
      public String id { get; set; }      
      public String name { get; set; }     
      public Boolean closed { get; set; }
      public String url { get; set; }
      public String shortUrl { get; set; }
      public Label labelNames { get; set; }
      public List<Action> actions {get; set; }
    }
}