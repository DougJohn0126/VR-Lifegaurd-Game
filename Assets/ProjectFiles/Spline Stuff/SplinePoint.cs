using UnityEditor;
using UnityEngine;

public class SplinePoint : MonoBehaviour
{

    public float distanceToNextPoint = 0;
    public Spline parentSpline;
    public Vector3 Position => transform.position;
    public Vector3 Forward => transform.forward;

    private Vector3 cachedPosition;
    private Vector3 cachedRotation;

    void Awake()
    {
        //Disable this if there's a performance issue
        parentSpline = GetComponentInParent<Spline>();
        CalculateDistanceToNextPoint();
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!parentSpline)
        {
			return;
        }
		Handles.color = parentSpline.splineColor;
		Handles.DrawWireDisc(transform.position, transform.forward,0.5f);
		parentSpline.DrawStep();
    }

    
#endif


    private void OnValidate()
    {
		parentSpline = GetComponentInParent<Spline>();
        CalculateDistanceToNextPoint();
    }

    public float GetDistanceToNext(){
        if(cachedPosition != Position || cachedRotation != Forward){
            CalculateDistanceToNextPoint();
        }
        return distanceToNextPoint;
    }

    public void CalculateDistanceToNextPoint()
    {
        // Debug.Log("This sibling index of " + transform.name + " " + transform.GetSiblingIndex());
        cachedPosition = Position;
        cachedRotation = Forward;
        distanceToNextPoint = 0;
        if(!parentSpline){
            parentSpline = GetComponentInParent<Spline>();
        }
        if(parentSpline == null){
            return;
        }
        if (transform.GetSiblingIndex() >= parentSpline.transform.childCount - 1)
        {
            return;
        }

        Transform nextPoint = parentSpline.transform.GetChild(transform.GetSiblingIndex() + 1);
        for (int i = 0; i < parentSpline.samplingValues; i++)
        {
            distanceToNextPoint += Vector3.Distance(
                BezierUtilities.BezierLerp(transform, nextPoint, (float)i / (float)parentSpline.samplingValues),
                BezierUtilities.BezierLerp(transform, nextPoint, ((float)i + 1) / (float)parentSpline.samplingValues)
            );
        }

    }

}
