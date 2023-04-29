using System;
using UnityEngine;

/// <summary>
/// Singleton managing progression through the game's orders, plot, etc.
/// </summary>
public class ProgressionManager : MonoBehaviour
{
	[Serializable]
	public struct ProgressionEventEntry
	{
		public BaseProgressionEvent Event;

		//TODO: conditions

		public bool CanTrigger()
		{
			return true;
		}

		public void Trigger()
		{
			Event.Execute();
		}
	}

	public static ProgressionManager Instance
	{
		get; private set;
	}

	/// <summary>
	/// List of events that can trigger over the game.
	/// </summary>
	[SerializeField]
	private ProgressionEventEntry[] m_events;

	/// <summary>
	/// Which events have already been triggered?
	/// </summary>
	private bool[] m_eventsTriggered;

	private void Awake()
	{
		Instance = this;

		m_eventsTriggered = new bool[m_events.Length];
	}

	private void Update()
	{
		//TODO: event-drive
		CheckFireEvents();
	}

	private void CheckFireEvents()
	{
		for (int i = 0; i < m_events.Length; i++)
		{
			if (!m_eventsTriggered[i] && m_events[i].CanTrigger())
			{
				TriggerEvent(i);
			}
		}
	}

	private void TriggerEvent(int index)
	{
		m_events[index].Trigger();
		m_eventsTriggered[index] = true;
	}
}
