using UnityEngine;

/// <summary>
/// Represents a machine that does an operation on an item.
/// </summary>
public abstract class CraftingMachine : Interactable
{
	public override void Interact(PlayerInteractor interactor)
	{
		PlayerInventory inventory = interactor.GetComponent<PlayerInventory>();
		CraftingItem topItem = inventory.PeekItem();
		if (topItem)
		{
			ItemInteract(topItem);
		}
	}

	/// <summary>
	/// Uses the specified item on this machine.
	/// </summary>
	protected virtual void ItemInteract(CraftingItem sourceItem)
	{
		
	}
}
