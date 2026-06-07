namespace ConOOD_RpgGame.Network.DTO
{
    public class NetworkMessageDto
    {
        public string Type { get; set; } = "";
        public int PlayerId { get; set; }
        public string Action { get; set; } = "";
        public GameStateDto State { get; set; }
        public string Message { get; set; } = "";
    }
}