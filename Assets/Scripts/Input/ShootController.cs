using System;
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

		[SerializeField] private int _maxBalls = 300;
		[SerializeField] private float _speed = 600f;

		private readonly Queue<TennisballBehaviour> _ballQueue = new();
		private GameObject _crosshair;

		private static Action<bool> _onSwitchLight;
		private static bool _lightsOn = true;

		public void Start()
		{
			if (_inputManager == null)
				_inputManager = GameObject.FindWithTag("InputManager").GetComponent<InputManager>();

			_crosshair = Instantiate(_crosshairPrefab, transform);
			_crosshair.SetActive(false);

			_onSwitchLight += SwitchTennisBallLights;

			_inputManager.OnAimStart += TakeAim;
			_inputManager.OnAimEnd += StopAim;
			_inputManager.OnShoot += ThrowBall;
		}

		public void Update()
		{
			if (_inputManager.IsAiming(_handSide))
				UpdateCrosshair();
		}

		public void SwitchTennisBallLights(bool lightsOn)
		{
			foreach (TennisballBehaviour ball in _ballQueue)
				ball.SwitchLight(lightsOn);
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
			if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Mathf.Infinity))
			{
				_crosshair.transform.GetChild(0).transform.rotation = Quaternion.Euler(hit.normal);
				_crosshair.transform.position = hit.point;
			}
		}

		private void ThrowBall(EHandSide side)
		{
			if (side != _handSide) return;

			TennisballBehaviour ball = _ballQueue.Count >= _maxBalls
				? _ballQueue.Dequeue()
				: Instantiate(_ammunitionPrefab, transform.position, transform.rotation);

			ball.SwitchLight(_lightsOn);
			ball.MakeSound();
			ball.transform.SetPositionAndRotation(transform.position, transform.rotation);
			ball.GetComponent<Rigidbody>().AddForce(transform.forward * _speed);
			_ballQueue.Enqueue(ball);
		}

		public static void SwitchLights(bool isOn)
		{
			if (_lightsOn == isOn) return;
			_lightsOn = isOn;
			_onSwitchLight(isOn);
		}
	}
}
