using System;
using System.Collections.Generic;
namespace Engine.Model.Pieces {
	public abstract class SlidingPiece : Piece, IPinCreator {
		public SlidingPiece(Piece other) : base(other) {}
		public SlidingPiece(string name, string abb, bool white) : base(name, abb, white) {}
		public abstract Tuple<int, int>[] getDirections();
		public List<Tuple<int, int>> ValidMoves<P> (Board board, in int col, in int row) where P : SlidingPiece {
			if (board[col, row] is P && board[col, row].White == White) {
				List<Tuple<int, int>> ret = new List<Tuple<int, int>>();
				int targetCol, targetRow;
				foreach (Tuple<int, int> dir in getDirections()) {
					for (int multiplier = 1; multiplier<=board.MaximumMovedistance; multiplier++) {
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
		public List<Tuple<int, int>> ProtectedTeammates<P>(Board board, in int col, in int row) where P:SlidingPiece {
			if (board[col, row] is P && board[col, row].White == White) {
				List<Tuple<int, int>> ret = new List<Tuple<int, int>>();
				int targetCol, targetRow;
				foreach (Tuple<int, int> dir in getDirections()) {
					for (int multiplier = 1; multiplier<=board.MaximumMovedistance; multiplier++) {
						targetCol = col + dir.Item1 * multiplier;
						targetRow = row + dir.Item2 * multiplier;
						//Können wir das Feld überhaupt betreten? (Zylinder-Brett)
						if (board.IsAccessible(targetCol, targetRow)) {
							//Ist das Feld frei?
							if (board.IsFree(targetCol, targetRow)) {
								continue;
							} else {
								//Steht auf diesem Feld ein Teamkollege?
								if (board[targetCol, targetRow].White == White) {
									ret.Add(new Tuple<int, int>(targetCol, targetRow));
								} else {
									break;//Ein Feind kam jedoch zuerst.
								}
							}
						} else {
							break;
						}
					}
				}
				return ret;
			} else {
				throw new Exception(typeof(P).Name + "-Piece of color " + (White ? "white" : "black") + " expected.");
			}
		}
		public List<PinData> CurrentPins<P>(Board board, in int col, in int row) where P:SlidingPiece {
			if (board[col, row] is P && board[col, row].White == White) {
				List<PinData> ret = new List<PinData>();
				int targetCol, targetRow, targetCol2, targetRow2;
				foreach (Tuple<int, int> dir in getDirections()) {
					for (int multiplier = 1; multiplier <= board.MaximumMovedistance; multiplier++) {
						targetCol = col + dir.Item1 * multiplier;
						targetRow = row + dir.Item2 * multiplier;
						//Können wir das Feld überhaupt betreten? (Zylinder-Brett)
						if (board.IsAccessible(targetCol, targetRow)) {
							//Ist das Feld frei?
							if (board.IsFree(targetCol, targetRow)) {
								continue;
							} else {
								//Steht auf diesem Feld ein Feind?
								if (board[targetCol, targetRow].White != White) {
									for (int protrusion=1; multiplier+protrusion <= board.MaximumMovedistance; protrusion++) {
										targetCol2 = col + dir.Item1 * (multiplier + protrusion);
										targetRow2 = row + dir.Item2 * (multiplier + protrusion);
										//Können wir das Feld hinter dem ersten Gegner überhaupt betreten? (Zylinder-Brett)
										if (board.IsAccessible(targetCol2, targetRow2)) {
											//Ist dieses frei?
											if (board.IsFree(targetCol2, targetRow2)) {
												continue;
											} else {
												//Steht auf diesem Feld ein weiterer Feind?
												if (board[targetCol, targetRow].White != White) {
													//Pin erzeugt.
													ret.Add(new PinData(board, new Tuple<int, int>(col, row), new Tuple<int, int>(targetCol, targetRow), new Tuple<int, int>(targetCol2, targetRow2)));
												}
												goto dirs;//Gehe zur nächsten Richtung.
											}
										} else {
											goto dirs;//Gehe zur nächsten Richtung.
										}
									}
								} else {
									break;//Auf einem Freun ist kein Pin möglich!
								}
							}
						} else {
							break;
						}
					}
					dirs:;
				}
				return ret;
			} else {
				throw new Exception(typeof(P).Name + "-Piece of color " + (White ? "white" : "black") + " expected.");
			}
		}
		public abstract List<PinData> CurrentPins(Board board, in int col, in int row);
	}
}