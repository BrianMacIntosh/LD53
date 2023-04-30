using UnityEngine;

public class ToggleInteractable : Interactable
{
	private Animator m_animator;

	private bool m_toggleState = false;

	[Header("WWise")]

	[SerializeField]
	private AK.Wwise.Event m_toggleOnOpenEvent;

	[SerializeField]
	private AK.Wwise.Event m_toggleOffClosedEvent;

	public bool ToggleState
	{
		get { return m_toggleState; }
	}

	private void Awake()
	{
		m_animator = GetComponent<Animator>();
	}

	public override void Interact(PlayerInteractor interactor)
	{
		m_toggleState = !m_toggleState;
		m_animator.SetBool("ToggleState", m_toggleState);

		if (m_toggleState)
		{
			m_toggleOnOpenEvent.Post(gameObject);
		}
		else
		{
			m_toggleOffClosedEvent.Post(gameObject);
		}
	}
}
