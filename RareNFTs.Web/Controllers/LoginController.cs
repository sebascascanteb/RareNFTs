using RareNFTs.Application.Services.Implementations;
using RareNFTs.Application.Services.Interfaces;
using RareNFTs.Web.ViewModels;
using Humanizer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;
using RareNFTs.Web.ViewModels;
using RareNFTs.Application.Services.Interfaces;

namespace Electronics.Web.Controllers;

public class LoginController : Controller
{

    private readonly IServiceUser _serviceUser;
    private readonly ILogger<LoginController> _logger;
    public LoginController(IServiceUser serviceUsuario, ILogger<LoginController> logger)
    {
        _serviceUser = serviceUsuario;
        _logger = logger;
    }


    //This function handles GET requests for the login page. It simply returns the corresponding view.

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    //This function handles POST requests to log in. It checks the validity of the model and performs user authentication.
    //If there are errors, it logs an error message and redirects to the login view. If the login is successful,
    //it creates and stores user claims in a claims identity and redirects the user to the home page.

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogIn(ViewModelLogin viewModelLogin)
    {

        if (!ModelState.IsValid)
        {
            // Lee del ModelState todos los errores que
            // vienen para el lado del server
            string errors = string.Join("; ", ModelState.Values
                               .SelectMany(x => x.Errors)
                               .Select(x => x.ErrorMessage));
            ViewBag.Message = $"Error accessing {errors}";

            _logger.LogInformation($"Error login {viewModelLogin}, Errors: --> {errors}");
            return View("Index");
        }
        // User exist ?
        var usuarioDTO = await _serviceUser.LoginAsync(viewModelLogin.User, viewModelLogin.Password);
        if (usuarioDTO == null)
        {
            ViewBag.Message = "Error accessing";
            _logger.LogInformation($"Error login {viewModelLogin.User}, Error: --> {ViewBag.Message}");
            return View("Index");
        }

        // Claim stores  User information like Name, role and others.  
        List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.Email, usuarioDTO.Email),
                new Claim(ClaimTypes.Role, usuarioDTO.DescriptionRole!)
            };

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        AuthenticationProperties properties = new AuthenticationProperties()
        {
            AllowRefresh = true
        };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            properties);

        _logger.LogInformation($"Correct connection of {viewModelLogin.User}");

        return RedirectToAction("Index", "Home");
    }


    //This function handles the logoff of authenticated users. Only logged-in users can access this function. It logs the user's logoff and removes the authentication, then redirects to the login page.

    /*Only user connected can logoff*/
    [Authorize]
    public async Task<IActionResult> LogOff()
    {
        _logger.LogInformation($"Correct disconnection of {User!.Identity!.Name}");
        await HttpContext.SignOutAsync();

        return RedirectToAction("Index", "Login");
    }
}
