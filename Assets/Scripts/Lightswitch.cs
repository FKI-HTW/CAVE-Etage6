using UnityEngine;

namespace HTW.CAVE.Etage6App
{
	public class Lightswitch : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("Projektil") 
			    || !other.TryGetComponent<TennisballBehaviour>(out var ball) 
			    || ball.Disabled)
				return;

			ball.Disabled = true;
			LightManager.SwitchLight();
		}
	}
}
