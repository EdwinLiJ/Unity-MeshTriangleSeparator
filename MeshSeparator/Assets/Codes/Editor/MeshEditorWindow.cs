using System.IO;
using Codes.Base;
using UnityEditor;
using UnityEngine;

namespace Codes.Editor {
    public class MeshEditorWindow : EditorWindow {
        [MenuItem("CustomTools/MeshEditor")]
        public static void ShowWindow() {
            var window = GetWindow<MeshEditorWindow>();
            window.titleContent = new GUIContent("MeshEditor");
            window.Show();
        }

        private static string[] s_pageNames = {
            "Convert To Triangles",
            "Convert To Points"
        };

        private static string SOURCE_VERTEX_NUM_PATTERN = "Source Vertex Num:  {0}";
        private static string TARGET_VERTEX_NUM_PATTERN = "Target Vertex Num:  {0}";

        private int m_currentPageIndex;

        private Mesh m_sourceMesh;
        private Mesh m_targetMesh;

        private void OnGUI() {
            DrawPageBars();
            DrawMainPage();
        }

        private void DrawPageBars() {
            EditorGUILayout.BeginHorizontal(GUILayout.Height(30));
            m_currentPageIndex = GUILayout.Toolbar(m_currentPageIndex, s_pageNames);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawMainPage() {
            switch (m_currentPageIndex) {
                case 0:
                    DrawTrianglePage();
                    break;
                case 1:
                    DrawVertexPage();
                    break;
            }
        }

        private void DrawTrianglePage() {
            var meshObj = EditorGUILayout.ObjectField("Source Mesh", m_sourceMesh, typeof(Mesh), false);

            int sourceVertexNum = 0;
            int targetVertexNum = 0;
            if (meshObj) {
                m_sourceMesh = (Mesh)meshObj;
                sourceVertexNum = m_sourceMesh.vertexCount;
                targetVertexNum = sourceVertexNum * 3;
            }

            EditorGUILayout.LabelField(string.Format(SOURCE_VERTEX_NUM_PATTERN, sourceVertexNum));
            EditorGUILayout.LabelField(string.Format(TARGET_VERTEX_NUM_PATTERN, targetVertexNum));
            if (targetVertexNum > 65535) {
                EditorGUILayout.HelpBox("Vertex count is too large.", MessageType.Error);
            }

            if (m_targetMesh) {
                EditorGUILayout.ObjectField(m_targetMesh, typeof(Mesh), false);
                if (GUILayout.Button("SaveTo", GUILayout.Height(30))) {
                    var folderPath = EditorUtility.SaveFilePanel("Save to", "Assets/MeshOutput", "Result", "asset");
                    if (folderPath.Length != 0) {
                        var strs = folderPath.Split("/Assets/");
                        folderPath = "Assets/" + strs[^1];
                        if (AssetDatabase.LoadAssetAtPath<Mesh>(folderPath)) {
                            AssetDatabase.DeleteAsset(folderPath);
                        }
                        
                        AssetDatabase.CreateAsset(m_targetMesh, folderPath);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }
            }

            if (GUILayout.Button("Convert", GUILayout.Height(30))) {
                if (m_sourceMesh) {
                    var result = m_sourceMesh.GenerateSeparateTriangleMesh();
                    result.name = "Result";
                    m_targetMesh = result;
                }
            }
        }

        private void DrawVertexPage() {
            EditorGUILayout.HelpBox("To be continued..", MessageType.Info);
        }
    }
}