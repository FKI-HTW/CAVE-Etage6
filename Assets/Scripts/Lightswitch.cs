using UnityEngine;

namespace HTW.CAVE.Etage6App
{
	public class Lightswitch : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			Debug.Log(other.tag);
			if (!other.CompareTag("Projektil")) 
				return;
			LightManager.SwitchLight();
		}
	}
}
