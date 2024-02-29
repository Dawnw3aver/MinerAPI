namespace MinerAPI.APIObjects
{
    public class NewGameRequest
    {
        public int height { get; set; }
        public int mines_count { get; set; }
        public int width { get; set; }
    }
}
