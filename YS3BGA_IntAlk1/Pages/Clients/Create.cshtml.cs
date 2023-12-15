using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection;

namespace YS3BGA_IntAlk1.Pages.Clients
{
    public class CreateModel : PageModel
    {


        // session kezel�shez
        public string Username { get; set; }
        // session kezel�shez v�ge





        public ClientInfo clientInfo = new ClientInfo(); 
        public String errorMessage = ""; 
        public String successMessage = "";
        public IActionResult OnGet()
        {

            // session kezel�shez
            // Ellen�rizd, hogy a felhaszn�l� be van-e jelentkezve
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                // Ha nincs bejelentkezve, �tir�ny�t�s az Index oldalra
                return RedirectToPage("Index");
            }

            // Sikeres bejelentkez�s, megjelen�t�s a bejelentkezett felhaszn�l�n�vvel
            Username = username;
            // session kezel�shez v�ge


            // session kezel�shez
            return Page();
            // session kezel�shez v�ge

        }

        // session kezel�shez
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Remove("Username");
            return RedirectToPage("/Index");
        }
        // session kezel�shez v�ge


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
