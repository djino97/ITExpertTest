namespace ITExpertTestService.Models.Dto.Requests
{
    public record ObjectRequest
    {
        public int Code { get; init; }
        public string Value { get; init; }
    }
}
