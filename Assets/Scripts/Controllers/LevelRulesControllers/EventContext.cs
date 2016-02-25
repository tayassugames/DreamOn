using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class works as a global billboard for posting level events.
/// The events will then be used by the rules evaluator.
/// </summary>
public static class EventContext {
	
	//Internal event dictionary
	private static Dictionary<string,int> _events;
	public static Dictionary<string,int> Events {
		get {
			if(_events == null) {
				_events = new Dictionary<string, int>();
			}
			return _events;
		}
	}
	
	public static void ClearEvents() {
		Events.Clear();
	}
	
	/// <summary>
	/// Adds a named event. If the event already exists, increases its value
	/// </summary>
	/// <param name='eventName'>
	/// The name of the event
	/// </param>
	public static void AddEvent(string eventName) {
		if(!Events.ContainsKey (eventName)) {
			Events.Add (eventName, 0);
		}
		Events[eventName]++;
	}
	
	/// <summary>
	/// Gets the event count.
	/// </summary>
	/// <returns>
	/// The event count.
	/// </returns>
	/// <param name='eventName'>
	/// Event name.
	/// </param>
	public static int GetEventCount(string eventName) {
		if(Events.ContainsKey(eventName)) {
			return Events[eventName];
		}
		return 0;
	}
	
}
