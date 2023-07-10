namespace ITExpertTestService.Models.Dto.Requests
{
    public record ObjectInfo
    {
        public int SerialNumber { get; set; }
        public int Code { get; set; }
        public string Value { get; set; }
    }
}
