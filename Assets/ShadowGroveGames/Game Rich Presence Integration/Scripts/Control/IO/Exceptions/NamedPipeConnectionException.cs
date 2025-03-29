using System;

namespace ShadowGroveGames.GameRichPresenceIntegration.Scripts.Control.IO.Exceptions
{
    public class NamedPipeConnectionException : Exception
    {
        internal NamedPipeConnectionException(string message) : base(message) { }
    }
}
