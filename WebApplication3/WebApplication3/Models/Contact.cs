using System.Collections.Generic;

namespace WebApplication3.Models
{
    public class Contact
    {
        public string id { get; set; }
        public string email { get; set; }
        public List<EmailAddress> email_addresses { get; set; }
        public string email_opted_in { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string company_name { get; set; }
        public string job_title { get; set; }
    }
}