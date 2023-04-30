using System.Collections.Generic;
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

	/// <summary>
	/// The number of kernel rigidbodies that represent one serving.
	/// </summary>
	[SerializeField]
	private int m_bodiesPerCorn = 30;

	[SerializeField]
	private float m_popVelocity = 3f;

	private List<GameObject> m_bodies = new List<GameObject>();

	[SerializeField]
	private Transform m_bodyOrigin;

	private float m_nextPopTime;

	private GameObject m_poppedCornPrefab;

	[Header("WWise")]

	[SerializeField]
	private AK.Wwise.Event m_depositEvent;

	[Tooltip("Event fired when the player attempts to deposit a corn but it didn't fit.")]
	[SerializeField]
	private AK.Wwise.Event m_depositFailEvent;

	[SerializeField]
	private AK.Wwise.Event m_cornPopEvent;

	[SerializeField]
	private AK.Wwise.Event m_cornReadyEvent;

	[SerializeField]
	private AK.Wwise.Event m_cornReadyOverflowEvent;

	[SerializeField]
	private AK.Wwise.Event m_withdrawSuccessEvent;

	[SerializeField]
	private AK.Wwise.Event m_withdrawFailEvent;

	private void Awake()
	{
		m_poppedCornPrefab = Resources.Load<GameObject>("PoppedCorn");
	}

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
					m_cornReadyEvent.Post(gameObject);
				}
				else
				{
					//TODO: overflow machine
					m_cornReadyOverflowEvent.Post(gameObject);
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

		// calculate how many bodies should be spawned
		float partialCorn = 0f;
		if (m_nextPopTime > 0f)
		{
			partialCorn = Mathf.Pow(Mathf.Clamp01(1f - (m_nextPopTime - Time.time) / m_popTime), 3f);
		}
		int desiredBodies = Mathf.FloorToInt(m_bodiesPerCorn * (m_poppedCorns + partialCorn));
		while (m_bodies.Count < desiredBodies)
		{
			SpawnBody();
		}
		while (m_bodies.Count > desiredBodies)
		{
			Destroy(m_bodies[0]);
			m_bodies.RemoveAt(0);
		}
	}

	private void SpawnBody()
	{
		GameObject newBody = Instantiate(m_poppedCornPrefab, m_bodyOrigin);

		Rigidbody rigid = newBody.GetComponent<Rigidbody>();
		Vector3 random = Random.onUnitSphere;
		random.y = Mathf.Abs(random.y);
		rigid.velocity = random * m_popVelocity;

		m_bodies.Add(newBody);

		m_cornPopEvent.Post(gameObject);
	}

	protected override void ItemInteract(CraftingItem sourceItem)
	{
		base.ItemInteract(sourceItem);

		if (sourceItem.ItemData.name == "ID_Corn")
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

				m_depositEvent.Post(gameObject);
			}
			else
			{
				m_depositFailEvent.Post(gameObject);
			}
		}
		else if (sourceItem.ItemData.name == "ID_EmptyCarton")
		{
			if (m_poppedCorns > 0)
			{
				// fill the carton with popped corns
				CraftingItem filledCartonPrefab = Resources.Load<CraftingItem>("Item_PopcornCarton");
				CraftingItem filledCarton = Instantiate(filledCartonPrefab);
				sourceItem.MachineReplace(filledCarton);
				m_poppedCorns--;

				m_withdrawSuccessEvent.Post(gameObject);
			}
			else
			{
				m_withdrawFailEvent.Post(gameObject);
			}
		}
	}

	public override void DebugSkip()
	{
		m_nextPopTime = Time.time;
	}
}
