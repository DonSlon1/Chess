// See https://aka.ms/new-console-template for more information

using Chess.Core.Board;
using Chess.Core.Helpers;

Console.WriteLine("Black king");
var black = Piece.MakePiece(PieceType.King, PieceColor.Black);
Console.WriteLine(Piece.GetColor(black));
Console.WriteLine(Piece.GetType
    (black));
Console.WriteLine(Piece.GetPieceSymbol(black));
Console.WriteLine("Whith king");
var whiteKing = Piece.MakePiece(PieceType.King, PieceColor.White);
Console.WriteLine(whiteKing);
Console.WriteLine(Piece.GetColor(whiteKing));
Console.WriteLine(Piece.GetType
    (whiteKing));


var fen = "Knbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
var board = new Board(fen);
Console.WriteLine(BoardUtility.NameFromIndex(board.epFile));
Console.WriteLine(board.ToString());
