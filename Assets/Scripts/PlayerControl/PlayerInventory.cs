﻿using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
	public static PlayerInventory Instance
	{
		get; private set;
	}

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

	public int SelectedItem
	{
		get { return m_selectedItem; }
	}

	public bool HasChangedSlot
	{
		get; private set;
	}

	/// <summary>
	/// Slots held items can be attached to.
	/// </summary>
	[SerializeField]
	private HeldItemSlot[] m_itemSlots;

	[SerializeField]
	private Transform m_itemTray;

	[SerializeField]
	private MeshRenderer m_itemPointer;

	[SerializeField]
	private Material m_itemPointerActiveMat;

	[SerializeField]
	private Material m_itemPointerEmptyMat;

	[Header("WWise")]

	[SerializeField]
	private AK.Wwise.Event m_changeToValidItemEvent;

	[SerializeField]
	private AK.Wwise.Event m_changeToEmptyItemEvent;

	[SerializeField]
	private AK.Wwise.Event m_pickupAnyItemSuccessEvent;

	[SerializeField]
	private AK.Wwise.Event m_pickupAnyItemFailEvent;

	[SerializeField]
	private AK.Wwise.Event m_dropAnyItemEvent;

	private void Awake()
	{
		Instance = this;

		m_items = new CraftingItem[m_heldItemCount];
		Debug.Assert(m_itemSlots.Length == m_heldItemCount);
	}

	private void Start()
	{
		UpdateItemPointerMat();
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
		HasChangedSlot = true;

		m_selectedItem = (m_selectedItem - 1 + m_items.Length) % m_items.Length;

		if (m_items[m_selectedItem])
		{
			m_changeToValidItemEvent.Post(gameObject);
		}
		else
		{
			m_changeToEmptyItemEvent.Post(gameObject);
		}

		UpdateItemPointerMat();
	}

	/// <summary>
	/// Input action for the NextItem action.
	/// </summary>
	public void OnNextItem(InputValue value)
	{
		HasChangedSlot = true;

		m_selectedItem = (m_selectedItem + 1) % m_items.Length;

		if (m_items[m_selectedItem])
		{
			m_changeToValidItemEvent.Post(gameObject);
		}
		else
		{
			m_changeToEmptyItemEvent.Post(gameObject);
		}

		UpdateItemPointerMat();
	}

	/// <summary>
	/// Input action for the DropItem action.
	/// </summary>
	public void OnDropItem(InputValue value)
	{
		CraftingItem item = m_items[m_selectedItem];
		if (item)
		{
			m_items[m_selectedItem] = null;
			m_itemSlots[m_selectedItem].DropItem(item);

			Rigidbody rigidbody = item.GetComponent<Rigidbody>();
			if (rigidbody)
			{
				rigidbody.isKinematic = false;
				Vector3 dropVelocity = (transform.forward + Vector3.up).normalized * m_dropVelocity;
				rigidbody.velocity = dropVelocity;
			}
			Collider collider = item.GetComponent<Collider>();
			if (collider)
			{
				collider.enabled = true;
			}

			item.NotifyDropped();
			m_dropAnyItemEvent.Post(gameObject);

			UpdateItemPointerMat();
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
			UpdateItemPointerMat();
			return true;
		}
		else
		{
			//TODO: picking up items when current slot is not free
			m_pickupAnyItemFailEvent.Post(gameObject);
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

	public bool HasItem(CraftingItemData data)
	{
		foreach (CraftingItem item in m_items)
		{
			if (item && item.ItemData == data)
			{
				return true;
			}
		}
		return false;
	}

	private void AttachItemToSlot(CraftingItem item, int slot)
	{
		m_itemSlots[slot].AttachItem(item);

		Rigidbody rigidbody = item.GetComponent<Rigidbody>();
		if (rigidbody)
		{
			rigidbody.isKinematic = true;
		}
		Collider collider = item.GetComponent<Collider>();
		if (collider)
		{
			collider.enabled = false;
		}

		if (item.ItemData.PickUpEvent)
		{
			ProgressionManager.Instance.TriggerEvent(item.ItemData.PickUpEvent);
		}

		m_pickupAnyItemSuccessEvent.Post(gameObject);
	}

	private void UpdateItemPointerMat()
	{
		m_itemPointer.sharedMaterial = m_items[m_selectedItem] ? m_itemPointerActiveMat : m_itemPointerEmptyMat;
	}
}
