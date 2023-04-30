using UnityEngine;

public class TriggerProgressionEvent : MonoBehaviour
{
	[SerializeField]
	private BaseProgressionEvent[] m_events;

	private void OnTriggerEnter(Collider other)
	{
		foreach (BaseProgressionEvent evt in m_events)
		{
			ProgressionManager.Instance.TriggerEvent(evt);
		}
	}
}
