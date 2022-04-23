using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProceduralTerrain))]
public class ProceduralTerrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ProceduralTerrain terrain = (ProceduralTerrain)target;

        DrawDefaultInspector();
        if(GUILayout.Button("Generate"))
        {
            terrain.UpdateMeshes();
        }
    }
}
