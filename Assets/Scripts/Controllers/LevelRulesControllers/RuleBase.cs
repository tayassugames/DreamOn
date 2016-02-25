using UnityEngine;
using System.Collections;

public abstract class RuleBase : MonoBehaviour, IRule {

	public abstract bool Evaluate();
	
}
