﻿using UnityEngine;

/// <summary>
/// Data asset describing one item order.
/// </summary>
[CreateAssetMenu(menuName = "Order Data")]
public class OrderData : ScriptableObject
{
	public CraftingItemData Item;

	public ItemModifiers Modifiers;

	public ItemCookedState CookedState;

	public string DisplayString;

	public bool AcceptsItem(CraftingItem item)
	{
		return item.ItemData == Item
			&& (item.Modifiers & Modifiers) == Modifiers
			&& item.CookedState == CookedState;
	}
}
