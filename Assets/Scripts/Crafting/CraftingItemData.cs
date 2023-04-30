using System;
using UnityEngine;

[Serializable]
public class ItemCombination
{
	public CraftingItemData OtherItem;

	public CraftingItem ResultItem;

	public AK.Wwise.Event CombinedEvent;
}

[CreateAssetMenu(menuName = "Crafting Item")]
public class CraftingItemData : ScriptableObject
{
	[Tooltip("The item that is created when this item is cooked.")]
	public CraftingItem CookResult;

	public ItemCookedState DefaultCookedState;

	[Tooltip("Time in seconds to cook this item.")]
	public float CookTime = 15f;

	public ItemCombination[] Combinations;

	public ItemCombination GetCombinationWith(CraftingItemData other)
	{
		foreach (ItemCombination combo in Combinations)
		{
			if (combo.OtherItem = other)
			{
				return combo;
			}
		}
		return null;
	}
}
