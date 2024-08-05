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


var png = "1. e4 c6 2. Nc3 d5 3. Qf3 dxe4 4. Nxe4 Nd7 5. Bc4 Ngf6 6. Nxf6+ Nxf6 7. Qg3 Bf5 8. d3 Bg6 9. Ne2 e6 10. Bf4 Nh5 11. Qf3 Nxf4 12. Nxf4 Be7 13. Bxe6 fxe6 14. Nxe6 Qa5+ 15. c3 Qe5+ 16. Qe3 Qxe3+ 17. fxe3 Kd7 18. Nf4 Bd6 19. Nxg6 hxg6 20. h3 Bg3+ 21. Kd2 Raf8 22. Rhf1 Ke7 23. d4 Rxf1 24. Rxf1 Rf8 25. Rxf8 Kxf8 26. e4 Ke7 27. Ke3 g5 28. Kf3 Be1 29. Kg4 Bd2 30. Kf5 Bc1 31. Kg6 Kf8 32. e5 Bxb2 33. Kxg5 Bxc3 34. h4 Bxd4 35. h5 Bxe5 36. g4 Bb2 37. Kf5 Kf7 38. g5 Bc1 39. g6+ Ke7 40. Ke5 b5 41. Kd4 Kd6 42. Kc3 c5 43. a3 Bg5 44. a4 bxa4 45. Kb2 Kd5 46. Ka3 Kd4 47. Kxa4 c4 0-1";

var move = new Move(8,17,MoveFlag.CheckFlag,0);
var move2 = new Move(8,17,MoveFlag.CastleFlag,0);
Console.WriteLine(move.StartSquare);
Console.WriteLine(move.TargetSquare);
Console.WriteLine(move.Flag);
var board = PGNUtitlity.ParsePGN(png);
Console.WriteLine(board.ToString());
