using ITExpertTestService.Mappers.Interfaces;
using ITExpertTestService.Models.Db;
using ITExpertTestService.Models.Dto.Requests;

namespace ITExpertTestService.Mappers
{
    public class MapperObjectRequest : IMapperCreateObjectRequest
    {
        public DbObject Map(ObjectRequest objectInfo)
        {
            return new DbObject
            {
                Code = objectInfo.Code,
                Value = objectInfo.Value
            };
        }
    }
}
