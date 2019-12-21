using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication3.Managers;
using WebApplication3.Models;

namespace WebApplication3
{
    public partial class Authenticate : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            var queryStringParameters = HttpUtility.ParseQueryString(HttpUtility.UrlDecode(Page.ClientQueryString));

            var baseTokenUrl = ConfigurationManager.AppSettings["BaseTokenUrl"];
            var clientId = ConfigurationManager.AppSettings["ClientId"];

            var authenticateUrl = $"{baseTokenUrl}";

            var payload = new Dictionary<string, string>()
            {
                { "client_id", clientId },
                { "client_secret", ConfigurationManager.AppSettings["ClientSecret"] },
                { "code", queryStringParameters.Get("code") },  
                { "grant_type", ConfigurationManager.AppSettings["GrantType"] },
                { "redirect_uri", ConfigurationManager.AppSettings["RedirectUri"] }
            };

            var content = new FormUrlEncodedContent(payload);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");

            var response = await client.PostAsync(authenticateUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var authenticationResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(json);

                // save to cookie
                //HttpCookie cookie = new HttpCookie("access_token");
                //cookie.Value = authenticationResponse.access_token;
                //Response.Cookies.Add(cookie);

                // save to db
                var accessTokenManager = new AccessTokenManager();
                accessTokenManager.saveAccessToken(authenticationResponse.access_token);

                var referringActionManager = new ReferringActionManager();
                var referringAction = referringActionManager.getReferringAction();

                Response.Redirect("Hooks.aspx");
            }
        }
    }
}