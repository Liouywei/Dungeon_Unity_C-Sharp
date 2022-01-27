using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class FloorTrigger : MonoBehaviour {
	//此腳本掛在紅色未點擊的圖上
	[SerializeField]
	private Sprite choseSprite;
	private SpriteRenderer sr;
	private bool canSarch; //可以被點擊
	private Animator anim;

	void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
	}

	void OnTriggerEnter(Collider hit)
	{
		if (hit.tag == "Chose")
		{
			sr.sprite = choseSprite;
			canSarch = true;
		}
	}

	void OnMouseDown()
	{
		if (!canSarch) return;

		//如果沒有點到UI才執行該物件的程式碼
		if (!EventSystem.current.IsPointerOverGameObject ()) {
			anim.SetTrigger("Click");
			FightManager.fm.ImagePos(int.Parse(gameObject.name));		
		}
	}
}
