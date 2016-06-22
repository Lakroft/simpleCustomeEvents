/* Работает в паре с SysEvents. Подписывает на нее свою функцию при старте. По нажатию на Esc удаляет себя без отписки.
 * Это показывает, что в events при попытке выполнить функцию у объекта, который уже не существует, произойдет ошибка.
 */
using UnityEngine;
using System.Collections;

public class SysEvSender : MonoBehaviour {
	public SysEvents sys;
	// Use this for initialization
	void Start () {
		sys.onClick += MoveUp;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Destroy (gameObject);
		}
	}
	 
	void MoveUp()
	{
		if (gameObject == null)
			return;
		gameObject.transform.position += Vector3.up * 3;
	}
}
