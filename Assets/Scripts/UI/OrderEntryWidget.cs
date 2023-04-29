using TMPro;
using UnityEngine;

public class OrderEntryWidget : MonoBehaviour
{
	[SerializeField]
	private TMP_Text m_locationText = null;

	[SerializeField]
	private TMP_Text m_orderText = null;

	public void Load(Order order)
	{
		m_orderText.text = order.Data.DisplayString;
	}
}
