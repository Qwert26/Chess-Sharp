using System;
using System.Collections.Generic;
namespace Engine.Model.Pieces {
	public class Knight : Piece {
		public static readonly Tuple<int, int>[] condensedDirections = { new Tuple<int, int>(2, 1), new Tuple<int, int>(1, 2) };
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
		public Knight() : base("Knight", "N", true) {
			Value = 300;
		}
		public Knight(Knight other) : base(other) {}
		public override PieceStatus CurrentStatus(Board board, in int col, in int row) {
			if (board[col, row] is Knight && board[col, row].White == White) {
				PieceStatus ret = new PieceStatus {
					attackedEnemyPieces = new List<Tuple<int, int>>(),
					freeMoveSpaces = new List<Tuple<int, int>>(),
					protectedTeammates = new List<Tuple<int, int>>()
				};
				int targetCol, targetRow;
				foreach (Tuple<int, int> dir in condensedDirections) {
					for (sbyte colMult = -1; colMult <= 1; colMult += 2) {
						for (sbyte rowMult = -1; rowMult <= 1; rowMult += 2) {
							targetCol = col + colMult * dir.Item1;
							targetRow = row + rowMult * dir.Item2;
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
				throw new Exception((White ? "White" : "Black") + " Knight-Piece expected!");
			}
		}
	}
}