namespace ProxyCore.Dto
{
    public class ProxyResponseDto
    {
        public string ReturnStr { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public int StatusCode { get; set; }
    }
}
