using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace YS3BGA_IntAlk1.Pages.Clients
{
    public class IndexModel : PageModel
    {

        // session kezeléshez
        public string Username { get; set; }
        // session kezeléshez vége



        public List<ClientInfo> listClients = new List<ClientInfo>();
       
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






            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=YS3BGA_IntAlk1_db;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM clients order BY created_at desc"; 
                    using (SqlCommand command = new SqlCommand(sql, connection)) 
                    { 
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                                clientInfo.created_at = reader.GetDateTime(5).ToString();

                                listClients.Add(clientInfo);
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

    }


    public class ClientInfo
    {
        public String id;
        public String name;
        public String email;
        public String phone;
        public String address;
        public String created_at;
        
    }

}
