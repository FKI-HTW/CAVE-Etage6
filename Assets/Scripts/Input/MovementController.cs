using UnityEngine;

namespace HTW.CAVE.Etage6App.Input
{
	public class MovementController : MonoBehaviour
	{
		[SerializeField] private InputManager _inputManager;
		[SerializeField] private float _movementSpeed;
		[SerializeField] private float _rotationSpeed;

		private void Start()
		{
			if (_inputManager == null) 
				_inputManager = GameObject.FindWithTag("InputManager").GetComponent<InputManager>();
		}

		private void Update()
		{
			transform.position += _movementSpeed * Time.deltaTime * (
				_inputManager.MovementInput.x * transform.right + _inputManager.MovementInput.y * transform.forward
			);

			transform.rotation = Quaternion.Euler(transform.eulerAngles + Vector3.up * _inputManager.RotationalInput.x);
		}
	}
}
