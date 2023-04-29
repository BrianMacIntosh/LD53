using UnityEngine;

public class ConditionNoOrders : BaseProgressionCondition
{
	public override bool Evaluate()
	{
		return OrderManager.Instance.HasNoOrders();
	}
}
