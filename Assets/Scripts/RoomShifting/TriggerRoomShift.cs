using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoomShifting
{
    public class TriggerRoomShift : MonoBehaviour
    {
        [SerializeField] RoomShifter shifter;

        private void OnTriggerEnter(Collider other)
        {
            shifter.SwapToRandomLocation();
        }
    }
}
