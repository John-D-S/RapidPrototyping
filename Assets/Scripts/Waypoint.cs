using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;

using Random = UnityEngine.Random;

public class Waypoint : CarTarget
{
	[SerializeField] private List<Waypoint> nextWaypoints;
	
	public override void ReachTarget(ref Car _car)
	{
		_car.SetCarTarget(nextWaypoints[Random.Range(0, nextWaypoints.Count)]);
	}

	private void FixedUpdate()
	{
		foreach(Car controlledCar in controlledCars)
		{
			float forwardThrottle = Mathf.Lerp(controlledCar.LastCarTarget.ThrottlePercentage, controlledCar.CurrentCarTarget.ThrottlePercentage, controlledCar.PercentageToNextTarget);
			controlledCar.DriveForward(forwardThrottle);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		
		void DrawArrow(Vector3 tailPos, Vector3 headPos, float arrowHeadAngle = 0.25f)
		{
			Gizmos.DrawLine(tailPos, headPos);
       
			Vector3 right = Quaternion.LookRotation(headPos - tailPos) * Quaternion.Euler(0,180+arrowHeadAngle,0) * new Vector3(0,0,1);
			Vector3 left = Quaternion.LookRotation(headPos - tailPos) * Quaternion.Euler(0,180-arrowHeadAngle,0) * new Vector3(0,0,1);
			Gizmos.DrawRay(headPos, right * 1);
			Gizmos.DrawRay(headPos, left * 1);
		}

		foreach(Waypoint nextWaypoint in nextWaypoints)
		{
			DrawArrow(gameObject.transform.position, nextWaypoint.transform.position);
		}
	}
}
