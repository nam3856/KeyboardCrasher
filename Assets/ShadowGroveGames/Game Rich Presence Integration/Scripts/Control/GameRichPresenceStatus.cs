using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using ShadowGroveGames.GameRichPresenceIntegration.Scripts.Extensions;

namespace ShadowGroveGames.GameRichPresenceIntegration.Scripts.Control
{
    public class GameRichPresenceStatus
    {
        private const int DISCORD_RPC_PORT_START = 6463;
        private const int DISCORD_RPC_PORT_END = 6472;

        public static bool IsActive()
        {
#if PLATFORM_STANDALONE_OSX || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            // GetActiveTcpListeners is not supported on osx
            var portTestClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            for (int port = DISCORD_RPC_PORT_START; port <= DISCORD_RPC_PORT_END; port++)
            {
                portTestClient.Connect("127.0.0.1", port, TimeSpan.FromMilliseconds(100));
                if (!portTestClient.Connected)
                    continue;

                portTestClient.Close();
                return true;
            }

            return false;
#else
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();
            return ipEndPoints.Any(portStatus => portStatus.Port >= DISCORD_RPC_PORT_START && portStatus.Port <= DISCORD_RPC_PORT_END);
#endif
        }
    }
}