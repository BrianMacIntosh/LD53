using UnityEngine;

public class HeldItemSlot : MonoBehaviour
{
	private PlayerInventory m_parentInventory;

	[SerializeField]
	private int m_index;

	private float m_alpha;

	[SerializeField]
	private float m_lerpDuration = 0.3f;

	[SerializeField]
	private Transform m_pivoter;

	private void Awake()
	{
		m_parentInventory = GetComponentInParent<PlayerInventory>();
	}

	private void Update()
	{
		if (m_parentInventory.SelectedItem == m_index)
		{
			m_alpha = Mathf.Clamp01(m_alpha + Time.deltaTime / m_lerpDuration);
		}
		else
		{
			m_alpha = Mathf.Clamp01(m_alpha - Time.deltaTime / m_lerpDuration);
		}

		float alphaRemapped = 1f - (1f - m_alpha) * (1f - m_alpha);
		m_pivoter.localRotation = Quaternion.AngleAxis(alphaRemapped * -15f, Vector3.forward);

		float scale = 1f + 0.2f * alphaRemapped;
		transform.localScale = new Vector3(scale, scale, scale);
	}

	public void AttachItem(CraftingItem item)
	{
		item.transform.SetParent(m_pivoter, false);
		item.transform.localPosition = Vector3.zero;
		item.transform.localRotation = Quaternion.identity;
	}

	public void DropItem(CraftingItem item)
	{
		transform.localScale = Vector3.one;
		item.transform.SetParent(null, true);
	}
}
