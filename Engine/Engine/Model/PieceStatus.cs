using System;
using System.Collections.Generic;
namespace Engine.Model {
	/// <summary>
	/// Aktueller Status einer Figur, die auf dem Brett steht.
	/// </summary>
	public struct PieceStatus {
		/// <summary>
		/// Enthält die Felder, die frei sind und auf denen man ziehen kann.
		/// </summary>
		public List<Tuple<int, int>> freeMoveSpaces;
		/// <summary>
		/// Enthält die Felder mit gegnerischen Figuren, die aktuell angegriffen werden.
		/// </summary>
		public List<Tuple<int, int>> attackedEnemyPieces;
		/// <summary>
		/// Enthält die Felder mit befreundeten Figuren, die aktuell beschützt werden.
		/// </summary>
		public List<Tuple<int, int>> protectedTeammates;
		/// <summary>
		/// Wenn vorhanden, enthält dies sämtliche "Pins", die erzeugt werden.
		/// </summary>
		public List<PinData> currentPins;
		/// <summary>
		/// 
		/// </summary>
		public bool promotable;
	}
}
