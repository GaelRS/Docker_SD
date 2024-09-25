using MongoDB.Bson;
using MongoDB.Driver;
using RestApi.Infrastructure.Mongo;
using RestApi.Mappers;
using RestApi.Models;

namespace RestApi.Repositories;
public class GroupRepository : IGroupRepository
{private readonly IMongoCollection<GroupEntity> _groups;

    public GroupRepository(IMongoClient mongoClient, IConfiguration configuration) {
        var database = mongoClient.GetDatabase(configuration.GetValue<string>("MongoDb:Groups:DatabaseName"));
        _groups = database.GetCollection<GroupEntity>(configuration.GetValue<string>("MongoDb:Groups:CollectionName"));
    }

    public async Task<GroupModel> CreateAsync(string name, Guid[] users, CancellationToken cancellationToken)
    {
        var group = new GroupEntity {
            Name = name,
            Users = users,
            CreatedAt = DateTime.UtcNow,
            Id = ObjectId.GenerateNewId().ToString()
        };

        await _groups.InsertOneAsync(group, new InsertOneOptions(), cancellationToken);
        return group.ToModel();
    }

    public async Task DeleteByIdAsync(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<GroupEntity>.Filter.Eq(s => s.Id, id);
        await _groups.DeleteOneAsync(filter, cancellationToken);
    }

    public async Task<GroupModel> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        try {
            var filter = Builders<GroupEntity>.Filter.Eq(x => x.Id, id);
            var group = await _groups.Find(filter).FirstOrDefaultAsync(cancellationToken);

            return group.ToModel();
        } catch(FormatException) {
            return null;
        }
    }
   /* public async Task<IList<GroupModel>> GetByNameAsync(string name, int page, int pageS, string orderBy, CancellationToken cancellationToken){
        var filter = Builders<GroupEntity>.Filter.Regex(group => group.Name, new BsonRegularExpression(name, "i"));
        
        var sort = Builders<GroupEntity>.Sort.Ascending(n => n.Name);

        if (orderBy != "Name") {
            sort = Builders<GroupEntity>.Sort.Descending(n => n.CreatedAt);
        }

        var groupsOr = _groups.Find(filter).Sort(sort).Skip((page - 1) * pageS).Limit(pageS);

        var groups = await groupsOr.ToListAsync(cancellationToken);
        return groups.Select(group => group.ToModel()).ToList();
    }*/

    //Nuevo método de GetGroupByName2, este funciona para hacer una busqueda exacta

   /* public async Task<IList<GroupModel>> GetByNameAsync2(string name, int page, int pageS, string orderBy, CancellationToken cancellationToken)
    {
        var filter = Builders<GroupEntity>.Filter.Eq(x => x.Name, name);
        //NOTA para recordar: Si se quiere buscar sin distinguir mayúsculas y minúsculas se puede usar el siguiente código
        //var filter = Builders<GroupEntity>.Filter.Eq(x => x.Name.ToLower(), name.ToLower());
        
        var sort = Builders<GroupEntity>.Sort.Ascending(n => n.Name);

        if (orderBy != "Name") {
            sort = Builders<GroupEntity>.Sort.Descending(n => n.CreatedAt);
        }

        var groupsOr = _groups.Find(filter).Sort(sort).Skip((page - 1) * pageS).Limit(pageS);

        var groups = await groupsOr.ToListAsync(cancellationToken);
        return groups.Select(group => group.ToModel()).ToList();
    }*/

    //Nuevo método de GetGroupByName2, este funciona para hacer una busqueda exacta sin paginación

   public async Task<IList<GroupModel>> GetByNameAsync2(string name, CancellationToken cancellationToken)
    {
        var filter = Builders<GroupEntity>.Filter.Eq(x => x.Name, name);
        //NOTA para recordar: Si se quiere buscar sin distinguir mayúsculas y minúsculas se puede usar el siguiente código
        //var filter = Builders<GroupEntity>.Filter.Eq(x => x.Name.ToLower(), name.ToLower());
        
        var groups = await _groups.Find(filter).ToListAsync(cancellationToken);
        return groups.Select(group => group.ToModel()).ToList();
    }
}
