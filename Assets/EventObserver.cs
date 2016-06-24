using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EventObserver : MonoBehaviour {
	public delegate void EventDelegate();
	protected static List<EventDelegate> forUnsubscribe;
	private static bool needUnsubscribe = false;
	//Это нужно будет перенести в другой скрипт:
	private class MyCustomEvent //Класс события. На каждую подписку создается 1 такой, хранит список делегатов, отлавливает ошибки при попытке 
	{							// вызвать метод уже уничтоженного объекта.
		public MyCustomEvent()
		{
			fancs = new List<EventDelegate>();
		}
		//public string name;
		public List<EventDelegate> fancs;

		public void Subscribe(EventDelegate newDelegate)
		{
			fancs.Add (newDelegate);
		}

		public void Unsubscribe(EventDelegate forRemove)
		{
			fancs.Remove (forRemove);
		}

		public int Count{
			get{ return fancs.Count;}
		}

		public void Execute()
		{
			
			foreach(EventDelegate deleg in fancs)
			{
				if(deleg != null)
				{

					try
					{
						deleg();
					} catch (Exception ex) {
						if (forUnsubscribe == null) {
							forUnsubscribe  = new List<EventDelegate> ();
						}
						needUnsubscribe = true;
						forUnsubscribe.Add(deleg);
						#if UNITY_EDITOR
						Debug.LogError(ex.Message);
						Debug.LogError("Metod, tryed to call: " + deleg.Method.ToString());
						#endif
					}
				}
			}
			if(needUnsubscribe && forUnsubscribe.Count > 0)
			{
				foreach(EventDelegate tempDeleg in forUnsubscribe)
				{
					fancs.Remove(tempDeleg);
					#if UNITY_EDITOR
					Debug.Log("Removed delegat");
					#endif
				}
				forUnsubscribe.Clear();
				needUnsubscribe = false;
			}
		}

	}

	private static EventObserver _this;
	public static EventObserver Instance {
		get {
			if (_this == null)
				_this = new EventObserver ();
			return _this;
		}
	}

	//Здесь начинается собственно действо
	private Dictionary<string, MyCustomEvent> eventDictionary = new Dictionary<string, MyCustomEvent>();
	private Queue<string> events = new Queue<string> ();
	public bool DebugMode = false;
	private MyCustomEvent tempEvent;

	void Awake () {
		_this = this;
	}

	void Update () { //Выполнение событий происходит в апдейте, до тех пор события хранятся в очереди.
		//Profiler.BeginSample("Observer");
		if (events.Count > 0) {
			string tempKey;
			while (events.Count > 0) {
				tempKey = events.Dequeue ();
				if (eventDictionary.TryGetValue (tempKey, out tempEvent)) {
					tempEvent.Execute ();
					#if UNITY_EDITOR
					if(DebugMode)
					{
						Debug.Log("Executing event " + tempKey);
					}
					#endif
				} else {
					Debug.LogError ("No Event " + tempKey + " Registered"); //Здесь отлавливаются попытки вызвать несуществующее событие.
				}
			}
		}
		//Profiler.EndSample();
	}

	public void Subscribe(string key, EventDelegate newEvent, GameObject sender) //Подписка на событие. Геймобджект нужен, что-бы в эдиторе можно было отследить, кто и когда подписывается.
	{
		if (!eventDictionary.ContainsKey (key)) {
			tempEvent = new MyCustomEvent ();
			eventDictionary.Add (key, tempEvent);
		} else {
			eventDictionary.TryGetValue(key, out tempEvent);
		}
		tempEvent.Subscribe (newEvent);
		#if UNITY_EDITOR
		if (DebugMode) {
			Debug.Log ("Subscribe " + sender.name + " for event " + key);
		}
		#endif
	}

	public void Unsubscribe(string key, EventDelegate forRemove)
	{
		if (eventDictionary.TryGetValue (key, out tempEvent)) {
			tempEvent.Unsubscribe (forRemove);
			if (tempEvent.Count == 0) {
				eventDictionary.Remove (key); //Очищаем лишние события.
				if (eventDictionary.Count == 0) {//Если подписок нет, можно не занимать ресурсы и удалиться.
					Destroy (_this);
				}
			}
		} else {
			Debug.LogError("Trying to unsubscribe unexisting event " + key);
		}
	}

	public void ThrowEvent(string key) //
	{
		events.Enqueue (key);
	}
}
