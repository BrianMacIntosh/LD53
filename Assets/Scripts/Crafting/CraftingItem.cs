using UnityEngine;

/// <summary>
/// Represents an item in the world.
/// </summary>
public class CraftingItem : Interactable
{
	/// <summary>
	/// Tag uniquely identifying this type of item.
	/// </summary>
	[SerializeField]
	private string m_itemTag;

	public string ItemTag
	{
		get { return m_itemTag; }
	}

	public override void Interact(PlayerInteractor interactor)
	{
		PlayerInventory inventory = interactor.GetComponent<PlayerInventory>();

		// check to combine with the top item
		CraftingItem topItem = inventory.PeekItem();
		if (topItem)
		{
			//TODO:
		}

		// try to pick up the item
		inventory.PushItem(this);
	}

	/// <summary>
	/// A machine has eaten this item.
	/// </summary>
	public void MachineEat()
	{
		Destroy(gameObject);
	}

	/// <summary>
	/// A machine has replaced this item with a different one.
	/// </summary>
	public void MachineReplace(CraftingItem other)
	{
		other.transform.SetParent(transform.parent, false);
		other.transform.localPosition = transform.localPosition;
		other.transform.localRotation = transform.localRotation;

		PlayerInventory containingInventory = GetComponentInParent<PlayerInventory>();
		if (containingInventory)
		{
			containingInventory.ReplaceItem(this, other);
		}
		Destroy(gameObject);
	}
}
