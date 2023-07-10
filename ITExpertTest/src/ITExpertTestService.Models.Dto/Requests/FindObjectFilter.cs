using Microsoft.AspNetCore.Mvc;

namespace ITExpertTestService.Models.Dto.Requests
{
    public record FindObjectFilter
    {
        [FromQuery(Name = "serialNumber")]
        public int? SerialNumber { get; init; }

        [FromQuery(Name = "code")]
        public int? Code { get; init; }

        [FromQuery(Name = "value")]
        public string Value { get; init; }

        [FromQuery(Name = "skipCount")]
        public int SkipCount { get; init; }

        [FromQuery(Name = "takeCount")]
        public int TakeCount { get; init; }
    }
}
