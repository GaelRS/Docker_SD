using SoapApi2.Models;

namespace SoapApi2.Repositories;

public interface IUserRepository {
    public Task<UserModel> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    public Task<IList<UserModel>> GetAllAsync(CancellationToken cancellationToken);
    public Task<IList<UserModel>> GetAllByEmailAsync(string email, CancellationToken cancellationToken);
    
}