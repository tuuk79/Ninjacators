using System;
using System.Linq;
using System.Web;
using CookComputing.XmlRpc;
using System.Data;
using System.Collections;
using System.Web.Configuration;
using InfusionSoft;

namespace WebApplication3
{
    public partial class getSKUs : System.Web.UI.Page
    {
        private string emailAddress = "";
        private string verbose = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            /* Test emails:
             * cp130:
             * cheng67@charter.net
             */
            GetParameters();
            if (emailAddress != null && emailAddress.Length > 5)
            {
                GetSkuForEmail();
            }
            else
            {
                lblError.Text = "Invalid email address";
            }
        }

        protected void GetParameters()
        {
            emailAddress = Request.QueryString["email"];
            verbose = Request.QueryString["verbose"];
        }

        protected void GetSkuForEmail()
        {
            string application = WebConfigurationManager.AppSettings["application"];
            string apiKey = WebConfigurationManager.AppSettings["apiKey"];

            var customer = new Customer(application, apiKey);
            var client = customer.Connect();

            /*
             * Get Contact ID for email 
             */
            emailText2.Text = emailAddress;
            var contacts = client.ContactService.FindByEmail(emailAddress, new[] { "Id", "Email" });

            int contactCount = contacts.Count();

            int contactId = 0;

            foreach (InfusionSoft.Tables.Contact contact in contacts)
            {
                contactId = contact.Id;
            }

            lblContactID.Text = contactId.ToString();

            if(contactId != 0)
            {
                /*
                 * Get invoices for contact ID
                 */
                var queryData = new XmlRpcStruct() { { "ContactId", contactId.ToString() } };
                string[] fieldList = new string[] { "Id", "ContactId", "ProductSold" };

                var invoices = client.DataService.Query("Invoice", 1000, 0, queryData, fieldList);

                DataTable dt = ConvertToDataTable(fieldList, invoices);

                /*
                 * Get products from invoices
                 */
                DataTable dtProducts = new DataTable();
                dtProducts.Columns.Add("ProductId", typeof(string));
                dtProducts.Columns.Add("ProductName", typeof(string));
                dtProducts.Columns.Add("Sku", typeof(string));

                DataTable dtProductsCSV = new DataTable();
                dtProductsCSV.Columns.Add("Sku", typeof(string));

                fieldList = new string[] { "Id", "ProductName", "Sku" };

                foreach (DataRow invoiceRow in dt.Rows)
                {
                    string productList = invoiceRow.ItemArray[2].ToString();
                    string[] products = productList.Split(',');

                    foreach (string singleProduct in products)
                    {
                        DataRow dr = dtProducts.NewRow();
                        dr[0] = singleProduct;

                        queryData = new XmlRpcStruct() { { "Id", singleProduct } };
                        var productDetails = client.DataService.Query("Product", 1000, 0, queryData, fieldList);
                        DataTable dtProductDetails = ConvertToDataTable(fieldList, productDetails);

                        dr[1] = dtProductDetails.Rows[0].ItemArray[1].ToString();
                        dr[2] = dtProductDetails.Rows[0].ItemArray[2].ToString();

                        dtProducts.Rows.Add(dr);

                        DataRow dr2 = dtProductsCSV.NewRow();
                        dr2[0] = dtProductDetails.Rows[0].ItemArray[2].ToString();
                        if (dr2[0].ToString() != "")
                        {
                            dtProductsCSV.Rows.Add(dr2);
                        }
                    }

                }

                gvProducts.DataSource = dtProducts;
                gvProducts.DataBind();

                /*
                 * Save to CSV and display link
                 */
                string csvPath = HttpContext.Current.Server.MapPath("~/App_Data/");
                string fileName = emailAddress + ".csv";

                try
                {
                    dtProductsCSV.ToCSV(csvPath + fileName);
                }
                catch (Exception ex)
                {
                    lblError.Text = ex.Message + "<br/><br/>" + ex.StackTrace;
                }

                /*
                 * Add hyperlink to CSV file
                 * TODO: Where will they be ultimately stored?
                 */
                string link = "http://ninjacatorslicensing.com/App_Data/" + fileName;
                lnkCSV.NavigateUrl = link;
                lnkCSV.Text = link;
                lnkCSV.Visible = false; //TODO: Temporarily hiding until path is determined

                if (verbose == null || verbose != "1")
                {
                    gvProducts.Visible = false;
                }

            }
            else
            {
                lblError.Text = "Contact email not found in Infusionsoft";
            }
        }

        protected DataTable ConvertToDataTable(string[] fieldList, IEnumerable itemList)
        {
            DataTable dt = new DataTable();
            foreach (string f in fieldList)
            {
                dt.Columns.Add(f, typeof(string));
            }

            foreach (CookComputing.XmlRpc.XmlRpcStruct item in itemList)
            {
                DataRow dr = dt.NewRow();
                foreach (DictionaryEntry d in item)
                {
                    try
                    {
                        dr[d.Key.ToString()] = d.Value;
                    }
                    catch (Exception ex)
                    {
                        // TODO: handle errors
                    }

                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}