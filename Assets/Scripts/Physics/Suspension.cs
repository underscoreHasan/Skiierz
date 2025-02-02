using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour {

    public Rigidbody parentPhys;
    public float neutralOffset;
    public float maxOffset;
    public float spring;
    public float damper;
    public float minAngledForceFactor;

    private Vector3 _dbgSpringForce;
    private Vector3 _dbgDamperForce;
    private Vector3 _dbgAxialVel;

    public bool isGrounded {
        get { return _isGrounded; }
    }

    public Vector3 normal {
        get { return _normal; }
    }

    private float _currentOffset;
    private bool _isGrounded;
    private Vector3 _normal;

    // Use this for initialization
    void Start () {
		if (parentPhys == null) {
            Debug.LogError("missing parentPhys!");
        }
	}

    // In-editor visualization
    void OnDrawGizmos() {
        // draw point from which rays are being cast
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.2f);

        // draw suspension "slider" line
        Vector3 localSpringNeutral = Vector3.down * neutralOffset;
        Vector3 localSpringTop = Vector3.down * (neutralOffset - maxOffset);
        // Vector3 localSpringBottom = Vector3.down * (neutralOffset + maxOffset);

        Vector3 globalSpringNeutral = transform.TransformPoint(localSpringNeutral);
        Vector3 globalSpringTop = transform.TransformPoint(localSpringTop);
        // Vector3 globalSpringBottom = transform.TransformPoint(localSpringBottom);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(globalSpringNeutral, Vector3.one * 0.25f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(globalSpringNeutral, globalSpringTop);
        Gizmos.color = Color.blue;
        // Gizmos.DrawLine(globalSpringNeutral, globalSpringBottom);

        // everything after here only working while playing
        if (!Application.isPlaying) {
            return;
        }

        // draw point which suspension is "touching" the ground
        Vector3 offsetPoint = Vector3.down * (neutralOffset + _currentOffset);
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(transform.TransformPoint(offsetPoint), 0.2f);

        // draw upforce applied by node
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(globalSpringNeutral, globalSpringNeutral + _dbgSpringForce * 0.05f);

        // draw dampening force applied by node
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(globalSpringNeutral, globalSpringNeutral + _dbgDamperForce * 0.05f);

        // draw precieved axial velocity
        Gizmos.color = Color.green;
        Gizmos.DrawLine(globalSpringNeutral, globalSpringNeutral + _dbgAxialVel * 0.5f);
    }

    // Update is called every frame
    void Update() {
        
    }

    // Update is called in a fixed manner
    void FixedUpdate() {

        // handle suspension touching the ground
        RaycastHit hitOut;
        _isGrounded = Physics.Raycast(transform.position, transform.rotation * Vector3.down, out hitOut, neutralOffset);
        _normal = hitOut.normal;
        if (_isGrounded) {
            _currentOffset = Mathf.Max(-maxOffset, (hitOut.distance - neutralOffset)); 
        } else {
            _currentOffset = 0.0f; // this ensures no forces are applied
        }

        // if NOT grounded, no forces required
        if (!_isGrounded) {
            _normal = Vector3.zero;
            return;
        }

        // we could be raycasting the ground at an angle, the higher the angle, the less force we apply
        Vector3 upVector = transform.rotation * Vector3.up;
        float angledForceFactor = Mathf.Max(minAngledForceFactor, Vector3.Dot(hitOut.normal, upVector));

        Vector3 globalSpringAnchor = transform.TransformPoint(Vector3.down * neutralOffset);

        // apply spring forces
        Vector3 localSpringForce = Vector3.up * spring * -_currentOffset;
        Vector3 totalSpringForce = transform.rotation * localSpringForce * angledForceFactor;
        _dbgSpringForce = totalSpringForce;
        parentPhys.AddForceAtPosition(totalSpringForce, globalSpringAnchor, ForceMode.Acceleration);
 
        // apply dampening forces
        Vector3 axialVelocity = Vector3.Project(
            parentPhys.GetPointVelocity(globalSpringAnchor), 
            parentPhys.transform.rotation * Vector3.up);
        _dbgAxialVel = axialVelocity;
        Vector3 damperForce = -axialVelocity * damper;
        _dbgDamperForce = damperForce;
        parentPhys.AddForceAtPosition(damperForce, globalSpringAnchor, ForceMode.Acceleration);
	}

    
}
