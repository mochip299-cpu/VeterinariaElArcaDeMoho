using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Veterinaria.Pages.Auth;

public class LogoutModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signIn;
    public LogoutModel(SignInManager<IdentityUser> signIn) => _signIn = signIn;

    public async Task<IActionResult> OnPostAsync()
    {
        await _signIn.SignOutAsync();
        return RedirectToPage("/Auth/Login");
    }

    public IActionResult OnGet() => RedirectToPage("/Auth/Login");
}
