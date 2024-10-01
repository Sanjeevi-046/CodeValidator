namespace CodeNest.DTO.Models
{
    public class ValidationDto
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }

        public JsonDto? jsonDto { get; set; }

        public string? objectID { get; set; }
    }
}
