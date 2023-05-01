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
		string name;
		switch(order.Customer.MyRoom)
		{
			case CustomerRoom.RedTheater:
				color = Color.red;
				name = "Red Theater";
				break;
			case CustomerRoom.BlueTheater:
				color = Color.blue;
				name = "Blue Theater";
				break;
			case CustomerRoom.GreenTheater: 
				color = Color.green;
				name = "Green Theater";
				break;
			case CustomerRoom.PartyRoom: 
				color = Color.magenta;
				name = "Party Room";
				break;
			case CustomerRoom.CoachRoom: 
				color = Color.yellow;
				name = "Lounge";
				break;
			default:
				color = Color.white;
				name = "Unknown Location";
				break;
		}

		m_locationText.color = color;
		m_locationText.text = name;

		m_orderText.text = order.Data.DisplayString;
	}
}
