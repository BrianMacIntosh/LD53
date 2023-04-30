using System;
using UnityEngine;

public enum ItemCookedState
{
	None,
	Raw,
	Cooked,
	Burnt
}

[Flags]
public enum ItemModifiers
{
	Ketchup = 1 << 0,
	Mustard = 1 << 1,
	Popcorn = 1 << 2,
	Terrified = 1 << 3,
}

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

	private ItemCookedState m_cookedState = ItemCookedState.None;

	public ItemCookedState CookedState
	{
		get { return m_cookedState; }
	}

	public static ItemCookedState CombineCookedState(ItemCookedState a, ItemCookedState b)
	{
		return (ItemCookedState)Mathf.Max((int)a, (int)b);
	}

	private ItemModifiers m_modifiers = 0;

	public ItemModifiers Modifiers
	{
		get { return m_modifiers; }
		set
		{
			if (m_modifiers != value)
			{
				m_modifiers = value;
				UpdateVisualState();
			}
		}
	}

	public void AddModifier(ItemModifiers mod)
	{
		Modifiers = Modifiers | mod;
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

	private void Awake()
	{
		SetCookedState(ItemData.DefaultCookedState);
	}

	public override void Interact(PlayerInteractor interactor)
	{
		PlayerInventory inventory = interactor.GetComponent<PlayerInventory>();

		// check to combine with the top item
		CraftingItem topItem = inventory.PeekItem();
		if (topItem)
		{
			ItemCombination combo = topItem.ItemData.GetCombinationWith(ItemData);
			if (combo != null)
			{
				CraftingItem newItem = Instantiate(combo.ResultItem);
				newItem.SetCookedState(CombineCookedState(topItem.CookedState, CookedState));
				combo.CombinedEvent.Post(newItem.gameObject);
				MachineReplace(newItem);
				topItem.CombineEat();
				return;
			}

			combo = ItemData.GetCombinationWith(topItem.ItemData);
			if (combo != null)
			{
				CraftingItem newItem = Instantiate(combo.ResultItem);
				newItem.SetCookedState(CombineCookedState(topItem.CookedState, CookedState)); //TODO: globalize transfering data on combine
				combo.CombinedEvent.Post(newItem.gameObject);
				topItem.MachineReplace(newItem);
				CombineEat();
				return;
			}

			// attempt to transfer modifiers between the items
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

	public void Cook()
	{
		if (ItemData.DefaultCookedState != ItemCookedState.None)
		{
			switch (m_cookedState)
			{
				case ItemCookedState.Raw:
					SetCookedState(ItemCookedState.Cooked);
					break;
				case ItemCookedState.Cooked:
					SetCookedState(ItemCookedState.Burnt);
					break;
			}
		}
		else
		{
			MachineReplace(Instantiate(ItemData.CookResult));
		}
	}

	private void SetCookedState(ItemCookedState state)
	{
		m_cookedState = state;

		UpdateVisualState();
	}

	private void UpdateVisualState()
	{
		foreach (ItemVisualStateComponent stateComponent in GetComponentsInChildren<ItemVisualStateComponent>())
		{
			stateComponent.UpdateVisualState(this);
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

	public void CombineEat()
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
