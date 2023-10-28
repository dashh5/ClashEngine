using UnityEngine;
using System.Collections;


// The 'DetectTouchMovement' class is a Unity script used to detect and calculate touch 
// movements for gestures like pinch and turn on a touch screen. This script is meant 
// to be used in mobile applications or any Unity project that utilizes a touch screen 
// for input.

// The class has static public variables 'turnAngleDelta', 'turnAngle', 'pinchDistanceDelta', 
// and 'pinchDistance', which store the angular change, total angle, distance change, and 
// total distance between two touch points on the screen, respectively.

// The 'Calculate' method performs the calculations and is intended to be called in the 
// 'LateUpdate' method. It checks if two fingers are touching the screen and if at least 
// one of them has moved. If these conditions are met, the method calculates the pinch 
// distance and the turn angle between the two touch points. If the calculated pinch 
// distance delta or turn angle delta is above the specified minimum thresholds, they 
// are adjusted by their respective ratios.

// The 'Angle' method calculates the angle between two points in screen space, and adjusts
// the result based on the cross product to ensure it provides a correct angle in all 
// quadrants.

// This script does not handle the touch input directly, but rather provides calculations 
// based on the touch input, which can then be used in other parts of the application to 
// implement features like zooming, rotating, or panning.

public class DetectTouchMovement : MonoBehaviour
{
	const float pinchTurnRatio = Mathf.PI / 2;
	const float minTurnAngle = 0;

	const float pinchRatio = 1;
	const float minPinchDistance = 0;

	const float panRatio = 1;
	const float minPanDistance = 0;

	///   The delta of the angle between two touch points
	static public float turnAngleDelta;
	///   The angle between two touch points
	static public float turnAngle;
	///   The delta of the distance between two touch points that were distancing from each other
	static public float pinchDistanceDelta;
	///   The distance between two touch points that were distancing from each other
	static public float pinchDistance;

	///   Calculates Pinch and Turn - This should be used inside LateUpdate
	static public void Calculate()
	{
		pinchDistance = pinchDistanceDelta = 0;
		turnAngle = turnAngleDelta = 0;

		// if two fingers are touching the screen at the same time ...
		if (Input.touchCount == 2)
		{
			Touch touch1 = Input.touches[0];
			Touch touch2 = Input.touches[1];

			// ... if at least one of them moved ...
			if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
			{
				// ... check the delta distance between them ...
				pinchDistance = Vector2.Distance(touch1.position, touch2.position);
				float prevDistance = Vector2.Distance(touch1.position - touch1.deltaPosition,
													  touch2.position - touch2.deltaPosition);
				pinchDistanceDelta = pinchDistance - prevDistance;

				// ... if it's greater than a minimum threshold, it's a pinch!
				if (Mathf.Abs(pinchDistanceDelta) > minPinchDistance)
				{
					pinchDistanceDelta *= pinchRatio;
				}
				else
				{
					pinchDistance = pinchDistanceDelta = 0;
				}

				// ... or check the delta angle between them ...
				turnAngle = Angle(touch1.position, touch2.position);
				float prevTurn = Angle(touch1.position - touch1.deltaPosition,
									   touch2.position - touch2.deltaPosition);
				turnAngleDelta = Mathf.DeltaAngle(prevTurn, turnAngle);

				// ... if it's greater than a minimum threshold, it's a turn!
				if (Mathf.Abs(turnAngleDelta) > minTurnAngle)
				{
					turnAngleDelta *= pinchTurnRatio;
				}
				else
				{
					turnAngle = turnAngleDelta = 0;
				}
			}
		}
	}

	static private float Angle(Vector2 pos1, Vector2 pos2)
	{
		Vector2 from = pos2 - pos1;
		Vector2 to = new Vector2(1, 0);

		float result = Vector2.Angle(from, to);
		Vector3 cross = Vector3.Cross(from, to);

		if (cross.z > 0)
		{
			result = 360f - result;
		}

		return result;
	}
}