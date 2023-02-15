using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace NodeGraph
{
    [CustomEditor(typeof(NodeGraphImporterEditor))]
    public class NodeGraphImporterEditor : ScriptedImporterEditor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Shader Editor"))
            {
                AssetImporter importer = target as AssetImporter;
                Debug.Assert(importer != null, "importer != null");
                ShowGraphEditWindow(importer.assetPath);
            }
        }

        internal static bool ShowGraphEditWindow(string path)
        {
            var guid = AssetDatabase.AssetPathToGUID(path);
            var extension = Path.GetExtension(path);
            if (extension != ".nodeGraph")
                return false;

            var foundWindow = false;
            foreach (var w in Resources.FindObjectsOfTypeAll<NodeGraphWindow>())
            {
                if (w.SelectedGuid == guid)
                {
                    foundWindow = true;
                    w.Focus();
                }
            }

            if (!foundWindow)
            {
                var window = CreateInstance<NodeGraphWindow>();
                window.Show();
                window.Initialize(guid);
            }
            return true;
        }

        [OnOpenAsset(0)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var path = AssetDatabase.GetAssetPath(instanceID);
            return ShowGraphEditWindow(path);
        }
    }

}
