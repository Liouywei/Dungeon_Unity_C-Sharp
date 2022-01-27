using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ShopOpen : MonoBehaviour {
	[SerializeField]
	private GameObject uiObj;
	[SerializeField]
	private AudioSource ads;
	[SerializeField]
	private AudioClip[] au;

	void OnMouseDown()
	{
		//如果沒有點到UI才執行該物件的程式碼
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			ads.PlayOneShot (au [0]);
			uiObj.SetActive(true);
		} 
	}

	public void CloseUI()
	{
		uiObj.SetActive(false);
	}
}
