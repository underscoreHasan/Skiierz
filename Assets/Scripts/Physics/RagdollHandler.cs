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

    public SkinnedMeshRenderer animatedRenderer;
    public SkinnedMeshRenderer animatedGoggleRenderer;
    public SkinnedMeshRenderer aniamtedBeanieRenderer;

    public void ActivateRagdoll(Vector3 velocity) {
        ActivateRecursive(ragdollBoneRoot, animatedBoneRoot, velocity);

        ragdollBoard.position = animatedBoard.position;
        Collider boardCollider = ragdollBoard.GetComponent<Collider>();
        boardCollider.enabled = true;

        Rigidbody boardPhys = ragdollBoard.GetComponent<Rigidbody>();
        boardPhys.isKinematic = false;
        boardPhys.velocity = velocity;

        ragdollRenderer.enabled = true;
        ragdollGoggleRenderer.enabled = true;
        ragdollBeanieRenderer.enabled = true;

        animatedRenderer.enabled = false;
        animatedGoggleRenderer.enabled = false;
        aniamtedBeanieRenderer.enabled = false;
    }

    private void ActivateRecursive(Transform current, Transform target, Vector3 velocity) {

        // some children wont have colliders or physics, we can just skip these
        try {
            current.rotation = target.rotation;

            Collider collider = current.GetComponent<Collider>();
            collider.enabled = true;

            Rigidbody rigidbody = current.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
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
            ActivateRecursive(currentChildren.Pop(), targetChildren.Pop(), velocity);
        }
    }
}
