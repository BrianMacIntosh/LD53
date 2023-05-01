using UnityEngine;
using UnityEngine.Serialization;

public class TriggerProgressionEvent : MonoBehaviour
{
	[SerializeField]
	[FormerlySerializedAs("m_events")]
	private BaseProgressionEvent[] m_enterEvents;

	[SerializeField]
	private BaseProgressionEvent[] m_leaveEvents;

	private void OnTriggerEnter(Collider other)
	{
		foreach (BaseProgressionEvent evt in m_enterEvents)
		{
			ProgressionManager.Instance.TriggerEvent(evt);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		foreach (BaseProgressionEvent evt in m_leaveEvents)
		{
			ProgressionManager.Instance.TriggerEvent(evt);
		}
	}
}
