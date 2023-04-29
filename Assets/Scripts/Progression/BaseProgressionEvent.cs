using UnityEngine;

/// <summary>
/// Base class for an event that can happen during the progression of the game.
/// </summary>
public abstract class BaseProgressionEvent : ScriptableObject
{
	[TextArea]
	public string Comment;

	public BaseProgressionCondition Condition;

	public float Delay = 0f;

	public bool CanTrigger()
	{
		return !Condition || Condition.Evaluate();
	}

	public abstract void Execute();
}
