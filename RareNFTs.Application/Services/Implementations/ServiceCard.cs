using AutoMapper;
using RareNFTs.Application.DTOs;
using RareNFTs.Application.Services.Interfaces;
using RareNFTs.Infraestructure.Models;
using RareNFTs.Infraestructure.Repository.Interfaces;

namespace RareNFTs.Application.Services.Implementations;

public class ServiceCard : IServiceCard
{
    private readonly IRepositoryCard _repository;
    private readonly IMapper _mapper;

    public ServiceCard(IRepositoryCard repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    //This function asynchronously adds a new card to the repository based on the provided CardDTO. It maps the
    //CardDTO to a Card entity, adds it to the repository, and returns the ID of the newly added card.

    public async Task<Guid> AddAsync(CardDTO dto)
    {
        // Map CardDTO to Card
        var objectMapped = _mapper.Map<Card>(dto);

        // Return
        return await _repository.AddAsync(objectMapped);
    }


    //This function asynchronously deletes a card from the repository based on the provided ID.

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

   // This function asynchronously finds cards in the repository by their description.It retrieves a
   // list of cards with matching descriptions, maps them to CardDTO objects, and returns the collection.

    public async Task<ICollection<CardDTO>> FindByDescriptionAsync(string description)
    {
        var list = await _repository.FindByDescriptionAsync(description);
        var collection = _mapper.Map<ICollection<CardDTO>>(list);
        return collection;

    }

    //This function asynchronously finds a card in the repository by its ID. It retrieves the card object, maps it to a CardDTO, and returns the DTO.

    public async Task<CardDTO> FindByIdAsync(Guid id)
    {
        var @object = await _repository.FindByIdAsync(id);
        var objectMapped = _mapper.Map<CardDTO>(@object);
        return objectMapped;
    }

    //This function asynchronously retrieves a list of all cards from the repository. It maps the list of cards to a collection of CardDTO objects and returns it.

    public async Task<ICollection<CardDTO>> ListAsync()
    {
        // Get data from Repository
        var list = await _repository.ListAsync();
        // Map List<Card> to ICollection<CardDTO>
        var collection = _mapper.Map<ICollection<CardDTO>>(list);
        // Return Data
        return collection;
    }

    //This function asynchronously updates a card in the repository based on the provided ID and CardDTO.
    //It retrieves the card object by ID, maps the CardDTO properties to it, and updates it in the repository.

    public async Task UpdateAsync(Guid id, CardDTO dto)
    {
        var @object = await _repository.FindByIdAsync(id);
        //       source, destination
        _mapper.Map(dto, @object!);
        await _repository.UpdateAsync();

    }
}

