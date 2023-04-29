using System;
using UnityEngine;

[Serializable]
public struct NarrationLine
{
	[TextArea]
	public string Caption;
}

[CreateAssetMenu(menuName = "Narration Set")]
public class NarrationSetData : ScriptableObject
{
	public AK.Wwise.Event WwiseEvent;

	public NarrationLine[] Lines;

	//HACK: storing data on this scriptable object, ick
	private bool[] m_linesPlayed;

	private void OnEnable()
	{
		m_linesPlayed = new bool[Lines.Length];
	}

	public bool HasLinePlayed(int index)
	{
		if (index < 0)
		{
			index = Lines.Length + index;
		}
		return m_linesPlayed[index];
	}

	public void MarkLinePlayed(int index)
	{
		m_linesPlayed[index] = true;
	}
}
