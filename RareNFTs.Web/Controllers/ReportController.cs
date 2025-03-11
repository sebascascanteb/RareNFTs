using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RareNFTs.Application.Services.Interfaces;
using RareNFTs.Infraestructure.Models;
using RareNFTs.Web.ViewModels;

namespace RareNFTs.Web.Controllers;

[Authorize(Roles = "admin, report")]

public class ReportController : Controller
{
    private readonly IServiceReport _serviceReport;
    private readonly IServiceClient _serviceClient;
    private readonly IServiceNft _serviceNft;
    public ReportController(IServiceReport serviceReport, IServiceClient serviceClient, IServiceNft serviceNft)
    {
        _serviceReport = serviceReport;
        _serviceClient = serviceClient;
        _serviceNft = serviceNft;
    }

    //This function returns the view for the index page.

    public IActionResult Index()
    {
        return View();
    }

    //This function returns the view for the product report page.

    public IActionResult ProductReport()
    {
        return View();
    }

    //This function returns the view for the client report page.

    public IActionResult ClientReport()
    {
        return View();
    }

    //This function returns the view for the owner NFT report page.

    public IActionResult OwnerNftReport()
    {
        return View();
    }

    //This function returns the view for the sales report page.

    public IActionResult SalesReport()
    {
        return View("SalesReport");
    }

    //This function generates and returns a PDF file for the product report.

    [HttpPost]
    [RequireAntiforgeryToken]
    public async Task<FileResult> ProductReportPDF()
    {

        byte[] bytes = await _serviceReport.ProductReport();
        return File(bytes, "text/plain", "ProductReport.pdf");

    }


    //This function generates and returns a PDF file for the client report.

    [HttpPost]
    [RequireAntiforgeryToken]
    public async Task<FileResult> ClientReportPDF()
    {

        byte[] bytes = await _serviceReport.ClientReport();
        return File(bytes, "text/plain", "ClientReport.pdf");


    }

    //This function generates and returns a PDF file for the sales report within the specified date range.

    [HttpPost]

    [RequireAntiforgeryToken]
    public async Task<IActionResult> SalesReportPDF(DateTime startDate, DateTime endDate)
    {
        if (startDate == null && endDate == null)
        {
            ViewBag.Message = "Descripción requerida";
            return View("SalesReport");
        }

        byte[] bytes = await _serviceReport.SalesReport(startDate, endDate);
        return File(bytes, "text/plain", "SalesReport.pdf");
    }


    //This function retrieves client-NFT associations based on the NFT name and returns a partial view with the client-NFT details.

    [RequireAntiforgeryToken]

     public async Task<IActionResult> GetOwnerByNft(string name)
    {
        // Obtener la lista de ClientNft asociados al nombre del NFT
        var clientNftList = await _serviceClient.FindByNftNameAsync(name);

        // Lista para almacenar la información completa del cliente y del NFT
        var viewModelList = new List<ViewModelClientNft>();

        // Recorrer la lista de ClientNft
        foreach (var clientNft in clientNftList)
        {
            // Obtener la información completa del cliente utilizando el ID de cliente
            var client = await _serviceClient.FindByIdAsync(clientNft.IdClient);

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

            // Agregar el objeto ClientNftViewModel a la lista
            viewModelList.Add(viewModel);
        }

        return PartialView("_ClientByNftReport", viewModelList);
    }


}
