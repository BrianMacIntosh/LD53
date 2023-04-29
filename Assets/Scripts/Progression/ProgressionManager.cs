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

		progEvent.Execute();
	}
}
