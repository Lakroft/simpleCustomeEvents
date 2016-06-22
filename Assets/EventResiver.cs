/* Работает в паре с EventSender. Смотри описание там. Плюс позволяет проверить, что будет при попытке отписать из делегата
 * метод, которого там нет.
*/
using UnityEngine;
using System.Collections;
using System;

public class EventResiver : MonoBehaviour {

	public delegate void MyDelegate();
	public MyDelegate mydeleg;
	// Use this for initialization
	void Start () {
		
		mydeleg += MoveUp;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.W)) {
			if (mydeleg != null)
			{
//				System.Delegate[] mass =  mydeleg.GetInvocationList ();
//				foreach (System.Delegate temp in mass) {
//					if (temp.Target == null)
//						Debug.Log ("null gameobject");
//					else
//						Debug.Log (temp.Method);
//				}
				try {
					mydeleg ();
				} catch(Exception ex) {
					Debug.Log (ex.Message);
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.E)) { //Отписка функции от делегата
			mydeleg -= MoveUp; //Если попытаться вычесть несуществующую функцию - ничего не случится
			Debug.Log ("unsubscribe");
		}

		if (Input.GetKeyDown (KeyCode.R)) { //Подписка функции на делегат
			mydeleg += MoveUp;
			Debug.Log ("subscribe");
		}
	}

	void MoveUp()
	{
		gameObject.transform.position += Vector3.up * 3;
	}
}
