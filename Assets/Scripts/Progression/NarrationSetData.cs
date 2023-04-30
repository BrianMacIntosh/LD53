using System;
using System.Linq;
using UnityEngine;

[Serializable]
public struct NarrationLine
{
	[TextArea]
	public string Caption;
}

public enum NarrationSetPlayMode
{
	Sequential,
	RandomRepeat,
	RandomNoRepeat,
}

[CreateAssetMenu(menuName = "Narration Set")]
public class NarrationSetData : ScriptableObject
{
	public AK.Wwise.Event WwiseEvent;

	public NarrationSetPlayMode PlayMode;

	public NarrationLine[] Lines;

	//HACK: storing data on this scriptable object, ick
	private bool[] m_linesPlayed;

	public int LineCount
	{
		get { return m_linesPlayed.Length; }
	}

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

	public int GetRandomUnplayedLine()
	{
		int unplayedLineCount = m_linesPlayed.Count(state => !state);
		if (unplayedLineCount == 0)
		{
			return int.MaxValue;
		}

		int randomIndex = UnityEngine.Random.Range(0, unplayedLineCount);
		for (int index = 0; index < Lines.Length; index++)
		{
			if (!m_linesPlayed[index])
			{
				if (randomIndex <= 0)
				{
					return index;
				}
				randomIndex--;
			}
		}

		Debug.Assert(false);
		return int.MaxValue;
	}
}
