using UnityEngine;

namespace HTW.CAVE.Etage6App
{
    public class Buzzer : MonoBehaviour
    {
        [SerializeField] private Animation wallAnimation;
        private bool _wallsRaised = true;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("HandCollider")) return;
            wallAnimation.Play(_wallsRaised ? "lower_walls" : "raise_walls");
            _wallsRaised = !_wallsRaised;
        }
    }
}
