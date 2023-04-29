using System.Collections;
using UnityEngine;

public class NarrationManager : MonoBehaviour
{
	public static NarrationManager Instance
	{
		get; private set;
	}

	private NarrationSetData m_activeSet;

	private int m_activeSetIndex = -1;

	public delegate void CaptionChangedDelegate(NarrationManager sender, string caption);
	public static event CaptionChangedDelegate OnCaptionChanged;

	private void Awake()
	{
		Instance = this;
	}

	public void Play(NarrationSetData narration)
	{
		Debug.Log("Start narration set '" + narration.name + "'");
		m_activeSet = narration;
		m_activeSetIndex = -1;
		PostNextLine();
	}

	private void HandleNarrationCallback(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
	{
		Debug.Log("Narration next line from WWise.");
		PostNextLine();
	}

	private IEnumerator FallbackWait()
	{
		string caption = m_activeSet.Lines[m_activeSetIndex].Caption;
		float time = caption.Split(' ').Length * 0.5f;
		yield return new WaitForSeconds(time);

		Debug.Log("Narration next line from fallback wait.");
		PostNextLine();
	}

	private void PostNextLine()
	{
		if (m_activeSet == null)
		{
			return;
		}

		if (m_activeSetIndex >= 0)
		{
			m_activeSet.MarkLinePlayed(m_activeSetIndex);
		}
		m_activeSetIndex++;

		if (m_activeSetIndex >= m_activeSet.Lines.Length)
		{
			m_activeSet = null;
			m_activeSetIndex = -1;
			if (OnCaptionChanged != null)
			{
				OnCaptionChanged(this, "");
			}
			return;
		}

		uint result = m_activeSet.WwiseEvent.Post(gameObject, (uint)AkCallbackType.AK_EndOfEvent, HandleNarrationCallback);

		//TODO: use Wwise markers
		if (OnCaptionChanged != null)
		{
			OnCaptionChanged(this, m_activeSet.Lines[m_activeSetIndex].Caption);
		}

		if (result == 0)
		{
			// fallback: no audio line
			StartCoroutine(FallbackWait());
		}
	}
}
