using UnityEngine;

[CreateAssetMenu(menuName = "Crafting Item")]
public class CraftingItemData : ScriptableObject
{
	[Tooltip("The item that is created when this item is cooked.")]
	public CraftingItem CookResult;

	[Tooltip("Time in seconds to cook this item.")]
	public float CookTime = 15f;
}
