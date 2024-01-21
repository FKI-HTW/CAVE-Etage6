using UnityEngine;

namespace HTW.CAVE.Etage6App
{
	public class Enemy : MonoBehaviour
	{
		private enum EnemyType
		{
			standing,
			rotating,
			dropping
		}

		[SerializeField] private EnemyType enemyType = EnemyType.standing;
		[SerializeField] private Texture normalTexture;
		[SerializeField] private Texture hitTexture;
		[SerializeField] private Renderer _frontRenderer;
		[SerializeField] private AudioSource audioSource;
		[SerializeField, Range(1, 5)] private float speed = 3f;
		[SerializeField, Range(3, 60)] private float reactivationInterval = 3;

		private bool readyToHit;
		private Vector3 startPosition;
		private Vector3 startRotation;
		private Vector3 targetRotation;

		private void Start()
		{
			if (_frontRenderer == null)
				transform.Find("Front").GetComponent<Renderer>();
			if (audioSource == null)
				audioSource = GetComponent<AudioSource>();
			audioSource.volume = 0.2f;

			startRotation = transform.localEulerAngles;
			targetRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + 180, transform.localEulerAngles.z);
			startPosition = transform.position;
			readyToHit = true;
		}


		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Projektil") && readyToHit && !other.GetComponent<TennisballBehaviour>().Disabled)
			{
				audioSource.Play();
				readyToHit = false;
				_frontRenderer.material.SetTexture("_BaseColorMap", hitTexture);
			}
		}

		private void Reactivate()
		{
			readyToHit = true;
			_frontRenderer.material.SetTexture("_BaseColorMap", normalTexture);
		}

		private void Update()
		{
			switch (enemyType)
			{
				case EnemyType.dropping:
					if (readyToHit && transform.position.y > 0)
					{
						transform.position += speed * Time.deltaTime * (-transform.up);
					}
					else if (!readyToHit && transform.position.y < startPosition.y)
					{
						transform.position += speed * Time.deltaTime * transform.up;
						if (transform.position.y >= startPosition.y)
						{
							Invoke(nameof(Reactivate), reactivationInterval);
						}
					}
					break;
				case EnemyType.rotating:
					if (readyToHit) //startrotation.y = 150
					{
						if (transform.localEulerAngles.y < targetRotation.y)
						{
							transform.RotateAround(transform.position, transform.up, Time.deltaTime * 30 * speed);
						}
						return;
					}

					if (transform.localEulerAngles.y > startRotation.y)
					{
						transform.RotateAround(transform.position, -transform.up, Time.deltaTime * 30 * speed);
						if (transform.localEulerAngles.y <= startRotation.y)
						{
							Invoke(nameof(Reactivate), reactivationInterval);
						}
					}
					break;
				case EnemyType.standing:
					if (readyToHit)
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
