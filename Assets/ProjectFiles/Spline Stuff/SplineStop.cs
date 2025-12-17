using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineStop : MovementStep{

    public bool resumeAutomatically = true;

	

	public override float GetLenght()
    {
        return 0;
    }


#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		DrawStep();	
	}
#endif

	override
	public void DrawStep()
	{
#if UNITY_EDITOR
		Gizmos.color = Color.red;
		Gizmos.DrawCube(transform.position, Vector3.one * 0.25f);
#endif
	}

	
}
