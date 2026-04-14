using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HalKonzol
{
    public class ServerConnection
    {
        HttpClient client = new();
        
        public ServerConnection(string url)
        {
            //http://localhost:3000
            client.BaseAddress = new Uri(url);
        }

        public async Task<List<Fish>> GetFish()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("/fish");
                response.EnsureSuccessStatusCode();
                //a response-t kiolvassuk stringként
                string responseString = await response.Content.ReadAsStringAsync();
                //a válaszból feltöltjük a listát
                List<Fish> result = JsonSerializer.Deserialize<List<Fish>>(responseString);
                return result;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public async Task<List<Fish>> GetMyFish()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("/myfish");
                response.EnsureSuccessStatusCode();
                //a response-t kiolvassuk stringként
                string responseString = await response.Content.ReadAsStringAsync();

                List<Fish> result = JsonSerializer.Deserialize<List<Fish>>(responseString);
                return result;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public async Task PostFish(string name, double weight)
        {
            try
            {
                Fish newFish = new Fish(name, weight);
                string jsonString = JsonSerializer.Serialize(newFish);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("/fish", content);
                response.EnsureSuccessStatusCode();
                Console.WriteLine("Sikeres hozzáadás!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<bool> Login(string username, string password)
        {
            try
            {
                //létrehozunk egy példányt/objektumot
                User user = new User(username, password);
                //JSON-né alakítjuk
                string jsonString = JsonSerializer.Serialize(user);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                //elküldjük a POST-ot
                HttpResponseMessage response = await client.PostAsync("/login", content);
                //biztos sikerül-e
                response.EnsureSuccessStatusCode();
                //kiolvassuk a választ
                string responseString = await response.Content.ReadAsStringAsync();
                //elmentjük a választ (jelen esetben a tokent)
                Token result = JsonSerializer.Deserialize<Token>(responseString);
                //beállítjuk headernek: Authorization: token
                client.DefaultRequestHeaders.Add("authorization", result.token);
                Console.WriteLine("Sikeres bejelentkezés!");
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Sikertelen bejelentkezés!");
            }
            return false;
        }
    }
}
