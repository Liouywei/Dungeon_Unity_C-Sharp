using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
	public static FightManager fm;
	private JsonScript js;
	[SerializeField]
	private AudioSource ads;
	[SerializeField]
	private AudioClip[] au;
	[Header("[角色物件]")]
	[SerializeField]
	private Transform player_Pos;
	[Header("角色頭盔貼圖")]
	[SerializeField]
	private SpriteRenderer pHead;
	[Header("角色盔甲貼圖")]
	[SerializeField]
	private SpriteRenderer pBody;
	[Header("角色褲子貼圖")]
	[SerializeField]
	private SpriteRenderer pFoot;
	[Header("[角色頭盔Sprite陣列]")]
	[SerializeField]
	private Sprite[] pl_Head_Sprite = new Sprite[3];
	[Header("[角色盔甲Sprite陣列]")]
	[SerializeField]
	private Sprite[] pl_Body_Sprite = new Sprite[3];
	[Header("[角色褲子Sprite陣列]")]
	[SerializeField]
	private Sprite[] pl_Foot_Sprite = new Sprite[3];
	[Header("[Cube位置]")]
	[SerializeField]
	private Transform[] cubePos = new Transform[16];
	[Header("[紅色地板Prefab]")]
	[SerializeField]
	private GameObject floorPrefab;
	[Header("門物件]")]
	[SerializeField]
	private GameObject gateObj;
	[Header("[石頭Prefab]")]
	[SerializeField]
	private GameObject stonePrefab;
	[Header("[怪物小圖]")]
	[SerializeField]
	private GameObject monsterImg;
	[Header("[事件小圖]")]
	[SerializeField]
	private GameObject surpriseImg;
	[SerializeField]
	[Header("小圖物件池")]
	private GameObject[] icon;
	private Vector3[] floorPos = new Vector3[16];
	[Header("[三種怪物Prefabs]")]
	[SerializeField]
	private GameObject[] monsterPrefabs = new GameObject[3];  //怪物的種類   
	[Header("[寶箱Prefab]")]
	[SerializeField]
	private GameObject surprisePrefabs;   //寶箱
	private int[,] monsterDatas;          //怪物的屬性   需要重新選擇陣列類型   
	[Header("[怪物的類型]")]
	[SerializeField]
	private int[] monsterType = new int[16];           //紀錄被生成的怪物是哪一種
	private GameObject[] ObjPool = new GameObject[16]; //怪物、寶箱的物件池   
	[Header("[觸發地板的物件]")]
	[SerializeField]
	private Transform triggerObj;         //觸發開啟可點選Floor的物件  
	private int? choseFloorId;            //紀錄玩家點選的Floor編號
	[Header("玩家站的位置")]
	[SerializeField]
	private Transform FightPosA;
	[Header("怪物/事件站的位置")]
	[SerializeField]
	private Transform FightPosB;


	[Header("[Cube動畫]")]
	[SerializeField]
	private Animator cubeAnim;
	[Header("[舞台的黑幕_淡入動畫]")]
	[SerializeField]
	private Animator blackPanel_Anim;
	[Header("[舞台的背景移動動畫]")]
	[SerializeField]
	private Animator background_Anim;
	[Header("[角色動畫]")]
	[SerializeField]
	private Animator player_Anim;
	[Header("切換場景的黑幕動畫")]
	[SerializeField]
	private Animator BlackPanel_Scene;
	private Animator[] surpriseAnim = new Animator[16];  //事件們的動畫


	[SerializeField]
	[Header("阻止玩家繼續操作的Panel")]
	private GameObject stopPanel;
	[Header("錢的Txt")]
	[SerializeField]
	private Text coinUI_txt;
	[Header("血條UI")]
	[SerializeField]
	private Image hpBar;
	[SerializeField]
	[Header("回城按鈕")]
	private GameObject townBTN;
	[SerializeField]
	[Header("藥水按鈕")]
	private GameObject healBTN;
	[SerializeField]
	[Header("藥水數量文字")]
	private Text waterTxt;
	[SerializeField]
	[Header("詢問是否進入下一層UI")]
	private GameObject askUI;
	[Header("死亡UI動畫")]
	[SerializeField]
	private Animator deathImg;


	[Header("玩家血量")]
	[SerializeField]
	private int maxHP;
	[Header("玩家當前血量")]
	[SerializeField]
	private int hp;
	[Header("[玩家的攻擊力]")]
	[SerializeField]
	private int attack;
	[Header("[玩家的防禦力]")]
	[SerializeField]
	private int defend;
	[Header("[玩家的藥水數量]")]
	[SerializeField]
	private int redwater;
	[Header("[獲得金錢]")]
	[SerializeField]
	private int tempMoney;
	[Header("當前怪物血量")]
	[SerializeField]
	private int tempMonsterHP;
	[Header("當前怪物攻擊力")]
	[SerializeField]
	private int tempMonsterAttack;
	[Header("當前怪物防禦力")]
	[SerializeField]
	private int tempMonsterDefend;


	[Header("[生成石頭數量]")]
	[SerializeField]
	private int stoneCount;
	[Header("[生成怪物數量]")]
	[SerializeField]
	private int monsterCount;
	[Header("[生成事件數量]")]
	[SerializeField]
	private int surpriseCount;


	[Header("[隨機後的事件圖]")]
	[SerializeField]
	private int[] eventArr;
	/*紀錄地圖配置
      1 : 代表入口(起始位置)

      2 : 代表出口(終點位置)

      3 : 代表石頭(路障)

      4 : 代表怪物

      5 : 代表事件

      0   1   2   3
      4   5   6   7
      8   9  10  11
     12  13  14  15
    */

	void Awake()
	{
		fm = this;
		js = GetComponent<JsonScript> ();
	}

	void Start()
	{
		SetMonsterData();
		GetPlayerData();  //取得玩家資料

		EventRandom();    //隨機地圖分配
		CreateFloor();
		CreateStone();
		CreatMonsterIcon();
		CreateSurpriseIcon();
		CreateMonster();
		CreateSurprise();

		StartCoroutine("CubeRotation");  //旋轉Cube動畫
		BackGroundAnim(true);            //背景移動動畫
		PlayerAnim("Move");              //角色移動動畫
	}

	//遊戲初始 相關
	//------------------------------------------------
	void SetMonsterData()  //初始化怪物資料
	{
		monsterDatas = new int[3, 4]{
			{0, 30, 15, 5},   //編號 , 血量, 攻擊, 防禦
			{1, 60, 21, 1},
			{2, 80, 16, 8}
		};
	}

	void GetPlayerData() //取得玩家資料
	{
		//JsonScript.js.Load ();

		attack = int.Parse(JsonScript.Datas["Attack"].ToString()); 
		defend = int.Parse(JsonScript.Datas["Defend"].ToString()); 
		redwater = int.Parse(JsonScript.Datas["RedWater"].ToString()); 
		pHead.sprite = pl_Head_Sprite[int.Parse(JsonScript.Datas["Head"].ToString())];
		pBody.sprite = pl_Body_Sprite[int.Parse(JsonScript.Datas["Body"].ToString())];
		pFoot.sprite = pl_Foot_Sprite[int.Parse(JsonScript.Datas["Foot"].ToString())];

		maxHP = 100;
		hp = maxHP;
		waterTxt.text = redwater.ToString();
		hpBar.fillAmount = (float)hp / maxHP;
		player_Pos.position = FightPosA.position;
	}

	void EventRandom()   //隨機地圖物件排放
	{
		if ((stoneCount + monsterCount + surpriseCount) > eventArr.Length - 2) { print("物件數量 大於 地圖格子"); return; }

		bool[] isBuild = new bool[16];
		int range = 0;
		bool isnull = false;


		isBuild[12] = true;  //決定入口
		eventArr[12] = 1;    //產生 "入口" 程式碼


		isBuild[3] = true;  //決定出口
		eventArr[3] = 2;    //產生 "出口" 程式碼
		GameObject temp = Instantiate(gateObj, new Vector3(cubePos[3].position.x, cubePos[3].position.y, 0.52f), Quaternion.identity) as GameObject;
		temp.transform.SetParent (cubePos [3]);
		icon [3] = temp;
		icon[3].SetActive(false);

		//亂數 "石頭" 的位置
		for (int i = 0; i < stoneCount; i++)
		{
			range = Random.Range(0, isBuild.Length);
			while (!isnull)
			{
				if (isBuild[range] == false)
				{
					isBuild[range] = true;
					eventArr[range] = 3;  //產生石頭物件程式碼
					isnull = true;
				}
				else
				{
					range = Random.Range(0, isBuild.Length);
					isnull = false;
				}
			}
			isnull = false;
		}


		//亂數 "怪物" 的位置
		for (int i = 0; i < monsterCount; i++)
		{
			range = Random.Range(0, isBuild.Length);
			while (!isnull)
			{
				if (isBuild[range] == false)
				{
					isBuild[range] = true;
					eventArr[range] = 4;  //產生怪物程式碼
					isnull = true;
				}
				else
				{
					range = Random.Range(0, isBuild.Length);
					isnull = false;
				}
			}
			isnull = false;
		}

		//亂數 "事件" 的位置
		for (int i = 0; i < surpriseCount; i++)
		{
			range = Random.Range(0, isBuild.Length);
			while (!isnull)
			{
				if (isBuild[range] == false)
				{
					isBuild[range] = true;
					eventArr[range] = 5;  //產生 "事件" 程式碼
					isnull = true;
				}
				else
				{
					range = Random.Range(0, isBuild.Length);
					isnull = false;
				}
			}
			isnull = false;
		}
	}

	void CreateFloor()   //生成紅色地板並將座標指定給Cube
	{
		for (int i = 0; i < cubePos.Length; i++)
		{
			if (eventArr[i] != 3 && eventArr[i] != 1)  //如果該座標是石頭(路障)/ 入口 則跳過
			{
				GameObject temp = Instantiate(floorPrefab, new Vector3(cubePos[i].position.x, cubePos[i].position.y, 0.52f), Quaternion.identity) as GameObject;
				temp.name = i.ToString();
				floorPos[i] = temp.transform.position;
				temp.transform.SetParent(cubePos[i]);
			}
		}
	}

	void CreateStone()   //生成石頭(路障)並將座標指定給Cube
	{
		for (int i = 0; i < eventArr.Length; i++)
		{
			if (eventArr[i] == 3)
			{
				GameObject temp = Instantiate(stonePrefab, new Vector3(cubePos[i].position.x, cubePos[i].position.y, 0.52f), Quaternion.identity) as GameObject;
				temp.transform.SetParent(cubePos[i]);
			}
		}
	}

	void CreatMonsterIcon()  //生成怪物小圖
	{
		for (int i = 0; i < eventArr.Length; i++)
		{
			if (eventArr[i] == 4)
			{
				GameObject temp = Instantiate(monsterImg, new Vector3(cubePos[i].position.x, cubePos[i].position.y, 0.52f), Quaternion.identity) as GameObject;
				icon[i] = temp;
				icon[i].SetActive(false);
				temp.transform.SetParent(cubePos[i]);
			}
		}
	}

	void CreateSurpriseIcon()  //生成事件小圖
	{
		for (int i = 0; i < eventArr.Length; i++)
		{
			if (eventArr[i] == 5)
			{
				GameObject temp = Instantiate(surpriseImg, new Vector3(cubePos[i].position.x, cubePos[i].position.y, 0.52f), Quaternion.identity) as GameObject;
				icon[i] = temp;
				icon[i].SetActive(false);
				temp.transform.SetParent(cubePos[i]);
			}
		}
	}

	void CreateMonster() //生成怪物並將座標指定到舞台
	{
		for (int i = 0; i < eventArr.Length; i++)
		{
			if (eventArr[i] == 4)
			{
				int x = Random.Range(0, monsterPrefabs.Length);
				GameObject temp = Instantiate(monsterPrefabs[x], FightPosB.position, Quaternion.identity) as GameObject;
				monsterType[i] = x;          //紀錄怪物的種內
				ObjPool[i] = temp;           //存入物件池
				ObjPool[i].SetActive(false); //關閉物件
			}
		}
	}

	void CreateSurprise() //生成寶箱並將座標指定到舞台
	{
		for (int i = 0; i < eventArr.Length; i++)
		{
			if (eventArr[i] == 5)
			{
				GameObject temp = Instantiate(surprisePrefabs, FightPosB.position, Quaternion.identity) as GameObject;
				ObjPool[i] = temp;           //存入物件池
				surpriseAnim[i] = temp.GetComponent<Animator>();
				ObjPool[i].SetActive(false); //關閉物件
			}
		}
	}

	IEnumerator CubeRotation()  //播放地板旋轉動畫
	{
		yield return new WaitForSeconds(1.5f);
		stopPanel.SetActive (false);
		cubeAnim.SetTrigger("Rot");
		yield return new WaitForSeconds (1.2f);
		TriggerObjMove(12);
	}
	//------------------------------------------------



	//點擊地板 相關
	//------------------------------------------------

	//此函式在 Floor被點擊時，由被點擊的Floor呼叫
	public void ImagePos(int id)  //id : floor編號
	{
		CloseButtonUI(); //關閉喝水、回城按鈕
		SoundEffect(4);  

		if (eventArr [id] == 0) {
			NothingEvent (id);
			return;           //如果該Floor沒有任何事件，直接跳出
		}  

		int eventKind = eventArr[id];  //取得該座標的事件種類
		choseFloorId = id;             //暫存玩家點選的Floor編號


		//點開地板後，開啟特效結束後
		//將怪物/事件 小圖座標移到該floor的座標
		if (eventKind == 4)
		{
			icon[id].SetActive(true);
			SoundEffect (1);
		}

		if (eventKind == 5)
		{
			icon[id].SetActive(true);
			SoundEffect (2);
		}

		if (eventKind == 2)
		{
			icon[id].SetActive(true);
			SoundEffect (3);
		}
			
		switch (eventArr[id])
		{
		case 4:
			StartCoroutine("Event_Monster");
			break;
		case 5:
			StartCoroutine("Event_Surprise");
			break;
		case 2:
			StartCoroutine("Event_Gate");
			break;
		default:
			break;
		}
	}

	IEnumerator Event_Monster()
	{
		stopPanel.SetActive(true);  //打開阻擋玩家繼續操作的Panel
		yield return new WaitForSeconds(2f);
		BlackPanel();          //播放黑幕淡入
		BackGroundAnim(false); //停止背景移動動畫
		PlayerAnim("Idle");    //播放玩家待機動畫
		yield return new WaitForSeconds(0.5f);
		ObjPool[(int)choseFloorId].SetActive(true);  //打開怪物物件
		yield return new WaitForSeconds(1.5f);
		PlayerAnim("Attack");  //播放玩家攻擊動畫
		SoundEffect(0);
		yield return new WaitForSeconds(1f);
		Fighting();            //計算戰鬥結果
	}

	IEnumerator Event_Surprise()
	{
		stopPanel.SetActive(true);  //打開阻擋玩家繼續操作的Panel
		yield return new WaitForSeconds(2f);
		BlackPanel();          //播放黑幕淡入
		BackGroundAnim(false); //停止背景移動動畫
		PlayerAnim("Idle");    //播放玩家待機動畫
		yield return new WaitForSeconds(0.5f);
		ObjPool[(int)choseFloorId].SetActive(true); //打開事件物件
		yield return new WaitForSeconds(1.5f);
		surpriseAnim[(int)choseFloorId].SetTrigger("Play");  //播放事件動畫
		yield return new WaitForSeconds(1.2f);
		Surprising();          //計算金錢結果
	}

	IEnumerator Event_Gate()
	{
		stopPanel.SetActive(true);  //打開阻擋玩家繼續操作的Panel
		yield return new WaitForSeconds(1f);
		askUI.SetActive(true); //打開詢問UI
	}

	void NothingEvent(int id) //沒有遇到任何事件
	{
		CloseButtonUI();       //打開喝水、回城按鈕
		TriggerObjMove(id);        //打開可點選的Floor
	}

	IEnumerator EventEnd()  //結束任何事件
	{
		yield return new WaitForSeconds(1f);
		BlackPanel();          //播放黑幕淡入
		BackGroundAnim(true);  //播放背景移動動畫
		PlayerAnim("Move");    //播放玩家移動動畫
		CloseButtonUI();       //打開喝水、回城按鈕
		stopPanel.SetActive(false);  //關閉阻擋玩家繼續操作的Panel
		icon[(int)choseFloorId].SetActive(false); //關閉該Floor上的小圖示
		TriggerObjMove((int)choseFloorId);        //打開可點選的Floor
	}

	IEnumerator Death()    //角色死亡
	{
		yield return new WaitForSeconds(2f);
		deathImg.enabled = true;  //打開死亡UI動畫
		yield return new WaitForSeconds(4f);
		Application.LoadLevel("Town");   //回城
	}

	IEnumerator BackToTown()  //角色回城
	{
		SaveData();
		BlackPanel_Scene.SetTrigger("Close");  //播放黑幕淡出
		yield return new WaitForSeconds(1.5f);
		Application.LoadLevel("Town");   //回城

	}
	//------------------------------------------------


	//Animator 相關
	//------------------------------------------------
	void BlackPanel()  //打開舞台(地下城畫面) 黑幕的動畫 淡入
	{
		blackPanel_Anim.SetTrigger("Open");
	}

	void BackGroundAnim(bool playing)  //地下城背景動畫
	{
		background_Anim.SetBool("Move", playing);
	}

	void PlayerAnim(string state)      //角色動畫
	{
		player_Anim.SetTrigger(state);
	}
	//------------------------------------------------

	void TurnStopPanel() //打開 / 關閉 阻擋玩家繼續操作的Panel
	{
		stopPanel.SetActive(!stopPanel.activeSelf);
	}

	void CloseButtonUI()
	{
		townBTN.SetActive(!townBTN.activeSelf);  //打開/關閉回城按鈕
		healBTN.SetActive(!healBTN.activeSelf);  //打開/關閉喝水按鈕
	}

	void Fighting()  //戰鬥
	{
		bool fighting = true;

		tempMonsterHP = monsterDatas[monsterType[(int)choseFloorId], 1];      //取得怪物血量
		tempMonsterAttack = monsterDatas[monsterType[(int)choseFloorId], 2];  //取得怪物攻擊
		tempMonsterDefend = monsterDatas[monsterType[(int)choseFloorId], 3];  //取得怪物防禦

		//計算公式 : hp -= (敵方攻擊力 - 自己防禦力) 
		while (fighting)
		{
			//int playerAttack = (玩家攻擊力 <= 敵人防禦力) ? 0 : (玩家攻擊力 - 敵方防禦力);
			int playerAttack = (attack <= tempMonsterDefend) ? 0 : (attack - tempMonsterDefend);
			//int EnemyAttack = (怪物攻擊力 <= 玩家防禦力) ? 0 : (怪物攻擊力 - 玩家防禦力);
			int EnemyAttack = (tempMonsterAttack <= defend) ? 0 : (tempMonsterAttack - defend);

			hp -= EnemyAttack;              //玩家Hp -= EnemyAttack;
			tempMonsterHP -= playerAttack; //怪物Hp -= playerAttack;

			hpBar.fillAmount = (float)hp / maxHP;  //變更玩家血條UI

			if (hp <= 0 || tempMonsterHP <= 0)
			{
				fighting = false;
			}
		}


		if (hp <= 0)
		{
			StartCoroutine("Death");
		}
		else if (tempMonsterHP <= 0)
		{
			ObjPool[(int)choseFloorId].SetActive(false);  //關閉怪物物件
			GetCoin(); //獲得金錢
			StartCoroutine("EventEnd");
		}

	}

	void Surprising()  //得到金幣(寶箱)
	{
		int x = Random.Range(10, 101);
		tempMoney += x;
		coinUI_txt.text = "" + tempMoney;

		ObjPool[(int)choseFloorId].SetActive(false);  //關閉事件物件
		StartCoroutine("EventEnd");
	}

	void GetCoin()  //得到金幣(殺死怪物)
	{
		int coin = Random.Range(1, 11);
		tempMoney += coin;
		coinUI_txt.text = "" + tempMoney;
	}

	void SaveData()  //儲存探險收穫
	{
		JsonScript.Datas["Money"] = int.Parse(JsonScript.Datas["Money"].ToString()) + tempMoney;
		JsonScript.Datas["RedWater"] = redwater;
	}

	public void TownButton() //回城按鈕
	{
		StartCoroutine("BackToTown");
	}

	public void RedWaterButton(int Heal)  //補血按鈕
	{
		if (redwater <= 0) return;
		if (hp == maxHP) return;
			
		redwater--;
		waterTxt.text = redwater.ToString();
		//角色HP = (角色HP+heal <= MaxHP) ? 角色HP+heal : MaxHP;
		hp = (hp + Heal <= maxHP) ? (hp + Heal) : maxHP;
		//更新角色血條UI
		hpBar.fillAmount = (float)hp / maxHP;
	}

	void TriggerObjMove(int id) //將觸發用的物件移動到玩家選擇的Floor座標上，選擇Floor後，開啟周圍可選擇的Floor
	{
		triggerObj.position = cubePos [id].position;
	}

	void SoundEffect(int num)
	{
		ads.PlayOneShot (au [num]);
	}
}
