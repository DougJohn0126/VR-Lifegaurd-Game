using UnityEngine;
using UnityEngine.Splines;

public class RouteFollower : MonoBehaviour
{
    public SplineContainer IntroSpline;
    public SplineContainer targetSpline;
    public float moveSpeed = 1f;
    public bool GameStarted = false;
    private bool mEndReached = false;

    private float normalizedTime = 0f;
    public float introSpeed = 1f;
    private float currentSplinePosition = 0f; // Normalized 0-1

    private void Start()
    {
        transform.position = IntroSpline.EvaluatePosition(0);
        transform.rotation = Quaternion.Euler(0, 48, 0);
        this.GetComponent<OVRCameraRig>().transform.rotation = Quaternion.Euler(0, 48, 0);
        transform.eulerAngles= (new Vector3 (0, 48, 0));
        Debug.Log(transform.rotation);
    }

    void Update()
    {
        if (GameStarted)
        {
            IntroAnimate();
        }

        if (mEndReached)
        {
            if (targetSpline == null) return;

            // Get input
            Vector2 thumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            float input = thumbstick.x;

            // Update spline position based on input and speed
            currentSplinePosition += input * 0.05f * moveSpeed * Time.deltaTime;
            currentSplinePosition = Mathf.Clamp01(currentSplinePosition); // Keep between 0 and 1

            // Evaluate position and tangent on the spline
            Vector3 newPosition = targetSpline.EvaluatePosition(currentSplinePosition);
            //Vector3 newTangent = targetSpline.EvaluateTangent(currentSplinePosition);

            // Apply position and rotation to the GameObject
            transform.position = newPosition;
            //transform.rotation = Quaternion.LookRotation(newTangent);
        }

    }

    private void  IntroAnimate ()
    {
        if (IntroSpline == null || IntroSpline.Spline == null)
        {
            return;
        }

        normalizedTime += ((Time.deltaTime) * introSpeed) / IntroSpline.Spline.GetLength();
        if (normalizedTime > 1f)
        {
            mEndReached = true;
            GameStarted = false;
        }

        Vector3 currentPosition = IntroSpline.Spline.EvaluatePosition(normalizedTime);
        transform.position = currentPosition;
    }

    public void SetGameStarted()
    {
        GameStarted = true;
    }
}
