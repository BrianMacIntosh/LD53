using System.Collections.Generic;
using UnityEngine;

public struct OrderId
{
	public int Id;

	public OrderId(int inId)
	{
		Id = inId;
	}

	public override int GetHashCode()
	{
		return Id.GetHashCode();
	}
}

public class Order
{
	public OrderId Id;

	public OrderData Data;

	public CustomerId Customer;

	public Order(OrderId id, OrderSpecification spec)
	{
		Id = id;
		Data = spec.Data;
		Customer = spec.Customer;
	}

	public bool AcceptsItem(CraftingItem item)
	{
		return Data.AcceptsItem(item);
	}
}

public class OrderManager : MonoBehaviour
{
	public static OrderManager Instance
	{
		get; private set;
	}

	private void Awake()
	{
		Instance = this;
	}

	private Dictionary<OrderId, Order> m_orders = new Dictionary<OrderId, Order>();

	private int m_nextOrderId = 0;

	public delegate void OrdersChangedDelegate(OrderManager sender);
	public static event OrdersChangedDelegate OnOrdersChanged;

	[Header("WWise")]

	[SerializeField]
	private AK.Wwise.Event m_orderReceivedEvent;

	[SerializeField]
	private AK.Wwise.Event m_orderFilledEvent;

	/// <summary>
	/// Adds a new order to the system.
	/// </summary>
	public void AddOrder(OrderSpecification orderSpec)
	{
		OrderId orderId = new OrderId(m_nextOrderId++);
		m_orders.Add(orderId, new Order(orderId, orderSpec));

		if (OnOrdersChanged != null)
		{
			OnOrdersChanged(this);
		}

		m_orderReceivedEvent.Post(gameObject);
	}

	/// <summary>
	/// Marks an order as filled.
	/// </summary>
	public void FillOrder(OrderId orderId)
	{
		m_orders.Remove(orderId);

		if (OnOrdersChanged != null)
		{
			OnOrdersChanged(this);
		}

		m_orderFilledEvent.Post(gameObject);
	}

	/// <summary>
	/// Returns true if there are no outstanding orders.
	/// </summary>
	public bool HasNoOrders()
	{
		return m_orders.Count == 0;
	}


	/// <summary>
	/// Returns the ids of every outstanding order for the specified customer.
	/// </summary>
	public void GetOrdersForCustomer(CustomerId customerId, List<Order> outBuffer)
	{
		outBuffer.Clear();
		foreach (Order order in m_orders.Values)
		{
			if (order.Customer == customerId)
			{
				outBuffer.Add(order);
			}
		}
	}

	public IEnumerable<Order> GetOrders()
	{
		return m_orders.Values;
	}
}
