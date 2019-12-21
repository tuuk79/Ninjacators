using System.Configuration;

namespace WebApplication3.Managers
{
    public class UrlManager
    {
        public UrlManager()
        {

        }

        public string GetAuthorizationUrl()
        {
            var clientId = ConfigurationManager.AppSettings["ClientId"];
            var redirectUri = ConfigurationManager.AppSettings["RedirectUri"];
            var baseAuthorizeUrl = ConfigurationManager.AppSettings["BaseAuthorizeUrl"];
            var responseType = ConfigurationManager.AppSettings["ResponseType"];
            var state = ConfigurationManager.AppSettings["State"];

            var authorizeUrl = $"{baseAuthorizeUrl}?client_id={clientId}&redirect_uri={redirectUri}&response_type={responseType}";

            return authorizeUrl;
        }
    }
}