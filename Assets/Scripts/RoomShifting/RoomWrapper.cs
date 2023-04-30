using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoomShifting
{
    [System.Serializable]
    public class RoomWrapper
    {
        public Transform roomLocation;
        public List<GameObject> enableObjects;
        public List<GameObject> disableObjects;
    }
}
