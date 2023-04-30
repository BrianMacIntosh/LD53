using UnityEngine;

[CreateAssetMenu(menuName = "Condition/Event Executed")]
public class ConditionEventExecuted : BaseProgressionCondition
{
	[SerializeField]
	private BaseProgressionEvent m_event;

	public override bool Evaluate()
	{
		return ProgressionManager.Instance.HasEventExecuted(m_event);
	}
}
