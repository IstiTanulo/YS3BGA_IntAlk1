using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection;

namespace YS3BGA_IntAlk1.Pages.Clients
{
    public class CreateModel : PageModel
    {


        // session kezeléshez
        public string Username { get; set; }
        // session kezeléshez vége





        public ClientInfo clientInfo = new ClientInfo(); 
        public String errorMessage = ""; 
        public String successMessage = "";
        public IActionResult OnGet()
        {

            // session kezeléshez
            // Ellenõrizd, hogy a felhasználó be van-e jelentkezve
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                // Ha nincs bejelentkezve, átirányítás az Index oldalra
                return RedirectToPage("Index");
            }

            // Sikeres bejelentkezés, megjelenítés a bejelentkezett felhasználónévvel
            Username = username;
            // session kezeléshez vége


            // session kezeléshez
            return Page();
            // session kezeléshez vége

        }

        // session kezeléshez
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Remove("Username");
            return RedirectToPage("/Index");
        }
        // session kezeléshez vége


        public void OnPost()
        {
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 ||
                clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }
            //Az uj kliens mentese az adatbazisba

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=YS3BGA_IntAlk1_db;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO clients " +
                                 "(name, email, phone, address) VALUES " +
                                 "(@name, @email, @phone, @address);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);

                        command.ExecuteNonQuery();
                    }
                }
            }

            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

                clientInfo.name = "";
                clientInfo.email = "";
                clientInfo.phone = "";
                clientInfo.address = "";
                successMessage = "New client added succesfully";

                Response.Redirect("/Clients/Index");




           


        }

    

    }



}
