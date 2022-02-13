using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace SITConnect
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["Authtoken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    // If session authenticated -----------------------------------------------------------------------------------
                    string email = Session["LoggedIn"].ToString();
                    hellouser_lbl.Text = $"Hello, {Session["LoggedIn"]}";
                    logout_btn.Visible = true;
                    displayUserProfile(email);
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }

        protected void LogoutBtn_Click(object sender, EventArgs e)
        {
            // Session clear & expire cookies -----------------------------------------------------------------------------------

            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("Login.aspx", false);

            // Expire Session Cookies
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }

        // Display data from db -----------------------------------------------------------------------------------
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        byte[] Key;
        byte[] IV;
        byte[] cardNo= null;

        protected void displayUserProfile(string email)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select * FROM Account WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["FirstName"] != DBNull.Value)
                        {
                            hellouser_lbl.Text = "Hello, " + reader["FirstName"].ToString(); 
                        }

                        if (reader["CardName"] != DBNull.Value)
                        {
                            cname_tb.Text = reader["CardName"].ToString();
                        }
                        if (reader["CVV"] != DBNull.Value)
                        {
                            cvv_tb.Text = reader["CVV"].ToString();
                        }
                        if (reader["ExpDate"] != DBNull.Value)
                        {
                            string mmyy = reader["ExpDate"].ToString();
                            expdatem_tb.Text = mmyy.Substring(0, 2);
                            expdatey_tb.Text = mmyy.Substring(mmyy.Length - 2);
                        }

                        if (reader["CardNo"] != DBNull.Value)
                        {
                            cardNo = Convert.FromBase64String(reader["CardNo"].ToString());
                        }
                        if (reader["IV"] != DBNull.Value)
                        {
                            IV = Convert.FromBase64String(reader["IV"].ToString());
                        }
                        if (reader["Key"] != DBNull.Value)
                        {
                            Key = Convert.FromBase64String(reader["Key"].ToString());
                        }
                    }
                    cardno_tb.Text = decryptData(cardNo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        // Symetric key decyrption -----------------------------------------------------------------------------------
        protected string decryptData(byte[] cipherText)
        {
            string plainText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;

                // Create decryptor to perform the stream transform
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                
                // Create the streams used for decryption
                using (MemoryStream msDecrpt = new MemoryStream(cipherText)) 
                {
                    using(CryptoStream csDecrypt = new CryptoStream(msDecrpt, decryptTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read decrypted bytes and place in string
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return plainText;
        }

        protected void PasswordBtn_Click(object sender, EventArgs e)
        {

        }
    }
}