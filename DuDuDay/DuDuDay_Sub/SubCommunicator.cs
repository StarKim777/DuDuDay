using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DuDuDay_Core;

namespace DuDuDay_Sub
{
    public class SubCommunicator
    {
        private const string PipeName = "DuDuDayPipe";

        public event Action<MessagePacket>? OnMessageReceived;

        public void StartListening()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    using (var server = new NamedPipeServerStream(PipeName, PipeDirection.In))
                    {
                        server.WaitForConnection();

                        using (var reader = new StreamReader(server, Encoding.UTF8))
                        {
                            string? json = reader.ReadToEnd();
                            if (!string.IsNullOrWhiteSpace(json))
                            {
                                var msg = JsonSerializer.Deserialize<MessagePacket>(json);
                                if (msg != null)
                                    OnMessageReceived?.Invoke(msg);
                            }
                        }
                    }
                }
            });
        }
    }
}
