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

		private static bool _lightsOn = true;

		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("Projektil") || other.GetComponent<TennisballBehaviour>().Disabled) 
				return;
			_lightsOn = !_lightsOn;

			// change light and sky volume settings according to light state
			directionalLight.intensity = _lightsOn ? _intensityOn : _intensityOff;
			directionalLight.colorTemperature = _lightsOn ? _colorTemperatureOn : _colorTemperatureOff;

			// trigger light on all tennisballs
			Input.ShootController.SwitchLights(_lightsOn);
		}
	}
}
