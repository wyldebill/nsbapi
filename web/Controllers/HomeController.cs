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
            /*
            // the web client has no dependencies on identityserver4, just the nuget identitymodel
            // use the discovery document api to find the token endpoint.


            // USING OLD DEPRECATED SOON API....

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

            // END OLD API CODE
            */

            // NEW HOTNESS API FOR DISCOVERY AND TOKEN REQUESTS 


            // get the discovery document, which will tell us the token request endpoint to use
            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync("https://localhost:44301").Result;
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return View();

            }
            
            // ask the token request endpoint for a token, by providing our
            // clientid
            // shared secret
            // scope, or what resource we want to be able to use the token with.  
            // those three things are checked against what the token server has ....
            var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            }).Result;



            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return View();
            }

            // we should have a valid access token to the protected api,
            // put iT in the header of a request and make the api call
            var client2 = new HttpClient();
            client2.SetBearerToken(tokenResponse.AccessToken);

            var reply = client2.GetAsync("https://localhost:44302/api/identitytest").Result;


            if (reply.IsSuccessStatusCode)
            {
                var userClaims = reply.Content.ReadAsStringAsync();
                // it worked!
                int z = 100;
            }
           

            Console.WriteLine(reply.Content.ReadAsStringAsync());
            





            return View(reply.Content.ReadAsStringAsync());
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
