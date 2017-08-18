using ShopingTask.Model;
using ShopingTask.RestClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ShopingTask.Service
{
   public class ShoppingItemService
    {

        public static string BaseUrl = "https://api.zalando.com/articles";

        private int paging = 1;

        public int Paging
        {
            get { return paging ; }
            set { paging = value; }
        }

        public async Task<ObservableCollection<Content>> GetShoppItems()
        {            
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync(BaseUrl + "?page=" + Paging);
            var responseData = await responseMessage.Content.ReadAsStringAsync();
            var jsonSerializer = new DataContractJsonSerializer(typeof(RootObject));
            using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(responseData)))
            {
                var list = ((RootObject)jsonSerializer.ReadObject(stream)).content;
                ObservableCollection<Content> temp = new ObservableCollection<Content>();
                foreach (var oneItem in list)
                    temp.Add(oneItem);
                Paging++;
                return temp;
            }
        }

        public async Task<ObservableCollection<Content>> Filter(string brand, string gender)
        {
            var httpClient = new HttpClient();
            HttpResponseMessage responseMessage = null;
            if (!string.IsNullOrEmpty(brand) && !string.IsNullOrEmpty(gender))
            {
                 responseMessage = await httpClient.
                    GetAsync(string.Format(BaseUrl + "?fullText=" + brand + "&gender=" + gender));
            }
            else if(!string.IsNullOrEmpty(brand))
            {
                responseMessage = await httpClient.
                   GetAsync(string.Format(BaseUrl + "?fullText=" + brand ));
            }
            else if (!string.IsNullOrEmpty(gender))
            {
                responseMessage = await httpClient.
                   GetAsync(string.Format(BaseUrl + "?gender=" + gender));
            }else
                responseMessage = await httpClient.
                 GetAsync(string.Format(BaseUrl));
            var responseData = await responseMessage.Content.ReadAsStringAsync();
            var jsonSerializer = new DataContractJsonSerializer(typeof(RootObject));
            using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(responseData)))
            {
              return
                    new ObservableCollection<Content>(((RootObject)jsonSerializer.ReadObject(stream)).content);
            }
        }


    }
}
