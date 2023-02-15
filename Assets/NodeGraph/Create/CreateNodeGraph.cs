using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;

namespace NodeGraph
{
    public class CreateNodeGraph : EndNameEditAction
    {
        [MenuItem("Assets/Create/Node Graph", false, 100)]
        public static void CreateMaterialGraph()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, CreateInstance<CreateNodeGraph>(),
                "New Node Graph.nodeGraph", null, null);
        }

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            File.WriteAllText(pathName, EditorJsonUtility.ToJson(new NodeGraphData(pathName), true));
            AssetDatabase.Refresh();
        }
    }
}
