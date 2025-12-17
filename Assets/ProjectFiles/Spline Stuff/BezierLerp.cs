using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierLerp : MonoBehaviour{
    public GameObject objectA;
    public GameObject objectB;

    [Range(0,1)]
    public float t = 0;
    public float distanceDivider = 2;

    void OnDrawGizmos(){
        // BezierUtilities.GetControlPointsOf(objectA.transform, objectB.transform, out posA, out posB, out conA, out conB);
        // Gizmos.DrawSphere(BezierUtilities.GetBezierPointAt(posA, conA, conB, posB, t), 0.01f);
        Gizmos.color = new Color(1,1,1,1);
        // if(objectA && objectB){
            Gizmos.DrawSphere(BezierUtilities.BezierLerp(objectA.transform, objectB.transform, t),0.1f);
        // }
    }

}
