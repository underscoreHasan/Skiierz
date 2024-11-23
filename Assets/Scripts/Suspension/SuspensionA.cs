using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspensionA : MonoBehaviour {
    public Rigidbody parentPhys;
    public float raycastdist;
    public float forcemultiplier;

	// Use this for initialization
	void Start () {
		if (parentPhys == null)
        {
            Debug.LogError("missing parentPhys!");
        }
	}

    // Update is called once per frame
    void FixedUpdate() {
        RaycastHit hitOut;
        if (Physics.Raycast(transform.position, transform.rotation * Vector3.down, out hitOut, raycastdist))
        {
            Vector3 upforce = transform.rotation * (Vector3.up * (1.0f / (Mathf.Max(0.01f, hitOut.distance))));
            parentPhys.AddForce(upforce * forcemultiplier, ForceMode.Acceleration);
        }
	}
}
