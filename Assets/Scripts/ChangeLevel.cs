using UnityEngine;
using System.Collections;

public class ChangeLevel : MonoBehaviour {
	[SerializeField]
	private bool isButton;  //true : 按鈕執行切換  false : Start()執行
	[SerializeField]
	private string name;

	void Start()
	{
		if (!isButton)
			LoadLevel();
	}
		
	void LoadLevel()
	{
		Application.LoadLevel(name);
	}
		
	public void LoadLevel(string name)
	{
		Application.LoadLevel(name);
	}
}
