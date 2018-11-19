using System;
using System.Collections.Generic;
namespace Engine.Model.Pieces {
	public abstract class SlidingPiece : Piece {
		public SlidingPiece(Piece other) : base(other) {}
		public SlidingPiece(string name, string abb, bool white) : base(name, abb, white) {}
		public abstract Tuple<int, int>[] getDirections();
		public PieceStatus CurrentStatus<P>(Board board, in int col, in int row) where P:SlidingPiece {
			if (board[col, row] is P && board[col, row].White == White) {
				PieceStatus ret = new PieceStatus {
					attackedEnemyPieces = new List<Tuple<int, int>>(),
					freeMoveSpaces = new List<Tuple<int, int>>(),
					protectedTeammates = new List<Tuple<int, int>>(),
					currentPins = new List<PinData>()
				};
				int targetCol, targetRow, targetColProjection, targetRowProjection;
				foreach (Tuple<int, int> direction in getDirections()) {
					for (byte multiplier = 1; multiplier <= board.MaximumMovedistance; multiplier++) {
						targetCol = col + multiplier * direction.Item1;
						targetRow = row + multiplier * direction.Item2;
						if (board.IsAccessible(targetCol, targetRow)) {
							if (board.IsFree(targetCol, targetRow)) {
								ret.freeMoveSpaces.Add(new Tuple<int, int>(targetCol, targetRow));
							} else {
								if (board[targetCol, targetRow].White == White) {
									ret.protectedTeammates.Add(new Tuple<int, int>(targetCol, targetRow));
									break;
								} else {
									ret.attackedEnemyPieces.Add(new Tuple<int, int>(targetCol, targetRow));
									for (byte protrusion = 1; protrusion + multiplier <= board.MaximumMovedistance; protrusion++) {
										targetColProjection = col + (multiplier + protrusion) * direction.Item1;
										targetRowProjection = row + (multiplier + protrusion) * direction.Item2;
										if (board.IsAccessible(targetColProjection, targetRowProjection)) {
											if (board.IsFree(targetColProjection, targetRowProjection)) {
												continue;
											} else {
												if (board[targetColProjection, targetRowProjection].White != White) {
													ret.currentPins.Add(new PinData(board, new Tuple<int, int>(col, row), new Tuple<int, int>(targetCol, targetRow), new Tuple<int, int>(targetColProjection, targetRowProjection)));
													goto nextDirection;
												}
											}
										} else {
											goto nextDirection;
										}
									}
								}
							}
						} else {
							break;
						}
					}
					nextDirection:;
				}
				return ret;
			} else {
				throw new Exception((White?"White ":"Black ")+typeof(P).Name+"-Piece expected!");
			}
		}
	}
}