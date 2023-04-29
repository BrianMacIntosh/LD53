using UnityEngine;

/// <summary>
/// Represents a point that spawns crafting items.
/// </summary>
public class ItemSource : MonoBehaviour
{
	[SerializeField]
	private GameObject m_prefab;

	private void Start()
	{
		Spawn();
	}

	private void Spawn()
	{
		Instantiate<GameObject>(m_prefab, transform);
	}
}
