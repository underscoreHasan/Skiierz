using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TreeRandomizer))]
public class TreeRandomizerEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        TreeRandomizer tRand = (TreeRandomizer)target;
        if (GUILayout.Button("Randomize")) {
            tRand.Randomize();
        }
        if (GUILayout.Button("Emplace")) {
            tRand.Emplace();
        }
    }
}
