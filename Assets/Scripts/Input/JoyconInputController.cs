using UnityEngine;
using LookingGlass.JoyconLib;

namespace HTW.CAVE.Etage6App.Input
{
	// TODO : convert legacy input system and manual Joycon input to new InputSystem
	public class JoyconInputController : MonoBehaviour, IInputController
	{
		[SerializeField] private InputManager _inputManager;
		[SerializeField] private JoyconManager _joyconManager;
		[SerializeField] private Joycon.Button _shootButton;

		private Joycon _leftJoycon;
		private Joycon _rightJoycon;

		private void Awake()
		{
			if (_inputManager == null)
				_inputManager = GameObject.FindWithTag("InputManager").GetComponent<InputManager>();
			if (_joyconManager == null)
				_joyconManager = GetComponent<JoyconManager>();

			_inputManager.RegisterInputController(this);
		}

		private void OnDestroy()
		{
			_inputManager.UnregisterInputController(this);
		}

		private void Update()
		{
			if (_leftJoycon == null || _leftJoycon.state == Joycon.state_.NOT_ATTACHED)
				_leftJoycon = _joyconManager.j.Find(j => j.isLeft);
			
			if (_leftJoycon != null)
			{
				if (_leftJoycon.GetButton(_shootButton))
					_inputManager.Shoot(EHandSide.Left);
			}

			if (_rightJoycon == null || _rightJoycon.state == Joycon.state_.NOT_ATTACHED)
				_rightJoycon = _joyconManager.j.Find(j => !j.isLeft);

			if (_rightJoycon != null)
			{
				if (_rightJoycon.GetButton(_shootButton))
					_inputManager.Shoot(EHandSide.Right);
			}
		}

		public Vector2 GetMovementInput()
		{
			if (_leftJoycon == null || _leftJoycon.state == Joycon.state_.NOT_ATTACHED)
				return Vector2.zero;
			var stickLeft = _leftJoycon.GetStick();
			return new(stickLeft[0], stickLeft[1]);
		}

		public Vector2 GetRotationalInput()
		{
			if (_rightJoycon == null || _rightJoycon.state == Joycon.state_.NOT_ATTACHED)
				return Vector2.zero;
			var stickRight = _rightJoycon.GetStick();
			return new(stickRight[0], stickRight[1]);
		}
	}
}
