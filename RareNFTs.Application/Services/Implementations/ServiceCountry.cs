using AutoMapper;
using RareNFTs.Application.DTOs;
using RareNFTs.Application.Services.Interfaces;
using RareNFTs.Infraestructure.Models;
using RareNFTs.Infraestructure.Repository.Interfaces;

namespace RareNFTs.Application.Services.Implementations;

public class ServiceCountry : IServiceCountry
{
    private readonly IRepositoryCountry _repository;
    private readonly IMapper _mapper;

    public ServiceCountry(IRepositoryCountry repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    //This asynchronous function maps a CountryDTO object to a Country entity and adds it to the repository. It returns the ID of the added country.

    public async Task<string> AddAsync(CountryDTO dto)
    {
        // Map CountryDTO to Country
        var objectMapped = _mapper.Map<Country>(dto);

        // Return
        return await _repository.AddAsync(objectMapped);
    }


    //This asynchronous function deletes a country entity from the repository based on its ID.

    public async Task DeleteAsync(string id)
    {
        await _repository.DeleteAsync(id);
    }


    //This asynchronous function retrieves a collection of country entities from the repository based on
    //a partial match of their description field, after mapping them from CountryDTO objects.

    public async Task<ICollection<CountryDTO>> FindByDescriptionAsync(string description)
    {
        var list = await _repository.FindByDescriptionAsync(description);
        var collection = _mapper.Map<ICollection<CountryDTO>>(list);
        return collection;

    }

    //This asynchronous function retrieves a country entity from the repository based on its ID, after mapping it from a CountryDTO object.

    public async Task<CountryDTO> FindByIdAsync(string id)
    {
        var @object = await _repository.FindByIdAsync(id);
        var objectMapped = _mapper.Map<CountryDTO>(@object);
        return objectMapped;
    }

    //This asynchronous function retrieves a collection of all country entities from the repository, after mapping them from CountryDTO objects.

    public async Task<ICollection<CountryDTO>> ListAsync()
    {
        // Get data from Repository
        var list = await _repository.ListAsync();
        // Map List<Country> to ICollection<CountryDTO>
        var collection = _mapper.Map<ICollection<CountryDTO>>(list);
        // Return Data
        return collection;
    }

    //This asynchronous function updates an existing country entity in the repository based on its ID, using data provided in a CountryDTO object.

    public async Task UpdateAsync(string id, CountryDTO dto)
    {
        var @object = await _repository.FindByIdAsync(id);
        //       source, destination
        _mapper.Map(dto, @object!);
        await _repository.UpdateAsync();

    }
}

