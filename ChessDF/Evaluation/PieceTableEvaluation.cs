﻿using ChessDF.Core;
using System;
using System.Linq;

namespace ChessDF.Evaluation
{
    internal class PieceTableEvaluation : IEvaluator
    {
        public double Evaluate(Position position)
        {
            double materialScore = (position.Board.WhiteKing.PopCount() - position.Board.BlackKing.PopCount()) * 200
                + (position.Board.WhiteQueens.PopCount() - position.Board.BlackQueens.PopCount()) * 9
                + (position.Board.WhiteRooks.PopCount() - position.Board.BlackRooks.PopCount()) * 5
                + (position.Board.WhiteBishops.PopCount() - position.Board.BlackBishops.PopCount()) * 3.3
                + (position.Board.WhiteKnights.PopCount() - position.Board.BlackKnights.PopCount()) * 3.2
                + (position.Board.WhitePawns.PopCount() - position.Board.BlackPawns.PopCount()) * 1;

            double pawnPositionScore = PiecePositionScore(position.Board.WhitePawns, PawnTableWhite) - PiecePositionScore(position.Board.BlackPawns, PawnTableBlack);
            double knightPositionScore = PiecePositionScore(position.Board.WhiteKnights, KnightTableWhite) - PiecePositionScore(position.Board.BlackKnights, KnightTableBlack);
            double bishopPositionScore = PiecePositionScore(position.Board.WhiteBishops, BishopTableWhite) - PiecePositionScore(position.Board.BlackBishops, BishopTableBlack);
            double rookPositionScore = PiecePositionScore(position.Board.WhiteRooks, RookTableWhite) - PiecePositionScore(position.Board.BlackRooks, RookTableBlack);
            double queenPositionScore = PiecePositionScore(position.Board.WhiteQueens, QueenTableWhite) - PiecePositionScore(position.Board.BlackQueens, QueenTableBlack);

            bool isEndgame = IsEndGame(position);
            var kingTableWhite = isEndgame ? KingTableWhiteEnd : KingTableWhiteMid;
            var kingTableBlack = isEndgame ? KingTableBlackEnd : KingTableBlackMid;
            double kingPositionScore = PiecePositionScore(position.Board.WhiteKing, kingTableWhite) - PiecePositionScore(position.Board.BlackKing, kingTableBlack);

            double positionScore = pawnPositionScore + knightPositionScore + bishopPositionScore + rookPositionScore + queenPositionScore + kingPositionScore;

            double total = materialScore + positionScore;
            return position.SideToMove == Side.White ? total : -total;
        }

        private static double PiecePositionScore(Bitboard positions, double[] scoreTable)
        {
            var pieces = positions.Serialize();
            double score = 0;
            for (int i = 0; i < pieces.Length; i++)
            {
                score += scoreTable[pieces[i]] / 100;
            }

            return score;
        }

        private static bool IsEndGame(Position position)
        {
            return position.Board[Side.White, Piece.Queen] == 0
                && position.Board[Side.Black, Piece.Queen] == 0;
        }

        private static double[] CopyAndReverse(double[] scoreTable) => scoreTable.Reverse().ToArray();

        // Values for these table are taken from https://www.chessprogramming.org/Simplified_Evaluation_Function
        private static readonly double[] PawnTableBlack = new double[]
        {
            0,  0,  0,  0,  0,  0,  0,  0,
            50, 50, 50, 50, 50, 50, 50, 50,
            10, 10, 20, 30, 30, 20, 10, 10,
             5,  5, 10, 25, 25, 10,  5,  5,
             0,  0,  0, 20, 20,  0,  0,  0,
             5, -5,-10,  0,  0,-10, -5,  5,
             5, 10, 10,-20,-20, 10, 10,  5,
             0,  0,  0,  0,  0,  0,  0,  0
        };

        private static readonly double[] PawnTableWhite = CopyAndReverse(PawnTableBlack);

        private static readonly double[] KnightTableBlack = new double[]
        {
            -50,-40,-30,-30,-30,-30,-40,-50,
            -40,-20,  0,  0,  0,  0,-20,-40,
            -30,  0, 10, 15, 15, 10,  0,-30,
            -30,  5, 15, 20, 20, 15,  5,-30,
            -30,  0, 15, 20, 20, 15,  0,-30,
            -30,  5, 10, 15, 15, 10,  5,-30,
            -40,-20,  0,  5,  5,  0,-20,-40,
            -50,-40,-30,-30,-30,-30,-40,-50,
        };

        private static readonly double[] KnightTableWhite = CopyAndReverse(KnightTableBlack);

        private static readonly double[] BishopTableBlack = new double[]
        {
            -20,-10,-10,-10,-10,-10,-10,-20,
            -10,  0,  0,  0,  0,  0,  0,-10,
            -10,  0,  5, 10, 10,  5,  0,-10,
            -10,  5,  5, 10, 10,  5,  5,-10,
            -10,  0, 10, 10, 10, 10,  0,-10,
            -10, 10, 10, 10, 10, 10, 10,-10,
            -10,  5,  0,  0,  0,  0,  5,-10,
            -20,-10,-10,-10,-10,-10,-10,-20,
        };

        private static readonly double[] BishopTableWhite = CopyAndReverse(BishopTableBlack);

        private static readonly double[] RookTableBlack = new double[]
        {
             0,  0,  0,  0,  0,  0,  0,  0,
             5, 10, 10, 10, 10, 10, 10,  5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
             0,  0,  0,  5,  5,  0,  0,  0
        };

        private static readonly double[] RookTableWhite = CopyAndReverse(RookTableBlack);

        private static readonly double[] QueenTableBlack = new double[]
        {
            -20,-10,-10, -5, -5,-10,-10,-20,
            -10,  0,  0,  0,  0,  0,  0,-10,
            -10,  0,  5,  5,  5,  5,  0,-10,
             -5,  0,  5,  5,  5,  5,  0, -5,
              0,  0,  5,  5,  5,  5,  0, -5,
            -10,  5,  5,  5,  5,  5,  0,-10,
            -10,  0,  5,  0,  0,  0,  0,-10,
            -20,-10,-10, -5, -5,-10,-10,-20
        };

        private static readonly double[] QueenTableWhite = CopyAndReverse(QueenTableBlack);

        private static readonly double[] KingTableBlackMid = new double[]
        {
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -20,-30,-30,-40,-40,-30,-30,-20,
            -10,-20,-20,-20,-20,-20,-20,-10,
             20, 20,  0,  0,  0,  0, 20, 20,
             20, 30, 10,  0,  0, 10, 30, 20
        };

        private static readonly double[] KingTableWhiteMid = CopyAndReverse(KingTableBlackMid);

        private static readonly double[] KingTableBlackEnd = new double[]
        {
            -50,-40,-30,-20,-20,-30,-40,-50,
            -30,-20,-10,  0,  0,-10,-20,-30,
            -30,-10, 20, 30, 30, 20,-10,-30,
            -30,-10, 30, 40, 40, 30,-10,-30,
            -30,-10, 30, 40, 40, 30,-10,-30,
            -30,-10, 20, 30, 30, 20,-10,-30,
            -30,-30,  0,  0,  0,  0,-30,-30,
            -50,-30,-30,-30,-30,-30,-30,-50
        };

        private static readonly double[] KingTableWhiteEnd = CopyAndReverse(KingTableBlackEnd);
    }
}
