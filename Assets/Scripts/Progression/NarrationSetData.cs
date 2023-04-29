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
}
