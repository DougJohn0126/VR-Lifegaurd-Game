using UnityEngine;
using Oculus.Haptics;

public class PlayerController : MonoBehaviour
{
    public OVRInput.Controller targetController = OVRInput.Controller.RTouch; // Or LTouch
    public float vibrationAmplitude = 0.2f; // 0.0 to 1.0
    public float vibrationFrequency = 1.0f; // Set to 1.0 to enable haptics

    private int mGuessCount = 0;
    private bool mGameIsRunning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mGameIsRunning)
        {
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                mGuessCount++;
                OVRInput.SetControllerVibration(vibrationFrequency, vibrationAmplitude, targetController);
                Invoke("StopRightControllerVibration", 0.1f); // Stop after a duration
            }

            if (OVRInput.GetDown(OVRInput.Button.Two))
            {
                mGuessCount--;
                OVRInput.SetControllerVibration(vibrationFrequency, vibrationAmplitude / 2, targetController);
                Invoke("StopRightControllerVibration", 0.1f); // Stop after a duration
            }

        }
    }

    public int GetPlayerGuessCount()
    {
        return mGuessCount;
    }
    public void SetGameIsRunning()
    {
        mGameIsRunning = true;
    }
    void StopRightControllerVibration()
    {
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }
}
