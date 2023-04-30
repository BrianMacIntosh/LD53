using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoomShifting
{
    public class RoomShifter : MonoBehaviour
    {
        [field: SerializeField] public List<RoomWrapper> Wrapper { get; private set; }

        public void SwapToRandomLocation()
        {
            int newLoc = Random.Range(0, Wrapper.Count);
            SwapToLocation(newLoc);
        }

        public void SwapToLocation(int roomIndex)
        {
            if (roomIndex >= Wrapper.Count || roomIndex < 0)
            {
                Debug.LogError("RoomShifter: loc outside wrapper bounds!");
                return;
            }

            // Prepare Objects
            Transform roomLocation = Wrapper[roomIndex].roomLocation;
            List<GameObject> enableObjects = Wrapper[roomIndex].enableObjects;
            List<GameObject> disableObjects = Wrapper[roomIndex].disableObjects;

            // Set Room Position + Rotation
            transform.position = roomLocation.position;
            transform.rotation = roomLocation.rotation;

            // Enable and Disable Necessary Objects
            enableObjects.ForEach(x => x.gameObject.SetActive(true));
            disableObjects.ForEach(x => x.gameObject.SetActive(false));

            for (int i = 0; i < Wrapper.Count; i++)
            {
                if (i == roomIndex)
                    continue;

                Wrapper[i].enableObjects.ForEach(x => x.gameObject.SetActive(false));
                Wrapper[i].disableObjects.ForEach(x => x.gameObject.SetActive(true));
            }
        }

        private void Start()
        {
            SwapToLocation(0);
        }
    }
}
