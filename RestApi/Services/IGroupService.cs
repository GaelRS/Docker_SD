using RestApi.Models;


namespace RestApi.Services;

public interface IGroupService {
    Task<GroupUserModel>GetGroupByIdAsync(string id, CancellationToken cancellationToken);
    Task<IList<GroupUserModel>>GetGroupByNameAsync(string name,int page, int pageS, string orderBy, CancellationToken cancellationToken);

    /*//Nuevo método de GetGroupByName2, este funciona para hacer una busqueda exacta
    Task<IList<GroupUserModel>>GetGroupByNameAsync2(string name,int page, int pageS, string orderBy, CancellationToken cancellationToken); */
    
    //Nuevo método de GetGroupByName2, este funciona para hacer una busqueda exacta sin paginación
    Task<IList<GroupUserModel>>GetGroupByNameAsync2(string name, CancellationToken cancellationToken); 
    
    Task DeleteGroupByIdAsync(string id, CancellationToken cancellationToken);
    Task<GroupUserModel> CreateGroupAsync(string name, Guid[] users, CancellationToken cancellationToken);
    Task UpdateGroupAsync(string id, string name, Guid[] users, CancellationToken cancellationToken);

}
