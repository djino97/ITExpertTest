using ITExpertTestService.Models.Dto.Requests;
using System.Collections.Generic;

namespace ITExpertTestService.Models.Dto
{
    public record FindObjectResponse
    {
        public int TotalCount { get; set; }
        public List<ObjectInfo> Objects { get; set; }
    }
}
