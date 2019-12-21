using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using WebApplication3.Managers;
using WebApplication3.Models;

namespace WebApplication3
{
    public partial class Hooks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GetAccessToken_Click(object sender, EventArgs e)
        {
            var urlManager = new UrlManager();
            var authorizeUrl = urlManager.GetAuthorizationUrl();

            var referringActionManager = new ReferringActionManager();
            referringActionManager.saveReferringAction("Hooks.aspx");

            Response.Redirect($"{authorizeUrl}");
        }

        protected async void SubscribeAddInvoicePayment_Click(object sender, EventArgs e)
        {
            var accessTokenManager = new AccessTokenManager();
            var accessToken = accessTokenManager.getAccessToken();

            var baseApiUrl = ConfigurationManager.AppSettings["BaseApiUrl"];
            var hookUrl = ConfigurationManager.AppSettings["HookUrl"];

            var createHookUrl = $"{baseApiUrl}/hooks?access_token={accessToken}";

            var payload = new Dictionary<string, string>()
                {
                    { "eventKey", lnkSubscribeAddInvoicePayment.CommandName },
                    { "hookUrl", hookUrl }
                };

            var httpContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");

            var response = await client.PostAsync(createHookUrl, httpContent);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var createHookResponse = JsonConvert.DeserializeObject<CreateHookResponse>(json);
            }
        }

        protected async void SubscribeOrderEdit_Click(object sender, EventArgs e)
        {
            var accessTokenManager = new AccessTokenManager();
            var accessToken = accessTokenManager.getAccessToken();

            var baseApiUrl = ConfigurationManager.AppSettings["BaseApiUrl"];
            var hookUrl = ConfigurationManager.AppSettings["HookUrl"];

            var createHookUrl = $"{baseApiUrl}/hooks?access_token={accessToken}";

            var payload = new Dictionary<string, string>()
                {
                    { "eventKey", lnkSubscribeOrderEdit.CommandName },
                    { "hookUrl", hookUrl }
                };

            var httpContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");

            var response = await client.PostAsync(createHookUrl, httpContent);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var createHookResponse = JsonConvert.DeserializeObject<CreateHookResponse>(json);
            }
        }

        protected async void SubscribeDeleteInvoicePayment_Click(object sender, EventArgs e)
        {
            var accessTokenManager = new AccessTokenManager();
            var accessToken = accessTokenManager.getAccessToken();

            var baseApiUrl = ConfigurationManager.AppSettings["BaseApiUrl"];
            var hookUrl = ConfigurationManager.AppSettings["HookUrl"];

            var createHookUrl = $"{baseApiUrl}/hooks?access_token={accessToken}";

            var payload = new Dictionary<string, string>()
                {
                    { "eventKey", lnkSubscribeDeleteInvoicePayment.CommandName },
                    { "hookUrl", hookUrl }
                };

            var httpContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");

            var response = await client.PostAsync(createHookUrl, httpContent);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var createHookResponse = JsonConvert.DeserializeObject<CreateHookResponse>(json);
            }
        }

        protected async void SubscribeEditInvoicePayment_Click(object sender, EventArgs e)
        {
            var accessTokenManager = new AccessTokenManager();
            var accessToken = accessTokenManager.getAccessToken();

            var baseApiUrl = ConfigurationManager.AppSettings["BaseApiUrl"];
            var hookUrl = ConfigurationManager.AppSettings["HookUrl"];

            var createHookUrl = $"{baseApiUrl}/hooks?access_token={accessToken}";

            var payload = new Dictionary<string, string>()
                {
                    { "eventKey", lnkSubscribeEditInvoicePayment.CommandName },
                    { "hookUrl", hookUrl }
                };

            var httpContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");

            var response = await client.PostAsync(createHookUrl, httpContent);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var createHookResponse = JsonConvert.DeserializeObject<CreateHookResponse>(json);
            }
        }

        protected async void SubscribeDeleteInvoice_Click(object sender, EventArgs e)
        {
            var accessTokenManager = new AccessTokenManager();
            var accessToken = accessTokenManager.getAccessToken();

            var baseApiUrl = ConfigurationManager.AppSettings["BaseApiUrl"];
            var hookUrl = ConfigurationManager.AppSettings["HookUrl"];

            var createHookUrl = $"{baseApiUrl}/hooks?access_token={accessToken}";

            var payload = new Dictionary<string, string>()
                {
                    { "eventKey", lnkSubscribeDeleteInvoice.CommandName },
                    { "hookUrl", hookUrl }
                };

            var httpContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");

            var response = await client.PostAsync(createHookUrl, httpContent);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var createHookResponse = JsonConvert.DeserializeObject<CreateHookResponse>(json);
            }
        }

        protected async void SubscribeEditInvoice_Click(object sender, EventArgs e)
        {
            var accessTokenManager = new AccessTokenManager();
            var accessToken = accessTokenManager.getAccessToken();

            var baseApiUrl = ConfigurationManager.AppSettings["BaseApiUrl"];
            var hookUrl = ConfigurationManager.AppSettings["HookUrl"];

            var createHookUrl = $"{baseApiUrl}/hooks?access_token={accessToken}";

            var payload = new Dictionary<string, string>()
                {
                    { "eventKey", lnkSubscribeEditInvoice.CommandName },
                    { "hookUrl", hookUrl }
                };

            var httpContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");

            var response = await client.PostAsync(createHookUrl, httpContent);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var createHookResponse = JsonConvert.DeserializeObject<CreateHookResponse>(json);
            }
        }
    }

}