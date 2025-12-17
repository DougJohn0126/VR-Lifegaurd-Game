using UnityEngine;
using UnityEditor;

public class Spline : MovementStep
{

	[Space]
	public Color splineColor = Color.white;


	[SerializeField]
	[Header("Animation Options")]
	[Tooltip("If ON the spline will take a SET time")]
	public bool OverrideSpeedWithTime = false;
	[SerializeField]
	private AnimationCurve animationMovementCurve;
	[Space]
	[Space]

	public int samplingValues = 10;

	public SplinePoint[] points;

	public float totalSplineDistance = 0;

	public float GetSpeedMultiplier(float t)
	{
		if (OverrideSpeedWithTime)
		{
			return animationMovementCurve.Evaluate(t);
		}
		return t;
	}

	protected void SetDefaultCurve()
	{
		if (animationMovementCurve == null)
		{
			animationMovementCurve = new AnimationCurve();
		}
		animationMovementCurve.ClearKeys();
		animationMovementCurve.AddKey(new Keyframe(0, 1f));
		animationMovementCurve.AddKey(new Keyframe(1f, 1f));
	}

	void Awake()
	{
		//Disable this if it was a performance issue. 
		if (points.Length == 0)
		{
			ReferenceSplinePoints();
		}
	}

	void Start()
	{
		totalSplineDistance = 0;
		foreach (SplinePoint item in points)
		{
			totalSplineDistance += item.distanceToNextPoint;
		}

	}

	override
	public void DrawStep()
	{
		DrawSplineLine(2f);
	}

#if UNITY_EDITOR
	void OnDrawGizmosSelected()
	{
		DrawSplineLine(3f);
	}
#endif


	public void DrawSplineLine(float thickness = 1f)
	{
#if UNITY_EDITOR
		for (int i = 0; i < transform.childCount - 1; i++)
		{
			Vector3 posA = transform.GetChild(i).position;
			Vector3 posB = transform.GetChild(i + 1).position;
			float distance = Vector3.Distance(posA, posB);
			Vector3 controlPointA = posA + transform.GetChild(i).forward * distance / 2;
			Vector3 controlPointB = posB + (transform.GetChild(i + 1).forward * (-distance / 2));
			// Handles.color = splineColor;
			Handles.DrawBezier(
				posA,
				posB,
				controlPointA,
				controlPointB,
				splineColor,
				EditorGUIUtility.whiteTexture,
				thickness
			);
			DrawSphere(transform.GetChild(i).position, 0.15f);
		}
#endif
	}

	void DrawSphere(Vector3 position, float radius = 0.1f)
	{
#if UNITY_EDITOR
		Handles.SphereHandleCap(-1, position, Quaternion.identity, radius, EventType.Repaint);
#endif
	}

	public void Reset()
	{
		//Chose a random color
		SetDefaultCurve();
		if (splineColor == Color.white)
		{
			splineColor = new Color(
				Random.Range(0f, 1f),
				Random.Range(0f, 1f),
				Random.Range(0f, 1f),
				1.0f
			);
		}
		ReferenceSplinePoints();
	}

	override
	protected void OnValidate()
	{
		base.OnValidate();
		ReferenceSplinePoints();
		UpdatePointDistance();
	}


	public void ReferenceSplinePoints()
	{
		points = new SplinePoint[transform.childCount];
		for (int i = 0; i < points.Length; i++)
		{
			points[i] = transform.GetChild(i).GetComponent<SplinePoint>();
			points[i].GetComponent<SplinePoint>().parentSpline = this;
		}
	}

	public void UpdatePointDistance()
	{
		totalSplineDistance = 0;
		foreach (SplinePoint item in points)
		{
			item.CalculateDistanceToNextPoint();
			totalSplineDistance += item.distanceToNextPoint;
		}
	}

	public SplinePoint GetLastPoint()
	{
		if (points.Length == 0)
		{
			GameObject newPoint = new GameObject("Point");
			newPoint.transform.position = transform.position;
			newPoint.transform.forward = transform.forward;
			newPoint.AddComponent<SplinePoint>();
			ReferenceSplinePoints();
			return newPoint.GetComponent<SplinePoint>();
		}
		return points[^1];
	}

	public override float GetLenght()
	{
		return totalSplineDistance;
	}
}
