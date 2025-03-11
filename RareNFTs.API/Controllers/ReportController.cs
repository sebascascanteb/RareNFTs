using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using RareNFTs.API.ViewModels;
using RareNFTs.Application.Services.Implementations;
using RareNFTs.Application.Services.Interfaces;
using RareNFTs.Infraestructure.Models;

namespace RareNFTs.API.Controllers;


[ApiController]
[Route("api/[controller]")]
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

    [HttpGet("nft")]
    public async Task<IActionResult> GetNfts()
    {
        var collection = await _serviceNft.ListAsync();
        return Ok(collection);
    }


    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetOwnerByNft(string name)
    {
        // Obtener la lista de ClientNft asociados al nombre del NFT
        var clientNftList = await _serviceClient.FindByNftNameAsync(name);

        // Lista para almacenar la información completa del cliente y del NFT
        var viewModelList = new List<ClientNftViewModel>();

        // Recorrer la lista de ClientNft
        foreach (var clientNft in clientNftList)
        {
            // Obtener la información completa del cliente utilizando el ID de cliente
            var client = await _serviceClient.FindByIdAsync(clientNft.IdClient);

            // Obtener la información completa del NFT utilizando el ID de NFT
            var nft = await _serviceNft.FindByIdAsync(clientNft.IdNft);

            // Crear un nuevo objeto ClientNftViewModel con la información del cliente y del NFT
            var viewModel = new ClientNftViewModel
            {
                Id = client.Id,
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

        return Ok(viewModelList);
    }
}
