using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationManager : MonoBehaviour
{
	public static NarrationManager Instance
	{
		get; private set;
	}

	private List<NarrationSetData> m_queuedSets = new List<NarrationSetData>();

	private NarrationSetData m_activeSet;

	private int m_activeSetIndex = -1;

	public delegate void CaptionChangedDelegate(NarrationManager sender, string caption);
	public static event CaptionChangedDelegate OnCaptionChanged;

	private Coroutine m_fallbackCoroutine;

	private void Awake()
	{
		Instance = this;
	}

	public void Play(NarrationSetData narration)
	{
		if (m_activeSet == narration)
		{
			return;
		}
		else if (m_activeSet == null || narration.Priority < m_activeSet.Priority)
		{
			Stop();

			Debug.Log("Start narration set '" + narration.name + "'");
			m_activeSet = narration;
			m_activeSetIndex = -1;
			PostNextLine();
		}
		else if (narration.Priority == NarrationSetPriority.Critical)
		{
			m_queuedSets.Add(narration);
		}
	}

	public void Stop()
	{
		if (m_fallbackCoroutine != null)
		{
			StopCoroutine(m_fallbackCoroutine);
		}
		m_activeSet = null;
		m_activeSetIndex = -1;
		if (OnCaptionChanged != null)
		{
			OnCaptionChanged(this, "");
		}

		if (m_queuedSets.Count > 0)
		{
			NarrationSetData newSet = m_queuedSets[0];
			m_queuedSets.RemoveAt(0);
			Play(newSet);
		}
	}

	public void Skip()
	{
		//TODO: cancel wwise playback
		if (m_fallbackCoroutine != null)
		{
			StopCoroutine(m_fallbackCoroutine);
		}
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

		switch (m_activeSet.PlayMode)
		{
			case NarrationSetPlayMode.Sequential:
				m_activeSetIndex++;
				break;

			case NarrationSetPlayMode.RandomRepeat:
				if (m_activeSetIndex < 0)
				{
					m_activeSetIndex = Random.Range(0, m_activeSet.LineCount);
				}
				else
				{
					// only play one line
					m_activeSetIndex = int.MaxValue;
				}
				break;

			case NarrationSetPlayMode.RandomNoRepeat:
				if (m_activeSetIndex < 0)
				{
					m_activeSetIndex = m_activeSet.GetRandomUnplayedLine();
				}
				else
				{
					// only play one line
					m_activeSetIndex = int.MaxValue;
				}
				break;
		}

		if (m_activeSetIndex >= m_activeSet.Lines.Length)
		{
			Stop();
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
			m_fallbackCoroutine = StartCoroutine(FallbackWait());
		}
	}
}
