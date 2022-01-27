using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerDesgin : MonoBehaviour {

	[SerializeField]
	private Text btnTxt_h;
	[SerializeField]
	private Text btnTxt_b;
	[SerializeField]
	private Text btnTxt_f;
	[SerializeField]
	private Image img_h;  //Head Sprite position
	[SerializeField]
	private Image img_b;  //body Sprite position
	[SerializeField]
	private Image img_f;  //foot Sprite position

	[SerializeField]
	private Sprite[] head;
	[SerializeField]
	private string[] head_txt;
	[SerializeField]
	private Sprite[] body;
	[SerializeField]
	private string[] body_txt;
	[SerializeField]
	private Sprite[] foot;
	[SerializeField]
	private string[] foot_txt;
	[SerializeField]
	private AudioSource ads;
	[SerializeField]
	private AudioClip[] au;

	private int countH;
	private int countB;
	private int countF;


	public void ChangeHead(bool num)  // true: +1  false: -1
	{
		if (num) {
			if (countH + 1 < head.Length)
				countH++;
			else
				countH = 0;
		} else {
			if (countH - 1 > -1)
				countH--;
			else
				countH = head.Length-1;
		}
		btnTxt_h.text = head_txt [countH];
		img_h.sprite = head [countH];
	}

	public void ChangeBody(bool num)  // true: +1  false: -1
	{
		if (num) {
			if (countB + 1 < body.Length)
				countB++;
			else
				countB = 0;
		} else {
			if (countB - 1 > -1)
				countB--;
			else
				countB = body.Length-1;
		}
		btnTxt_b.text = body_txt [countB];
		img_b.sprite = body [countB];
	}

	public void ChangeFoot(bool num)  // true: +1  false: -1
	{
		if (num) {
			if (countF + 1 < foot.Length)
				countF++;
			else
				countF = 0;
		} else {
			if (countF - 1 > -1)
				countF--;
			else
				countF = foot.Length-1;
		}
		btnTxt_f.text = foot_txt [countF];
		img_f.sprite = foot [countF];
	}

	public void SaveChatacter()
	{
		JsonScript.Datas ["Head"] = countH;
		JsonScript.Datas ["Body"] = countB;
		JsonScript.Datas ["Foot"] = countF;
	}

	public void SoundEffect(int num)
	{
		ads.PlayOneShot (au [num]);
	}
}
