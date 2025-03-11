using Azure.Core;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RareNFTs.Application.DTOs;
using RareNFTs.Application.Services.Interfaces;
using RareNFTs.Infraestructure.Models;
using RareNFTs.Web.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using System.Threading.Channels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using X.PagedList;

namespace RareNFTs.Web.Controllers;
[Authorize(Roles = "admin,process")]

public class NftController : Controller
{

    private readonly IServiceNft _serviceNft;
    private readonly IServiceClient _serviceClient;
    public NftController(IServiceNft serviceNft, IServiceClient serviceClient)
    {
        _serviceNft = serviceNft;
        _serviceClient = serviceClient;
    }

    //This function handles GET requests for displaying the index page. It retrieves a collection of NFTs and passes it to the view for rendering.

    [HttpGet]
    public async Task<IActionResult> Index(int? page)
    {
        var collection = await _serviceNft.ListAsync();
        return View(collection.ToPagedList(page ?? 1,5));
    }

    //This function handles GET requests to display the create NFT page. It simply returns the corresponding view.

    // GET: NftController/Create
    public async Task<IActionResult> Create()
    {
        return View();
    }

    //This function handles POST requests to create a new NFT. It receives the NFT data and an image file, processes the image, adds the NFT to the database, and redirects to the index page.

    // POST: NftController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NftDTO dto, IFormFile imageFile)
    {
        MemoryStream target = new MemoryStream();

        // Cuando es Insert Image viene en null porque se pasa diferente
        if (dto.Image == null)
        {
            if (imageFile != null)
            {
                imageFile.OpenReadStream().CopyTo(target);

                dto.Image = target.ToArray();
                ModelState.Remove("Image");
            }
        }

        if (!ModelState.IsValid)
        {
            // Lee del ModelState todos los errores que
            // vienen para el lado del server
            string errors = string.Join("; ", ModelState.Values
                               .SelectMany(x => x.Errors)
                               .Select(x => x.ErrorMessage));
            // Response errores
            return BadRequest(errors);
        }

        await _serviceNft.AddAsync(dto);
        return RedirectToAction("Index");

    }

    //This function handles GET requests to display the details of a specific NFT. It retrieves the NFT object by ID and returns a partial view with its details.

