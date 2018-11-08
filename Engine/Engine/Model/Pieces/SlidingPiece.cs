using System;
using System.Collections.Generic;
namespace Engine.Model.Pieces {
	public abstract class SlidingPiece : Piece {
		public SlidingPiece(Piece other) : base(other) {}
		public SlidingPiece(string name, string abb, bool white) : base(name, abb, white) {}
		public abstract Tuple<int, int>[] getDirections();
		public List<Tuple<int, int>> ValidMoves<P> (Board board, in int col, in int row) where P : SlidingPiece {
			if (board[col, row] is P && board[col, row].White == White) {
				List<Tuple<int, int>> ret = new List<Tuple<int, int>>();
				int targetCol, targetRow;
				foreach (Tuple<int, int> dir in getDirections()) {
					for (int multiplier = 1; ; multiplier++) {
						targetCol = col + dir.Item1 * multiplier;
						targetRow = row + dir.Item2 * multiplier;
						//Können wir das Feld überhaupt betreten? (Zylinder-Brett)
						if (board.IsAccessible(targetCol, targetRow)) {
							//Ist das Feld frei?
							if (board.IsFree(targetCol, targetRow)) {
								ret.Add(new Tuple<int, int>(targetCol, targetRow));
								//Oder steht auf dem Feld eine gegnerische Figur?
							} else if (board[targetCol, targetRow].White != White) {
								ret.Add(new Tuple<int, int>(targetCol, targetRow));
								break;
							} else {
								break;
							}
						} else {
							break;
						}
					}
				}
				return ret;
			} else {
				throw new Exception(typeof(P).Name+"-Piece of color " + (White ? "white" : "black") + " expected.");
			}
		}
	}
}