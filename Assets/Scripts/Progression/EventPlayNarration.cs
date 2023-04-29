using UnityEngine;

/// <summary>
/// Progression event that plays some narration.
/// </summary>
[CreateAssetMenu(menuName = "Events/Play Narration")]
public class EventPlayNarration : BaseProgressionEvent
{
	[SerializeField]
	private NarrationSetData m_narrationSet;

	public override void Execute()
	{
		NarrationManager.Instance.Play(m_narrationSet);
	}
}
