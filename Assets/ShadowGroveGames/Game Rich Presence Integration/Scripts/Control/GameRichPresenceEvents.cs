using DiscordRPC;
using DiscordRPC.Message;
using System;
using UnityEngine.Events;

namespace ShadowGroveGames.GameRichPresenceIntegration.Scripts.Control
{
    [Serializable]
    public class GameRichPresenceEvents
    {
        [Serializable]
        public class ReadyMessageEvent : UnityEvent<ReadyMessage> { }

        [Serializable]
        public class CloseMessageEvent : UnityEvent<CloseMessage> { }

        [Serializable]
        public class ErrorMessageEvent : UnityEvent<ErrorMessage> { }

        [Serializable]
        public class PresenceMessageEvent : UnityEvent<PresenceMessage> { }

        [Serializable]
        public class ConnectionFailedMessageEvent : UnityEvent<ConnectionFailedMessage> { }

        public ReadyMessageEvent OnReady = new ReadyMessageEvent();
        public CloseMessageEvent OnClose = new CloseMessageEvent();
        public ErrorMessageEvent OnError = new ErrorMessageEvent();
        public PresenceMessageEvent OnPresenceUpdate = new PresenceMessageEvent();

        public void RegisterEvents(DiscordRpcClient Client)
        {
            Client.OnReady += (s, args) => OnReady.Invoke(args);
            Client.OnClose += (s, args) => OnClose.Invoke(args);
            Client.OnError += (s, args) => OnError.Invoke(args);

            Client.OnPresenceUpdate += (s, args) => OnPresenceUpdate.Invoke(args);
        }
    }
}