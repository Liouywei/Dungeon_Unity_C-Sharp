using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SellItem : MonoBehaviour {
	private enum item
	{
		RedWater,
		Attack,
		Defence
	};
	[SerializeField]
	private item sellItem;
	[SerializeField]
	private bool moreCost;  //下一次購買，商品會漲價
	[SerializeField]
	private int cost; //商品價格
	private int nowCost; // 目前的價格

	[SerializeField]
	private Button mBtn;
	[SerializeField]
	private Text mTxt;
	[SerializeField]
	private Sprite[] sprite = new Sprite[2];
	[SerializeField]
	private Image sr;
	[SerializeField]
	private Text moneyTxt;
	[SerializeField]
	private AudioSource ads;
	[SerializeField]
	private AudioClip[] au;
	void OnEnable()
	{
		switch (sellItem)
		{
		case item.RedWater:
			nowCost = Town_PlayerData.tpd.redwatercost;
			break;
		case item.Attack:
			nowCost = Town_PlayerData.tpd.attackcost;
			break;
		case item.Defence:
			nowCost = Town_PlayerData.tpd.defendcost;
			break;
		default:
			break;
		}
		CheckMoney ();
	}

	void OnDisable()
	{
		switch (sellItem)
		{
		case item.RedWater:
			Town_PlayerData.tpd.redwatercost = nowCost;
			break;
		case item.Attack:
			Town_PlayerData.tpd.attackcost = nowCost;
			break;
		case item.Defence:
			Town_PlayerData.tpd.defendcost = nowCost;
			break;
		default:
			break;
		}
	}

	void CheckMoney() //檢查玩家金錢
	{
		moneyTxt.text = "$" + Town_PlayerData.tpd.money;
		mTxt.text = "$" + nowCost;
		if (Town_PlayerData.tpd.money < nowCost) {
			sr.sprite = sprite [1];
			mBtn.interactable = false;
		} else {
			sr.sprite = sprite [0];
			mBtn.interactable = true;
		}
	}

	public void BuyButton()
	{
		if (Town_PlayerData.tpd.money < nowCost) {
			CheckMoney ();
			return;
		}

		ads.PlayOneShot (au [0]);

		//玩家金錢 - newCost
		Town_PlayerData.tpd.money -= nowCost;

		//購買成功，給予商品
		ItemEffect(sellItem);

		//商品價格提高
		if (moreCost)
			ItemCostUP();

		//結束後檢查金錢
		CheckMoney();
	}
		
	void ItemCostUP()  //提高商品售價
	{
		nowCost += cost;
	}

	void ItemEffect(item effect)  //購買後的效果
	{
		switch (effect)
		{
		case item.RedWater:
			Town_PlayerData.tpd.redwater += 1;
			break;
		case item.Attack:
			Town_PlayerData.tpd.attack += 3;
			break;
		case item.Defence:
			Town_PlayerData.tpd.defend += 2;
			break;
		default:
			break;
		}
	}
}
