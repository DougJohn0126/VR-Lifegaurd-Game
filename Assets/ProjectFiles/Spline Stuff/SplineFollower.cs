using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum RotationType { twoDimensional, ThreeDimensional };
public class SplineFollower : MonoBehaviour
{

	[SerializeField] public SplineGroup splineGroup;

	[Tooltip("Want the object to depart a bit late? Granted")]
	[SerializeField] float initialDelay = 0f;

	public bool isMoving;
	float moving = 10;
	[SerializeField] bool loopAnimation = true;
	[SerializeField] bool syncRotation = true;
	[Tooltip("If set to 2D it will only rotate on the Y axis. If set to 3D it will fully rotate")]
	[SerializeField] RotationType rotationType;
	[SerializeField]
	private float _followerSpeed;

	[SerializeField] bool activeOnStart = true;

	float rotationOffset = 0.01f;

	public UnityEvent<string> DoAnimation;

#if UNITY_EDITOR
	//public ProxyBird proxyBird;
#endif
	[Header("Animation Events")]
	public UnityEvent OnAnimationBegin;
	public UnityEvent OnAnimationEnd;

	public Coroutine movementRoutine;
	private Coroutine currentStepRoutine;

	private MovementStep currentStep;
	private MovementStep nextStep;
	private float currentStepProgress;
	private string mCurrentAnimation; 

	private void Awake()
	{
#if UNITY_EDITOR
		//proxyBird = GetComponent<ProxyBird>();
#endif
	}

	void Start()
	{
		if (activeOnStart && splineGroup)
		{
			StartMoving();
		}
	}

	public void SetGroupReference(SplineGroup group)
	{
		splineGroup = group;
	}

	IEnumerator FollowGroupDynamic()
	{
		yield return new WaitForSeconds(initialDelay / 1);
		OnAnimationBegin.Invoke();
		nextStep = splineGroup.MovementSteps[0];
		int escapeThing = 0;
		do
		{
			currentStepRoutine = StartCoroutine(FollowStep(nextStep));
			escapeThing++;
			if (escapeThing >= 1000)
			{
				Debug.Log($"[{GetType()}] Emergency exit out of the loop", this);
				break;
			}
			yield return currentStepRoutine;
			nextStep = currentStep.GetNextStepOrOverride();
			// if(nextStep){
			// 	Debug.Log($"[{GetType()}] Next step is {nextStep.transform.name}", this);
			// }else{
			// 	Debug.Log($"[{GetType()}] Next step is Finish animation", this);
			// }

		} while (nextStep != null);
		OnAnimationEnd.Invoke();
	}


	IEnumerator FollowStep(MovementStep step)
	{
		if (step == null)
		{
			Debug.Assert(step != null, $"[{GetType()}] Somehow {currentStep.transform.name}'s next is null");
			yield break;
		}
		currentStep = step;
		//TODO: replace with custom component

		string targetAnim = "goWalk";
		if (step.TryGetComponent<SwimmerAnimationStep>(out SwimmerAnimationStep swimmerStep))
		{
			targetAnim = swimmerStep.GetAnimationForThisStep();
			//SetAnimation(targetAnim);
			mCurrentAnimation = targetAnim;
			DoAnimation.Invoke(targetAnim);
		}
		

		if (step is SplineStop stop)
		{
			StopMoving();

			transform.position = stop.transform.position;
			transform.forward = stop.transform.forward;
			yield return new WaitForSeconds(step.AnimationTime);
			if (!stop.resumeAutomatically)
			{
				ShutdownFollow();
				yield break;
			}
		}
		else if (step is Spline sp)
		{
			for (int index = 0; index < sp.points.Length - 1; index++)
			{
				if (syncRotation)
				{
					if (rotationType == RotationType.twoDimensional)
					{
						Vector3 lookAtpostion = sp.points[0].transform.position + sp.points[0].transform.forward;
						lookAtpostion = lookAtpostion.WithHeightOf(transform.position);
						transform.LookAt(lookAtpostion);
					}
					else if (rotationType == RotationType.ThreeDimensional)
					{
						transform.rotation = sp.points[0].transform.rotation;
					}
				}
				float targetAnimationTime = sp.AnimationTime;
				if (sp.OverrideSpeedWithTime == false)
				{
					targetAnimationTime = sp.totalSplineDistance / _followerSpeed;
				}
				float percentage = (sp.points[index].distanceToNextPoint / sp.totalSplineDistance) * targetAnimationTime;
				for (float i = 0; i < percentage; i = i + (Time.fixedDeltaTime * 1))
				{
					currentStepProgress = i / percentage;
					//Set the position
					transform.position = BezierUtilities.BezierLerp(
						sp.points[index].transform,
						sp.points[index + 1].transform,
						i / percentage
					);
					if (syncRotation)
					{
						Vector3 lookAtpostion = BezierUtilities.BezierLerp(
							sp.points[index].transform,
							sp.points[index + 1].transform,
							((i / percentage) + rotationOffset).NoMoreThan(1)
						);
						if (rotationType == RotationType.twoDimensional)
						{
							// Vector3 lookAtpostion = sp.points[0].transform.position + sp.points[0].transform.forward;
							lookAtpostion = lookAtpostion.WithHeightOf(transform.position);
						}
						transform.LookAt(lookAtpostion);
					}
					// Debug.Log(percentage + " of " + sp.points[index].distanceToNextPoint + "/" + sp.totalSplineDistance);
					yield return new WaitForFixedUpdate();
				}
			}
		}
	}

