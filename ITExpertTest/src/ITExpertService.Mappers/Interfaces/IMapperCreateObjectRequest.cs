using ITExpertTestService.Models.Db;
using ITExpertTestService.Models.Dto.Requests;

namespace ITExpertTestService.Mappers.Interfaces
{
    public interface IMapperCreateObjectRequest
    {
        DbObject Map(ObjectRequest objectInfo);
    }
}
