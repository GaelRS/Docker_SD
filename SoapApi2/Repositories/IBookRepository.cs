using SoapApi2.Models;

namespace SoapApi2.Repositories;

public interface IBookRepository {
    public Task<BookModel> GetByIdAsync(Guid bookId, CancellationToken cancellationToken);
  
}