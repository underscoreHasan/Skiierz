using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;

public class VolumeApply : MonoBehaviour
{
    public Color color;
    public int objectCount;
    public float objectScaleMin;
    public float objectScaleMax;
    public GameObject[] toSpawn;
    private void OnDrawGizmos() {
        Gizmos.color = color;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
        Gizmos.matrix = Matrix4x4.identity;
    }

    public void PlaceThatShit() {
        
        // Undo.IncrementCurrentGroup();

        List<GameObject> allThatsSpawned = new List<GameObject>();

        int failsafe = objectCount + 128;
        for (int i = 0; i < objectCount; i++) {
            if (--failsafe <= 0) {
                // dont do this forever
                break;
            }

            GameObject toInstantiate = toSpawn[Random.Range(0, toSpawn.Length)];

            Vector3 rayCastOffset = new Vector3(
                Random.Range(-0.5f, 0.5f) * transform.lossyScale.x, 
                0.0f,
                Random.Range(-0.5f, 0.5f) * transform.lossyScale.z);

            RaycastHit hit;
            bool didHit = Physics.Raycast(transform.position + transform.rotation * rayCastOffset, transform.rotation * Vector3.down, out hit);

            // didnt get anything, or hit something we already spawned, try again with a different case
            if (!didHit || allThatsSpawned.Contains(hit.transform.gameObject)) {
                --i;
                continue;
            }

            GameObject placedThing = Instantiate(toInstantiate, hit.point, Quaternion.identity);
            // Undo.RegisterCreatedObjectUndo(placedThing, "Spawn Objects");

            placedThing.transform.localScale *= Random.Range(objectScaleMin, objectScaleMax);
            allThatsSpawned.Add(placedThing);

            // IF THIS IS A ROCK
            RockFloorStick stickScript = placedThing.GetComponent<RockFloorStick>();
            if (stickScript != null) {
                stickScript.StickToSurfaceNormal();
            }

            // IF THIS IS A TREE
            TreeRandomizer treeScript = placedThing.GetComponent<TreeRandomizer>();
            if (treeScript != null) {
                treeScript.Emplace();
                treeScript.Randomize();
            }
        } // END FOR LOOP

        // create a quick parent to contain all these things
        Vector3 posAccum = Vector3.zero;
        foreach (GameObject obj in allThatsSpawned) {
            posAccum += obj.transform.position;
        }
        posAccum /= (float)allThatsSpawned.Count;

        GameObject allParent = new GameObject("Cluster");
        // Undo.RegisterCreatedObjectUndo(allParent, "Cluster Parent");

        allParent.transform.position = posAccum;
        foreach (GameObject obj in allThatsSpawned) {
            obj.transform.parent = allParent.transform;
        }

        // Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
    }
}
