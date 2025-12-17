using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BezierUtilities {



    public static void GetControlPointsOf(Transform pointA, Transform pointB, out Vector3 positionA, out Vector3 positionB, out Vector3 controlA, out Vector3 controlB, float distanceDivider = 2){
        positionA = pointA.position;
        positionB = pointB.position;
        float distance = Vector3.Distance(positionA, positionB);
        controlA = positionA + (pointA.forward * (distance / distanceDivider));
        controlB = positionB + ((pointB.forward * - 1) * (distance / distanceDivider));
    }

    public static Vector3 GetBezierPointAt(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t){
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        Vector3 cd = Vector3.Lerp(c, d, t);

        Vector3 abc = Vector3.Lerp(ab, bc, t);
        Vector3 cbd = Vector3.Lerp(bc, cd, t);

        return Vector3.Lerp(abc, cbd, t);
    }

    public static Vector3 BezierLerp(Transform pointA, Transform pointB, float percentage){
        Vector3 a,b,c,d;
        GetControlPointsOf(
            pointA,pointB,
            out a,
            out d,
            out b, 
            out c
        );
        return GetBezierPointAt(a,b,c,d, percentage);
    }

}
