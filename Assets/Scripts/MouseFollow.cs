using UnityEngine;
using System.Collections;

public class MouseFollow : MonoBehaviour {
	private Vector3 mouPos;

	void Start()
	{
		Cursor.visible = false;
	}

	void Update()
	{
		MousePosMove ();
	}

	void MousePosMove()
	{
		mouPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mouPos.z = -2f;
		transform.position = mouPos;
	}

	void OnDisable()
	{
		Cursor.visible = true;
	}
}
