using UnityEngine;

public class ItemVisualStateComponent : MonoBehaviour
{
	[SerializeField]
	private int m_cookedMaterialSlot = -1;

	[SerializeField]
	private Material m_rawMaterial;

	[SerializeField]
	private Material m_cookedMaterial;

	[SerializeField]
	private Material m_burntMaterial;

	[SerializeField]
	private ItemModifiers m_requireModifiers;

	public void UpdateVisualState(CraftingItem parent)
	{
		MeshRenderer renderer = GetComponent<MeshRenderer>();

		if (m_cookedMaterialSlot >= 0)
		{
			Material[] mats = renderer.sharedMaterials;
			switch (parent.CookedState)
			{
				case ItemCookedState.Raw:
					mats[m_cookedMaterialSlot] = m_rawMaterial;
					break;
				case ItemCookedState.Cooked:
					mats[m_cookedMaterialSlot] = m_cookedMaterial;
					break;
				case ItemCookedState.Burnt:
					mats[m_cookedMaterialSlot] = m_burntMaterial;
					break;
			}
			renderer.sharedMaterials = mats;
		}

		if (m_requireModifiers != 0)
		{
			renderer.enabled = (parent.Modifiers & m_requireModifiers) == m_requireModifiers;
		}
	}
}
