using UnityEngine;
using System.Collections;

public class CreditsCapturedRule : RuleBase {
	
	public int neededCredits = 1;
	
	public override bool Evaluate ()
	{
		if (EventContext.GetEventCount("Credit") >= neededCredits)
			return true;
		return false;
	}
}

