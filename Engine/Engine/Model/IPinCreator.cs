using System.Collections.Generic;
namespace Engine.Model {
	public interface IPinCreator {
		List<PinData> CurrentPins(Board board, in int col, in int row);
	}
}