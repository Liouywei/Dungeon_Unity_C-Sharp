using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System.IO;

public class JsonScript : MonoBehaviour {
	[SerializeField]
	private string Path = @"./AAAA";  //存放檔案資料夾路徑
	//要儲存進Json檔的陣列
	public static Hashtable Datas = new Hashtable(){
		{"Head", 0},
		{"Body", 0},
		{"Foot", 0},
		{"Attack", 15},
		{"Defend", 10},
		{"Money", 10},
		{"RedWater", 2},
		{"RedWaterCost", 10},
		{"AttackCost", 1},
		{"DefendCost", 1}
	};

	public void Save()
	{
		if (!Directory.Exists(Path))             //如果資料夾不存在
			Directory.CreateDirectory(Path);     //建立資料夾

		//                              路徑 + 檔名
		FileStream fs = new FileStream(Path + @"\PData.json", FileMode.OpenOrCreate);

		StreamWriter sw = new StreamWriter(fs);

		string s = JsonConvert.SerializeObject(Datas);
		sw.Write(s);

		//一定要寫，不然程式碼會出錯
		sw.Close();
	}

	public void Load()
	{
		if (!Directory.Exists (Path)) 
			return;

		FileStream fs = new FileStream(Path + @"\PData.json", FileMode.Open);

		StreamReader sr = new StreamReader(fs);

		//要有個變數來裝讀取的資料
		string loadData = sr.ReadToEnd();
		//讀取完就關閉，避免出錯
		sr.Close();

		//把先前存進string的資料在放回陣列裡
		Datas = JsonConvert.DeserializeObject<Hashtable>(loadData);
	}
}
