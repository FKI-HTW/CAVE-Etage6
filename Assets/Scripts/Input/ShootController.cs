using System.Collections.Generic;
using UnityEngine;

namespace HTW.CAVE.Etage6App.Input
{
	public class ShootController : MonoBehaviour
	{
		[SerializeField] private InputManager _inputManager;
		[SerializeField] private TennisballBehaviour _ammunitionPrefab;
		[SerializeField] private Transform _cameraPivot;
		[SerializeField] private EHandSide _handSide;

		[SerializeField] private int _maxBalls = 100;
		[SerializeField] private float _speed = 600f;
		[SerializeField] private float _shootDelay = 1f;

		private readonly Queue<TennisballBehaviour> _ballQueue = new();

		private float _shootTime;

		private void Start()
		{
			if (_inputManager == null)
				_inputManager = GameObject.FindWithTag("InputManager").GetComponent<InputManager>();
			if (_cameraPivot == null)
				_cameraPivot = GameObject.FindWithTag("ActorHead").transform;

			_inputManager.OnShoot += ThrowBall;
			_shootTime = _shootDelay;
		}

		private void OnDestroy()
		{
			_inputManager.OnShoot -= ThrowBall;
		}

		public void Update()
		{
			_shootTime += Time.deltaTime;
		}

		private void ThrowBall(EHandSide side)
		{
			if (side != _handSide || _shootTime < _shootDelay)
				return;

			var ball = _ballQueue.Count >= _maxBalls
				? _ballQueue.Dequeue()
				: Instantiate(_ammunitionPrefab, transform.position, transform.rotation);

			ball.Disabled = false;
			ball.MakeSound();
			ball.transform.SetPositionAndRotation(transform.position, transform.rotation);
			ball.GetComponent<Rigidbody>().AddForce(transform.forward * _speed);
			_ballQueue.Enqueue(ball);
			_shootTime = 0;
		}
	}
}
