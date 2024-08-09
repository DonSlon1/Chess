using System.Text;
using Chess.Core.Helpers;

namespace Chess.Core.Board;

public class Board
{
    private const string StartFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    private readonly byte[] _squares;

    public byte[] Squares
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

    public bool isWhiteToMove;

    // Castling rights
    public bool WhiteCastleKingSide { get; set; }
    public bool WhiteCastleQueenSide { get; set; }
    public bool BlackCastleKingSide { get; set; }
    public bool BlackCastleQueenSide { get; set; }

    // en passant square if avilable
    public readonly int? epFile;
    public readonly int fityMovePlayCount;
    public readonly int moveCount;

    //Game state
    public bool GameOver { get; set; }
    public bool WhiteWon { get; set; }

    //Moves
    public List<Move> AllGameMoves { get; } = new();

    public Board(): this(StartFen)
    {
    }
    public bool IsWhiteSquare(int squareIndex) => (squareIndex / 8 + squareIndex % 8) % 2 == 0;

    public Board(string fen)
    {
        _fen = fen;
        _squares = new byte[64];
        var posInfo = new FenUtility.PositionInfo(fen);
        posInfo.Squares.CopyTo(Squares,0);
        isWhiteToMove = posInfo.IsWhiteToMove;
        WhiteCastleKingSide = posInfo.WhiteCastleKingSide;
        WhiteCastleQueenSide = posInfo.WhiteCastleQueenSide;
        BlackCastleKingSide = posInfo.BlackCastleKingSide;
        BlackCastleQueenSide = posInfo.BlackCastleQueenSide;
        epFile = posInfo.EpFile;
        fityMovePlayCount = posInfo.FiftyMovePlayCount;
        moveCount = posInfo.MoveCount;

    }

    public override string ToString()
    {
        return BoardUtility.CreateDiagram(this);
    }

    public void MakeMove(Move move)
    {
        _squares[move.TargetSquare] = _squares[move.StartSquare];
        _squares[move.StartSquare] = (byte)PieceType.None;
        isWhiteToMove = !isWhiteToMove;
        //check if king moved
        if (move.Piece == Piece.WhiteKing)
        {
            //disable castling
            WhiteCastleKingSide = false;
            WhiteCastleQueenSide = false;
        }
        //check for check
        AllGameMoves.Add(move);
    }
}
