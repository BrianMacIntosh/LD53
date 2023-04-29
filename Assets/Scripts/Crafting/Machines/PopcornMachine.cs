using UnityEngine;

/// <summary>
/// A machine that turns popcorn kernels into popped corn.
/// </summary>
public class PopcornMachine : CraftingMachine
{
	private int m_queuedCorns = 0;

	private int m_poppedCorns = 0;

	/// <summary>
	/// Maximum number of unpopped corns that can be stored.
	/// </summary>
	[SerializeField]
	private int m_maxQueuedCorns = 3;

	/// <summary>
	/// Maximum number of popped corns that can be stored.
	/// </summary>
	[SerializeField]
	private int m_maxPoppedCorns = 3;

	/// <summary>
	/// Amount of time to pop a corn (seconds).
	/// </summary>
	[SerializeField]
	private float m_popTime = 30f;

	private float m_nextPopTime;

	private void Update()
	{
		if (m_nextPopTime > 0f && Time.time >= m_nextPopTime)
		{
			if (m_queuedCorns > 0)
			{
				// pop a corn
				m_queuedCorns--;
				if (m_poppedCorns < m_maxPoppedCorns)
				{
					m_poppedCorns++;
				}
				else
				{
					//TODO: overflow machine
				}

				// schedule next pop
				if (m_queuedCorns > 0)
				{
					m_nextPopTime = Time.time + m_popTime;
				}
				else
				{
					m_nextPopTime = 0f;
				}
			}
		}
	}

	protected override void ItemInteract(CraftingItem sourceItem)
	{
		base.ItemInteract(sourceItem);

		if (sourceItem.ItemTag == "Corn")
		{
			if (m_queuedCorns < m_maxQueuedCorns)
			{
				// deposit the unpopped corn
				sourceItem.MachineEat();
				m_queuedCorns++;

				if (m_nextPopTime == 0f)
				{
					m_nextPopTime = Time.time + m_popTime;
				}
			}
		}
		else if (sourceItem.ItemTag == "EmptyCarton")
		{
			if (m_poppedCorns > 0)
			{
				// fill the carton with popped corns
				CraftingItem filledCartonPrefab = Resources.Load<CraftingItem>("Item_PopcornCarton");
				CraftingItem filledCarton = Instantiate(filledCartonPrefab);
				sourceItem.MachineReplace(filledCarton);
				m_poppedCorns--;
			}
		}
	}
}
