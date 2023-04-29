using UnityEngine;

[CreateAssetMenu(menuName = "Condition/Orders Filled")]
public class ConditionOrdersFilled : BaseProgressionCondition
{
	[SerializeField]
	private EventAddOrders m_ordersEvent;

	public override bool Evaluate()
	{
		return ProgressionManager.Instance.HasEventExecuted(m_ordersEvent)
			&& OrderManager.Instance.HasNoOrders();
	}
}
