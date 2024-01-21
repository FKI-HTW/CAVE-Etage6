using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTW.CAVE.Etage6App
{
	public class DoorController : MonoBehaviour
	{
		[SerializeField] private DoorManager _doorManager;
		[SerializeField] private AudioSource _audioSource;

		[SerializeField] private float openAngle = 90;

		private Transform playerPosition;
		private bool isTriggered = false;
		private bool isOpen = false;
		private float startTime = 0;
		private float startAngle = 0;

		public void Start()
		{
			if (_doorManager == null)
				_doorManager = GetComponentInParent<DoorManager>();
			if (_audioSource == null)
				_audioSource = GetComponent<AudioSource>();

			playerPosition = _doorManager.cavePlayer.transform;
			startAngle = transform.rotation.eulerAngles.y;
		}

		public void Update()
		{
			if (!isTriggered)
			{
				if (
					!isOpen
					&& Vector3.Distance(transform.position, playerPosition.position) <= _doorManager.openingDistance
					)
				{
					Trigger();
				}
				else if (
					isOpen
					&& Vector3.Distance(transform.position, playerPosition.position) > _doorManager.openingDistance
					&& Time.realtimeSinceStartup - startTime >= _doorManager.openTime + _doorManager.closeAfterSeconds
					)
				{
					Trigger();
				}
				return;
			}
			MoveDoor();
		}

		private void MoveDoor()
		{
			float perc = (Time.realtimeSinceStartup - startTime) / _doorManager.openTime;
			if (!isOpen)
				perc = 1 - perc;

			perc = Mathf.Clamp01(perc);
			if (perc == 1 || perc == 0)
				isTriggered = false;

			transform.eulerAngles = new Vector3(0, startAngle + perc * openAngle, 0);
		}

		private void Trigger()
		{
			if (!isTriggered && !isOpen)
			{
				_audioSource.clip = _doorManager.openingSound;
				_audioSource.Play();
			}
			else if (isOpen && !isTriggered)
			{
				_audioSource.clip = _doorManager.closingSound;
				_audioSource.Play();
			}

			isTriggered = true;
			startTime = Time.realtimeSinceStartup;
			isOpen = !isOpen;
		}
	}
}
