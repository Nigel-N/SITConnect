using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text.RegularExpressions;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SITConnect
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                    
        }
        // Server side password check --------------------------------------------------------------
        private int checkPassword(string password)
        {
            int score = 0;
            // score 1 very weak
            if (password.Length < 8)
            {
                return 1;
            }
            else
            {
                score = 1;
            }
            // score 2 weak
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }
            // score 3 medium
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }
            // score 4 strong
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }
            // score 5 very strong
            if (Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                score++;
            }

            return score;
        }

        protected void pwdstrength_btn_click(object sender, EventArgs e)
        {
            int scores = checkPassword(password_tb.Text);
            string status = "";
            switch (scores)
            {
                case 1:
                    status = "Very Weak";
                    break;
                case 2:
                    status = "Weak";
                    break;
                case 3:
                    status = "Medium";
                    break;
                case 4:
                    status = "Strong";
                    break;
                case 5:
                    status = "Very Strong";
                    break;
                default:
                    break;
            }
            strchecker_lbl.Text = "Status : " + status;
            if (scores < 4)
            {
                strchecker_lbl.ForeColor = Color.Red;
                return;
            }
            strchecker_lbl.ForeColor = Color.Green;
        }

        // Hashing Password ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string expDate;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        protected void RegisterBtn_Click(object sender, EventArgs e)
        {
            // Input Validation
            if (fname_tb.Text.Length > 30 || lname_tb.Text.Length > 30)
            {
                namee_lbl.Text = "First name or Last name too long";
                namee_lbl.ForeColor = Color.Red;
            }
            //else if (regex.ismatch(email_tb, "[(?=.*\d)(?=.*[a-z])(?=.*[a-z])(?=.*[^a-za-z0-9])(?!.*\s).{8,15}]"))
            //{
            //    score++;
            //}
            else if (cname_tb.Text.Length > 30)
            {
                cnamee_lbl.Text = "Cardholder name too long";
                cnamee_lbl.ForeColor = Color.Red;
            }
            else if (cvv_tb.Text.Length < 3 )
            {
                cvve_lbl.Text = "Invalid CVV";
                cvve_lbl.ForeColor = Color.Red;
            }
            else if (cardno_tb.Text.Length < 16)
            {
                cardnoe_lbl.Text = "Invalid Card number";
                cardnoe_lbl.ForeColor = Color.Red;
            }
            else if (expdatem_tb.Text.Length < 2 || expdatey_tb.Text.Length < 2)
            {
                expdatee_lbl.Text = "Invalid expiration date";
                expdatee_lbl.ForeColor = Color.Red;
            }
            //&& (Regex.IsMatch(cvv_tb.Text, "[0-9]")
            else
            {
                string pwd = password_tb.Text.ToString().Trim();

                //Generate random "salt"
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] saltByte = new byte[8];

                //Fills array of bytes with a cryptographically strong sequence of random values.
                rng.GetBytes(saltByte);
                salt = Convert.ToBase64String(saltByte);

                SHA512Managed hashing = new SHA512Managed();

                string pwdWithSalt = pwd + salt;
                byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

                finalHash = Convert.ToBase64String(hashWithSalt);

                RijndaelManaged cipher = new RijndaelManaged();
                cipher.GenerateKey();
                Key = cipher.Key;
                IV = cipher.IV;

                //Adding CC Expiry date
                expDate = expdatem_tb.Text.ToString() + expdatey_tb.Text.ToString();

                createAccount();
            }
        }

        protected void createAccount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@FirstName, @LastName, @Email, @DOB, @PasswordHash, @PasswordSalt, @CardName, @CVV, @CardNo, @ExpDate ,@IV, @Key)"))        
                {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FirstName", fname_tb.Text.Trim());
                            cmd.Parameters.AddWithValue("@LastName", lname_tb.Text.Trim());
                            cmd.Parameters.AddWithValue("@Email", email_tb.Text.Trim());
                            cmd.Parameters.AddWithValue("@DOB", dob_tb.Text.Trim());

                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);

                            cmd.Parameters.AddWithValue("@CardName", cname_tb.Text.Trim());
                            cmd.Parameters.AddWithValue("@CVV", cvv_tb.Text.Trim());
                            cmd.Parameters.AddWithValue("@ExpDate", expDate);


                            cmd.Parameters.AddWithValue("@CardNo", Convert.ToBase64String(encryptData(cardno_tb.Text.Trim())));
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));


                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                Response.Redirect("Login.aspx?name=" + HttpUtility.UrlEncode(fname_tb.Text) + "&email=" + HttpUtility.UrlEncode(email_tb.Text), true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                System.Diagnostics.Debug.WriteLine("Either DB Error or Response.Redirect Error");

            }
        }

        // Symetric key encryption -----------------------------------------------------------------------------------
        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0,
               plainText.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally { }
            return cipherText;
        }

    }
}