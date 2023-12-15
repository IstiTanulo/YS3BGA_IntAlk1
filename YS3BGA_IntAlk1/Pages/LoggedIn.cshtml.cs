using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class LoggedInModel : PageModel
{
    public string Username { get; set; }

    public IActionResult OnGet()
    {
        // Ellenõrizd, hogy a felhasználó be van-e jelentkezve
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrEmpty(username))
        {
            // Ha nincs bejelentkezve, átirányítás az Index oldalra
            return RedirectToPage("Index");
        }

        // Sikeres bejelentkezés, megjelenítés a bejelentkezett felhasználónévvel
        Username = username;
        
        return Page();
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.Session.Remove("username");
        return RedirectToPage("/Index");
    }


}
