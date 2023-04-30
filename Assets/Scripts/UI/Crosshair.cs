using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
	private PlayerInteractor m_playerInteractor;

	[SerializeReference]
	private Sprite m_defaultCrosshair;

	[SerializeReference]
	private Sprite m_interactCrosshair;

	Interactable lastInteractTarget;

	private void Start()
	{
		m_playerInteractor = FindObjectOfType<PlayerInteractor>();
	}

	private void Update()
	{
		Interactable interactTarget = m_playerInteractor.GetInteractTarget();

		// Outline Logic
		if(lastInteractTarget != null && interactTarget != lastInteractTarget)
		{ 
			if (lastInteractTarget.TryGetComponent<Outline>(out Outline lastOutline))
			{
                lastOutline.OutlineMode = Outline.Mode.None;
			}
		}
        if (interactTarget != null && interactTarget.TryGetComponent<Outline>(out Outline outline))
		{
            outline.OutlineMode = Outline.Mode.OutlineAll;
		}


        Image crosshairImage = GetComponent<Image>();
		if (interactTarget)
		{
			crosshairImage.sprite = m_interactCrosshair;
		}
		else
		{
			crosshairImage.sprite = m_defaultCrosshair;
		}

		lastInteractTarget = interactTarget;
    }
}
