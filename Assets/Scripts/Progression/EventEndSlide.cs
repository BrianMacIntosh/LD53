using UnityEngine;

/// <summary>
/// Progression event that ends the game.
/// </summary>
[CreateAssetMenu(menuName = "Events/End Slide")]
public class EventEndSlide : BaseProgressionEvent
{
	[SerializeField]
	private Sprite[] m_slides;

	public override void Execute()
	{
		FindObjectOfType<EndGameSlides>().Play(m_slides);
	}
}
