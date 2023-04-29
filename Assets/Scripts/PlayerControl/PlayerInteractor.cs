using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField]
    private float m_interactRange = 5f;

    /// <summary>
    /// Returns the interactable the player is currently targeting.
    /// </summary>
    public Interactable GetInteractTarget()
	{
        Camera mainCamera = Camera.main;

        RaycastHit hit;
        //TODO: raycast against an interact channel?
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, m_interactRange))
		{
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            return interactable;
        }
        else
		{
            return null;
		}
	}

    /// <summary>
    /// Input callback for the Interact action.
    /// </summary>
    public void OnInteract(InputValue value)
    {
        if (value.Get<float>() > 0.5f)
		{
            Interactable target = GetInteractTarget();
            if (target)
            {
                target.Interact(this);
            }
        }
    }
}
