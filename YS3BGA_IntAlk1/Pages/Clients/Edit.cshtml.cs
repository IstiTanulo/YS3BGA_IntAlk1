using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace YS3BGA_IntAlk1.Pages.Clients
{
    public class EditModel : PageModel
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


            
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=YS3BGA_IntAlk1_db;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM clients WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);

                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }

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

            clientInfo.id = Request.Form["id"];
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            if (clientInfo.id.Length == 0 || clientInfo.name.Length == 0 || 
                clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 || 
                clientInfo.address.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {

                    String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=YS3BGA_IntAlk1_db;Integrated Security=True;Encrypt=False";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        String sql = "UPDATE clients " + 
                                     "SET name=@name, email=@email, phone=@phone, address=@address " + 
                                     "WHERE id=@id";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@name", clientInfo.name);
                            command.Parameters.AddWithValue("@email", clientInfo.email);
                            command.Parameters.AddWithValue("@phone", clientInfo.phone);
                            command.Parameters.AddWithValue("@address", clientInfo.address);
                            command.Parameters.AddWithValue("@id", clientInfo.id);

                            command.ExecuteNonQuery();
                        }
                    }
                

            }
            catch (Exception ex)
            {

                errorMessage=ex.Message;
                return;
            }

            Response.Redirect("/Clients/Index");

        }
    }
}
