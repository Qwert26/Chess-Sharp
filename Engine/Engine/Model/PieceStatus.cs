using System;
using System.Collections.Generic;
namespace Engine.Model {
	public struct PieceStatus {
		public List<Tuple<int, int>> freeMoveSpaces;
		public List<Tuple<int, int>> attackedEnemyPieces;
		public List<Tuple<int, int>> protectedTeammates;
		public List<PinData> currentPins;
	}
}
