using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SplineGroup : MonoBehaviour{

    public MovementStep[] MovementSteps;


    void Awake(){
        if(MovementSteps.Length == 0){
            ReferencePoints();
        }
    }

    void ReferencePoints(){
        MovementSteps = new MovementStep[transform.childCount];
        for (int i = 0; i < MovementSteps.Length; i++)
        {
            MovementSteps[i] = transform.GetChild(i).GetComponent<MovementStep>();
        }
    }

    void Reset(){
        ReferencePoints();
    }

	private void OnValidate()
	{
        // ReferencePoints();
		
	}

	private void OnDrawGizmosSelected()
	{
		foreach(var step in MovementSteps){
			if(step == null){
				ReferencePoints();
				return;
			}
			step.DrawStep();
		}
	}
}
[Serializable]
public class AnimationStep{
    public MovementStep step;
    public float stepTime = 1;

}
