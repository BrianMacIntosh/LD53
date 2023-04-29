using UnityEngine;

/// <summary>
/// Represents a machine that does an operation on an item.
/// </summary>
public abstract class CraftingMachine : MonoBehaviour
{
	/// <summary>
	/// Uses the specified item on this machine.
	/// </summary>
	public virtual void Interact(CraftingItem sourceItem)
	{
		
	}
}
