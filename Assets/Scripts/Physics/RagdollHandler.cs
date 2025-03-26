using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollHandler : MonoBehaviour
{
    public Transform animatedBoneRoot;
    public Transform ragdollBoneRoot;

    public Transform animatedBoard;
    public Transform ragdollBoard;

    public SkinnedMeshRenderer ragdollRenderer;
    public SkinnedMeshRenderer ragdollGoggleRenderer;
    public SkinnedMeshRenderer ragdollBeanieRenderer;
    public MeshRenderer ragdollBoardRenderer;

    public SkinnedMeshRenderer animatedRenderer;
    public SkinnedMeshRenderer animatedGoggleRenderer;
    public SkinnedMeshRenderer animatedBeanieRenderer;
    public MeshRenderer animatedBoardRenderer;


    public void ToggleRagdoll(Vector3 velocity) {
        ToggleRecursive(ragdollBoneRoot, animatedBoneRoot, velocity);

        ragdollBoard.position = animatedBoard.position;
        ragdollBoard.rotation = animatedBoard.rotation;
        Collider boardCollider = ragdollBoard.GetComponent<Collider>();
        boardCollider.enabled ^= true;

        Rigidbody boardPhys = ragdollBoard.GetComponent<Rigidbody>();
        boardPhys.isKinematic ^= true;
        boardPhys.velocity = velocity;

        ragdollRenderer.enabled ^= true;
        ragdollGoggleRenderer.enabled ^= true;
        ragdollBeanieRenderer.enabled ^= true;
        ragdollBoardRenderer.enabled ^= true;

        animatedRenderer.enabled ^= true;
        animatedGoggleRenderer.enabled ^= true;
        animatedBeanieRenderer.enabled ^= true;
        animatedBoardRenderer.enabled ^= true;
    }

    private void ToggleRecursive(Transform current, Transform target, Vector3 velocity) {

        // some children wont have colliders or physics, we can just skip these
        try {
            current.position = target.position;
            current.rotation = target.rotation;

            Collider collider = current.GetComponent<Collider>();
            collider.enabled ^= true;

            Rigidbody rigidbody = current.GetComponent<Rigidbody>();
            rigidbody.isKinematic ^= true;
            rigidbody.velocity = velocity;

        } catch (Exception _) {
            // do nothing
        }
        
        Stack<Transform> currentChildren = new Stack<Transform>();
        Stack<Transform> targetChildren = new Stack<Transform>();
        foreach (Transform child in current) {
            currentChildren.Push(child);
        }
        foreach (Transform child in target) {
            targetChildren.Push(child);
        }

        while (currentChildren.Count > 0) {
            ToggleRecursive(currentChildren.Pop(), targetChildren.Pop(), velocity);
        }
    }
}
