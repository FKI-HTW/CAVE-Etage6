using UnityEngine;

namespace HTW.CAVE.Etage6App
{
	public class TennisballBehaviour : MonoBehaviour
	{
		[SerializeField] private AudioSource audioSource;

		public bool Disabled { get => disabled; }
		private bool disabled = false;

		private void Start()
		{
			if (audioSource == null)
				audioSource = GetComponent<AudioSource>();
		}

		private void OnCollisionEnter(Collision collision)
		{
			disabled = true;
		}

		public void MakeSound()
		{
			audioSource.Play();
		}

		public void SwitchLight(bool lightsOn)
		{
			GameObject light = transform.GetChild(0).gameObject;
			light.SetActive(!lightsOn);
		}
	}
}
