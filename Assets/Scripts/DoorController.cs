using UnityEngine;

namespace HTW.CAVE.Etage6App
{
	public class DoorController : MonoBehaviour
	{
		[SerializeField] private DoorManager _doorManager;
		[SerializeField] private AudioSource _audioSource;
		[SerializeField] private float openAngle = 90;

		private Transform _playerPosition;
		private bool _isTriggered;
		private bool _isOpen;
		private float _startTime;
		private float _startAngle;

		public void Start()
		{
			if (_doorManager == null)
				_doorManager = GetComponentInParent<DoorManager>();
			if (_audioSource == null)
				_audioSource = GetComponent<AudioSource>();

			_playerPosition = _doorManager.cavePlayer.transform;
			_startAngle = transform.rotation.eulerAngles.y;
		}

		public void Update()
		{
			if (!_isTriggered)
			{
				if (!_isOpen
				    && Vector3.Distance(transform.position, _playerPosition.position) <= _doorManager.openingDistance)
				{
					Trigger();
				}
				else if (_isOpen
				         && Vector3.Distance(transform.position, _playerPosition.position) > _doorManager.openingDistance
				         && Time.realtimeSinceStartup - _startTime >= _doorManager.openTime + _doorManager.closeAfterSeconds)
				{
					Trigger();
				}
				return;
			}
			
			MoveDoor();
		}

		private void MoveDoor()
		{
			var progress = (Time.realtimeSinceStartup - _startTime) / _doorManager.openTime;
			if (!_isOpen)
				progress = 1 - progress;

			progress = Mathf.Clamp01(progress);
			if (progress is 1f or 0f)
				_isTriggered = false;

			transform.eulerAngles = new(0, _startAngle + progress * openAngle, 0);
		}

		private void Trigger()
		{
			switch (_isOpen)
			{
				case false when !_isTriggered:
					_audioSource.clip = _doorManager.openingSound;
					_audioSource.Play();
					break;
				case true when !_isTriggered:
					_audioSource.clip = _doorManager.closingSound;
					_audioSource.Play();
					break;
			}

			_isTriggered = true;
			_startTime = Time.realtimeSinceStartup;
			_isOpen = !_isOpen;
		}
	}
}
