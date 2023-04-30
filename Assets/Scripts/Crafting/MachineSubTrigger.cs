using UnityEngine;

public class MachineSubTrigger : MonoBehaviour
{
	public delegate void SubTriggerDelegate(MachineSubTrigger sender, Collider other);

	public event SubTriggerDelegate OnTriggerEnterEvent;
	public event SubTriggerDelegate OnTriggerExitEvent;

	private void OnTriggerEnter(Collider other)
	{
		if (OnTriggerEnterEvent != null)
		{
			OnTriggerEnterEvent(this, other);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (OnTriggerExitEvent != null)
		{
			OnTriggerExitEvent(this, other);
		}
	}
}
