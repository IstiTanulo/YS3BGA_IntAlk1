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
        // Ellenõrizd a bejelentkezési adatokat
        if (Username != "aaa" || Password != "aaa")
        {
            // Sikertelen bejelentkezés, hozzáadás a ModelState-hez
            ModelState.AddModelError(string.Empty, "Wrong Username or Password");
            return Page();
        }

        // Sikeres bejelentkezés, tárold el a felhasználónevet a Session-ben
        HttpContext.Session.SetString("Username", Username);
        return RedirectToPage("/Clients/Index", new { username = Username });
    }
}
