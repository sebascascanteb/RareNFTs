using RareNFTs.Application.DTOs;
using RareNFTs.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;

namespace RareNFTs.Web.Controllers;
[Authorize(Roles = "admin,process")]

public class CardController : Controller
{
    private readonly IServiceCard _serviceCard;

    public CardController(IServiceCard serviceCard)
    {
        _serviceCard = serviceCard;
    }

    // Retrieves a collection of cards from the service
    // Returns the "Index" view passing the collection as the model
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var collection = await _serviceCard.ListAsync();
        return View(collection);
    }

    // Returns the view to create a new card

    // GET: CardController/Create
    public IActionResult Create()
    {
        return View();
    }


    // Checks if the received model is valid
    // If there are model errors, joins them into a string and returns a bad request response
    // If the model is valid, adds the card to the service and redirects to the "Index" action
    // POST: CardController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CardDTO dto)
    {

        if (!ModelState.IsValid)
        {
            // Lee del ModelState todos los errores que
            // vienen para el lado del server
            string errors = string.Join("; ", ModelState.Values
                               .SelectMany(x => x.Errors)
                               .Select(x => x.ErrorMessage));
            return BadRequest(errors);
        }

        await _serviceCard.AddAsync(dto);
        return RedirectToAction("Index");

    }


    // Retrieves the details of a card by its identifier
    // Returns a partial view with the card details
    // GET: CardController/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        var @object = await _serviceCard.FindByIdAsync(id);
        return PartialView("_Details",@object);
    }

// Retrieves the details of a card by its identifier for editing
// Returns the view to edit the card, passing the details as the model
// GET: CardController/Edit/5
public async Task<IActionResult> Edit(Guid id)
    {
        var @object = await _serviceCard.FindByIdAsync(id);
        return View(@object);
    }


    // Updates the details of a card by its identifier
    // Redirects to the "Index" action after editing
    // POST: CardController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CardDTO dto)
    {
        await _serviceCard.UpdateAsync(id, dto);
        return RedirectToAction("Index");
    }

    // Retrieves the details of a card by its identifier for deletion
    // Returns the deletion confirmation view, passing the details as the model

    // GET: CardController/Delete/5
    public async Task<IActionResult> Delete(Guid id)
    {
        var @object = await _serviceCard.FindByIdAsync(id);
        return View(@object);
    }



    // Deletes a card by its identifier
    // Redirects to the "Index" action after deletion

    // POST: CardController/Delete/5
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, IFormCollection collection)
    {
        await _serviceCard.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}
