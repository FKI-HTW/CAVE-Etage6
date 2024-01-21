using UnityEngine;

namespace HTW.CAVE.Etage6App
{
    public class Buzzer : MonoBehaviour
    {
        [SerializeField] private Animation wallAnimation;
        private bool wallsRaised = true;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("HandCollider"))
            {
                if (wallsRaised) 
                    wallAnimation.Play("lower_walls"); 
                else 
                    wallAnimation.Play("raise_walls");
                wallsRaised = !wallsRaised;
            }
        }
    }
}
