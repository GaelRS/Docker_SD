using Microsoft.EntityFrameworkCore;
using SoapApi2.Infrastructure.Entities;

namespace SoapApi2.Infrastructure;

public class RelationalDbContext : DbContext {
    public RelationalDbContext(DbContextOptions<RelationalDbContext> options) : base(options) {

    }

    public DbSet<UserEntity> Users {get; set;}

    internal object Where(Func<object, bool> value)
    {
        throw new NotImplementedException();
    }
}