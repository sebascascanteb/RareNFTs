using RareNFTs.Application.DTOs;
using RareNFTs.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RareNFTs.Application.Services.Implementations;

namespace Electronics.Web.Controllers;

[Authorize(Roles = "admin")]
public class UserController : Controller
{
    private readonly IServiceUser _serviceUser;

    public UserController(IServiceUser serviceUsuario)
    {
        _serviceUser = serviceUsuario;
    }

    //This function handles GET requests to display the list of users. It retrieves the collection of users and passes it to the view for rendering.

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var collection = await _serviceUser.ListAsync();
        return View(collection);
    }

    //This function handles GET requests to display the create user page. It retrieves a list of roles and passes it to the view, then returns the view for creating a new user.

    // GET: UsuarioController/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.ListRole = await _serviceUser.ListRoleAsync();
        return View();
    }

    //This function handles POST requests to create a new user. It receives the user data, validates it, adds the user to the database if validation passes, and redirects to the index page.

    // POST: UsuarioController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserDTO dto)
    {
        ModelState.Remove("IdRoleNavigation");
        if (!ModelState.IsValid)
        {
            // Lee del ModelState todos los errores que
            // vienen para el lado del server
            string errors = string.Join("; ", ModelState.Values
                               .SelectMany(x => x.Errors)
                               .Select(x => x.ErrorMessage));
            return BadRequest(errors);
        }

        await _serviceUser.AddAsync(dto);
        return RedirectToAction("Index");

    }

    //This function handles GET requests to display the details of a specific user. It retrieves the user object by ID and returns a partial view with its details.

    // GET: UsuarioController/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        var @object = await _serviceUser.FindByIdAsync(id);
        return PartialView("_Details", @object);
    }

    //This function handles GET requests to display the edit page for a specific user. It retrieves the user object by ID, retrieves a list of roles, and returns the edit view.

    // GET: UsuarioController/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        ViewBag.ListRole = await _serviceUser.ListRoleAsync();

        var @object = await _serviceUser.FindByIdAsync(id);
        return View(@object);
    }

    //This function handles POST requests to edit an existing user. It receives the user ID and updated data, updates the user in the database, and redirects to the index page.

    // POST: UsuarioController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UserDTO dto)
    {
        await _serviceUser.UpdateAsync(id, dto);
        return RedirectToAction("Index");
    }

    //This function handles GET requests to display the delete confirmation page for a specific user. It retrieves the user object by ID and returns the delete confirmation view.

    // GET: UsuarioController/Delete/5
    public async Task<IActionResult> Delete(Guid id)
    {
        var @object = await _serviceUser.FindByIdAsync(id);
        return View(@object);
    }

    //This function handles POST requests to delete an existing user. It receives the user ID and deletes it from the database, then redirects to the index page.

    // POST: UsuarioController/Delete/5
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, IFormCollection collection)
    {
        await _serviceUser.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}
