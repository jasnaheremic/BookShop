using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace BookShop.Pages
{
    public class ContactModel : PageModel
    {
        public void OnGet()
        {
        }

        [BindProperty]
        [Required(ErrorMessage = "The First Name is required")]
        [Display(Name = "First Name*")]
        public string FirstName { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "The Last Name is required")]
        [Display(Name = "Last Name*")]
        public string LastName { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Display(Name = "Email*")]
        public string Email { get; set; } = "";

        [BindProperty]
        public string? Phone { get; set; } = "";

        [BindProperty]
        [Display(Name = "Subject*")]
        public string Subject { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "The Mesage is required!")]
        [MinLength(5, ErrorMessage = "The Message should be at least 5 characters")]
        [MaxLength(1024, ErrorMessage = "The Message should be at less than 1024 characters")]
        [Display(Name = "Message*")]
        public string Message { get; set; } = "";

        public List<SelectListItem> SubjectList { get; } = new List<SelectListItem> {

            new SelectListItem {Value = "Order Status", Text = "Order Status"},
            new SelectListItem {Value = "Refund Request", Text = "Refund Request"},
            new SelectListItem {Value = "Job Application", Text = "Job Application"},
            new SelectListItem {Value = "Other", Text = "Other"}

        };
 


        public string SuccessMessage { get; set; } = "";
        public string ErrorMessage { get; set; } = "";

        public void OnPost() 
        {

            //provjera da li postoje prazna polja
            //check if any required filed is empty
            if (!ModelState.IsValid) 
            {
                //error 
                ErrorMessage = "Please fill all required fields";
                return;
            }

            if (Phone == null)
            {
                Phone = "";
            }

            // Dodavnje poruke u bazu podataka
            // Add this message to the database

            try
            {
                string connectionString = "Data Source=DESKTOP-VRSONHD;Initial Catalog=BookShop;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO messages " +
                        "(firstname, lastname, email, phone, subject, message) VALUES " +
                        "(@firstname, @lastname, @email, @phone, @subject, @message);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@firstname", FirstName);
                        command.Parameters.AddWithValue("@lastname",LastName);
                        command.Parameters.AddWithValue("@email", Email);
                        command.Parameters.AddWithValue("@phone", Phone);
                        command.Parameters.AddWithValue("@subject", Subject);
                        command.Parameters.AddWithValue("@message", Message);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            //Slanje Email-a potvrde klijentu
            //Send Confirmation Email to client 

            SuccessMessage = "Your message has been received correctly";

            FirstName = "";
            LastName = "";
            Email = "";
            Phone = "";
            Subject = "";
            Message = "";

            ModelState.Clear();
        }
    
    }
}
