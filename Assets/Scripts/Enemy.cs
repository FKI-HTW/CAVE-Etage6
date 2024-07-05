using UnityEngine;

namespace HTW.CAVE.Etage6App
{
	public class Enemy : MonoBehaviour
	{
		private enum EnemyType
		{
			Standing,
			Rotating,
			Dropping
		}

		[SerializeField] private EnemyType enemyType = EnemyType.Standing;
		[SerializeField] private Texture normalTexture;
		[SerializeField] private Texture hitTexture;
		[SerializeField] private Renderer _frontRenderer;
		[SerializeField] private AudioSource audioSource;
		[SerializeField, Range(1, 5)] private float speed = 3f;
		[SerializeField, Range(3, 60)] private float reactivationInterval = 3;

		private bool _readyToHit;
		private Vector3 _startPosition;
		private Vector3 _startRotation;
		private Vector3 _targetRotation;

		private void Start()
		{
			if (_frontRenderer == null)
				transform.Find("Front").GetComponent<Renderer>();
			if (audioSource == null)
				audioSource = GetComponent<AudioSource>();
			audioSource.volume = 0.2f;

			var localEulerAngles = transform.localEulerAngles;
			_startRotation = localEulerAngles;
			_targetRotation = new(localEulerAngles.x, localEulerAngles.y + 180, localEulerAngles.z);
			_startPosition = transform.position;
			_readyToHit = true;
		}


		private void OnTriggerEnter(Collider other)
		{
			if (!_readyToHit 
			    || !other.CompareTag("Projektil")
			    || !other.TryGetComponent<TennisballBehaviour>(out var ball) 
			    || ball.Disabled) 
				return;

			ball.Disabled = true;
			audioSource.Play();
			_readyToHit = false;
			_frontRenderer.material.SetTexture("_BaseColorMap", hitTexture);
		}

		private void Reactivate()
		{
			_readyToHit = true;
			_frontRenderer.material.SetTexture("_BaseColorMap", normalTexture);
		}

		private void Update()
		{
			switch (enemyType)
			{
				case EnemyType.Dropping:
					if (_readyToHit && transform.position.y > 0)
					{
						transform.position += speed * Time.deltaTime * -transform.up;
					}
					else if (!_readyToHit && transform.position.y < _startPosition.y)
					{
						transform.position += speed * Time.deltaTime * transform.up;
						if (transform.position.y >= _startPosition.y)
						{
							Invoke(nameof(Reactivate), reactivationInterval);
						}
					}
					break;
				case EnemyType.Rotating:
					if (_readyToHit) //startrotation.y = 150
					{
						if (transform.localEulerAngles.y < _targetRotation.y)
						{
							transform.RotateAround(transform.position, transform.up, Time.deltaTime * 30 * speed);
						}
						return;
					}

					if (transform.localEulerAngles.y > _startRotation.y)
					{
						transform.RotateAround(transform.position, -transform.up, Time.deltaTime * 30 * speed);
						if (transform.localEulerAngles.y <= _startRotation.y)
						{
							Invoke(nameof(Reactivate), reactivationInterval);
						}
					}
					break;
				case EnemyType.Standing:
					if (_readyToHit)
					{
						if (gameObject.name == "EnemyS2")
						{
						}
						if (transform.localEulerAngles.x < 350f)
						{
							transform.Rotate(30 * speed * Time.deltaTime * Vector3.right);
						}
						return;
					}

					if (transform.localEulerAngles.x <= 275f)
					{
						Invoke(nameof(Reactivate), reactivationInterval);
					}
					else if (transform.localEulerAngles.x > 275f)
					{
						transform.Rotate(30 * speed * Time.deltaTime * -Vector3.right, Space.Self);
					}
					break;
			}
		}

	}
}
