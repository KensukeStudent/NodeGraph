using System.Text;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace NodeGraph
{
    /// <summary>
    /// ノードグラフ用エディタースクリプト
    /// </summary>
    public class NodeGraphWindow : EditorWindow
    {
        private string selectedGuid = "";
        public string SelectedGuid
        {
            private set { selectedGuid = value; }
            get => selectedGuid;
        }

        /// <summary>
        /// 現在windowに表示しているデータ
        /// </summary>
        private NodeGraphData currentData = null;

        private void OnEnable()
        {
            // コンパイル時用に再読み込み
            if (!string.IsNullOrEmpty(selectedGuid))
            {
                PaintWindow();
            }
            else
            {
                ReloadNodeGraphAsset();
            }
        }

        /// <summary>
        /// 初期化時にWindowが表示されていれば再読み込みを行う
        /// </summary>
        private void ReloadNodeGraphAsset()
        {
            // NodeGraphWindowクラス名の場合処理しない
            if (titleContent.text == typeof(NodeGraphWindow).ToString())
                return;
            string path = FileUtility.GetFilePath($"{titleContent.text}.nodeGraph");
            var guid = AssetDatabase.AssetPathToGUID(path);
            Initialize(guid);
        }

        public void Initialize(string assetGuid)
        {
            try
            {
                var asset = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(assetGuid));
                if (asset == null)
                    return;

                if (!EditorUtility.IsPersistent(asset))
                    return;

                if (selectedGuid == assetGuid)
                    return;

                var path = AssetDatabase.GetAssetPath(asset);
                selectedGuid = assetGuid;

                string textGraph = File.ReadAllText(path, Encoding.UTF8);
                currentData = JsonUtility.FromJson<NodeGraphData>(textGraph);
                titleContent = new GUIContent(asset.name);
                PaintWindow();
            }
            catch (System.Exception)
            {
                selectedGuid = "";
                currentData = null;
                rootVisualElement.Clear();
                throw;
            }
        }

        private void PaintWindow()
        {
            // graphViewをwindowで使用可能にする
            var graphView = new NodeGraphView(this, currentData);
            rootVisualElement.Add(graphView);

            var button = new Button(() => SaveAsset(graphView)) { text = "データ保存" };
            rootVisualElement.Add(button);
            Repaint();
        }

        private void SaveAsset(NodeGraphView view)
        {
            string path = AssetDatabase.GUIDToAssetPath(selectedGuid);
            if (string.IsNullOrEmpty(path))
                return;
            File.WriteAllText(path, EditorJsonUtility.ToJson(view.CreateCurrentNodeGraphData(), true));
        }
    }
}