using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(VolumeApply))]
public class VolumeApplyEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        VolumeApply vApply = (VolumeApply)target;
        if (GUILayout.Button("Spawn Objects")) {
            vApply.PlaceThatShit();
        }
    }
}
