using System;
using System.Collections.Generic;
namespace Engine.Model.Pieces {
	public class Knight : Piece {
		public static readonly Tuple<int, int>[] condensedTargets = { new Tuple<int, int>(2, 1), new Tuple<int, int>(1, 2) };
		public static Knight CreateKnight(string abb) {
			Knight ret = new Knight();
			if (abb.Equals("N")) {
				ret.White = true;
			} else if (abb.Equals("n")) {
				ret.White = false;
			} else {
				throw new Exception("Mapping Error!");
			}
			return ret;
		}
		static Knight() {
			StaticPieces.AddRegistration("N", CreateKnight);
		}
		public Knight() : base("Knight", "N", true) {}
		public Knight(Knight other) : base(other) {}
		public override List<Tuple<int, int>> ValidMoves(Board board, in int col, in int row) {
			if (board[col, row] is Knight && board[col, row].White == White) {
				List<Tuple<int, int>> ret = new List<Tuple<int, int>>();
				int targetCol, targetRow;
				foreach (Tuple<int, int> ct in condensedTargets) {
					for (int multCol = -1; multCol <= 1; multCol += 2) {
						for (int multRow = -1; multRow <= 1; multRow += 2) {
							targetCol = col + ct.Item1 * multCol;
							targetRow = row + ct.Item2 * multRow;
							if (board.IsAccessible(targetCol, targetRow) && (board.IsFree(targetCol, targetRow) || board[targetCol, targetRow].White != White)) {
								ret.Add(new Tuple<int, int>(targetCol, targetRow));
							}
						}
					}
				}
				return ret;
			} else {
				throw new Exception("Knight-Piece of color " + (White ? "white" : "black") + " expected.");
			}
		}
	}
}