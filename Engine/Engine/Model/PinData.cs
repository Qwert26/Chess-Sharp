using System;
namespace Engine.Model {
	public struct PinData {
		/// <summary>
		/// Die Figur, die angreift.
		/// </summary>
		public Piece Pinner;
		/// <summary>
		/// Die Figur, die angegriffen wird.
		/// </summary>
		public Piece Pinned;
		/// <summary>
		/// Die Figur, die geschlagen werden könnte, wenn sich die angegriffene Figur bewegt.
		/// </summary>
		public Piece Hidden;
		public Tuple<int, int> PositionPinner, PositionPinned, PositionHidden;
		public PinData(Board board, Tuple<int, int> posPinner, Tuple<int, int> posPinned, Tuple<int, int> posHidden) {
			PositionPinner = posPinner;
			PositionPinned = posPinned;
			PositionHidden = posHidden;
			Pinner = board[PositionPinner];
			Pinned = board[PositionPinned];
			Hidden = board[PositionHidden];
		}
		public bool IsRoyalPin {
			get {
				return Hidden.Royal;
			}
		}
		public bool IsSkewer {
			get {
				return Pinned.Royal;
			}
		}
		public int GetPinValue {
			get {
				return Hidden.Value - Pinned.Value;
			}
		}
	}
}