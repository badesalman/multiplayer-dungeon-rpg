using System.IO;
using System.Text.Json;
using ConOOD_RpgGame.Network.DTO;

namespace ConOOD_RpgGame.Network
{
    public static class JsonProtocol
    {
        private static readonly JsonSerializerOptions Options =
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

        public static void Send(StreamWriter writer, NetworkMessageDto message)
        {
            string json = JsonSerializer.Serialize(message, Options);
            writer.WriteLine(json);
            writer.Flush();
        }

        public static NetworkMessageDto Receive(StreamReader reader)
        {
            try
            {
                string json = reader.ReadLine();

                if (string.IsNullOrWhiteSpace(json))
                {
                    return null;
                }

                return JsonSerializer.Deserialize<NetworkMessageDto>(json, Options);
            }
            catch (IOException)
            {
                return null;
            }
            catch (System.Net.Sockets.SocketException)
            {
                return null;
            }
        }
    }
}