	IEnumerator FollowGroup()
	{
		yield return new WaitForSeconds(initialDelay / 1);
		//Debug.Log("Time Start: " + Time.time);
		OnAnimationBegin.Invoke();
		foreach (var step in splineGroup.MovementSteps)
		{
			currentStep = step;
			//TODO: replace with custom component

			//BirdBehaviorType targetAnim = BirdBehaviorType.Soaring;
			//if (step.TryGetComponent<BirdAnimationStep>(out BirdAnimationStep bstep))
			{
				//targetAnim = bstep.GetRandomBehaviourForThisStep();
			}

			//SetAnimation(targetAnim);

			if (step is SplineStop stop)
			{
				StopMoving();

				transform.position = stop.transform.position;
				transform.forward = stop.transform.forward;
				if (!stop.resumeAutomatically)
				{
					ShutdownFollow();
				}
				yield return new WaitForSeconds(step.AnimationTime);

				continue;
			}

			Spline sp = step as Spline;

			for (int index = 0; index < sp.points.Length - 1; index++)
			{
				if (syncRotation)
				{
					if (rotationType == RotationType.twoDimensional)
					{
						Vector3 lookAtpostion = sp.points[0].transform.position + sp.points[0].transform.forward;
						lookAtpostion = lookAtpostion.WithHeightOf(transform.position);
						transform.LookAt(lookAtpostion);
					}
					else if (rotationType == RotationType.ThreeDimensional)
					{
						transform.rotation = sp.points[0].transform.rotation;
					}
				}
				float targetAnimationTime = sp.AnimationTime;
				if (sp.OverrideSpeedWithTime == false)
				{
					targetAnimationTime = sp.totalSplineDistance / _followerSpeed;
				}
				float percentage = (sp.points[index].distanceToNextPoint / sp.totalSplineDistance) * targetAnimationTime;
				for (float i = 0; i < percentage; i = i + (Time.fixedDeltaTime * 1))
				{
					currentStepProgress = i / percentage;
					//Set the position
					transform.position = BezierUtilities.BezierLerp(
						sp.points[index].transform,
						sp.points[index + 1].transform,
						i / percentage
					);
					if (syncRotation)
					{
						Vector3 lookAtpostion = BezierUtilities.BezierLerp(
							sp.points[index].transform,
							sp.points[index + 1].transform,
							((i / percentage) + rotationOffset).NoMoreThan(1)
						);
						if (rotationType == RotationType.twoDimensional)
						{
							// Vector3 lookAtpostion = sp.points[0].transform.position + sp.points[0].transform.forward;
							lookAtpostion = lookAtpostion.WithHeightOf(transform.position);
						}
						transform.LookAt(lookAtpostion);
					}
					// Debug.Log(percentage + " of " + sp.points[index].distanceToNextPoint + "/" + sp.totalSplineDistance);
					yield return new WaitForFixedUpdate();
				}
			}
		}

		OnAnimationEnd.Invoke();
		if (loopAnimation)
		{
			StartMoving();
		}
	}

	private void SetAnimation(String target)
	{
		//GPSwimmer?.SetAnimation(target);
#if UNITY_EDITOR
		// GPSwimmer?.SetAnimation(target);
#endif
	}

	public void StartMoving()
	{
		isMoving = true;
		if (movementRoutine != null)
		{
			StopCoroutine(movementRoutine);
		}
		if(currentStepRoutine != null){
			StopCoroutine(currentStepRoutine);
		}
		// movementRoutine = StartCoroutine(FollowGroup());
		movementRoutine = StartCoroutine(FollowGroupDynamic());
	}

	public void StopMoving()
	{
		// Debug.Log(transform.name + " Stopped moving");
		isMoving = false;
	}

	public bool IsMoving()
	{
		return isMoving;
	}

	public void ShutdownFollow()
	{
		if (movementRoutine != null)
		{
			StopCoroutine(movementRoutine);
		}
		if (currentStepRoutine != null)
		{
			StopCoroutine(currentStepRoutine);

		}
	}

#if UNITY_EDITOR

	private void OnDrawGizmosSelected()
	{
		if(currentStep){
			currentStep.DrawStep();
		}
	}

#endif

	public string GetCurrentAnimation()
    {
		return mCurrentAnimation;
    }
}
