using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Check if a URL argument is provided
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide the URL as an argument.");
            return;
        }

        string url = args[0];

        // Set the protocol to TLS 1.2 explicitly
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("TLS 1.2 connection succeeded.");
                }
                else
                {
                    Console.WriteLine("Connected, but received non-success response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection failed: " + ex.Message);
            }
        }
    }
}
