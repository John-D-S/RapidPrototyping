using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

using UnityEngine;

public class PlayerInputTarget : CarTarget
{
	private float currentThrottle = 0;
	private float throttleModifier = 0;
	public override void ReachTarget(ref Car _car)
	{
		if(controlledCars.Contains(_car))
		{
			throttleModifier = 0;
		}
	}

	private Vector3 MousePositionOverTerrain()
	{
		RaycastHit hit = new RaycastHit();
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000))
		{
			return hit.point;
		}
		return transform.position;
	}

	private void UpdateTargetPosition()
	{
		gameObject.transform.position = MousePositionOverTerrain();
	}

	private void Update()
	{
		UpdateTargetPosition();
		if(Input.GetMouseButton(0))
		{
			currentThrottle = ThrottlePercentage * throttleModifier;
		}
		else
		{
			currentThrottle = 0;
		}
	}

	private void FixedUpdate()
	{
		foreach(Car car in controlledCars)
		{
			car.DriveForward(currentThrottle);
		}
	}
}
