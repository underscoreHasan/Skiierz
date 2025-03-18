using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
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
        Gizmos.DrawCube(transform.position, transform.lossyScale);
    }

    public void PlaceThatShit() {
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
            bool didHit = Physics.Raycast(transform.position + rayCastOffset, Vector3.down, out hit);

            // didnt get anything, try again with a different case
            if (!didHit) {
                --i;
                continue;
            }

            GameObject placedThing = Instantiate(toInstantiate, hit.point, Quaternion.identity);
            placedThing.transform.localScale = Vector3.one * Random.Range(objectScaleMin, objectScaleMax);

            RockFloorStick stickScript;
            try {
                stickScript = placedThing.GetComponent<RockFloorStick>();
                stickScript.StickToSurfaceNormal();
            } catch (UnityException _) {
                // whatever its fine
            }
        }
    }
}
