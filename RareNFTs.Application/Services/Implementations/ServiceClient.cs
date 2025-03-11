using AutoMapper;
using RareNFTs.Application.DTOs;
using RareNFTs.Application.Services.Interfaces;
using RareNFTs.Infraestructure.Models;
using RareNFTs.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Application.Services.Implementations;

public class ServiceClient: IServiceClient
{
    private readonly IRepositoryClient _repository;
    private readonly IMapper _mapper;

    public ServiceClient(IRepositoryClient repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    //This asynchronous function adds a new client entity to the repository after mapping it from a ClientDTO object. It returns the ID of the added client.

    public async Task<Guid> AddAsync(ClientDTO dto)
    {
        // Map ClientDTO to Client
        var objectMapped = _mapper.Map<Client>(dto);

        // Return
        return await _repository.AddAsync(objectMapped);
    }

    //This asynchronous function deletes a client entity from the repository based on its ID.

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }


    //This asynchronous function retrieves a collection of client entities from the repository based on a partial match of their description field, after mapping them from ClientDTO objects.


    public async Task<ICollection<ClientDTO>> FindByDescriptionAsync(string description)
    {
        var list = await _repository.FindByDescriptionAsync(description);
        var collection = _mapper.Map<ICollection<ClientDTO>>(list);
        return collection;

    }


    //This asynchronous function retrieves a client entity from the repository based on its ID, after mapping it from a ClientDTO object.

    public async Task<ClientDTO> FindByIdAsync(Guid id)
    {
        var @object = await _repository.FindByIdAsync(id);
        var objectMapped = _mapper.Map<ClientDTO>(@object);
        return objectMapped;
    }


    //This asynchronous function retrieves a collection of client-NFT relationships from the repository based on the name of the NFT, after mapping them from ClientNftDTO objects.

    public async Task<IEnumerable<ClientNftDTO>> FindByNftNameAsync(string name)
    {
        // Get data from Repository
        var list = await _repository.FindByNftNameAsync(name);
        // Map List<Bodega> to ICollection<BodegaDTO>
        var collection = _mapper.Map<ICollection<ClientNftDTO>>(list);
        // Return Data
        return collection;
    }


    //This asynchronous function retrieves a collection of all client entities from the repository, after mapping them from ClientDTO objects.

    public async Task<ICollection<ClientDTO>> ListAsync()
    {
        // Get data from Repository
        var list = await _repository.ListAsync();
        // Map List<Client> to ICollection<ClientDTO>
        var collection = _mapper.Map<ICollection<ClientDTO>>(list);
        // Return Data
        return collection;
    }

    //This asynchronous function retrieves a collection of client entities who are owners of NFTs from the repository, after mapping them from ClientDTO objects.

    public async Task<ICollection<ClientDTO>> ListOwnersAsync()
    {
        // Get data from Repository
        var list = await _repository.ListOwnersAsync();
        // Map List<Client> to ICollection<ClientDTO>
        var collection = _mapper.Map<ICollection<ClientDTO>>(list);
        // Return Data
        return collection;
    }


    //This asynchronous function updates an existing client entity in the repository based on its ID, using data provided in a ClientDTO object.

    public async Task UpdateAsync(Guid id, ClientDTO dto)
    {
        var @object = await _repository.FindByIdAsync(id);
        //       source, destination
        _mapper.Map(dto, @object!);
        await _repository.UpdateAsync();

    }

   
}
