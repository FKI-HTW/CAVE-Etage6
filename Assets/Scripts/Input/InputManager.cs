using System;
using System.Collections.Generic;
using UnityEngine;

namespace HTW.CAVE.Etage6App.Input
{
	public class InputManager : MonoBehaviour
	{
		private readonly List<IInputController> _controllers = new();

		public event Action<EHandSide> OnShoot;
		public event Action<EHandSide> OnAimStart;
		public event Action<EHandSide> OnAimEnd;

		public bool IsAiming(EHandSide side) => _isAiming[side];
		private readonly Dictionary<EHandSide, bool> _isAiming = new()
		{
			{ EHandSide.Left, false }, {  EHandSide.Right, false }
		};

		public Vector2 MovementInput => _movementInput;
		private Vector2 _movementInput;

		public Vector2 RotationalInput => _rotationalInput;
		private Vector2 _rotationalInput;

		private void Update()
		{
			_movementInput = Vector2.zero;
			_rotationalInput = Vector2.zero;
			foreach (var controller in _controllers)
			{
				_movementInput += controller.GetMovementInput();
				_rotationalInput += controller.GetRotationalInput();
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

		public void AimStart(EHandSide side)
		{
			if (IsAiming(side)) return;
			_isAiming[side] = true;
			OnAimStart?.Invoke(side);
		}

		public void AimEnd(EHandSide side)
		{
			if (!IsAiming(side)) return;
			_isAiming[side] = false;
			OnAimEnd?.Invoke(side);
		}
	}
}
