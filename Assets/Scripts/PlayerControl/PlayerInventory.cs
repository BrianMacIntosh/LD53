using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
	/// <summary>
	/// The max number of items the player can hold.
	/// </summary>
	[SerializeField]
	private int m_heldItemCount = 3;

	[SerializeField]
	private float m_dropVelocity = 3f;

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

	[SerializeField]
	private Transform m_itemTray;

	private void Awake()
	{
		m_items = new CraftingItem[m_heldItemCount];
		Debug.Assert(m_itemSlots.Length == m_heldItemCount);
	}

	private void Update()
	{
		// rotate the item tray to the current selection
		Quaternion targetRotation = Quaternion.FromToRotation(m_itemSlots[m_selectedItem].transform.localPosition, Vector3.left);
		//HACK: not timer-based
		m_itemTray.transform.localRotation = Quaternion.Slerp(m_itemTray.transform.localRotation, targetRotation, 0.1f);
	}

	/// <summary>
	/// Input action for the PrevItem action.
	/// </summary>
	public void OnPrevItem(InputValue value)
	{
		m_selectedItem = (m_selectedItem - 1 + m_items.Length) % m_items.Length;
	}

	/// <summary>
	/// Input action for the NextItem action.
	/// </summary>
	public void OnNextItem(InputValue value)
	{
		m_selectedItem = (m_selectedItem + 1) % m_items.Length;
	}

	/// <summary>
	/// Input action for the DropItem action.
	/// </summary>
	public void OnDropItem(InputValue value)
	{
		CraftingItem dropItem = m_items[m_selectedItem];
		if (dropItem)
		{
			m_items[m_selectedItem] = null;
			dropItem.transform.SetParent(null, true);

			Rigidbody rigidbody = dropItem.GetComponent<Rigidbody>();
			if (rigidbody)
			{
				rigidbody.isKinematic = false;
				Vector3 dropVelocity = (transform.forward + Vector3.up).normalized * m_dropVelocity;
				rigidbody.velocity = dropVelocity;
			}
		}
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
		item.transform.SetParent(m_itemSlots[slot], false);
		item.transform.localPosition = Vector3.zero;
		item.transform.localRotation = Quaternion.identity;

		Rigidbody rigidbody = item.GetComponent<Rigidbody>();
		if (rigidbody)
		{
			rigidbody.isKinematic = true;
		}
	}
}
