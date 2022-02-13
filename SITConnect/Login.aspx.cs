using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SITConnect
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Prevent XSS using html url encoding -----------------------------------------------------------------------------------
            if (Request.QueryString["name"] != null)
            {
                name_lbl.Text = "as " + HttpUtility.HtmlEncode(Request.QueryString["name"]);
                email_tb.Text = HttpUtility.HtmlEncode(Request.QueryString["email"]);
            }
            
        }

        protected void LoginBtn_Click(object sender, EventArgs e)
        {
            // Input Validation
            if (String.IsNullOrEmpty(password_tb.Text) || String.IsNullOrEmpty(email_tb.Text))
            {
                xss_msg.Text = "Email or password field empty";
                xss_msg.ForeColor = Color.Red;
            }
            else
            {
                string pwd = password_tb.Text.ToString().Trim();
                string email = email_tb.Text.ToString().Trim();
                SHA512Managed hashing = new SHA512Managed();



                string dbHash = getDBHash(email);
                string dbSalt = getDBSalt(email);

                try
                {
                    // Check for account lockout timer
                    if (Session["MinutesLeft"] == null && Session["SecondsLeft"] == null)
                    {
                        if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                        {
                            string pwdWithSalt = pwd + dbSalt;
                            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                            string userHash = Convert.ToBase64String(hashWithSalt);

                            if (userHash.Equals(dbHash))
                            {
                                // Check captcha true false
                                if (ValidateCaptcha())
                                {
                                    // Add session var
                                    Session["LoggedIn"] = email_tb.Text.Trim();

                                    // Create GUID save into session
                                    string guid = Guid.NewGuid().ToString();
                                    Session["AuthToken"] = guid;

                                    // Create cookie with guid value
                                    Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                                    System.Diagnostics.Debug.WriteLine("User Authenticated");


                                    Response.Redirect("Home.aspx", false);
                                }
                                else
                                {
                                    xss_msg.Text = "You have failed the recaptcha. Please try again.";
                                    xss_msg.ForeColor = Color.Red;
                                }
                            }
                            else
                            {
                                accountLockout();
                            }
                        }
                        else
                        {
                            accountLockout();

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

        }

        // reCaptcha V3 ---------------------------------------------------------------------------------------------------------------------------
        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get;set; }
        }

        public bool ValidateCaptcha()
        {
            bool result = true;

            //When user submits the recaptcha form, the user gets a response POST parameter. 
            //captchaResponse consist of the user click pattern. Behaviour analytics! AI :) 
            string captchaResponse = Request.Form["g-recaptcha-response"];

            //To send a GET request to Google along with the response and Secret key.
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
           (" https://www.google.com/recaptcha/api/siteverify?secret=6LfeWGkeAAAAAArp1hw8k1xSkI_eykT1hbEMEm95 &response=" + captchaResponse);


            try
            {

                //Codes to receive the Response in JSON format from Google Server
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        //The response in JSON format
                        string jsonResponse = readStream.ReadToEnd();

                        //To show the JSON response string for learning purpose
                        //xss_msg.Text = jsonResponse.ToString();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        //Create jsonObject to handle the response e.g success or Error
                        //Deserialize Json
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        //Convert the string "False" to bool false or "True" to bool true
                        result = Convert.ToBoolean(jsonObject.success);//

                    }
                }

                return result;
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }

        // Retrieve Hash and Salt from db & encrypt data -----------------------------------------------------------------------------------
        protected string getDBHash(string email)
        {
            string h = null;
            string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;


            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Account WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", email);

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally { connection.Close(); }

            return h;
        }

        protected string getDBSalt(string email)
        {
            string s = null;
            string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PASSWORDSALT FROM ACCOUNT WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally { connection.Close(); }
            return s;
        }

        // Account lockout timer -----------------------------------------------------------------------------------
        public void Timer_Tick(object sender, EventArgs e)
        {
            var currentTimeMinutes = int.Parse(Session["MinutesLeft"].ToString());
            var currentTimeSeconds = int.Parse(Session["SecondsLeft"].ToString());

            currentTimeSeconds--;

            if (currentTimeSeconds == 0 && currentTimeMinutes == 0)
            {
                // Reset session
                Session["LoggedInAttempt"] = null;
                Session["MinutesLeft"] = null;
                Session["SecondsLeft"] = null;
                DisplayTextMinutes.Text = "";
                DisplayTextSeconds.Text =  "";
                Timer.Enabled = false;
                return;

            }
            else if (currentTimeSeconds < 0)
            {
                currentTimeMinutes--;
                currentTimeSeconds = 59;
            }

            if (currentTimeMinutes != 0)
            {
                DisplayTextMinutes.Text = $"Account locked out for {currentTimeMinutes} min";
                DisplayTextSeconds.Text = $"{currentTimeSeconds} sec";
                DisplayTextMinutes.ForeColor = Color.Red;
                DisplayTextSeconds.ForeColor = Color.Red;

            }
            else
            {
                DisplayTextMinutes.Text = $"Account locked out for";
                DisplayTextSeconds.Text = $"{currentTimeSeconds} sec";
                DisplayTextMinutes.ForeColor = Color.Red;
                DisplayTextSeconds.ForeColor = Color.Red;
            }

            Session["MinutesLeft"] = currentTimeMinutes;
            Session["SecondsLeft"] = currentTimeSeconds;

        }


        public void accountLockout()
        {
            // Check for login attempts
            if (Session["LoggedInAttempt"] == null)
            {
                Session["LoggedInAttempt"] = 1;
                xss_msg.Text = "Invalid email or password. You have 2 tries left";
                xss_msg.ForeColor = Color.Red;
                System.Diagnostics.Debug.WriteLine("Attempt 1");
            }
            else
            {
                int count = Convert.ToInt32(Session["LoggedInAttempt"]);
                Session["LoggedInAttempt"] = count + 1;
                

                if (count + 1 > 2)
                {
                    // Lockout set to 1 min 30 secs
                    Session["MinutesLeft"] = 1;
                    Session["SecondsLeft"] = 30;
                    Timer.Enabled = true;
                    System.Diagnostics.Debug.WriteLine("Attempt 3");
                }
                else
                {
                    xss_msg.Text = "Invalid email or password. You have 1 tries left";
                    xss_msg.ForeColor = Color.Red;
                    System.Diagnostics.Debug.WriteLine("Attempt 2");
                }

            }         
        }

    }
}