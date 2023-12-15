using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public string ErrorMessage { get; set; }

    public IActionResult OnPost()
    {
        // Ellen�rizd a bejelentkez�si adatokat
        if (Username != "aaa" || Password != "aaa")
        {
            // Sikertelen bejelentkez�s, hozz�ad�s a ModelState-hez
            ModelState.AddModelError(string.Empty, "Wrong Username or Password");
            return Page();
        }

        // Sikeres bejelentkez�s, t�rold el a felhaszn�l�nevet a Session-ben
        HttpContext.Session.SetString("Username", Username);
        return RedirectToPage("/Clients/Index", new { username = Username });
    }
}
