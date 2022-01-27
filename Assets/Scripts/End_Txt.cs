using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class End_Txt : MonoBehaviour {
	private Text txt;
	[SerializeField]
	private string[] words;
	private int count;
	[SerializeField]
	private Animator blackPanel;

	void Start()
	{
		txt = GetComponent<Text> ();
		StartCoroutine ("StoryTxt");
	}

	IEnumerator StoryTxt()
	{
		yield return new WaitForSeconds (3f);
		txt.text += words [count] + "\n" + "\n";
		count++;

		if (count < words.Length) {
			StartCoroutine ("StoryTxt");
		} else {
			blackPanel.SetTrigger ("Close");	
			Invoke ("EndGame", 6f);
		}
	}

	void EndGame()
	{
		Application.LoadLevel ("Logo");
	}
}
