using System;
using System.Runtime.Serialization;

namespace ChessDF.Exceptions
{
    [Serializable]
    internal class UnrecognisedPieceStringException : Exception
    {
        public UnrecognisedPieceStringException()
        {
        }

        public UnrecognisedPieceStringException(string? message) : base(message)
        {
        }

        public UnrecognisedPieceStringException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UnrecognisedPieceStringException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}