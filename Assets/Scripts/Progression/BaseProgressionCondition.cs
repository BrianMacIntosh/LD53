using UnityEngine;

/// <summary>
/// Base class for a condition that gates a progression event.
/// </summary>
public abstract class BaseProgressionCondition : ScriptableObject
{
	public abstract bool Evaluate();
}
