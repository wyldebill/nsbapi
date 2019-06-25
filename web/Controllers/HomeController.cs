using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web.Models;

namespace web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {


            // use the discovery document api to find the token endpoint.
            var discoveryClient =  DiscoveryClient.GetAsync("https://localhost:44301").Result;
            if (discoveryClient.IsError)
            {
                Console.WriteLine(discoveryClient.Error);
                return View(); ;
            }

            // this request's a token, a jwt token that is signed with the 'secret' string
            var tokenClient = new TokenClient(discoveryClient.TokenEndpoint, "client", "secret");
            var response =  tokenClient.RequestClientCredentialsAsync("api1").Result;

            var client = new HttpClient();
            client.SetBearerToken(response.AccessToken);

            var reply = client.GetAsync("https://localhost:44302/api/identitytest").Result;
            if (reply.IsSuccessStatusCode)
            {
                var userClaims = reply.Content.ReadAsStringAsync();
                // it worked!
                int z = 100;
            }
            if (response.IsError)
            {
                Console.WriteLine(response.Error);
                return View(); ;
            }

            Console.WriteLine(response.Json);

        

    


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
