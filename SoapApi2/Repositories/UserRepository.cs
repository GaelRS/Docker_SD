using Microsoft.EntityFrameworkCore;
using SoapApi2.Infrastructure;
using SoapApi2.Models;
using SoapApi2.Mappers;
using SoapApi2.Dtos;
using System.ServiceModel;

namespace SoapApi2.Repositories;

public class UserRespository : IUserRepository {
    private readonly RelationalDbContext _dbContext;
    public UserRespository(RelationalDbContext dbContext) {
        _dbContext = dbContext;
    }
    public async Task<UserModel> GetByIdAsync(Guid id, CancellationToken cancellationToken) {
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        return user.ToModel();
    }

    public async Task<IList<UserModel>> GetAllAsync(CancellationToken cancellationToken) {
        var users = await _dbContext.Users.AsNoTracking().ToListAsync(cancellationToken);
        return users.Select(user => user.ToModel()).ToList();
    }
    
    public async Task<IList<UserModel>> GetAllByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var users = await _dbContext.Users.AsNoTracking().ToListAsync(cancellationToken);
        return users .Where(users => users.Email.Contains(email, StringComparison.Ordinal)).Select(users => users.ToModel()).ToList();
        /*Nota para recordar:
        Tambien funciona con StringComparison.OrdinalIgnoreCase, pero no diferencia mayus y minus */
    }

    public async Task DeleteByIdAsync(UserModel user, CancellationToken cancellationToken)
    {
        var userEntity = user.ToEntity();
        _dbContext.Users.Remove(userEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);

    }

   public async Task<UserModel> CreateAsync(UserModel user, CancellationToken cancellationToken)
    {
        var userEntity = user.ToEntity();
        userEntity.Id = Guid.NewGuid();
        
        await _dbContext.AddAsync(userEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return userEntity.ToModel();
    }

    public async Task<UserModel> UpdateAsync(UserModel user, CancellationToken cancellationToken)
    {
        var usere = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken);
        if (usere is null) {
            throw new FaultException("User not found");
        }

        usere.FirstName = user.FirstName;
        usere.LastName = user.LastName;
        usere.Birthday = user.BirthDate;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return usere.ToModel();

    }
}