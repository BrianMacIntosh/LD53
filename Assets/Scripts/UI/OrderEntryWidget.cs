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
				color = new Color(.99f, .57f, .57f);
				name = "Red Theater";
				break;
			case CustomerRoom.BlueTheater:
				color = new Color(.39f, .93f, .99f);
                name = "Blue Theater";
				break;
			case CustomerRoom.GreenTheater: 
				color = new Color(.35f, .93f, .60f);
				name = "Green Theater";
				break;
			case CustomerRoom.PartyRoom:
				color = new Color(.98f, .70f, 1f);
				name = "Party Room";
				break;
			case CustomerRoom.CoachRoom: 
				color = new Color(.99f, 1f, .88f);
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
