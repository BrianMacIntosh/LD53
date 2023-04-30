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

	public void UpdateVisualState(CraftingItem parent)
	{
		if (m_cookedMaterialSlot >= 0)
		{
			MeshRenderer renderer = GetComponent<MeshRenderer>();
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
	}
}
