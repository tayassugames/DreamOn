using UnityEngine;
using System.Collections;

public abstract class TimerEventBase : MonoBehaviour, ITimerEvent {

	public abstract void Execute();
}
