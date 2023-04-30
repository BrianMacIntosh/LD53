using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace RoomShifting
{
    public class RoomShifterInator : MonoBehaviour
    {
        //public static event Action<int> ShiftRoom1;
        //public static event Action<int> ShiftRoom2;
        //public static event Action<int> ShiftRoom3;

        public static RoomShifterInator Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        void ShiftRoom1(int roomPos)
        {

        }
    }
}
