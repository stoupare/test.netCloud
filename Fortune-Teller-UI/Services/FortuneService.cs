using Newtonsoft.Json.Linq;
using Pivotal.Discovery.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using Steeltoe.Discovery.Eureka;
using Steeltoe.Discovery.Eureka.AppInfo;

namespace Fortune_Teller_UI.Services
{
    public class FortuneService : IFortuneService
    {
        DiscoveryClient _manager;

        private const string RANDOM_FORTUNE_URL = "http://fortuneService/api/fortunes/random";
        private const string TEST_EP = "test";
        private const string BUZZGRAPH_EP = "buzzgraph";

        public FortuneService()
        {
            EurekaClientConfig clientConfig = new EurekaClientConfig()
            {
                EurekaServerServiceUrls = "http://localhost:8761/eureka/"
            };

            EurekaInstanceConfig instanceConfig = new EurekaInstanceConfig()
            {
                AppName = "my.Net Service",
                InstanceId = "Some_ID",
                IsInstanceEnabledOnInit = true
            };

            // Register services and fetch the registry
            DiscoveryManager.Instance.Initialize(clientConfig, instanceConfig);
            _manager = DiscoveryManager.Instance.Client;

            //Test _manager
            getBuzzgraphResult();
        }
    
        public string shuffle(Mention req)
        { 

                char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

                string[] words = req.Text.Split(delimiterChars);


                reshuffle(words);
                return string.Join(" ", words);
            }


        void reshuffle(string[] items)
        {
            Random rand = new Random();

            for (int i = 0; i < items.Length - 1; i++)
            {
                int j = rand.Next(i, items.Length);
                string temp = items[i];
                items[i] = items[j];
                items[j] = temp;
            }
        }

        /* Below Random Buzzgraph Functions*/

        private void getBuzzgraphResult()
        {
            Application appList = _manager.GetApplication("BUZZGRAPH");
            if (appList !=  null && appList.Instances.Count > 0)
            {
                InstanceInfo service = appList.Instances[0];
                Debug.WriteLine(service.HomePageUrl);
                BuzzGraph[] buzzGraphs = SendReqToBuzzService(new Uri(service.HomePageUrl + BUZZGRAPH_EP));
                foreach (BuzzGraph buzz in buzzGraphs)
                {
                    string allNames = "";
                    foreach (string name in buzz.Names)
                    {
                        allNames += name + ", ";
                    }
                    Debug.WriteLine("Buzz NAMES :" + allNames + ", And VALUE is: " + buzz.Value + ", Excluded: " + buzz.Excluded);
                }
            }
        }

        private BuzzGraph[] SendReqToBuzzService (Uri uri)
        {
            var requestClient = new HttpClient();
            Task<string> buzzGraphTask = requestClient.GetStringAsync(uri);
            string buzzString = buzzGraphTask.Result;
            var resultArray = JsonConvert.DeserializeObject<Root>(buzzString);
            return resultArray.buzzGraphResultList;
        }

        public async Task<string> GetBuzzgraphAsync()
        {
           IList<InstanceInfo> instances = _manager.GetApplication("BUZZGRAPH").Instances;
            if (instances != null && instances.Count > 0)
            {
                InstanceInfo service = instances[0];
                Debug.WriteLine(service.HomePageUrl);
                var requestClient = new HttpClient();
                return await requestClient.GetStringAsync(new Uri(service.HomePageUrl + BUZZGRAPH_EP));
            }
            
            return null; 

        } 
    } 
}
