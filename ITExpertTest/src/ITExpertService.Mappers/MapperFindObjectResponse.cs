using ITExpertTestService.Mappers.Interfaces;
using ITExpertTestService.Models.Db;
using ITExpertTestService.Models.Dto.Requests;

namespace ITExpertTestService.Mappers
{
    public class MapperFindObjectResponse : IMapperFindObjectResponse
    {
        public ObjectInfo Map(DbObject dbObject)
        {
            if (dbObject == null)
                return null;

            return new ObjectInfo
            {
                SerialNumber = dbObject.SerialNumber,
                Code = dbObject.Code,
                Value = dbObject.Value
            };
        }
    }
}
