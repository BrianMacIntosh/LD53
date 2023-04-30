using UnityEngine;

[CreateAssetMenu(menuName = "Customer ID")]
public class CustomerId : ScriptableObject
{
    [field: SerializeField] public CustomerRoom MyRoom { get; private set; }
}

public enum CustomerRoom
{
    RedTheater,
    BlueTheater,
    GreenTheater,
    PartyRoom,
    CoachRoom
}
