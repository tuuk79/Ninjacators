using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WebApplication3.Models;

namespace WebApplication3.Managers
{
    public class AccessTokenManager
    {
        public string getAccessToken()
        {
            using (var db = new NinjacatorsEntities())
            {
                var clientId = ConfigurationManager.AppSettings["ClientId"];
                var latestAccessTokenRecord = db.AccessTokens
                                .Where(x => x.client_id == clientId)
                                .OrderByDescending(x => x.id)
                                .FirstOrDefault();
                return latestAccessTokenRecord.access_token;
            }
        }

        public void saveAccessToken(string accessToken)
        {
            using (var db = new NinjacatorsEntities())
            {
                db.AccessTokens.Add(new AccessToken()
                {
                    client_id = ConfigurationManager.AppSettings["ClientId"],
                    access_token = accessToken
                });
                db.SaveChanges();
            }
        }
    }
}