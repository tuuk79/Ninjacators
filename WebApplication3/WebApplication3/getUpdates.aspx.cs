using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CookComputing.XmlRpc;
using InfusionSoft;
using InfusionSoft.Tables;
using System.Data;
using System.Collections;
using System.Web.Configuration;

namespace WebApplication3
{
    //TODO: Error trapping

    public partial class GetInvoice : System.Web.UI.Page
    {
        private string since = "";
        private string verbose = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            GetParameters();

            if (since != null)
            {
                GetInvoiceList();
            }
        }

        protected void GetParameters()
        {
            since = Request.QueryString["since"];
            verbose = Request.QueryString["verbose"];
        }

        protected void GetInvoiceList()
        {
            string application = WebConfigurationManager.AppSettings["application"];
            string apiKey = WebConfigurationManager.AppSettings["apiKey"];

            var customer = new Customer(application, apiKey);
            var client = customer.Connect();

            //TODO: Format date if needed

            string queryDate = "~>=~ " + since + " 00:00:00";
            var queryData = new XmlRpcStruct() { { "DateCreated", queryDate } };
            string[] fieldList = new string[] { "Id", "ContactId", "ProductSold" };

            var invoices = client.DataService.Query("Invoice", 1000, 0, queryData, fieldList);

            DataTable dt = ConvertToDataTable(fieldList, invoices);

            GridView1.DataSource = dt;
            GridView1.DataBind();

            lblProducts.Text = "Products purchased since " + since;

            /*
             * Get products from invoices
             */
            DataTable dtProducts = new DataTable();
            dtProducts.Columns.Add("ProductId", typeof(string));
            dtProducts.Columns.Add("ProductName", typeof(string));
            dtProducts.Columns.Add("Sku", typeof(string));

            DataTable dtProductsCSV = new DataTable();
            dtProductsCSV.Columns.Add("Email", typeof(string));
            dtProductsCSV.Columns.Add("ProductId", typeof(string));
            dtProductsCSV.Columns.Add("Sku", typeof(string));

            fieldList = new string[] { "Id", "ProductName", "Sku" };

            string[] contactFields = new string[] { "Id", "Email" };
            foreach (DataRow invoiceRow in dt.Rows)
            {
                int contactId = int.Parse(invoiceRow.ItemArray[1].ToString());
                var contact = client.ContactService.Load(contactId, contactFields);
                string contactEmail = contact.Email;
                string productList = invoiceRow.ItemArray[2].ToString();
                string[] products = productList.Split(',');

                foreach (string singleProduct in products)
                {
                    queryData = new XmlRpcStruct() { { "Id", singleProduct } };
                    var productDetails = client.DataService.Query("Product", 1000, 0, queryData, fieldList);
                    DataTable dtProductDetails = ConvertToDataTable(fieldList, productDetails);

                    DataRow dr2 = dtProductsCSV.NewRow();
                    dr2[0] = contactEmail;
                    dr2[1] = singleProduct;
                    dr2[2] = dtProductDetails.Rows[0].ItemArray[2].ToString();
                    dtProductsCSV.Rows.Add(dr2);
                }

            }

            gvProducts.DataSource = dtProductsCSV;
            gvProducts.DataBind();

            lblOutput.Text = "CSV output";

            /*
             * Save to CSV and display link
             */
            Guid g = Guid.NewGuid();
            string csvPath = HttpContext.Current.Server.MapPath("~/App_Data/");
            string fileName = "ByDate" + g.ToString().Substring(0, 8) + ".csv";

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
                GridView1.Visible = false;
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