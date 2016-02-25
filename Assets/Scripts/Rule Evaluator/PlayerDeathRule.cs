using UnityEngine;
using System.Collections;

public class PlayerDeathRule : RuleBase {
	
	public int deathLimit = 0;
	
	public override bool Evaluate ()
	{
		if (deathLimit > 0 && EventContext.GetEventCount("Dead") >= deathLimit)
			return true;
		return false;
	}
}
