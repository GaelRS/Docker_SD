using Microsoft.Extensions.Configuration.UserSecrets;
using RestApi.Exceptions;
using RestApi.Models;
using RestApi.Repositories;

namespace RestApi.Services;

public class GroupService : IGroupService
{
  
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;

    public GroupService (IGroupRepository groupRepository, IUserRepository userRepository) {
        _groupRepository = groupRepository;
        _userRepository = userRepository;
    }

    public async Task<GroupUserModel> CreateGroupAsync(string name, Guid[] users, CancellationToken cancellationToken)
    {
        if(users.Length == 0){
            throw new InvalidGroupRequestFormatException();
        }

        //var groups = await _groupRepository.GetByNameAsync(name, 1, 1, "Name", cancellationToken);
        var groups = await _groupRepository.GetByNameAsync2(name, cancellationToken);
        
        if(groups.Any()){
            throw new GroupAlreadyExistsException();
        }
        var usersE = await Task.WhenAll(users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken)));
        if(usersE.Any(user => user is null)){
            throw new UserNotFoundException();
        }

        var group = await _groupRepository.CreateAsync(name, users, cancellationToken);

        return new GroupUserModel {
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreationDate,
            Users = (await Task.WhenAll(
                group.Users.Select(userId => _userRepository.GetByIdAsync(
                    userId, cancellationToken)))).Where(user => user != null)
                    .ToList()
        };

    }

    public async Task DeleteGroupByIdAsync(string id, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByIdAsync(id, cancellationToken);

        if(group is null){
            throw new GroupNotFoundException();
        }

        await _groupRepository.DeleteByIdAsync(id, cancellationToken);
    }

    public async Task<GroupUserModel> GetGroupByIdAsync(string id, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByIdAsync(id, cancellationToken);

        if (group is null) {
           return null;
        }

        return new GroupUserModel {
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreationDate,
            Users = (await Task.WhenAll(
                group.Users.Select(userId => _userRepository.GetByIdAsync(
                    userId, cancellationToken)))).Where(user => user != null)
                    .ToList()
        };
    }

   public async Task<IList<GroupUserModel>> GetGroupByNameAsync(string name,int page, int pageS, string orderBy, CancellationToken cancellationToken)
    {
        var groups = await _groupRepository.GetByNameAsync(name, page, pageS, orderBy,  cancellationToken);
    
        if (groups == null || !groups.Any())
        {
            return new List<GroupUserModel>(); 
        }

        return await Task.WhenAll(groups.Select(async group => new GroupUserModel
        {
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreationDate,
            Users = (await Task.WhenAll(
                group.Users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken))))
                .Where(user => user != null)
                .ToList()
        }));
    }



    //Nuevo método de GetGroupByName2, este funciona para hacer una busqueda exacta sin paginación
    public async Task<IList<GroupUserModel>> GetGroupByNameAsync2(string name, CancellationToken cancellationToken)
    {
        var groups = await _groupRepository.GetByNameAsync2(name, cancellationToken);
    
        if (groups == null || !groups.Any())
        {
            return new List<GroupUserModel>(); 
        }

        return await Task.WhenAll(groups.Select(async group => new GroupUserModel
        {
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreationDate,
            Users = (await Task.WhenAll(
                group.Users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken))))
                .Where(user => user != null)
                .ToList()
        }));
    }

    public async Task UpdateGroupAsync(string id, string name, Guid[] users, CancellationToken cancellationToken)
    {
        if(users.Length == 0){
            throw new InvalidGroupRequestFormatException();
        }

        var group = await _groupRepository.GetByIdAsync(id, cancellationToken);
        if(group is null){
            throw new GroupNotFoundException();
        }

        var usersE = await Task.WhenAll(users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken)));
        if(usersE.Any(user => user is null)){
            throw new UserNotFoundException();
        }

        var groups = await _groupRepository.GetByNameAsync2(name, cancellationToken);
        if(groups.Any(s => s.Id != id)){
            throw new GroupAlreadyExistsException();
        }

        await _groupRepository.UpdateGroupAsync(id, name, users, cancellationToken);
    }
}