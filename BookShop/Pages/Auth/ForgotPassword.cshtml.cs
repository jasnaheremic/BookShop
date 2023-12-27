using BookShop.MyHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace BookShop.Pages.Auth
{
    [RequireNoAuth]
    public class ForgotPasswordModel : PageModel
    {
        [BindProperty, Required(ErrorMessage = "The Email is required"), EmailAddress]
        public string Email { get; set; } = "";

        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                errorMessage = "Data validation failed";
                return;
            }

            // 1. create token 2. save token in the database 3. send token by email to the user

            try
            {
                string connectionString = "Data Source=DESKTOP-VRSONHD;Initial Catalog=BookShop;Integrated Security=True";

                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM users WHERE email = @email";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@email", Email);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string firstname = reader.GetString(1);
                                string lastname = reader.GetString(2);

                                string token = Guid.NewGuid().ToString();

                                //SaveToken(Email, token);

                                //send token by email to the user
                                string resetUrl = Url.PageLink("/Auth/ResetPassword") + "?token=" + token;
                                string username = firstname + " " + lastname;
                                string subject = "Password Reset";
                                string message = "Dear " + username + ", \n\n" +
                                    "You can reset your password using the following link:\n\n" +
                                    resetUrl + "\n\n" + "Best Regards";
                            }
                            else
                            {
                                errorMessage = "We have no user with this email address";
                                return; 
                            }
                        }
                    }
                }
            }catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            successMessage = "Please check your email and click on the reset password link";
        }
    }
}