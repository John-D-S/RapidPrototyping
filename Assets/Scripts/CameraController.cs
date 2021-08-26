using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject objectToFollow;
    [SerializeField] private float heightAboveObject;
    [SerializeField] private Vector3 targetRotation;

    private void Update()
    {
        gameObject.transform.position = objectToFollow.transform.position + Vector3.up * heightAboveObject;
        gameObject.transform.rotation = Quaternion.Euler(targetRotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(objectToFollow.transform.position, 0.25f);
    }
}
