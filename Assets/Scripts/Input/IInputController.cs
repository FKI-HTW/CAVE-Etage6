using UnityEngine;

namespace HTW.CAVE.Etage6App.Input
{
	public interface IInputController
	{
		public Vector2 GetMovementInput();
		public Vector2 GetRotationalInput();
	}
}
