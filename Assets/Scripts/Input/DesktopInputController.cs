using UnityEngine;

namespace HTW.CAVE.Etage6App.Input
{
	public class DesktopInputController : MonoBehaviour, IInputController
	{
		[SerializeField] private InputManager _inputManager;
		[SerializeField] private KeyCode _shootButton;

		private void Start()
		{
			if (_inputManager == null)
				_inputManager = GameObject.FindWithTag("InputManager").GetComponent<InputManager>();

			_inputManager.RegisterInputController(this);
		}

		private void OnDestroy()
		{
			_inputManager.UnregisterInputController(this);
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(_shootButton))
			{
				_inputManager.Shoot(EHandSide.Right);
			}
		}

		public Vector2 GetMovementInput()
		{
			return new(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
		}

		public Vector2 GetRotationalInput()
		{
			return new(UnityEngine.Input.GetAxis("Mouse X"), UnityEngine.Input.GetAxis("Mouse Y"));
		}
	}
}
