using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Populates a child object for each element dictated by a controller.
/// </summary>
//TODO: handle cases where dividers no longer sort correctly with children
public class Populator : MonoBehaviour
{
	[SerializeField]
	private GameObject m_baseObject = null;

	[Tooltip("Object to activate between each item.")]
	[SerializeField]
	private GameObject m_divider = null;

	[Tooltip("Object to activate if the populator is empty.")]
	[SerializeField]
	private GameObject m_emptyObject = null;

	private List<GameObject> m_children = new List<GameObject>();

	private List<GameObject> m_dividers = new List<GameObject>();

	/// <summary>
	/// The next index to populate.
	/// </summary>
	private int m_populateIndex = -1;

	/// <summary>
	/// Returns the number of currently-active children.
	/// </summary>
	public int ActiveChildren
	{
		get
		{
			for (int i = 0; i < m_children.Count; i++)
			{
				if (!m_children[i].activeSelf)
				{
					return i;
				}
			}
			return m_children.Count;
		}
	}

	public delegate void PopulatorDelegate(Populator sender);

	/// <summary>
	/// Event called after children are activated or deactivated.
	/// </summary>
	public event PopulatorDelegate OnPostChildActivationChanged;

	/// <summary>
	/// The current list of children, including inactive. Do not modify.
	/// </summary>
	public List<GameObject> Children
	{
		get { return m_children; }
	}

	protected virtual void Awake()
	{
		m_baseObject.SetActive(false);
		if (m_divider)
		{
			m_divider.SetActive(false);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <see cref="FinishPopulate"/>
	public void BeginPopulate()
	{
		if (m_populateIndex >= 0)
		{
			throw new System.InvalidOperationException("Called BeginPopulate while a populate was already active.");
		}
		m_populateIndex = 0;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <see cref="BeginPopulate"/>
	public void FinishPopulate()
	{
		if (m_populateIndex < 0)
		{
			throw new System.InvalidOperationException("Called FinishPopulate without calling BeginPopulate.");
		}
		for (;  m_populateIndex < m_children.Count; m_populateIndex++)
		{
			Deactivate(m_populateIndex);
		}
		m_populateIndex = -1;
	}

	/// <summary>
	/// Activates the specified number of children and deactivates the rest.
	/// </summary>
	public void Populate(int count)
	{
		int i;
		for (i = 0; i < count; i++)
		{
			ActivateOrClone(i);
		}
		for (; i < m_children.Count; i++)
		{
			Deactivate(i);
		}
	}

	/// <summary>
	/// Returns the first inactive child.
	/// </summary>
	public T ActivateOrClone<T>() where T : class
	{
		if (m_populateIndex >= 0)
		{
			return ActivateOrClone<T>(m_populateIndex);
		}
		for (int i = 0; i < Children.Count; i++)
		{
			if (Children[i] == null || !Children[i].activeSelf)
			{
				return ActivateOrClone<T>(i);
			}
		}
		return ActivateOrClone<T>(Children.Count);
	}

	/// <summary>
	/// Returns the first inactive child.
	/// </summary>
	public GameObject ActivateOrClone()
	{
		if (m_populateIndex >= 0)
		{
			return ActivateOrClone(m_populateIndex);
		}
		for (int i = 0; i < Children.Count; i++)
		{
			if (Children[i] == null || !Children[i].activeSelf)
			{
				return ActivateOrClone(i);
			}
		}
		return ActivateOrClone(Children.Count);
	}

	/// <summary>
	/// Returns the child object at the specified index.
	/// </summary>
	public T ActivateOrClone<T>(int index) where T : class
	{
		if (index < 0)
		{
			throw new System.ArgumentOutOfRangeException("index");
		}

		GameObject child = ActivateOrClone(index);
		T t = child.GetComponent<T>();
		return t;
	}

	/// <summary>
	/// Returns the child object at the specified index.
	/// </summary>
	public GameObject ActivateOrClone(int index)
	{
		if (index < 0)
		{
			throw new System.ArgumentOutOfRangeException("index");
		}
		if (m_populateIndex >= 0)
		{
			m_populateIndex = index + 1;
		}

		//HACK: makes assumption that dividers will never be destroyed by anyone else
		while (index >= m_children.Count)
		{
			if (index > 0)
			{
				GameObject divider = AllocateDivider();
				if (divider)
				{
					m_dividers.Add(divider);
				}
			}

			m_children.Add(AllocateClone());
		}
		if (m_children[index] == null)
		{
			m_children[index] = AllocateClone();
		}

		GameObject child = m_children[index];
		child.SetActive(true);
		PostChildActivationChanged();
		return child;
	}

	private GameObject AllocateClone()
	{
		GameObject clone = Instantiate(m_baseObject, m_baseObject.transform.parent);
		clone.SetActive(false);
		return clone;
	}

	private GameObject AllocateDivider()
	{
		if (m_divider)
		{
			GameObject clone = Instantiate(m_divider, m_divider.transform.parent);
			clone.SetActive(false);
			return clone;
		}
		else
		{
			return null;
		}
	}

	/// <summary>
	/// Returns the active child at the specified index, or null if none.
	/// </summary>
	public T GetChild<T>(int index) where T : Component
	{
		if (index < 0)
		{
			throw new System.ArgumentOutOfRangeException("index");
		}
		else if (index >= m_children.Count)
		{
			return null;
		}
		else
		{
			GameObject child = m_children[index];
			return child && child.activeSelf ? child.GetComponent<T>() : null;
		}
	}

	/// <summary>
	/// Deactivates the specified child.
	/// </summary>
	public void Deactivate(int index)
	{
		if (index < 0)
		{
			throw new System.ArgumentOutOfRangeException("index");
		}
		if (index < m_children.Count)
		{
			Deactivate(m_children[index]);
		}
	}

	/// <summary>
	/// Deactivates the specified child.
	/// </summary>
	public void Deactivate(GameObject child)
	{
		if (child)
		{
			child.SetActive(false);
		}
		PostChildActivationChanged();
	}

	/// <summary>
	/// Deactivates the specified child and kicks it to the end of the list.
	/// </summary>
	public void Kick(int index)
	{
		if (index < 0)
		{
			throw new System.ArgumentOutOfRangeException("index");
		}
		else if (index < m_children.Count)
		{
			GameObject child = m_children[index];
			child.SetActive(false);
			m_children.RemoveAt(index);
			m_children.Add(child);
			PostChildActivationChanged();
		}
	}

	/// <summary>
	/// Deactivates the specified child and kicks it to the end of the list.
	/// </summary>
	public void Kick(GameObject child)
	{
		if (m_children.Remove(child))
		{
			child.SetActive(false);
			m_children.Add(child);
			PostChildActivationChanged();
		}
		else
		{
			throw new System.ArgumentException("child is not a child of this populator", "child");
		}
	}

	private void PostChildActivationChanged()
	{
		if (m_emptyObject)
		{
			m_emptyObject.SetActive(ActiveChildren <= 0);
		}

		// update divider activation
		for (int i = 0; i < m_dividers.Count; i++)
		{
			bool active = i + 1 < m_children.Count && m_children[i + 1] && m_children[i + 1].activeSelf;
			m_dividers[i].SetActive(active);
		}

		if (OnPostChildActivationChanged != null)
		{
			OnPostChildActivationChanged(this);
		}
	}
}
