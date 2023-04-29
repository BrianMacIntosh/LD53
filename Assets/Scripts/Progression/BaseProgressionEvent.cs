using UnityEngine;

public enum Operator
{
	And,
	Or
}

/// <summary>
/// Base class for an event that can happen during the progression of the game.
/// </summary>
public abstract class BaseProgressionEvent : ScriptableObject
{
	[TextArea]
	public string Comment;

	public BaseProgressionCondition[] Conditions;

	public Operator ConditionOperator;

	public float Delay = 0f;

	public bool CanTrigger()
	{
		bool result = ConditionOperator == Operator.Or ? false : true;
		foreach (BaseProgressionCondition condition in Conditions)
		{
			switch (ConditionOperator)
			{
				case Operator.Or:
					result = result || condition.Evaluate();
					break;
				case Operator.And:
					result = result && condition.Evaluate();
					break;
			}
		}
		return result;
	}

	public abstract void Execute();
}
