using System;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Collections;

namespace BlazorReportingTools.Data
{
    public class GitHubApi
    {

        public async static Task<List<Contributor>> GetContributors()
        {
            //Istazio la classe HttpClient
            var client = new HttpClient();

            //API: https://api.github.com/repos/symfony/symfony/contributors
            
            //Passo l'indirizzo root della webAPI
            client.BaseAddress = new Uri("https://api.github.com");
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //Aggiungo l'URL della WEb API
            var url = "repos/symfony/symfony/contributors";
            //Eseguo la richiesta GET 
            HttpResponseMessage response = await client.GetAsync(url);
            
            string result="";
              if( response.IsSuccessStatusCode)
              {
                   //Leggo la response
                  result = await response.Content.ReadAsStringAsync();

              }
            //Deserializzo secondo l'oggetto Contributor
            List<Contributor> contributors = JsonConvert.DeserializeObject<List<Contributor>>(result);   
            return contributors;

        }

     }

    public class Contributor
    {
        public string login { get; set; }

       public int id { get; set; }

       public string node_id { get; set; }

        public string avatar_url { get; set; }

       public string gravatar_id { get; set; }
       public string url { get; set; }
       public string html_url { get; set; }
       public string followers_url { get; set; }
       public string following_url { get; set; }
       public string gists_url { get; set; }
       public string starred_url { get; set; }
        public string subscriptions_url { get; set; }
       public string organizations_url { get; set; }
       public string repos_url { get; set; }
        public string events_url { get; set; }
        public string received_events_url { get; set; }
        public string type { get; set; }
        public bool site_admin { get; set; }
        public int contributions { get; set; }
    }
}