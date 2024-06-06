using System.Collections.Generic;
using UnityEngine;

namespace HTW.CAVE.Etage6App.Input
{
	public class ShootController : MonoBehaviour
	{
		[SerializeField] private InputManager _inputManager;
		[SerializeField] private TennisballBehaviour _ammunitionPrefab;
		[SerializeField] private GameObject _crosshairPrefab;
		[SerializeField] private EHandSide _handSide;
		[SerializeField] [Range(0,1)] private float _lerpStrength = 0.5f;

		[SerializeField] private int _maxBalls = 100;
		[SerializeField] private float _speed = 600f;

		private readonly Queue<TennisballBehaviour> _ballQueue = new();
		private GameObject _crosshair;
		private int _layerMask;

		private void Start()
		{
			if (_inputManager == null)
				_inputManager = GameObject.FindWithTag("InputManager").GetComponent<InputManager>();

			_layerMask = LayerMask.NameToLayer("CAVE");

			_crosshair = Instantiate(_crosshairPrefab, transform);
			_crosshair.SetActive(false);

			_inputManager.OnAimStart += TakeAim;
			_inputManager.OnAimEnd += StopAim;
			_inputManager.OnShoot += ThrowBall;
		}

		private void OnDestroy()
		{
			_inputManager.OnAimStart -= TakeAim;
			_inputManager.OnAimEnd -= StopAim;
			_inputManager.OnShoot -= ThrowBall;
		}

		public void Update()
		{
			if (_inputManager.IsAiming(_handSide))
				UpdateCrosshair();
		}

		private void TakeAim(EHandSide side)
		{
			if (side != _handSide) return;

			UpdateCrosshair();
			_crosshair.SetActive(true);
		}

		private void StopAim(EHandSide side)
		{
			if (side != _handSide) return;

			_crosshair.SetActive(false);
		}

		private void UpdateCrosshair()
		{
			if (Physics.Raycast(transform.position, transform.forward, out var hit, Mathf.Infinity, _layerMask, QueryTriggerInteraction.UseGlobal))
				_crosshair.transform.position = Vector3.Lerp(_crosshair.transform.position, hit.point, _lerpStrength);
		}

		private void ThrowBall(EHandSide side)
		{
			if (side != _handSide) return;

			var ball = _ballQueue.Count >= _maxBalls
				? _ballQueue.Dequeue()
				: Instantiate(_ammunitionPrefab, transform.position, transform.rotation);

			ball.MakeSound();
			ball.transform.SetPositionAndRotation(transform.position, transform.rotation);
			ball.GetComponent<Rigidbody>().AddForce(transform.forward * _speed);
			_ballQueue.Enqueue(ball);
		}
	}
}
