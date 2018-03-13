using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GameController))]
public class GameControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Show default inspector property editor
        DrawDefaultInspector();

        GameController myTarget = (GameController)target;
        if (GUILayout.Button("Kill Player"))
        {
            myTarget.KillPlayer();
        }
    }
}