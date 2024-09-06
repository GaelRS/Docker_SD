using Microsoft.EntityFrameworkCore;
using SoapApi2.Infrastructure;
using SoapApi2.Mappers;
using SoapApi2.Models;

namespace SoapApi2.Repositories;

public class BookRespository : IBookRepository
{
    private readonly RelationalDbContext _dbContext;
    public BookRespository(RelationalDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task<BookModel> GetByIdAsync(Guid bookId, CancellationToken cancellationToken)
    {
        var book = await _dbContext.Books.AsNoTracking().FirstOrDefaultAsync(s => s.Id == bookId, cancellationToken);
        return book.ToModel();
    }
}