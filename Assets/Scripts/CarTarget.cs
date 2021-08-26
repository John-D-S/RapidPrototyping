using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarTarget : MonoBehaviour
{
    [System.NonSerialized] public List<Car> controlledCars;
    [SerializeField, Range(0, 1)] private float throttlePercentage;
    public float ThrottlePercentage => throttlePercentage;
    public abstract void ReachTarget(ref Car _car);
}