using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
	private PlayerInteractor m_playerInteractor;

	[SerializeReference]
	private Sprite m_defaultCrosshair;

	[SerializeReference]
	private Sprite m_interactCrosshair;

	private void Start()
	{
		m_playerInteractor = FindObjectOfType<PlayerInteractor>();
	}

	private void Update()
	{
		Interactable interactTarget = m_playerInteractor.GetInteractTarget();

		Image crosshairImage = GetComponent<Image>();
		if (interactTarget)
		{
			crosshairImage.sprite = m_interactCrosshair;
		}
		else
		{
			crosshairImage.sprite = m_defaultCrosshair;
		}
	}
}
