using UnityEngine;

namespace HTW.CAVE.Etage6App
{
	public class Lightswitch : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("Projektil")) return;
			LightManager.SwitchLight();
		}
	}
}
