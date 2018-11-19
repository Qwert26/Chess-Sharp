using System;
using System.Collections.Generic;
namespace Engine.Model.Pieces {
	/// <summary>
	/// Modelliert einen weißen bzw. schwarzen Bauern.
	/// </summary>
	public class Pawn : Piece {
		private static Pawn CreatePawn(string abb) {
			Pawn ret = new Pawn();
			if (abb.Equals("P"))
			{
				ret.White = true;
			}
			else if (abb.Equals("p"))
			{
				ret.White = false;
			}
			else {
				throw new Exception("Mapping Error!");
			}
			return ret;
		}
		static Pawn() {
			StaticPieces.AddRegistration("P", CreatePawn);
		}
		public Pawn() : base("Pawn","P",true) {
			Value = 100;
		}
		public Pawn(Pawn other) : base(other) {}
		public override PieceStatus CurrentStatus(Board board, in int col, in int row) {
			if (board[col, row] is Pawn &&board[col,row].White==White) {
				PieceStatus ret = new PieceStatus {
					attackedEnemyPieces = new List<Tuple<int, int>>(),
					freeMoveSpaces = new List<Tuple<int, int>>(),
					protectedTeammates = new List<Tuple<int, int>>()
				};
				int startRank = board.PawnStartRank(White);
				if (White) {
					if (board.IsFree(col, row + 1)) {
						ret.freeMoveSpaces.Add(new Tuple<int, int>(col, row + 1));
						if (row == startRank && board.IsFree(col, row + 2)) {
							ret.freeMoveSpaces.Add(new Tuple<int, int>(col, row + 2));
						}
					}
					if(board.IsAccessible(col+1,row+1)&& !board.IsFree(col+1,row+1)) {
						if (board[col + 1, row + 1].White) {
							ret.protectedTeammates.Add(new Tuple<int, int>(col + 1, row + 1));
						} else {
							ret.attackedEnemyPieces.Add(new Tuple<int, int>(col + 1, row + 1));
						}
					}
					if (board.IsAccessible(col - 1, row + 1) && !board.IsFree(col - 1, row + 1)) {
						if (board[col - 1, row + 1].White) {
							ret.protectedTeammates.Add(new Tuple<int, int>(col - 1, row + 1));
						} else {
							ret.attackedEnemyPieces.Add(new Tuple<int, int>(col - 1, row + 1));
						}
					}
				} else {
					if (board.IsFree(col, row - 1)) {
						ret.freeMoveSpaces.Add(new Tuple<int, int>(col, row - 1));
						if (row == startRank && board.IsFree(col, row - 2)) {
							ret.freeMoveSpaces.Add(new Tuple<int, int>(col, row - 2));
						}
					}
					if (board.IsAccessible(col + 1, row - 1) && !board.IsFree(col + 1, row - 1)) {
						if (board[col + 1, row - 1].White) {
							ret.attackedEnemyPieces.Add(new Tuple<int, int>(col + 1, row - 1));
						} else {
							ret.protectedTeammates.Add(new Tuple<int, int>(col + 1, row - 1));
						}
					}
					if (board.IsAccessible(col - 1, row + 1) && !board.IsFree(col - 1, row - 1)) {
						if (board[col - 1, row - 1].White) {
							ret.attackedEnemyPieces.Add(new Tuple<int, int>(col - 1, row - 1));
						} else {
							ret.protectedTeammates.Add(new Tuple<int, int>(col - 1, row - 1));
						}
					}
				}
				return ret;
			} else {
				throw new Exception((White?"White":"Black")+" Pawn-Piece expected!");
			}
		}
	}
}
