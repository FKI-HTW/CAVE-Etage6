using UnityEngine;

namespace HTW.CAVE.Etage6App.Input
{
    [RequireComponent(typeof(BoxCollider), typeof(VirtualEnvironment))]
    public class CAVEBoxCollider : MonoBehaviour
    {
        private BoxCollider _boxCollider;
        private VirtualEnvironment _virtualEnvironment; 
        
        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _virtualEnvironment = GetComponent<VirtualEnvironment>();
        }

        private void Update()
        {
            _boxCollider.size = _virtualEnvironment.dimensions;
            _boxCollider.center = _virtualEnvironment.center;
        }
    }
}
