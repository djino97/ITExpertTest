using ITExpertTestService.Models.Dto.Requests;
using System.Collections.Generic;

namespace ITExpertTestService.Models.Dto.NewFolder
{
    public record CreateObjectsRequest
    {
        public IEnumerable<ObjectRequest> Objects { get; init; }
    }
}
