namespace MinerAPI.APIObjects
{
    public class ErrorResponse(string error)
    {
        public string Error { get; set; } = error;
    }
}
