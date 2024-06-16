using System;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace HTW.CAVE.Etage6App
{
    public class LightManager : MonoBehaviour
    {
        [SerializeField] private HDAdditionalLightData _directionalLight;
        [SerializeField] private float _colorTemperatureOn;
        [SerializeField] private float _colorTemperatureOff;
        [SerializeField] private float _intensityOn;
        [SerializeField] private float _intensityOff;
        
        public static event Action<bool> OnLightSwitched;

        private static bool _isLightOn;
        public static bool IsLightOn
        {
            get => _isLightOn;
            set
            {
                if (_isLightOn == value) return;
                _isLightOn = value;
                OnLightSwitched?.Invoke(value);
            }
        }

        public static void SwitchLight() => IsLightOn = !IsLightOn;

        private void Awake()
        {
            OnLightSwitched += SwitchLight;
        }

        private void OnDestroy()
        {
            OnLightSwitched -= SwitchLight;
        }

        private void SwitchLight(bool turnLightsOn)
        {
            _directionalLight.intensity = turnLightsOn ? _intensityOn : _intensityOff;
            _directionalLight.SetColor(Color.white, turnLightsOn ? _colorTemperatureOn : _colorTemperatureOff);
        }
    }
}
