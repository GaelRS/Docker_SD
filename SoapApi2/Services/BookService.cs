using System.ServiceModel;
using SoapApi2.Contracts;
using SoapApi2.Dtos;
using SoapApi2.Mappers;
using SoapApi2.Repositories;

namespace SoapApi2.Services;
public class BookService : IBookContract
{
     private readonly IBookRepository _bookRepository;
    public BookService(IBookRepository bookRepository) {
        _bookRepository = bookRepository;
    }
    public async Task<BookResponseDto> GetBookById(Guid bookId, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(bookId, cancellationToken);

        if (book is not null) {
            return book.ToDto();
        }

        throw new FaultException("Book not found");
    }
}
