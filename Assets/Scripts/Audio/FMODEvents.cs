using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Snow Kick")]
    [field: SerializeField] public EventReference snowKick { get; private set; }

    [field: Header("Skiing")]
    [field: SerializeField] public EventReference skiing { get; private set; }

    [field: Header("Jump")]
    [field: SerializeField] public EventReference jump { get; private set; }

    [field: Header("Land")]
    [field: SerializeField] public EventReference land { get; private set; }

    [field: Header("Ambience")]
    [field: SerializeField] public EventReference ambience { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one FMODEvents in the scene");
        }
        instance = this;
    }
}