    // GET: NftController/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        var @object = await _serviceNft.FindByIdAsync(id);
        return PartialView("_Details",@object);
    }

    //This function handles GET requests to display the edit page for a specific NFT. It retrieves the NFT object by ID and returns the edit view.

    // GET: NftController/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        var @object = await _serviceNft.FindByIdAsync(id);
        return View(@object);
    }


    //This function handles POST requests to edit an existing NFT. It receives the NFT ID, updated data, and an optional new image file. It updates the NFT in the database and redirects to the index page.

    // POST: NftController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, NftDTO dto, IFormFile imageFile)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                dto.Image = memoryStream.ToArray();
            }
        }
        else
        {
            // Si decides mantener la imagen existente cuando no se sube una nueva,
            // asegúrate de que el servicio que obtiene la imagen existente esté funcionando correctamente.
            var originalNft = await _serviceNft.FindByIdAsync(id);
            dto.Image = originalNft?.Image;
        }

        // Aquí se quita el campo Image del ModelState
        ModelState.Remove("imageFile");
        ModelState.Remove("Image");

        if (!ModelState.IsValid)
        {
            string errors = string.Join("; ", ModelState.Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage));
            return BadRequest(errors);
        }

        await _serviceNft.UpdateAsync(id, dto);

        return RedirectToAction("Index");
    }


    //This function handles GET requests to display the delete confirmation page for a specific NFT. It retrieves the NFT object by ID and returns the delete confirmation view.

    // GET: NftController/Delete/5
    public async Task<IActionResult> Delete(Guid id)
    {
        var @object = await _serviceNft.FindByIdAsync(id);
        return View(@object);
    }


    //This function handles POST requests to delete an existing NFT. It receives the NFT ID and deletes it from the database, then redirects to the index page.

    // POST: NftController/Delete/5
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, IFormCollection collection)
    {
        await _serviceNft.DeleteAsync(id);
        return RedirectToAction("Index");
    }


    //This function handles GET requests to display the list of NFTs owned by clients. It retrieves the list of owned NFTs, retrieves client and NFT details for each, and returns the view with the list of client-owned NFTs.

    public async Task<IActionResult> ListOwned(int? page)
    {
        var ListOwned = await _serviceNft.ListOwnedAsync();
        var ListViewModel = new List<ViewModelClientNft>();
        foreach (var item in ListOwned)
        {
            var client = await _serviceClient.FindByIdAsync(item.IdClient);
            var nft = await _serviceNft.FindByIdAsync(item.IdNft);

            var clientNft = new ViewModelClientNft()
            {
                IdClient = client.Id,
                Name = client.Name,
                Surname = client.Surname,
                Email = client.Email,
                IdNft = nft.Id,
                Description = nft.Description,
                Image = nft.Image,
                Price = nft.Price,
                Author = nft.Author,
                Date = item.Date,
            };

           ListViewModel.Add(clientNft);
        }
        return View(ListViewModel.ToPagedList(page ?? 1,5));
    }


    //This function handles GET requests to display the page for changing the owner of a specific NFT. It retrieves the NFT and client details and returns the view for changing the owner.

    public async Task<IActionResult> ChangeOwner(Guid id)
    {
        var @object = await _serviceNft.FindClientNftByIdAsync(id);
        // Obtener la información completa del cliente utilizando el ID de cliente
        var client = await _serviceClient.FindByIdAsync(@object!.IdClient);

        // Obtener la información completa del NFT utilizando el ID de NFT
        var nft = await _serviceNft.FindByIdAsync(@object.IdNft);

        // Crear un nuevo objeto ClientNftViewModel con la información del cliente y del NFT
        var clientNft = new ViewModelClientNft()
        {
            IdClient = client.Id,
            Name = client.Name,
            Surname = client.Surname,
            Email = client.Email,
            IdNft = nft.Id,
            Description = nft.Description,
            Image = nft.Image,
            Price = nft.Price,
            Author = nft.Author,
            Date = @object.Date,
        };
        return View(clientNft);
    }
   // This function handles POST requests to change the owner of an NFT.It receives the updated client-NFT association data, changes the owner in the database, and redirects to the list of owned NFTs.

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeOwner(ViewModelClientNft dto)
    {
        var @object = await _serviceNft.ChangeNFTOwnerAsync(dto.IdNft, dto.IdClient);
        return RedirectToAction("ListOwned");
    }

    //This function handles GET requests to retrieve NFTs by their description. It searches for NFTs with matching descriptions and returns them as JSON.

    [Authorize(Roles = "report, admin")]
    public async Task<IActionResult> GetNftByName(string filtro)
    {
        var collection = await _serviceNft.FindByDescriptionAsync(filtro);
        return Json(collection);
    }


   // This function handles GET requests to retrieve NFTs owned by clients based on the NFT name.It retrieves client-NFT associations, retrieves client and NFT details for the first association, and returns a partial view with the client-NFT details.

    [Authorize(Roles = "report, admin")]
    public async Task<IActionResult> GetNftOwnedByName(string name)
    {

        // Obtener la lista de ClientNft asociados al nombre del NFT
        var clientNftList = await _serviceClient.FindByNftNameAsync(name);


        var clientNft = clientNftList.FirstOrDefault();


        // Obtener la información completa del cliente utilizando el ID de cliente
        var client = await _serviceClient.FindByIdAsync(clientNft!.IdClient);

        // Obtener la información completa del NFT utilizando el ID de NFT
        var nft = await _serviceNft.FindByIdAsync(clientNft.IdNft);

        // Crear un nuevo objeto ClientNftViewModel con la información del cliente y del NFT
        var viewModel = new ViewModelClientNft
        {
            IdClient = client.Id,
            Name = client.Name,
            Surname = client.Surname,
            Genre = client.Genre,
            IdCountry = client.IdCountry,
            Email = client.Email,
            Description = nft.Description,
            Image = nft.Image // Suponiendo que Image es el campo que contiene la imagen del NFT
        };



        return PartialView("_ClientNft", viewModel);

    }
}
