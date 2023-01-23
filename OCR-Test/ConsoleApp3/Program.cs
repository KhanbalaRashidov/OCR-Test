// See https://aka.ms/new-console-template for more information
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;

while (true)
{
    HttpClient client = new HttpClient();
    client.BaseAddress = new Uri("https://www.haremaltin.com/dashboard/ajax/doviz");

    client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

    var content = JsonConvert.SerializeObject(new { dil_kodu = "tr" });



    var response = await client.PostAsync("", new StringContent(content));

    Console.WriteLine(await response.Content.ReadAsStringAsync());
    Thread.Sleep(TimeSpan.FromSeconds(20));
}





