using UnityEngine;
using System.Collections;

public class DummyWinRule : RuleBase {

	public override bool Evaluate ()
	{
		return (EventContext.GetEventCount ("Win") > 0);
	}
}
