using System;
using UnityEngine;

[Serializable]
public struct OrderSpecification
{
	public OrderData Data;

	public CustomerId Customer;
}

/// <summary>
/// Progression event that adds new orders to the system.
/// </summary>
[CreateAssetMenu(menuName = "Events/Add Orders")]
public class EventAddOrders : BaseProgressionEvent
{
	[SerializeField]
	private OrderSpecification[] m_orders;

	public OrderSpecification[] Orders
	{
		get { return m_orders; }
	}

	public override void Execute()
	{
		foreach (OrderSpecification order in m_orders)
		{
			OrderManager.Instance.AddOrder(order);
		}
	}
}
