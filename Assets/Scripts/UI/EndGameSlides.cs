using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndGameSlides : MonoBehaviour
{
	[SerializeField]
	private Image m_slide;

	public void Play(Sprite[] slides)
	{
		StartCoroutine(PlayHelper(slides));
	}

	private IEnumerator PlayHelper(Sprite[] slides)
	{
		CanvasGroup cg = GetComponent<CanvasGroup>();

		while (cg.alpha < 1f)
		{
			cg.alpha += Time.deltaTime / 3f;
			yield return null;
		}

		m_slide.sprite = slides[0];
		yield return new WaitForSecondsRealtime(3f);

		m_slide.sprite = slides[1];
		yield return new WaitForSecondsRealtime(6f);

		Application.Quit();
	}
}
