using CookComputing.XmlRpc;
using InfusionSoft;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication3.Managers;
using WebApplication3.Models;

namespace WebApplication3
{
    public class InfusionsoftController : ApiController
    {
        public async Task<IHttpActionResult> VerifyHook([FromBody] EventSubscriptionPayload eventSubscriptionPayload)
        {
            // hook creation verification
            if (Request.Headers.Contains("X-Hook-Secret"))
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var incomingHookSecret = Request.Headers.GetValues("X-Hook-Secret").FirstOrDefault();
                response.Headers.Add("X-Hook-Secret", incomingHookSecret);

                return ResponseMessage(response);
            }

            var accessTokenManager = new AccessTokenManager();
            var accessToken = accessTokenManager.getAccessToken();

            if (accessToken == null)
            {
                var referringActionManager = new ReferringActionManager();
                referringActionManager.saveReferringAction("VerifyHook");

                return await Task.Run(() => Redirect("/Authorize.aspx"));
            }

            if (eventSubscriptionPayload.event_key == "order.edit")
            {
                var orderId = eventSubscriptionPayload.object_keys.ToList().FirstOrDefault().id;
                var baseApiUrl = ConfigurationManager.AppSettings["BaseApiUrl"];
                var url = $"{baseApiUrl}/orders/{orderId}?access_token={accessToken}";

                var client = new HttpClient();
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadAsStringAsync();
                    var order = JsonConvert.DeserializeObject<Order>(results);
                    var skus = new List<string>();

                    foreach (var orderItem in order.order_items)
                    {
                        skus.Add(orderItem.product.sku);
                    }

                    var logFileManager = new LogFileManager();
                    logFileManager.writeToLogFile(eventSubscriptionPayload.event_key, order.contact.email, skus);
                }
            }
            else if (eventSubscriptionPayload.event_key == "invoice.edit")
            {
                string application = ConfigurationManager.AppSettings["application"];
                string apiKey = ConfigurationManager.AppSettings["apiKey"];

                var customer = new Customer(application, apiKey);
                var xmlRpcClient = customer.Connect();
                var invoiceId = eventSubscriptionPayload.object_keys.FirstOrDefault().id;

                var queryData = new XmlRpcStruct() { { "Id", invoiceId } };
                var fieldList = new string[] { "ProductSold", "RefundStatus", "ContactId" };

                var result = xmlRpcClient.DataService.Query("Invoice", 1000, 0, queryData, fieldList).FirstOrDefault();
                var isRefund = false;
                string[] productIds = Array.Empty<string>();
                var contactId = 0;

                foreach (DictionaryEntry d in (XmlRpcStruct)result)
                {
                    if (d.Key.ToString() == "RefundStatus")
                    {
                        if (Convert.ToInt32(d.Value) > 0)
                        {
                            isRefund = true;
                        }
                    }

                    if (d.Key.ToString() == "ProductSold")
                    {
                        productIds = d.Value.ToString().Split(',');
                    }

                    if (d.Key.ToString() == "ContactId")
                    {
                        contactId = Convert.ToInt32(d.Value);
                    }
                }

                if (isRefund)
                {
                    var eventKey = $"{eventSubscriptionPayload.event_key} (refund)";
                    var baseApiUrl = ConfigurationManager.AppSettings["BaseApiUrl"];
                    var httpClient = new HttpClient();
                    var skus = new List<string>();

                    foreach (string productId in productIds)
                    {
                        // lookup product sku from product id string
                        var productUrl = $"{baseApiUrl}/products/{productId}?access_token={accessToken}";
                        var productResponse = await httpClient.GetAsync(productUrl);

                        if (productResponse.IsSuccessStatusCode)
                        {
                            var results = await productResponse.Content.ReadAsStringAsync();
                            var product = JsonConvert.DeserializeObject<Product>(results);
                            skus.Add(product.sku);
                        }
                    }

                    // contact email lookup
                    var contactUrl = $"{baseApiUrl}/contacts/{contactId}?access_token={accessToken}";
                    var contactResponse = await httpClient.GetAsync(contactUrl);
                    var email = string.Empty;

                    if (contactResponse.IsSuccessStatusCode)
                    {
                        var results = await contactResponse.Content.ReadAsStringAsync();
                        var contact = JsonConvert.DeserializeObject<Contact>(results);
                        email = contact.email_addresses.FirstOrDefault().email;
                    }

                    var logFileManager = new LogFileManager();
                    logFileManager.writeToLogFile(eventKey, email, skus);
                }
            }
            else if (eventSubscriptionPayload.event_key == "invoice.payment.add")
            {
                var transactionId = eventSubscriptionPayload.object_keys.ToList().FirstOrDefault().id;
                var baseApiUrl = ConfigurationManager.AppSettings["BaseApiUrl"];
                var url = $"{baseApiUrl}/transactions/{transactionId}?access_token={accessToken}";

                var client = new HttpClient();
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadAsStringAsync();
                    var transaction = JsonConvert.DeserializeObject<Transaction>(results);

                    var order = transaction.orders.FirstOrDefault();
                    var email = order.contact.email;
                    var skus = new List<string>();

                    foreach (var orderItem in order.order_items)
                    {
                        skus.Add(orderItem.product.sku);
                    }

                    var logFileManager = new LogFileManager();
                    logFileManager.writeToLogFile(eventSubscriptionPayload.event_key, email, skus);
                }
            }

            return await Task.Run(() => Redirect(new Uri("Hooks.aspx", UriKind.Relative)));

        }
    }
}