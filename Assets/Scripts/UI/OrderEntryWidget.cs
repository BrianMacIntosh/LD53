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
		Color color;
		switch(order.Customer.MyRoom)
		{
			case CustomerRoom.RedTheater:
				color = Color.red;
				break;
			case CustomerRoom.BlueTheater:
				color = Color.blue;
				break;
			case CustomerRoom.GreenTheater: 
				color = Color.green;
                break;
			case CustomerRoom.PartyRoom: 
				color = Color.magenta; 
				break;
			case CustomerRoom.CoachRoom: 
				color = Color.yellow; 
				break;
			default:
				color = Color.white;
				break;
		}

		m_locationText.color = color;
		m_locationText.text = order.Customer.MyRoom.ToString();

		m_orderText.text = order.Data.DisplayString;
	}
}
