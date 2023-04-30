using UnityEngine;

public class WorldRotationLock : MonoBehaviour
{
	private void Update()
	{
		transform.rotation = Quaternion.identity;
	}
}
