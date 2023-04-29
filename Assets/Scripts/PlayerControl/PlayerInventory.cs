using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
	/// <summary>
	/// The max number of items the player can hold.
	/// </summary>
	[SerializeField]
	private int m_heldItemCount = 3;

	/// <summary>
	/// The set of items the player is holding.
	/// </summary>
	private CraftingItem[] m_items;

	/// <summary>
	/// The index of the currently selected item in <see cref="m_items"/>.
	/// </summary>
	private int m_selectedItem = 0;

	/// <summary>
	/// Slots held items can be attached to.
	/// </summary>
	[SerializeField]
	private Transform[] m_itemSlots;

	private void Awake()
	{
		m_items = new CraftingItem[m_heldItemCount];
		Debug.Assert(m_itemSlots.Length == m_heldItemCount);
	}

	public void ReplaceItem(CraftingItem oldItem, CraftingItem newItem)
	{
		for (int i = 0; i < m_items.Length; i++)
		{
			if (m_items[i] == oldItem)
			{
				m_items[i] = newItem;
				AttachItemToSlot(newItem, i);
			}
		}
	}

	/// <summary>
	/// Pushes an item into the inventory.
	/// </summary>
	/// <returns>True on success.</returns>
	public bool PushItem(CraftingItem item)
	{
		if (m_items[m_selectedItem] == null)
		{
			m_items[m_selectedItem] = item;
			AttachItemToSlot(item, m_selectedItem);
			return true;
		}
		else
		{
			//TODO: picking up items when current slot is not free
			return false;
		}
	}

	/// <summary>
	/// Pops an item off the inventory.
	/// </summary>
	public CraftingItem PopItem()
	{
		CraftingItem item = m_items[m_selectedItem];

		// forget the item
		m_items[m_selectedItem] = null;

		// advance to the previous item
		m_selectedItem = (m_selectedItem - 1 + m_items.Length) % m_items.Length;

		return item;
	}

	/// <summary>
	/// Peeks the top item from the inventory.
	/// </summary>
	public CraftingItem PeekItem()
	{
		return m_items[m_selectedItem];
	}

	private void AttachItemToSlot(CraftingItem item, int slot)
	{
		item.transform.SetParent(m_itemSlots[slot]);
		item.transform.localPosition = Vector3.zero;
		item.transform.localRotation = Quaternion.identity;
		item.transform.localScale = Vector3.one;
	}
}
