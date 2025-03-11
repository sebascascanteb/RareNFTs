using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RareNFTs.Application.DTOs;
using RareNFTs.Application.Services.Implementations;
using RareNFTs.Application.Services.Interfaces;
using X.PagedList;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RareNFTs.Web.Controllers;

[Authorize(Roles = "admin,process")]

public class ClientController : Controller
{
    private readonly IServiceClient _serviceClient;
    private readonly IServiceCountry _serviceCountry;

    public ClientController(IServiceClient serviceClient, IServiceCountry serviceCountry)
    {
        _serviceClient = serviceClient;
        _serviceCountry = serviceCountry;
    }

    // Retrieves a list of clients and returns the Index view
    [HttpGet]
    public async Task<IActionResult> Index(int? page)
    {
        var collection = await _serviceClient.ListAsync();
        return View(collection.ToPagedList(page ?? 1, 5));
    }

    // Returns the Create view along with the list of countries
    // GET: ClientController/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.ListCountry = await _serviceCountry.ListAsync();
        return View();
    }


    // Handles the creation of a new client
    // POST: ClientController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClientDTO dto)
    {            // Validates the incoming model

        ModelState.Remove("Id");
        if (!ModelState.IsValid)
        {
            // If there are model errors, returns a bad request response with error messages

            string errors = string.Join("; ", ModelState.Values
                               .SelectMany(x => x.Errors)
                               .Select(x => x.ErrorMessage));
            return BadRequest(errors);
        }
        // Adds the new client and redirects to the Index action

        await _serviceClient.AddAsync(dto);
        return RedirectToAction("Index");

    }


    // GET: ClientController/Details/5
    // Retrieves details of a client by ID and returns a partial view with the details

    public async Task<IActionResult> Details(Guid id)
    {
        var @object = await _serviceClient.FindByIdAsync(id);
        return PartialView("_Details",@object);
    }

    // GET: ClientController/Edit/5

    // Retrieves details of a client by ID for editing, along with the list of countries
    public async Task<IActionResult> Edit(Guid id)
    {
        ViewBag.ListCountry = await _serviceCountry.ListAsync();
        var @object = await _serviceClient.FindByIdAsync(id);
        return View(@object);
    }

    // POST: ClientController/Edit/5

    // Updates the details of a client and redirects to the Index action
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ClientDTO dto)
    {
        await _serviceClient.UpdateAsync(id, dto);
        return RedirectToAction("Index");
    }

    // GET: ClientController/Delete/5

    // Retrieves details of a client by ID for deletion
    public async Task<IActionResult> Delete(Guid id)
    {
        var @object = await _serviceClient.FindByIdAsync(id);
        return View(@object);
    }

    // POST: ClientController/Delete/5

    // Deletes a client by ID and redirects to the Index action
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, IFormCollection collection)
    {
        await _serviceClient.DeleteAsync(id);
        return RedirectToAction("Index");
    }

    // Retrieves clients by their description asynchronously
    // Returns a JSON response containing the matching clients

    [HttpGet]
    [Authorize(Roles = "report, admin")]
    public IActionResult GetClientByName(string filtro)
    {

        var collections = _serviceClient.FindByDescriptionAsync(filtro).GetAwaiter().GetResult();

        return Json(collections);
    }

}
