using UnityEngine;
using UnityEngine.Events;
using Hapticlabs.Player;

namespace Hapticlabs.Player
{
    [AddComponentMenu("Hapticlabs/HapticlabsBuiltIn")]
    public class HapticlabsBuiltIn : MonoBehaviour
    {
        [Header("Hapticlabs Settings")]
        [Tooltip("Effect to play on Android devices")]
        public AndroidBuiltInEffect androidEffect;

        [Tooltip("Effect to play on iOS devices")]
        public IOSBuiltInEffect iosEffect;

        private void Reset()
        {
        }

        private void Awake()
        {
        }

        // Call this method to trigger haptic playback
        /// <summary>
        /// Triggers playback of the selected built-in haptic effect using the service.
        /// </summary>
        public void TriggerHaptics()
        {
            HapticlabsService.Instance.PlayBuiltIn(androidEffect, iosEffect);
        }
    }
}