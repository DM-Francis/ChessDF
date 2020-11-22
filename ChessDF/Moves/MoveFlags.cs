using System;

namespace ChessDF.Moves
{
    [Flags]
    public enum MoveFlags
    {
        QuietMove = 0b0000,
        DoublePawnPush = 0b0001,
        KingCastle = 0b0010,
        QueenCastle = 0b0011,
        Capture = 0b0100,
        EnPassantCapture = 0b0101,
        KnightPromotion = 0b1000, // Promotions can be combined with captures
        BishopPromotion = 0b1001,
        RookPromotion = 0b1010,
        QueenPromotion = 0b1011
    }
}
