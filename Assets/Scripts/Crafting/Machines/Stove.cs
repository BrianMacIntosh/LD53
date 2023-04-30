using System.Collections.Generic;
using UnityEngine;

public class Stove : ToggleInteractable
{
	private struct StoveItem
	{
		public CraftingItem Item;

		public CraftingItem ResultPrefab;

		public float CookCountdown;

		public StoveItem(CraftingItem item, CraftingItem resultPrefab, float cookDuration)
		{
			Item = item;
			ResultPrefab = resultPrefab;
			CookCountdown = cookDuration;
		}
	}

	private List<StoveItem> m_items = new List<StoveItem>();

	private void Start()
	{
		foreach (MachineSubTrigger subTrigger in GetComponentsInChildren<MachineSubTrigger>())
		{
			subTrigger.OnTriggerEnterEvent += HandleSubTriggerEnter;
			subTrigger.OnTriggerExitEvent += HandleSubTriggerExit;
		}
	}

	private void Update()
	{
		if (!ToggleState)
		{
			return;
		}

		for (int index = m_items.Count - 1; index >= 0; --index)
		{
			StoveItem item = m_items[index];
			item.CookCountdown -= Time.deltaTime;
			if (item.CookCountdown <= 0f)
			{
				item.Item.MachineReplace(Instantiate(item.ResultPrefab));
				m_items.RemoveAt(index);
			}
			else
			{
				m_items[index] = item;
			}
		}
	}

	private void HandleSubTriggerEnter(MachineSubTrigger sender, Collider other)
	{
		Debug.Log("Stove trigger enter: " + other.name);

		// already exists?
		foreach (StoveItem item in m_items)
		{
			if (item.Item.gameObject == other.gameObject)
			{
				return;
			}
		}

		//TODO: does not handle overlappng subtriggers

		CraftingItem otherItem = other.GetComponentInParent<CraftingItem>();
		if (otherItem.ItemData.CookResult)
		{
			m_items.Add(new StoveItem(otherItem, otherItem.ItemData.CookResult, otherItem.ItemData.CookTime));
		}
	}

	private void HandleSubTriggerExit(MachineSubTrigger sender, Collider other)
	{
		Debug.Log("Stove trigger exit: " + other.name);

		for (int index = 0; index < m_items.Count; index++)
		{
			if (m_items[index].Item.gameObject == other.gameObject)
			{
				m_items.RemoveAt(index);
				return;
			}
		}
	}
}
