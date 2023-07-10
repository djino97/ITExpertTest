using ITExpertTestService.Models.Db;
using ITExpertTestService.Models.Dto.Requests;

namespace ITExpertTestService.Mappers.Interfaces
{
    public interface IMapperFindObjectResponse
    {
        ObjectInfo Map(DbObject dbObject);
    }
}
