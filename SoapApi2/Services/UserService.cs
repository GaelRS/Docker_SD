using System.ServiceModel;
using SoapApi2.Contracts;
using SoapApi2.Dtos;
using SoapApi2.Mappers;
using SoapApi2.Repositories;

namespace SoapApi2.Services;

public class UserService : IUserContract {
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    public async Task<IList<UserResponseDto>> GetAll(CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        return users.Select(user => user.ToDto()).ToList();
    }

    public async Task<IList<UserResponseDto>> GetAllByEmail(string email, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllByEmailAsync(email, cancellationToken);
        return users.Select(user => user.ToDto()).ToList();
    }   

    public async Task<UserResponseDto> GetUserById(Guid userId, CancellationToken cancellationToken) {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is not null) {
            return user.ToDto();
        }

        throw new FaultException("User not found");
    }

    public async Task<bool> DeleteUserById(Guid userId, CancellationToken cancellationToken) {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null) {
            throw new FaultException("User not found");
        }

        await _userRepository.DeleteByIdAsync(user, cancellationToken);
        return true;
    }

    public async Task<UserResponseDto> CreateUser(UserCreateRequestDto userRequest, CancellationToken cancellationToken)
    {
        var user = userRequest.ToModel();
        var createdUser = await _userRepository.CreateAsync(user, cancellationToken);
        return createdUser.ToDto();
    }

    public async Task<UserResponseDto> UpdateUser(UserUpdateRequestDto userRequest, CancellationToken cancellationToken)
    {
        var user = userRequest.ToModel();
        var updatedUser = await _userRepository.UpdateAsync(user, cancellationToken);
        return updatedUser.ToDto();
    }
}