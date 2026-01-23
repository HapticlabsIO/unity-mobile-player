
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Hapticlabs.Player
{
    [CustomEditor(typeof(HapticlabsCustom))]
    public class CustomHapticsEditor : Editor
    {
        private bool DrawPathField(string label, SerializedProperty pathProp, string fileExtension, string dialogTitle)
        {
            bool wasSelected = false;
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            pathProp.stringValue = EditorGUILayout.TextField(pathProp.stringValue);
            if (GUILayout.Button("Select", GUILayout.MaxWidth(60)))
            {
                string selected = EditorUtility.OpenFilePanel(dialogTitle, Application.streamingAssetsPath, fileExtension);
                if (!string.IsNullOrEmpty(selected))
                {
                    string rel = GetRelativeStreamingAssetsPath(selected);
                    if (!string.IsNullOrEmpty(rel))
                    {
                        pathProp.stringValue = rel;
                        wasSelected = true;
                    }
                    else
                        Debug.LogWarning("Selected file is not within StreamingAssets.");
                }
            }
            EditorGUILayout.EndHorizontal();
            if (!IsValidStreamingAssetsPath(pathProp.stringValue))
            {
                EditorGUILayout.HelpBox($"{label} must be a valid path within Assets/StreamingAssets.", MessageType.Warning);
            }
            return wasSelected;
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var androidPathProp = serializedObject.FindProperty("androidPath");
            var iosPathProp = serializedObject.FindProperty("iosPath");


            // iOS Path File Picker
            if (DrawPathField("Android HAC Path", androidPathProp, "hac", "Select Android HAC File"))
            {
                // Need to apply changes if a new file was selected
                serializedObject.ApplyModifiedProperties();
            }


            // Android Path File Picker
            DrawPathField("iOS AHAP Path", iosPathProp, "ahap", "Select iOS AHAP File");
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
