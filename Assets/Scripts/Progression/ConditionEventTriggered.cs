using UnityEngine;

[CreateAssetMenu(menuName = "Condition/Event Triggered")]
public class ConditionEventTriggered : BaseProgressionCondition
{
	[SerializeField]
	private BaseProgressionEvent m_event;

	public override bool Evaluate()
	{
		return ProgressionManager.Instance.HasEventTriggered(m_event);
	}
}
