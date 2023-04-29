using System;

public class OrdersPopulator : Populator
{
	private void Start()
	{
		OrderManager.OnOrdersChanged += HandleOrdersChanged;
		RefreshOrders();
	}

	private void HandleOrdersChanged(OrderManager sender)
	{
		RefreshOrders();
	}

	private void RefreshOrders()
	{
		BeginPopulate();

		foreach (Order order in OrderManager.Instance.GetOrders())
		{
			OrderEntryWidget child = ActivateOrClone<OrderEntryWidget>();
			child.Load(order);
		}

		FinishPopulate();
	}
}
