using System;
using System.Runtime.Serialization;

namespace ChessDF.Exceptions
{
    [Serializable]
    internal class MagicNotFoundException : Exception
    {
        public MagicNotFoundException()
        {
        }

        public MagicNotFoundException(string? message) : base(message)
        {
        }

        public MagicNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected MagicNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}