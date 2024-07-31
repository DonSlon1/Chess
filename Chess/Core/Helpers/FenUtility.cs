using System.Collections.ObjectModel;
using System.Text;
using Chess.Core.Board;

namespace Chess.Core.Helpers;
using Chess.Core.Board;

public class FenUtility
{
    public readonly struct PositionInfo
    {
        public readonly string fen;
        public readonly int[] squaers;
        public readonly bool isWhiteToMove;

        // Castling rights
        public readonly bool whiteCastleKingside;
        public readonly bool whiteCastleQueenside;
        public readonly bool blackCastleKingside;
        public readonly bool blackCastleQueenside;

        // en passant square if avilable
        public readonly int? epFile;
        public readonly int fityMovePlayCount;
        public readonly int moveCount;

        public PositionInfo(string fen)
        {
            this.fen = fen;
            int[] squarePieces = new int[64];
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

            squaers = squarePieces;
            isWhiteToMove = sections[1] == "w";
            var castlingRights = sections[2];
            whiteCastleKingside = castlingRights.Contains('K');
            whiteCastleQueenside = castlingRights.Contains('Q');

            blackCastleKingside = castlingRights.Contains('k');
            blackCastleQueenside = castlingRights.Contains('q');

            if (!sections[3].Contains('-'))
            {
                epFile = BoardUtility.IndexFromName(sections[3]);
            }

            int.TryParse(sections[4], out fityMovePlayCount);
            int.TryParse(sections[5], out moveCount);
        }
    }

    public static string CurrentFen(Board board)
    {
        var fen = new StringBuilder();
        fen.Append(CurrentFenFields(board.Squares));

        fen.Append(board.isWhiteToMove ? " w " : " b ");

        // Castling rights
        string castlingRights = string.Concat(
            board.whiteCastleKingside ? "K" : "",
            board.whiteCastleQueenside ? "Q" : "",
            board.blackCastleKingside ? "k" : "",
            board.blackCastleQueenside ? "q" : ""
        );
        fen.Append(castlingRights.Length > 0 ? castlingRights : "-");

        fen.Append($" {BoardUtility.NameFromIndex(board.epFile)} {board.fityMovePlayCount} {board.moveCount}");

        return fen.ToString();
    }

    private static string CurrentFenFields(int[] squares)
    {
        var fen = new StringBuilder();
        for (int i = 8 - 1; i >= 0; i--)
        {
            int empty = 0;
            for (int j = 0; j < 8; j++)
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
