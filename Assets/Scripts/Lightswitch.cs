using UnityEngine;

namespace HTW.CAVE.Etage6App
{
	public class Lightswitch : MonoBehaviour
	{
		[SerializeField] private Light directionalLight;
		[SerializeField] private float _colorTemperatureOn;
		[SerializeField] private float _colorTemperatureOff;
		[SerializeField] private float _intensityOn;
		[SerializeField] private float _intensityOff;

		private static bool lightsOn = true;

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Projektil") && !other.GetComponent<TennisballBehaviour>().Disabled)
			{
				lightsOn = !lightsOn;

				// change light and sky volume settings according to light state
				directionalLight.intensity = lightsOn ? _intensityOn : _intensityOff;
				directionalLight.colorTemperature = lightsOn ? _colorTemperatureOn : _colorTemperatureOff;

				// trigger light on all tennisballs
				Input.ShootController.SwitchLights(lightsOn);
			}
		}
	}
}
