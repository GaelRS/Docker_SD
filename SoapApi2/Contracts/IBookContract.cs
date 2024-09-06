using System.ServiceModel;
using SoapApi2.Dtos;

namespace SoapApi2.Contracts;

[ServiceContract]
public interface IBookContract {
    [OperationContract]
    public Task<BookResponseDto> GetBookById(Guid bookId, CancellationToken cancellationToken);
}