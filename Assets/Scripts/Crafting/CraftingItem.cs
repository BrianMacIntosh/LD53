using UnityEngine;

/// <summary>
/// Represents an item in the world.
/// </summary>
public class CraftingItem : Interactable
{
	[SerializeField]
	private CraftingItemData m_itemData;

	public CraftingItemData ItemData
	{
		get { return m_itemData; }
	}

	public delegate void CraftingItemDelegate(CraftingItem sender);
	public event CraftingItemDelegate OnCraftingItemPickedUp;

	[Header("WWise")]

	[SerializeField]
	private AK.Wwise.Event m_pickUpSuccessEvent;

	[SerializeField]
	private AK.Wwise.Event m_pickUpFailEvent;

	[SerializeField]
	private AK.Wwise.Event m_dropEvent;

	[SerializeField]
	private AK.Wwise.Event m_impactEvent;

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
		if (inventory.PushItem(this))
		{
			m_pickUpSuccessEvent.Post(gameObject);

			if (OnCraftingItemPickedUp != null)
			{
				OnCraftingItemPickedUp(this);
			}
		}
		else
		{
			m_pickUpFailEvent.Post(gameObject);
		}
	}

	/// <summary>
	/// A machine has eaten this item.
	/// </summary>
	public void MachineEat()
	{
		Destroy(gameObject);
	}

	/// <summary>
	/// A customer has eaten this item.
	/// </summary>
	public void CustomerEat()
	{
		Destroy(gameObject);
	}

	/// <summary>
	/// The player has tossed this item.
	/// </summary>
	public void NotifyDropped()
	{
		m_dropEvent.Post(gameObject);
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

	private void OnCollisionEnter(Collision collision)
	{
		//TODO: send force
		m_impactEvent.Post(gameObject);
	}
}
