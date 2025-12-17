using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public abstract class MovementStep : MonoBehaviour
{

	[SerializeField]
	public float AnimationTime = 5f;
	public UnityEvent onStepStarted;
	public UnityEvent OnStepEnded;
	[SerializeField]
	protected bool OverrideNextStep = false;
	[Tooltip("Reference the same step here to make it loop")]
	public RollableDictionary<MovementStep> rollableSteps;

	virtual
	public float GetLenght()
	{
		return 0;
	}


	abstract
	public void DrawStep();

	public MovementStep GetNextStepOrOverride()
	{

		if (rollableSteps.Options.Count == 0 || OverrideNextStep == false)
		{
			return rollableSteps.defaultItem;
		}

		if (OverrideNextStep)
		{
			if (DoesThisStepLoop())
			{
				Debug.Log($"[{GetType()}] THIS STEP LOOPS BTW {this}", this);
				return this;
			}
			return rollableSteps.GetWeightedOption();
		}

		return null;
	}

	public bool DoesThisStepLoop()
	{
		return rollableSteps.GetNumberOfOptions() == 1 && rollableSteps.Options.ContainsKey(this);
	}

	virtual
	protected void OnValidate()
	{
		if (rollableSteps == null)
		{
			return;
		}
		if (transform.parent == null)
		{
			return;
		}
		if (!transform.parent.TryGetComponent<SplineGroup>(out SplineGroup parentGroup))
		{
			return;
		}
		int mySiblingindex = transform.GetSiblingIndex();
		if (mySiblingindex != transform.parent.childCount - 1 && !OverrideNextStep)
		{
			if (rollableSteps.GetNumberOfOptions() == 0)
			{
				// Debug.Log($"[{GetType()}] Adding step for {mySiblingindex}", this);
				if (transform.parent.GetChild(mySiblingindex + 1).TryGetComponent<MovementStep>(out MovementStep movementStep))
				{
					rollableSteps.defaultItem = movementStep;
				}
			}
		}
	}

}