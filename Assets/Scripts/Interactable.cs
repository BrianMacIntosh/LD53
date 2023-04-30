﻿using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	public abstract void Interact(PlayerInteractor interactor);

	public virtual void DebugSkip() { }
}
