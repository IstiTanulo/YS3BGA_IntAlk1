using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace YS3BGA_IntAlk1.Pages.Clients
{
    public class IndexModel : PageModel
    {

        // session kezel�shez
        public string Username { get; set; }
        // session kezel�shez v�ge



        public List<ClientInfo> listClients = new List<ClientInfo>();
       
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
