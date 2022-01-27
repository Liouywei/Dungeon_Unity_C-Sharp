using UnityEngine;
using System.Collections;

public class Town_PlayerData : MonoBehaviour {
	public static Town_PlayerData tpd;
	private JsonScript js;

	public int attack;
	public int defend;
	public int money;
	public int redwater;
	public int redwatercost;
	public int attackcost;
	public int defendcost;

	void Awake()
	{
		tpd = this;
		js = GetComponent<JsonScript> ();
	}

	void Start () {
		attack = int.Parse(JsonScript.Datas ["Attack"].ToString());
		defend = int.Parse(JsonScript.Datas ["Defend"].ToString());
		money = int.Parse(JsonScript.Datas ["Money"].ToString());
		redwater = int.Parse(JsonScript.Datas ["RedWater"].ToString());
		redwatercost = int.Parse(JsonScript.Datas ["RedWaterCost"].ToString());
		attackcost = int.Parse(JsonScript.Datas ["AttackCost"].ToString());
		defendcost = int.Parse(JsonScript.Datas ["DefendCost"].ToString());
	}

	public void UpdateData()
	{
		JsonScript.Datas ["Attack"] = attack;
		JsonScript.Datas ["Defend"] = defend;
		JsonScript.Datas ["Money"] = money;
		JsonScript.Datas ["RedWater"] = redwater;
		JsonScript.Datas ["RedWaterCost"] = redwatercost;
		JsonScript.Datas ["AttackCost"] = attackcost;
		JsonScript.Datas ["DefendCost"] = defendcost;
	}
}
