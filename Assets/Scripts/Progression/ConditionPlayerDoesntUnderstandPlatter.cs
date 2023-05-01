using System;

public class ConditionPlayerDoesntUnderstandPlatter : BaseProgressionCondition
{
	public override bool Evaluate()
	{
		return !PlayerInventory.Instance.HasChangedSlot;
	}
}
