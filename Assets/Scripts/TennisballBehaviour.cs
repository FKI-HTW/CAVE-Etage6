using UnityEngine;

namespace HTW.CAVE.Etage6App
{
	public class TennisballBehaviour : MonoBehaviour
	{
		[SerializeField] private AudioSource _audioSource;
		[SerializeField] private Light _light;

		private void Awake()
		{
			LightManager.OnLightSwitched += SwitchLight;
			SwitchLight(LightManager.IsLightOn);
		}

		private void OnDestroy()
		{
			LightManager.OnLightSwitched -= SwitchLight;
		}

		public void MakeSound()
		{
			_audioSource.Play();
		}

		private void SwitchLight(bool lightsOn)
		{
			_light.gameObject.SetActive(lightsOn);
		}
	}
}
