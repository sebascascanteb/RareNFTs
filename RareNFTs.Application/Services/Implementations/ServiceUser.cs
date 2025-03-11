using AutoMapper;
using Azure;
using Microsoft.Extensions.Options;
using RareNFTs.Application.Config;
using RareNFTs.Application.DTOs;
using RareNFTs.Application.Services.Interfaces;
using RareNFTs.Application.Utils;
using RareNFTs.Infraestructure.Models;
using RareNFTs.Infraestructure.Repository.Interfaces;
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace RareNFTs.Application.Services.Implementations;

public class ServiceUser : IServiceUser
{
    private readonly IRepositoryUser _repository;
    private readonly IMapper _mapper;
    private readonly IOptions<AppConfig> _options;

    public ServiceUser(IRepositoryUser repository, IMapper mapper, IOptions<AppConfig> options)
    {
        _repository = repository;
        _mapper = mapper;
        _options = options;
    }

//    Purpose: Adds a new user to the repository.
//Process: Encrypts the user's password, maps the DTO to the domain model, and adds it to the repository.
//Returns: The ID (string) of the newly added user.
    public async Task<string> AddAsync(UserDTO dto)
    {
        // Read secret
        string secret = _options.Value.Crypto.Secret;
        //  Get Encrypted password
        string passwordEncrypted = Cryptography.Encrypt(dto.Password!, secret);
        // Set Encrypted password to dto
        dto.Password = passwordEncrypted;


        var objectMapped = _mapper.Map<User>(dto);
        // Return
        return await _repository.AddAsync(objectMapped);
    }


//    Purpose: Deletes a user by ID.
//Process: Invokes the repository's delete method for the specified ID.
//Returns: None(task-based asynchronous operation).
    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }


//    Purpose: Retrieves a list of users matching a specific description.
//Process: Fetches and maps the list of users from the repository that match the description.
//Returns: A collection of UserDTO.
    public async Task<ICollection<UserDTO>> FindByDescriptionAsync(string description)
    {
        var list = await _repository.FindByDescriptionAsync(description);
        var collection = _mapper.Map<ICollection<UserDTO>>(list);
        return collection;
    }


//    Purpose: Fetches a user by their ID.
//Process: Retrieves and maps a user from the repository using the specified ID.
//Returns: A UserDTO representing the user.
    public async Task<UserDTO> FindByIdAsync(Guid id)
    {
        var @object = await _repository.FindByIdAsync(id);
        var objectMapped = _mapper.Map<UserDTO>(@object);
        return objectMapped;
    }


//    Purpose: Lists all users.
//Process: Retrieves and maps all users from the repository.
//Returns: A collection of UserDTO.
    public async Task<ICollection<UserDTO>> ListAsync()
    {
        // Get data from Repository
        var list = await _repository.ListAsync();
        // Map List<*> to ICollection<*>
        var collection = _mapper.Map<ICollection<UserDTO>>(list);
        // Return Data
        return collection;
    }


//    Purpose: Lists all user roles.
//Process: Retrieves and maps all user roles from the repository.
//Returns: A collection of RoleDTO.
    public async Task<ICollection<RoleDTO>> ListRoleAsync()
    {
        // Get data from Repository
        var list = await _repository.ListRoleAsync();
        // Map List<*> to ICollection<*>
        var collection = _mapper.Map<ICollection<RoleDTO>>(list);
        // Return Data
        return collection;
    }


    //Purpose: Authenticates a user login attempt.
    //Process: Encrypts the provided password, checks for a matching user in the repository, and maps the result to a UserDTO.
    //Returns: The UserDTO if authentication is successful, otherwise null.
    public async Task<UserDTO> LoginAsync(string email, string password)
    {
        UserDTO UserDTO = null!;

        // Read secret
        string secret = _options.Value.Crypto.Secret;
        //  Get Encrypted password
        string passwordEncrypted = Cryptography.Encrypt(password, secret);

        var @object = await _repository.LoginAsync(email, passwordEncrypted);

        if (@object != null)
        {
            UserDTO = _mapper.Map<UserDTO>(@object);
        }

        return UserDTO;
    }


//    Purpose: Updates an existing user.
//Process: Retrieves the user by ID, encrypts the new password, maps the DTO onto the existing user object, and updates it in the repository.
//Returns: None (task-based asynchronous operation).
    public async Task UpdateAsync(Guid id, UserDTO dto)
    {
        var @object = await _repository.FindByIdAsync(id);
        //       source, destination


        string secret = _options.Value.Crypto.Secret;
        //  Get Encrypted password
        string passwordEncrypted = Cryptography.Encrypt(dto.Password!, secret);
        // Set Encrypted password to dto
        dto.Password = passwordEncrypted;

        _mapper.Map(dto, @object!);
        await _repository.UpdateAsync();
    }
}
