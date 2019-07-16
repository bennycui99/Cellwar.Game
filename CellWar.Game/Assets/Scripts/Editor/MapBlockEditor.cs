using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapBlockEditor : EditorWindow
{
    GameObject mBlockObject;
    int Aaxis;
    int Baxis;
    int Caxis;

    [MenuItem("Tools/Map/BlockBrush")]
    static void SpawnBlock()
    {
        EditorWindow editorWindow = GetWindow(typeof(MapBlockEditor));
        editorWindow.Show();

    }
    private void OnGUI()
    {
        GUILayout.Label("SELECT Spawn Cube.");
        mBlockObject = Selection.activeObject as GameObject;
    }
}
