using System;
using System.Collections;
using System.Collections.Generic;
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
	private HashSet<BaseProgressionEvent> m_eventsTriggered = new HashSet<BaseProgressionEvent>();

	private HashSet<BaseProgressionEvent> m_eventsExecuted = new HashSet<BaseProgressionEvent>();

	private void Awake()
	{
		Instance = this;
	}

	private void Update()
	{
		//TODO: event-drive
		CheckFireEvents();
	}

	public bool HasEventExecuted(BaseProgressionEvent evt)
	{
		return m_eventsExecuted.Contains(evt);
	}

	public bool HasEventTriggered(BaseProgressionEvent evt)
	{
		return m_eventsTriggered.Contains(evt);
	}

	private void CheckFireEvents()
	{
		for (int index = 0; index < m_events.Length; index++)
		{
			BaseProgressionEvent evt = m_events[index];
			TriggerEvent(evt);
		}
	}

	public void TriggerEvent(BaseProgressionEvent evt)
	{
		if ((!m_eventsTriggered.Contains(evt) || evt.DoesRepeat) && evt.CanTrigger())
		{
			StartCoroutine(TriggerEventCoroutine(evt));
			m_eventsTriggered.Add(evt);
		}
	}

	private IEnumerator TriggerEventCoroutine(BaseProgressionEvent evt)
	{
		yield return new WaitForSeconds(evt.Delay);

		m_eventsExecuted.Add(evt);
		evt.Execute();
	}
}
