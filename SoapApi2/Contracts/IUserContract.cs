using System.ServiceModel;
using SoapApi2.Dtos;

namespace SoapApi2.Contracts;

[ServiceContract]
public interface IUserContract {
    [OperationContract]
    public Task<UserResponseDto> GetUserById(Guid userId, CancellationToken cancellationToken);

    [OperationContract]
    public Task<IList<UserResponseDto>> GetAll(CancellationToken cancellationToken);

    [OperationContract]
    public Task<IList<UserResponseDto>> GetAllByEmail(string email, CancellationToken cancellationToken);
}