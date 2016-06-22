/* Работает в паре с EventResiver. Подписывает на нее свою функцию при старте. По нажатию на Esc удаляет себя без отписки.
 * Это показывает, что в delegate при попытке выполнить функцию у объекта, который уже не существует, произойдет ошибка.
 */
using UnityEngine;
using System.Collections;

public class EventSender : MonoBehaviour {
	public EventResiver res;
	public bool isDestroyable = false;
	//private GameObject _thisGO; 
	// Use this for initialization
	void Start () {
		//_thisGO = gameObject;
		res.mydeleg += forDelegat;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) && isDestroyable) {
			Destroy (gameObject);
		}
	}

	public void MoveRight()
	{

		gameObject.transform.position += Vector3.right * 3;
		Debug.Log (gameObject.name);

	}

	public void forDelegat ()
	{
		MoveRight ();
	}
}
