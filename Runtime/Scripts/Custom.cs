
using UnityEngine;
using UnityEngine.Events;
using Hapticlabs.Player;
using System.IO;

namespace Hapticlabs.Player
{
    [AddComponentMenu("Hapticlabs/HapticlabsCustom")]
    public class HapticlabsCustom : MonoBehaviour
    {
        [Header("Hapticlabs Settings")]
        [Tooltip("Path to the Android HAC file within the Assets/StreamingAssets directory (e.g., AndroidSamples/Button.hac if the file is located at Assets/StreamingAssets/AndroidSamples/Button.hac)")]
        public string androidPath;

        [Tooltip("Path to the iOS AHAP file within the Assets/StreamingAssets directory (e.g., iOSSamples/Button.ahap if the file is located at Assets/StreamingAssets/iOSSamples/Button.ahap)")]
        public string iosPath;

        private void Reset()
        {
        }

        private void Awake()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            HapticlabsService.Instance.PreloadAndroid(androidPath);
#endif
        }

        // Call this method to trigger haptic playback
        /// <summary>
        /// Triggers playback of the referenced haptic files using the service.
        /// </summary>
        public void TriggerHaptics()
        {
            HapticlabsService.Instance.Play(androidPath, $"Data/Raw/{iosPath}");
        }
    }
}
