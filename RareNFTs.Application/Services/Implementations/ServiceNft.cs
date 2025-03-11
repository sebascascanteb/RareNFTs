using AutoMapper;
using Azure;
using RareNFTs.Application.DTOs;
using RareNFTs.Application.Services.Interfaces;
using RareNFTs.Infraestructure.Models;
using RareNFTs.Infraestructure.Repository.Interfaces;
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RareNFTs.Application.Services.Implementations;

public  class ServiceNft: IServiceNft
{
    private readonly IRepositoryNft _repository;
    private readonly IMapper _mapper;

    public ServiceNft(IRepositoryNft repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }


//    Purpose: Adds a new NFT to the repository.
//Process: Maps the NftDTO to the Nft domain model and adds it to the repository.
//Returns: The ID (Guid) of the newly added NFT.
    public async Task<Guid> AddAsync(NftDTO dto)
    {
        // Map ClientDTO to Client
        var objectMapped = _mapper.Map<Nft>(dto);

        // Return
        return await _repository.AddAsync(objectMapped);
    }


//    Purpose: Changes the owner of an NFT.
//Process: Calls the repository method to update the owner of the specified NFT.
//Returns: true if the operation was successful, false otherwise.
    public async Task<bool> ChangeNFTOwnerAsync(Guid nftId, Guid clientId)
    {
        var @object = await _repository.ChangeNFTOwnerAsync(nftId, clientId);
        return @object;
    }


//    Purpose: Deletes an NFT by ID.
//Process: Invokes the repository's delete method for the specified ID.
//Returns: None(task-based asynchronous operation).
    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }


//    Purpose: Retrieves a list of NFTs matching a specific description.
//Process: Fetches and maps the list of NFTs from the repository that match the description.
//Returns: A collection of NftDTO.
    public async Task<ICollection<NftDTO>> FindByDescriptionAsync(string description)
    {
        var list = await _repository.FindByDescriptionAsync(description);

        var collection = _mapper.Map<ICollection<NftDTO>>(list);

        return collection;

    }


//    Purpose: Fetches an NFT by its ID.
//Process: Retrieves and maps an NFT from the repository using the specified ID.
//Returns: An NftDTO representing the NFT.
    public async Task<NftDTO> FindByIdAsync(Guid id)
    {
        var @object = await _repository.FindByIdAsync(id);
        var objectMapped = _mapper.Map<NftDTO>(@object);
        return objectMapped;
    }

//    Purpose: Gets details of a client's NFT by NFT ID.
//Process: Retrieves and maps client-NFT relation data from the repository.
//Returns: A ClientNftDTO with details about the NFT and its owner.
    public async Task<ClientNftDTO> FindClientNftByIdAsync(Guid id)
    {
        var @object = await _repository.FindClientNftByIdAsync(id);
        var objectMapped = _mapper.Map<ClientNftDTO>(@object);
        return objectMapped;
    }

//    Purpose: Lists all NFTs.
//Process: Retrieves and maps all NFTs from the repository.
//Returns: A collection of NftDTO.
    public async Task<ICollection<NftDTO>> ListAsync()
    {
        // Get data from Repository
        var list = await _repository.ListAsync();
        // Map List<Bodega> to ICollection<BodegaDTO>
        var collection = _mapper.Map<ICollection<NftDTO>>(list);
        // Return Data
        return collection;
    }

//    Purpose: Lists all NFTs along with their ownership details.
//Process: Retrieves and maps NFT ownership data from the repository.
//Returns: A collection of ClientNftDTO.

    public async Task<ICollection<ClientNftDTO>> ListOwnedAsync()
    {
        var list = await _repository.ListOwnedAsync();

        var collection = _mapper.Map<ICollection<ClientNftDTO>>(list);

        return collection;
    }

//    Purpose: Updates an existing NFT.
//Process: Fetches an NFT, maps the new data from NftDTO onto it, and updates it in the repository.
//Returns: None (task-based asynchronous operation).
    public async Task UpdateAsync(Guid id, NftDTO dto)
    {
        var @object = await _repository.FindByIdAsync(id);
        //       source, destination
        _mapper.Map(dto, @object!);
        await _repository.UpdateAsync();
    }
}
