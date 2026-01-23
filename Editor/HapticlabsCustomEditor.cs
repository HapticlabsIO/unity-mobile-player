
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Hapticlabs.Player
{
    [CustomEditor(typeof(HapticlabsCustom))]
    public class CustomHapticsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var androidPathProp = serializedObject.FindProperty("androidPath");
            var iosPathProp = serializedObject.FindProperty("iosPath");

            // Android Path File Picker
            EditorGUILayout.LabelField("Android HAC Path", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            androidPathProp.stringValue = EditorGUILayout.TextField(androidPathProp.stringValue);
            if (GUILayout.Button("Select", GUILayout.MaxWidth(60)))
            {
                string selected = EditorUtility.OpenFilePanel("Select Android HAC File", Application.streamingAssetsPath, "hac");
                if (!string.IsNullOrEmpty(selected))
                {
                    string rel = GetRelativeStreamingAssetsPath(selected);
                    if (!string.IsNullOrEmpty(rel))
                        androidPathProp.stringValue = rel;
                    else
                        Debug.LogWarning("Selected file is not within StreamingAssets.");
                }
            }
            EditorGUILayout.EndHorizontal();
            if (!IsValidStreamingAssetsPath(androidPathProp.stringValue))
            {
                EditorGUILayout.HelpBox("androidPath must be a valid path within Assets/StreamingAssets.", MessageType.Warning);
            }

            // iOS Path File Picker
            EditorGUILayout.LabelField("iOS AHAP Path", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            iosPathProp.stringValue = EditorGUILayout.TextField(iosPathProp.stringValue);
            if (GUILayout.Button("Select", GUILayout.MaxWidth(60)))
            {
                string selected = EditorUtility.OpenFilePanel("Select iOS AHAP File", Application.streamingAssetsPath, "ahap");
                if (!string.IsNullOrEmpty(selected))
                {
                    string rel = GetRelativeStreamingAssetsPath(selected);
                    if (!string.IsNullOrEmpty(rel))
                        iosPathProp.stringValue = rel;
                    else
                        Debug.LogWarning("Selected file is not within StreamingAssets.");
                }
            }
            EditorGUILayout.EndHorizontal();
            if (!IsValidStreamingAssetsPath(iosPathProp.stringValue))
            {
                EditorGUILayout.HelpBox("iosPath must be a valid path within Assets/StreamingAssets.", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private bool IsValidStreamingAssetsPath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return false;
            if (relativePath.Contains("..") || Path.IsPathRooted(relativePath)) return false;

            string fullPath = Path.GetFullPath(Path.Combine(Application.streamingAssetsPath, relativePath));
            string streamingAssetsFullPath = Path.GetFullPath(Application.streamingAssetsPath);

            return fullPath.StartsWith(streamingAssetsFullPath) && File.Exists(fullPath);
        }

        private string GetRelativeStreamingAssetsPath(string absolutePath)
        {
            string streamingAssetsFullPath = Path.GetFullPath(Application.streamingAssetsPath);
            string fullPath = Path.GetFullPath(absolutePath);
            if (fullPath.StartsWith(streamingAssetsFullPath))
            {
                string rel = fullPath.Substring(streamingAssetsFullPath.Length);
                if (rel.StartsWith(Path.DirectorySeparatorChar.ToString()) || rel.StartsWith(Path.AltDirectorySeparatorChar.ToString()))
                    rel = rel.Substring(1);
                return rel;
            }
            return null;
        }
    }
}
