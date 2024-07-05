using System;
using System.Collections.Generic;
using UnityEngine;

namespace HTW.CAVE.Etage6App.Input
{
	public class InputManager : MonoBehaviour
	{
		private readonly List<IInputController> _controllers = new();

		public event Action<EHandSide> OnShoot;

		public Vector2 MovementInput { get; private set; }
		public Vector2 RotationalInput { get; private set; }

		private void Update()
		{
			MovementInput = Vector2.zero;
			RotationalInput = Vector2.zero;
			foreach (var controller in _controllers)
			{
				MovementInput += controller.GetMovementInput();
				RotationalInput += controller.GetRotationalInput();
			}
		}

		public void RegisterInputController(IInputController controller)
		{
			_controllers.Add(controller);
		}

		public void UnregisterInputController(IInputController controller)
		{
			_controllers.Remove(controller);
		}

		public void Shoot(EHandSide side)
		{
			OnShoot?.Invoke(side);
		}
	}
}
