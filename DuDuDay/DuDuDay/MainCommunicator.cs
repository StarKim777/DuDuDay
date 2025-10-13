using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using DuDuDay_Core;

namespace DuDuDay
{
    public class MainCommunicator
    {
        private const string PipeName = "DuDuDayPipe";

        // Sub로 메시지 보내기
        public static void SendMessage(MessagePacket message)
        {
            try
            {
                using (var client = new NamedPipeClientStream(".", PipeName, PipeDirection.Out))
                {
                    client.Connect(1000); // 1초 대기
                    string json = JsonSerializer.Serialize(message);
                    byte[] buffer = Encoding.UTF8.GetBytes(json);
                    client.Write(buffer, 0, buffer.Length);
                }
            }
            catch
            {
                Console.WriteLine("[Main_MainCommunicator] 메세지 전송 오류");
                // Sub 프로그램이 실행 중이 아닐 수도 있음
            }
        }
    }
}
