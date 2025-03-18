using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RockFloorStick))]
public class RockFloorStickEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        RockFloorStick rfs = (RockFloorStick)target;
        if (GUILayout.Button("Stick To Surface")) {
            rfs.StickToSurfaceNormal();
        }
    }
}
