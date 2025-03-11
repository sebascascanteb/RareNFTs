using RareNFTs.Application.DTOs;
using RareNFTs.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;

namespace RareNFTs.Web.Controllers;
[Authorize(Roles = "admin,process")]

public class CountryController : Controller
{
    private readonly IServiceCountry _serviceCountry;

    public CountryController(IServiceCountry serviceCountry)
    {
        _serviceCountry = serviceCountry;
    }

    [HttpGet]

    // Retrieves a list of countries and returns the Index view
    public async Task<IActionResult> Index()
    {
        var collection = await _serviceCountry.ListAsync();
        return View(collection);
    }

    // GET: CountryController/Create

    // Returns the Create view
    public IActionResult Create()
    {
        return View();
    }


    // POST: CountryController/Create

    // Handles the creation of a new country
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CountryDTO dto)
    {
        // Validates the incoming model

        if (!ModelState.IsValid)
        {
            // If there are model errors, returns a bad request response with error messages

            string errors = string.Join("; ", ModelState.Values
                               .SelectMany(x => x.Errors)
                               .Select(x => x.ErrorMessage));
            return BadRequest(errors);
        }
        // Adds the new country and redirects to the Index action

        await _serviceCountry.AddAsync(dto);
        return RedirectToAction("Index");

    }


    // GET: CountryController/Details/5
    // Retrieves details of a country by ID and returns a partial view with the details

    public async Task<IActionResult> Details(string id)
    {
        var @object = await _serviceCountry.FindByIdAsync(id);
        return PartialView("_Details", @object);
    }

    // GET: CountryController/Edit/5
    // Retrieves details of a country by ID for editing

    public async Task<IActionResult> Edit(string id)
    {
        var @object = await _serviceCountry.FindByIdAsync(id);
        return View(@object);
    }

    // POST: CountryController/Edit/5
    // Updates the details of a country and redirects to the Index action

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, CountryDTO dto)
    {
        await _serviceCountry.UpdateAsync(id, dto);
        return RedirectToAction("Index");
    }

    // GET: CountryController/Delete/5
    // Retrieves details of a country by ID for deletion

    public async Task<IActionResult> Delete(string id)
    {
        var @object = await _serviceCountry.FindByIdAsync(id);
        return View(@object);
    }

    // POST: CountryController/Delete/5
    // Deletes a country by ID and redirects to the Index action

    [HttpPost]

    public async Task<IActionResult> Delete(string id, IFormCollection collection)
    {
        await _serviceCountry.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}
