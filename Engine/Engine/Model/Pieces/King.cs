using System;
using System.Collections.Generic;
namespace Engine.Model.Pieces {
	public class King : Piece {
		public static King CreateKing(string abb) {
			King ret = new King();
			if (abb.Equals("K")) {
				ret.White = true;
			} else if (abb.Equals("k")) {
				ret.White = false;
			} else {
				throw new Exception("Mapping Error!");
			}
			return ret;
		}
		static King() {
			StaticPieces.AddRegistration("K", CreateKing);
		}
		public King() : base("King", "K", true) {
			Royal = true;
		}
		public King(King other) : base(other) {}
		public override PieceStatus CurrentStatus(Board board, in int col, in int row) {
			if (board[col, row] is King && board[col, row].White == White) {
				PieceStatus ret = new PieceStatus {
					attackedEnemyPieces = new List<Tuple<int, int>>(),
					freeMoveSpaces = new List<Tuple<int, int>>(),
					protectedTeammates = new List<Tuple<int, int>>()
				};
				for (sbyte dc = -1; dc <= 1; dc++) {
					for (sbyte dr = -1; dr <= 1; dr++) {
						if (dc != 0 || dr != 0) {
							int targetCol = col + dc;
							int targetRow = row + dr;
							if (board.IsAccessible(targetCol, targetRow)) {
								if (board.IsFree(targetCol, targetRow)) {
									ret.freeMoveSpaces.Add(new Tuple<int, int>(targetCol, targetRow));
								} else {
									if (board[targetCol, targetRow].White == White) {
										ret.protectedTeammates.Add(new Tuple<int, int>(targetCol, targetRow));
									} else {
										ret.attackedEnemyPieces.Add(new Tuple<int, int>(targetCol, targetRow));
									}
								}
							}
						}
					}
				}
				return ret;
			} else {
				throw new Exception((White ? "White" : "Black") + " King-Piece expected!");
			}
		}
	}
}
