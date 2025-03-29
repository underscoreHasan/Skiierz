using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance {
        get { return _instance; }
    }

    private static CursorManager _instance;
    public bool initiallyLockCursor;

    private void Awake() {
        if (_instance == null) {
            _instance = this;
        } else {
            Debug.LogError("multiple cursor managers");
            Destroy(this);
        }
    }

    private void Start() {
        if (initiallyLockCursor) {
            CaptureCursor();
        } else {
            ReleaseCursor();
        }
    }

    public void CaptureCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ReleaseCursor() {
        Cursor.lockState= CursorLockMode.None;
        Cursor.visible = true;
    }
}
