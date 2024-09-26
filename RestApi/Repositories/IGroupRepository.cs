using RestApi.Models;

namespace RestApi.Repositories;
public interface IGroupRepository
{
    Task<GroupModel> GetByIdAsync (string id, CancellationToken cancellationToken);
    Task<IList<GroupModel>> GetByNameAsync (string name, int page, int pageS, string orderBy, CancellationToken cancellationToken);
    
    /*//Nuevo método de GetGroupByName2, este funciona para hacer una busqueda exacta
    Task<IList<GroupModel>> GetByNameAsync2 (string name, int page, int pageS, string orderBy, CancellationToken cancellationToken);*/

    //Nuevo método de GetGroupByName2, este funciona para hacer una busqueda exacta sin paginación
    Task<IList<GroupModel>> GetByNameAsync2 (string name, CancellationToken cancellationToken);

    Task DeleteByIdAsync (string id, CancellationToken cancellationToken);
    Task<GroupModel> CreateAsync (string name, Guid[] users, CancellationToken cancellationToken);
}