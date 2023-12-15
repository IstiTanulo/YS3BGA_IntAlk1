using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class LoggedInModel : PageModel
{
    public string Username { get; set; }

    public IActionResult OnGet()
    {
        // Ellen�rizd, hogy a felhaszn�l� be van-e jelentkezve
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrEmpty(username))
        {
            // Ha nincs bejelentkezve, �tir�ny�t�s az Index oldalra
            return RedirectToPage("Index");
        }

        // Sikeres bejelentkez�s, megjelen�t�s a bejelentkezett felhaszn�l�n�vvel
        Username = username;
        
        return Page();
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.Session.Remove("username");
        return RedirectToPage("/Index");
    }


}
