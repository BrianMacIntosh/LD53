using System.Collections.Generic;
using UnityEngine;

public class Customer : Interactable
{
	[SerializeField]
	private CustomerId m_customerId;

	private List<Order> m_orderBuffer = new List<Order>();

	public override void Interact(PlayerInteractor interactor)
	{
		// fetch this customer's order(s)
		OrderManager.Instance.GetOrdersForCustomer(m_customerId, m_orderBuffer);

		// satisfy the first possible order
		PlayerInventory inventory = interactor.GetComponent<PlayerInventory>();
		CraftingItem currentItem = inventory.PeekItem();
		foreach (Order order in m_orderBuffer)
		{
			if (order.AcceptsItem(currentItem))
			{
				OrderManager.Instance.FillOrder(order.Id);
				currentItem.CustomerEat();
				break;
			}
		}
	}
}
