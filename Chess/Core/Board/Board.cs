using System.Text;
using Chess.Core.Helpers;

namespace Chess.Core.Board;

public class Board
{
    private const string StartFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    private readonly int[] _squares;

    public int[] Squares
    {
        get => _squares;
        init
        {
            if (value.Length != 64) throw new ArgumentException("Invalid squares count");
            _squares = value;
        }
    }

    private string _fen;
    public string Fen
    {
        get => _fen;
        init => _fen = value ?? throw new ArgumentNullException(nameof(value));
    }

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

    //Moves
    public List<Move> AllGameMoves { get; } = new();

    public Board(): this(StartFen)
    {
    }

    public Board(string fen)
    {
        _fen = fen;
        _squares = new int[64];
        var posInfo = new FenUtility.PositionInfo(fen);
        posInfo.squaers.CopyTo(Squares,0);
        isWhiteToMove = posInfo.isWhiteToMove;
        whiteCastleKingside = posInfo.whiteCastleKingside;
        whiteCastleQueenside = posInfo.whiteCastleQueenside;
        blackCastleKingside = posInfo.blackCastleKingside;
        blackCastleQueenside = posInfo.blackCastleQueenside;
        epFile = posInfo.epFile;
        fityMovePlayCount = posInfo.fityMovePlayCount;
        moveCount = posInfo.moveCount;

    }

    public override string ToString()
    {
        return BoardUtility.CreateDiagram(this);
    }
}
