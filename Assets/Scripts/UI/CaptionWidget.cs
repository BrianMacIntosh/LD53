using UnityEngine;

public class CaptionWidget : MonoBehaviour
{
	[SerializeField]
	private TMPro.TMP_Text m_text;

	private CanvasGroup m_canvasGroup;

	private void Awake()
	{
		NarrationManager.OnCaptionChanged += HandleCaptionChanged;

		m_canvasGroup = GetComponent<CanvasGroup>();
	}

	private void HandleCaptionChanged(NarrationManager sender, string caption)
	{
		m_text.text = caption;
		m_canvasGroup.alpha = caption.Length == 0 ? 0f : 1f;
	}
}
