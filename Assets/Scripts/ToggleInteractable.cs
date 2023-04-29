using UnityEngine;

public class ToggleInteractable : Interactable
{
	private Animator m_animator;

	private bool m_toggleState = false;

	private void Awake()
	{
		m_animator = GetComponent<Animator>();
	}

	public override void Interact(PlayerInteractor interactor)
	{
		m_toggleState = !m_toggleState;
		m_animator.SetBool("ToggleState", m_toggleState);
	}
}
