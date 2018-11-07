namespace Engine.Model {
	public class Board {
		private Piece[,] pieces;
		/// <summary>
		/// K: white king-side
		/// Q: white queen-side
		/// k: black king-side
		/// q: black queen-side
		/// </summary>
		private string castleRights;
		public Board(int cols, int rows) {
			pieces = new Piece[cols, rows];
			Zylindrical = false;
			castleRights = "";
		}
		public bool Zylindrical { get; set; }
		public Piece this[int col, int row] {
			get {
				if (Zylindrical) {
					if (col < 0) {
						col = pieces.GetLength(0) + col;
					}
					col %= pieces.GetLength(0);
				}
				return pieces[col, row];
			}
		}
		public void SetDefaultCastleRights() {
			castleRights = "KQkq";
		}
		public void SetRookbasedCastleRights() {}
		public int PawnStartRank(bool white) {
			if (white) {
				return 1;
			} else {
				return pieces.GetLength(1) - 2;
			}
		}
		public bool IsFree(int col, int row) {
			return this[col, row] == null;
		}
		public bool IsAccessible(int col, int row) {
			if (0 <= row && row < pieces.GetLength(1)) {
				if (Zylindrical) {
					return true;
				} else if (0 <= col && col < pieces.GetLength(0)) {
					return true;
				} else {
					return false;
				}
			} else {
				return false;
			}
		}
	}
}