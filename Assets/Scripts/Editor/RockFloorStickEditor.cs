using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RockFloorStickEditor : Editor
{
    [CustomEditor(typeof(RockFloorStick))]
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        RockFloorStick rfs = (RockFloorStick)target;
        if (GUILayout.Button("Stick To Surface")) {
            rfs.StickToSurfaceNormal();
        }
    }
}
