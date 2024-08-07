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
// var board = new Board(fen);
// board.MakeMove(new Move(0,19,MoveFlag.NoFlag,0));
// Console.WriteLine(BoardUtility.NameFromIndex(board.epFile));
// Console.WriteLine(board.ToString());


var png = "1. e4 e6 2. d4 b6 3. a3 Bb7 4. Nc3 Nh6 5. Bxh6 gxh6 6. Be2 Qg5 7. Bg4 h5 8. Nf3 Qg6 9. Nh4 Qg5 10. Bxh5 Qxh4 11. Qf3 Kd8 12. Qxf7 Nc6 13. Qe8# 1-0";

var move = new Move(8,17,MoveFlag.CheckFlag,0);
var move2 = new Move(8,17,MoveFlag.CastleFlag,0);
Console.WriteLine(move.StartSquare);
Console.WriteLine(move.TargetSquare);
Console.WriteLine(move.Flag);
var board = PGNUtitlity.ParsePGN(png);
Console.WriteLine(board.ToString());
