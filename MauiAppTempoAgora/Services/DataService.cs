using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    internal class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            String chave = "971346c4aa842f18394b2c12a5e965ce";
            String url = $"https://api.openweathermap.org/data/2.5/weather?" +
                $"q={cidade}&units=metric&appid={chave}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(url);

                if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();
                    var rascunho = JObject.Parse(json);
                    
                    DateTime time = new();
                    DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    t = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        temp_min = (Double)rascunho["main"]["temp_min"],
                        temp_max = (int?)rascunho["main"]["temp_max"],
                        speed = (Double)rascunho["wind"]["speed"],
                        visibility = (int)rascunho["visibility"],
                        sunrise = sunrise.ToString(),
                        sunset = sunset.ToString(),
                    };//fecha objeto do tempo
                }//fecha laço do If
                if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    //cidade não envontrada
                    return null;
                }
                
            } // fecha o laço using
            return t;
        }
    }
}
