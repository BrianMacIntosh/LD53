using System;
using UnityEngine;

/// <summary>
/// Represents a point that spawns crafting items.
/// </summary>
public class ItemSource : MonoBehaviour
{
	[SerializeField]
	private GameObject m_prefab;

	[Tooltip("Time in seconds it takes for this item to respawn.")]
	[SerializeField]
	private float m_respawnTime = 15f;

	private float m_respawnAtTime = 0f;

	private GameObject m_spawnedObject;

	private void Start()
	{
		Spawn();
	}

	private void Update()
	{
		if (m_respawnAtTime <= 0f && m_spawnedObject == null)
		{
			ScheduleRespawn();
		}
		if (m_respawnAtTime > 0f && Time.time >= m_respawnAtTime)
		{
			Spawn();
		}
	}

	private void Spawn()
	{
		m_spawnedObject = Instantiate(m_prefab, transform);
		CraftingItem spawnedItem = m_spawnedObject.GetComponent<CraftingItem>();
		if (spawnedItem)
		{
			spawnedItem.OnCraftingItemPickedUp += HandleCraftingItemPickedUp;
		}

		m_respawnAtTime = 0f;
	}

	private void HandleCraftingItemPickedUp(CraftingItem sender)
	{
		ScheduleRespawn();
	}

	private void ScheduleRespawn()
	{
		m_respawnAtTime = Time.time + m_respawnTime;
	}
}
