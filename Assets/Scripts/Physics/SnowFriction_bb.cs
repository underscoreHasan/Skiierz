using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowFriction_bb : MonoBehaviour {

    private Rigidbody _phys;

    // Use this for initialization
    void Start () {
        _phys = GetComponent<Rigidbody>();
    }

    // In-editor visualization
    void OnDrawGizmos() {
        
        // only draw gizmos if game is running
        if (!Application.isPlaying) {
            return;
        }


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Update is called in a fixed manner
    void FixedUpdate() {

    }
}
