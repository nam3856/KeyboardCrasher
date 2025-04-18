﻿using System;

namespace ShadowGroveGames.GameRichPresenceIntegration.Scripts.Control.IO.Exceptions
{
    public class NamedPipeReadException : Exception
    {
        public int ErrorCode { get; private set; }
        internal NamedPipeReadException(int err) : base("An exception occured while reading from the pipe. Error Code: " + err)
        {
            ErrorCode = err;
        }
    }
}
