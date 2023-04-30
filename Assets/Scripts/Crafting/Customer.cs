using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Customer : Interactable
{
	[SerializeField]
	private CustomerId m_customerId;

	private List<Order> m_orderBuffer = new List<Order>();

	[Header("Wwise")]

	[SerializeField]
	private AK.Wwise.Event m_orderFilledEvent;

	[SerializeField] CustomerBubble bubble;

    private void Start()
    {
        bubble.DisableBubble();
    }

    private void OnEnable()
    {
		OrderManager.OnOrdersChanged += OnOrdersChanged;
    }

    private void OnDisable()
    {
        OrderManager.OnOrdersChanged -= OnOrdersChanged;
    }

	public void OnOrdersChanged(OrderManager sender)
	{
		sender.GetOrdersForCustomer(m_customerId, m_orderBuffer);

		if (m_orderBuffer.Count > 0)
		{
			var order = m_orderBuffer[0];
			bubble.EnableBubble();
			bubble.SetText(order.Data.DisplayString);
        }
		else
		{
            bubble.DisableBubble();
        }
    }

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
				m_orderFilledEvent.Post(gameObject);
				break;
			}
		}
	}
}
