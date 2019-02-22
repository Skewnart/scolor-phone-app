using System;
using System.Net.Sockets;
using System.Text;

namespace SColorRemote.Services
{
    public class UdpListener
    {
        private readonly UdpClient _udpClient = new UdpClient(32000);
        private bool Stop { get; set; }
        public Action<string> Callback { get; set; }

        public UdpListener(Action<string> callback)
        {
            this.Callback = callback;
        }

        public async void StartListening()
        {
            this.Stop = false;

            while (!this.Stop)
            {
                var result = await _udpClient.ReceiveAsync();
                var message = Encoding.ASCII.GetString(result.Buffer);
                if (message.StartsWith("SColor"))  
                    this.Callback(message);
            }
        }

        public void StopListening()
        {
            this.Stop = true;
        }
    }
}
