﻿using System;

namespace ShadowGroveGames.GameRichPresenceIntegration.Scripts.Control.IO.Exceptions
{
    public class NamedPipeWriteException : Exception
    {
        public int ErrorCode { get; private set; }
        internal NamedPipeWriteException(int err) : base("An exception occured while reading from the pipe. Error Code: " + err)
        {
            ErrorCode = err;
        }
    }
}
