using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Singleton managing progression through the game's orders, plot, etc.
/// </summary>
public class ProgressionManager : MonoBehaviour
{
	public static ProgressionManager Instance
	{
		get; private set;
	}

	/// <summary>
	/// List of events that can trigger in the game.
	/// </summary>
	[SerializeField]
	private BaseProgressionEvent[] m_events;

	/// <summary>
	/// Which events have already been triggered?
	/// </summary>
	private bool[] m_eventsTriggered;

	private bool[] m_eventsExecuted;

	private void Awake()
	{
		Instance = this;

		m_eventsTriggered = new bool[m_events.Length];
		m_eventsExecuted = new bool[m_events.Length];
	}

	private void Update()
	{
#if UNITY_EDITOR
		Array.Resize(ref m_eventsTriggered, m_events.Length);
		Array.Resize(ref m_eventsExecuted, m_events.Length);
#endif

		//TODO: event-drive
		CheckFireEvents();
	}

	public bool HasEventExecuted(BaseProgressionEvent evt)
	{
		for (int index = 0; index < m_events.Length; index++)
		{
			if (m_events[index] == evt)
			{
				return m_eventsExecuted[index];
			}
		}
		return false;
	}

	private void CheckFireEvents()
	{
		for (int index = 0; index < m_events.Length; index++)
		{
			if (!m_eventsTriggered[index] && m_events[index].CanTrigger())
			{
				StartCoroutine(TriggerEvent(index));
				m_eventsTriggered[index] = true;
			}
		}
	}

	private IEnumerator TriggerEvent(int index)
	{
		BaseProgressionEvent progEvent = m_events[index];

		yield return new WaitForSeconds(progEvent.Delay);

		m_eventsExecuted[index] = true;
		progEvent.Execute();
	}
}
