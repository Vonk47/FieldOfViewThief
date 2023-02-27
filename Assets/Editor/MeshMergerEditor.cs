#if UNITY_EDITOR
using Game.Utility;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [CustomEditor(typeof(MeshMerger))]
    public class MeshMergerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            MeshMerger merger = (MeshMerger)target;

            if (GUILayout.Button("Merge meshes"))
                merger.MergeMeshes();

            if (GUILayout.Button("Delete Children"))
                merger.DeleteChilden();
        }
    }
}
#endif
