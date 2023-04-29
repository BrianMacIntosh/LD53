using UnityEngine;

/// <summary>
/// Condition that passes if no orders are outstanding.
/// </summary>
public class ConditionNoOrders : BaseProgressionCondition
{
	public override bool Evaluate()
	{
		return OrderManager.Instance.HasNoOrders();
	}
}
