using System;
using System.Runtime.Serialization;

namespace ChessDF.Exceptions
{
    [Serializable]
    internal class PieceNotFoundException : Exception
    {
        public PieceNotFoundException()
        {
        }

        public PieceNotFoundException(string? message) : base(message)
        {
        }

        public PieceNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PieceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}