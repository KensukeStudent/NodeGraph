using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeGraph
{
    /// <summary>
    /// ノードウィンドウ内の検索Windowクラス
    /// </summary>
    public class NodeSearchMenuWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        struct NodeEntry
        {
            public NodeBase NodeBase;
            public string[] Title;
        }

        private EditorWindow window = null;
        private NodeGraphView view = null;

        public void Initilize(NodeGraphWindow _window, NodeGraphView _view)
        {
            window = _window;
            view = _view;
        }

        /// <summary>
        /// 検索Window上のリストを作成
        /// </summary>
        List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
        {
            // 検索対象ノード取得
            var nodeEntries = Sort(GetNodeEntryList());
            // 検索対象がWindowに表示されるように設定
            return SettingsSearchTreeEntry(nodeEntries);
        }

        /// <summary>
        /// 検索用のノードを取得
        /// </summary>
        private List<NodeEntry> GetNodeEntryList()
        {
            var nodeEntries = new List<NodeEntry>();
            // 全アセンブリを取得して
            // 指定の条件に合うものを検索windowに表示
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in GetTypesOrNothing(assembly))
                {
                    if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(NodeBase)))
                    {
                        // typeの継承先のAttributeは検索しなくても良いのでfalse指定
                        var attrs = type.GetCustomAttributes(typeof(TitleAttribute), false) as TitleAttribute[];
                        if (attrs != null && attrs.Length > 0)
                        {
                            var node = (NodeBase)Activator.CreateInstance(type);
                            nodeEntries.Add(new NodeEntry
                            {
                                NodeBase = node,
                                Title = attrs[0].title
                            });
                        }
                    }
                }
            }
            return nodeEntries;
        }

        private IEnumerable<Type> GetTypesOrNothing(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch
            {
                return Enumerable.Empty<Type>();
            }
        }

        /// <summary>
        /// 名前順にソート
        /// </summary>
        private List<NodeEntry> Sort(List<NodeEntry> nodeEntries)
        {
            // 例
            // Sample/Test01
            // Sample/Test02
            nodeEntries.Sort((entry1, entry2) =>
                {
                    for (var i = 0; i < entry1.Title.Length; i++)
                    {
                        if (i >= entry2.Title.Length)
                            return 1;
                        var value = entry1.Title[i].CompareTo(entry2.Title[i]);
                        if (value != 0)
                        {
                            if (entry1.Title.Length != entry2.Title.Length && (i == entry1.Title.Length - 1 || i == entry2.Title.Length - 1))
                                return entry1.Title.Length < entry2.Title.Length ? -1 : 1;
                            return value;
                        }
                    }
                    return 0;
                });
            return nodeEntries;
        }

        /// <summary>
        /// 検索Windowsの設定
        /// </summary>
        private List<SearchTreeEntry> SettingsSearchTreeEntry(List<NodeEntry> nodeEntries)
        {
            // `groups` contains the current group path we're in.
            var groups = new List<string>();

            // First item in the tree is the title of the window.
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node"), 0),
            };

            foreach (var nodeEntry in nodeEntries)
            {
                // `createIndex` represents from where we should add new group entries from the current entry's group path.
                var createIndex = int.MaxValue;

                // Compare the group path of the current entry to the current group path.
                for (var i = 0; i < nodeEntry.Title.Length - 1; i++)
                {
                    var group = nodeEntry.Title[i];
                    if (i >= groups.Count)
                    {
                        // The current group path matches a prefix of the current entry's group path, so we add the
                        // rest of the group path from the currrent entry.
                        createIndex = i;
                        break;
                    }
                    if (groups[i] != group)
                    {
                        // A prefix of the current group path matches a prefix of the current entry's group path,
                        // so we remove everyfrom from the point where it doesn't match anymore, and then add the rest
                        // of the group path from the current entry.
                        groups.RemoveRange(i, groups.Count - i);
                        createIndex = i;
                        break;
                    }
                }

                // Create new group entries as needed.
                // If we don't need to modify the group path, `createIndex` will be `int.MaxValue` and thus the loop won't run.
                for (var i = createIndex; i < nodeEntry.Title.Length - 1; i++)
                {
                    var group = nodeEntry.Title[i];
                    groups.Add(group);
                    tree.Add(new SearchTreeGroupEntry(new GUIContent(group)) { level = i + 1 });
                }

                // Finally, add the actual entry.
                tree.Add(new SearchTreeEntry(new GUIContent(nodeEntry.Title.Last())) { level = nodeEntry.Title.Length, userData = nodeEntry.NodeBase });
            }
            return tree;
        }

        bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var node = searchTreeEntry.userData as NodeBase;
            node.Initilize();

            // マウス座標にノードを追加
            Vector2 worldMousePosition = window.rootVisualElement.ChangeCoordinatesTo(window.rootVisualElement.parent, context.screenMousePosition - window.position.position);
            Vector2 localMousePosition = view.contentViewContainer.WorldToLocal(worldMousePosition);
            node.SetPosition(new Rect(localMousePosition, new Vector2(100, 100)));

            view.AddElement(node);
            return true;
        }
    }
}