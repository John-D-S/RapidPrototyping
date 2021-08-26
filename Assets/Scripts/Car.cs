using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Car : MonoBehaviour
{
    [SerializeField] private CarTarget currentCarTarget;
    private CarTarget lastCarTarget;
    public CarTarget CurrentCarTarget => currentCarTarget;
    public CarTarget LastCarTarget => lastCarTarget;
    [Header("-- Controlling Forces --")]
    [SerializeField] private float maxDriveForce;
    public float MaxDriveForce => maxDriveForce;
    [SerializeField] private float steerTorque;
    [SerializeField] private float nearTargetDistance = 3f;
    [Header("-- Misc Physics forces --")]
    [SerializeField] private float balanceTorque;
    [SerializeField] private float sidewaysFriction;
    
    private bool IsCloseEnoughToTarget => Vector3.Distance(currentCarTarget.transform.position, transform.position) < nearTargetDistance;
    private Vector3 DirectionToTarget => (currentCarTarget.transform.position - transform.position).normalized;

    private bool onGround;
    private Vector3 averageContactNormal;

    public float PercentageToNextTarget
    {
        get
        {
            if(lastCarTarget)
            {
                float distanceToLastTarget = Vector3.Distance(transform.position, lastCarTarget.transform.position);
                float distanceToNextTarget = Vector3.Distance(transform.position, currentCarTarget.transform.position);
                float totalDistance = distanceToLastTarget + distanceToNextTarget;
                return distanceToLastTarget / totalDistance;
            }
            return 1;
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        onGround = false;
    }

    private void OnCollisionStay(Collision other)
    {
        onGround = true;
        Vector3 averageNormal = new Vector3();
        List<ContactPoint> contactPoints = other.contacts.ToList();
        for(int i = 0; i < other.contactCount; i++)
        {
            averageNormal += contactPoints[i].normal;
        }
        averageContactNormal = averageNormal / other.contactCount;
    }

    private Rigidbody rb;
    
    public void DriveForward(float _throttlePercentage)
    {
        float driveForce = maxDriveForce * Mathf.Clamp01(_throttlePercentage);
        rb.AddForce(transform.forward * driveForce);
    }

    public void SetCarTarget(CarTarget _target)
    {
        currentCarTarget.controlledCars.Remove(this);
        currentCarTarget = _target;
        currentCarTarget.controlledCars.Add(this);
    }
    
    private void SteerTowardsTarget()
    {
        if(rb)
        {
            rb.AddRelativeTorque(Vector3.up * (Vector3.Dot(transform.right, DirectionToTarget) * steerTorque));
        }
    }
    
    private void KeepUpright()
    {
        if(onGround)
        {
            Quaternion rot = Quaternion.FromToRotation(transform.up, averageContactNormal);
            rb.AddRelativeTorque(new Vector3(rot.x, rot.y, rot.z) * balanceTorque);
        }
    }
    
    private void ApplyFriction()
    {
        rb.AddForce(-transform.right * (transform.InverseTransformDirection(rb.velocity).x * sidewaysFriction));
    }
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetCarTarget(currentCarTarget);
    }

    private void FixedUpdate()
    {
        KeepUpright();
        SteerTowardsTarget();
        ApplyFriction();
        if(IsCloseEnoughToTarget)
        {
            Car thisCar = this;
            currentCarTarget.ReachTarget(ref thisCar);
        }
    }
}
