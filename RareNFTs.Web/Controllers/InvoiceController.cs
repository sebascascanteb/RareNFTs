using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RareNFTs.Application.DTOs;
using RareNFTs.Application.Services.Implementations;
using RareNFTs.Application.Services.Interfaces;
using RareNFTs.Web.ViewModels;
using System.Text.Json;

namespace RareNFTs.Web.Controllers;
[Authorize(Roles = "admin,process")]

public class InvoiceController : Controller
{
    private readonly IServiceNft _serviceNft;
    private readonly IServiceCard _serviceCard;
    private readonly IServiceInvoice _serviceInvoice;
    private readonly IServiceClient _serviceClient;


    public InvoiceController(IServiceNft serviceNft,
                            IServiceCard serviceCard,
                            IServiceInvoice serviceInvoice,
                            IServiceClient serviceClient)
    {
        _serviceNft = serviceNft;
        _serviceCard = serviceCard;
        _serviceInvoice = serviceInvoice;
        _serviceClient = serviceClient;

    }

  //  This action initializes data for the invoice creation page, including generating a new invoice ID and retrieving a collection of cards for selection.

    public async Task<IActionResult> Index()
    {

        var newId = _serviceInvoice.GetNewId();
        ViewBag.InvoiceId = newId;
        var collection = await _serviceCard.ListAsync();
        ViewBag.ListCard = collection;

        // Clear CartShopping
        TempData["CartShopping"] = null;
        TempData.Keep();

        return View();
    }

    //This action adds a product to the shopping cart. It validates the quantity against available stock and updates the cart accordingly.

    public async Task<IActionResult> AddProduct(Guid id, int quantity)
    {
        InvoiceDetailDTO invoiceDetailDTO = new InvoiceDetailDTO();
        List<InvoiceDetailDTO> list = new List<InvoiceDetailDTO>();
        string json = "";
        var nft = await _serviceNft.FindByIdAsync(id);

        // Stock ??

        if (quantity > nft.Quantity)
        {
            return BadRequest("No stock available!");
        }

        invoiceDetailDTO.NftDescription = nft.Description!;
        invoiceDetailDTO.Quantity = quantity;
        invoiceDetailDTO.Price = nft.Price;
        invoiceDetailDTO.IdNft = id;
        invoiceDetailDTO.TotalLine = (invoiceDetailDTO.Price * invoiceDetailDTO.Quantity);

        if (TempData["CartShopping"] == null)
        {
            list.Add(invoiceDetailDTO);
            // Reenumerate 
            int idx = 1;
            list.ForEach(p => p.Sequence = idx++);
            json = JsonSerializer.Serialize(list);
            TempData["CartShopping"] = json;
        }
        else
        {
            json = (string)TempData["CartShopping"]!;
            list = JsonSerializer.Deserialize<List<InvoiceDetailDTO>>(json!)!;
            list.Add(invoiceDetailDTO);
            // Reenumerate 
            int idx = 1;
            list.ForEach(p => p.Sequence = idx++);
            json = JsonSerializer.Serialize(list);
            TempData["CartShopping"] = json;
        }

        TempData.Keep();
        return PartialView("_InvoiceDetail", list);
    }

    //This action retrieves the current invoice details from the shopping cart and prepares them for display.

    public IActionResult GetInvoiceDetail()
    {
        List<InvoiceDetailDTO> list = new List<InvoiceDetailDTO>();
        string json = "";
        json = (string)TempData["CartShopping"]!;
        list = JsonSerializer.Deserialize<List<InvoiceDetailDTO>>(json!)!;
        // Reenumerate 
        int idx = 1;
        list.ForEach(p => p.Sequence = idx++);
        json = JsonSerializer.Serialize(list);
        TempData["CartShopping"] = json;
        TempData.Keep();

        return PartialView("_InvoiceDetail", list);
    }

    //This action removes a product from the shopping cart based on its index. It updates the cart and refreshes the displayed invoice details.

    public IActionResult DeleteProduct(int id)
    {
        InvoiceDetailDTO invoiceDetailDTO = new InvoiceDetailDTO();
        List<InvoiceDetailDTO> list = new List<InvoiceDetailDTO>();
        string json = "";

        if (TempData["CartShopping"] != null)
        {
            json = (string)TempData["CartShopping"]!;
            list = JsonSerializer.Deserialize<List<InvoiceDetailDTO>>(json!)!;
            // Remove from list by Index
            int idx = list.FindIndex(p => p.Sequence == id);
            list.RemoveAt(idx);
            json = JsonSerializer.Serialize(list);
            TempData["CartShopping"] = json;
        }

        TempData.Keep();

        // return Content("Ok");
        return PartialView("_InvoiceDetail", list);

    }

    //This action creates a new invoice based on the provided invoice header and the contents of the shopping cart. It then redirects to the homepage.

    public async Task<IActionResult> Create(InvoiceHeaderDTO InvoiceHeaderDTO)
    {
        string json;
        try
        {

            if (!ModelState.IsValid)
            {

            }

            json = (string)TempData["CartShopping"]!;

            if (string.IsNullOrEmpty(json))
            {
                return BadRequest("No hay datos por facturar");
            }

            var list = JsonSerializer.Deserialize<List<InvoiceDetailDTO>>(json!)!;

            //Mismo numero de factura para el detalle FK
            list.ForEach(p => p.IdInvoice = InvoiceHeaderDTO.Id);
            InvoiceHeaderDTO.ListInvoiceDetail = list;

            await _serviceInvoice.AddAsync(InvoiceHeaderDTO);


            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            // Keep Cache data
            TempData.Keep();
            return BadRequest(ex.Message);
        }
    }


    //This action cancels an existing invoice asynchronously and redirects to the homepage.


    [HttpPost]
    public ActionResult Cancel(Guid id)
    {
        _serviceInvoice.CancelInvoiceAsync(id);
        return RedirectToAction("Index");
    }

    //This action retrieves a list of active invoices, prepares them for display, including client and card details, and returns the view for active invoices.

    public async Task<IActionResult> ListActives()
    {

        var listActives = await _serviceInvoice.ListActivesAsync();
        var listViewModel = new List<ViewModelInvoice>();



        foreach (var item in listActives)
        {
            var client = await _serviceClient.FindByIdAsync(item.IdClient);
            var card = await _serviceCard.FindByIdAsync(item.IdCard);
            listViewModel.Add(new ViewModelInvoice
            {
                Id = item.Id,
                IdCard = item.IdCard,
                IdClient = item.IdClient,
                Name = client.Name,
                Surname = client.Surname,
                Email = client.Email,
                CardDescription = card.Description,
                Date = item.Date,
                Total = item.Total,
                NumCard = item.NumCard,
                Status = item.Status,
            });
        }
        return View("ListActives", listViewModel);
    }
}
