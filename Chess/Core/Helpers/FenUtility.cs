using System.Collections.ObjectModel;
using System.Text;
using Chess.Core.Board;

namespace Chess.Core.Helpers;
using Chess.Core.Board;

public static class FenUtility
{
    public readonly struct PositionInfo
    {
        public string Fen { get; private init; }
        public byte[] Squares { get; private init; }
        public readonly bool IsWhiteToMove;

        // Castling rights
        public bool WhiteCastleKingSide { get; private init; }
        public bool WhiteCastleQueenSide { get; private init; }
        public bool BlackCastleKingSide { get; private init; }
        public bool BlackCastleQueenSide { get; private init; }

        // en passant square if avilable
        public int? EpFile { get; private init; }
        private readonly int _fiftyMovePlayCount;
        public int FiftyMovePlayCount => _fiftyMovePlayCount;
        private readonly int _moveCount;
        public int MoveCount => _moveCount;

        public PositionInfo(string fen)
        {
            Fen = fen;
            byte[] squarePieces = new byte[64];
            string[] sections = fen.Split(' ');

            int file = 0;
            int rank = 7;
            foreach (var symbol in sections[0])
            {
                if (symbol == '/')
                {
                    file = 0;
                    rank--;
                    continue;
                }

                if (char.IsDigit(symbol))
                {
                    file += (int)char.GetNumericValue(symbol);
                    continue;
                }

                squarePieces[rank * 8 + file] = Piece.GetPieceFromSymbol(symbol);
                file++;
            }

            Squares = squarePieces;
            IsWhiteToMove = sections[1] == "w";
            var castlingRights = sections[2];
            WhiteCastleKingSide = castlingRights.Contains('K');
            WhiteCastleQueenSide = castlingRights.Contains('Q');

            BlackCastleKingSide = castlingRights.Contains('k');
            BlackCastleQueenSide = castlingRights.Contains('q');

            if (!sections[3].Contains('-'))
            {
                EpFile = BoardUtility.IndexFromName(sections[3]);
            }

            int.TryParse(sections[4], out _fiftyMovePlayCount);
            int.TryParse(sections[5], out _moveCount);
        }
    }

    public static string CurrentFen(Board board)
    {
        var fen = new StringBuilder();
        fen.Append(CurrentFenFields(board.Squares));

        fen.Append(board.isWhiteToMove ? " w " : " b ");

        // Castling rights
        string castlingRights = string.Concat(
            board.WhiteCastleKingSide ? "K" : "",
            board.WhiteCastleQueenSide ? "Q" : "",
            board.BlackCastleKingSide ? "k" : "",
            board.BlackCastleQueenSide ? "q" : ""
        );
        fen.Append(castlingRights.Length > 0 ? castlingRights : "-");

        fen.Append($" {BoardUtility.NameFromIndex(board.epFile)} {board.fityMovePlayCount} {board.moveCount}");

        return fen.ToString();
    }

    private static string CurrentFenFields(byte[] squares)
    {
        StringBuilder fen = new StringBuilder();
        for (var i = 8 - 1; i >= 0; i--)
        {
            var empty = 0;
            for (var j = 0; j < 8; j++)
            {
                if (squares[i * 8 + j] == (int) PieceType.None)
                {
                    empty++;
                    continue;
                }
                if (empty > 0)
                {
                    fen.Append(empty);
                    empty = 0;
                }

                fen.Append(Piece.GetPieceSymbol(squares[i * 8 + j]));
            }

            if (empty > 0)
            {
                fen.Append(empty);
            }

            if (i != 0)
            {
                fen.Append("/");
            }
        }

        return fen.ToString();
    }
}
