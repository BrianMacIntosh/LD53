using UnityEngine;

[CreateAssetMenu(menuName = "Condition/Narration Finished")]
public class ConditionNarrationFinished : BaseProgressionCondition
{
	[SerializeField]
	private NarrationSetData m_narrationSet;

	[Tooltip("The line number that must be finished. Negative counts from the end.")]
	[SerializeField]
	private int m_lineNumber = -1;

	public override bool Evaluate()
	{
		return m_narrationSet.HasLinePlayed(m_lineNumber);
	}
}
