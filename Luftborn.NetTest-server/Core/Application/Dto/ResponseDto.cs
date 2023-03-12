namespace Core.Application.Dto
{
    public class ResponseDto
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public object? Body { get; set; }
    }
}
