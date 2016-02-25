using UnityEngine;
using System.Collections;

public class GlobalEventManager : MonoBehaviour {
	
	#region Event
	public delegate void GlobalEventHandler(string eventName, object[] eventParams);
	public static event GlobalEventHandler OnGlobalEvent;
	#endregion
	
	
	public static void NotifyEvent(string eventName, object[] eventParams) {
		if(OnGlobalEvent != null) {
			OnGlobalEvent(eventName, eventParams);	
		}
	}
}
