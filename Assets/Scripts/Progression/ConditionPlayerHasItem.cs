using UnityEngine;

[CreateAssetMenu(menuName = "Condition/Player Has Item")]
public class ConditionPlayerHasItem : BaseProgressionCondition
{
	[SerializeField]
	private CraftingItemData m_itemData;

	public override bool Evaluate()
	{
		return PlayerInventory.Instance.HasItem(m_itemData);
	}
}
