using UnityEngine;

/// <summary>
/// Base class for an event that can happen during the progression of the game.
/// </summary>
public abstract class BaseProgressionEvent : ScriptableObject
{
	public abstract void Execute();
}
