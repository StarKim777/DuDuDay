namespace DuDuDay_Core
{
    public class MessagePacket
    {
        public string Command { get; set; } = string.Empty;  // 예: "ShowMainWindow"
        public string? Payload { get; set; }                 // 추가 데이터(JSON 등)
    }
}
