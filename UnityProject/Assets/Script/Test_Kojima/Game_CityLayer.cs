using UnityEngine;
using System.Collections;

public class Game_CityLayer : SingletonMonoBehaviour<Game_CityLayer> {
	
	public	GameObject[]	Layers;
	private	bool[]			bLayerEnableList;
	
	void Start(){
		bLayerEnableList = new bool[Layers.Length];
		
		for(int i=0; i<Layers.Length; i++)
		{
			bLayerEnableList[i] = false;
			CityLayerEnable(i,true);
		}
	}
	
	// 現在のテンションレベルとtrue入れたらレイヤが表示されますー
	// シングルトンなのでクラス名.Instance.メソッド名でどっからでも呼び出せますー
	public	void	CityLayerEnable(int tention, bool enable){
		
		Renderer[] ren = Layers[tention].GetComponentsInChildren<Renderer>();
		
		foreach(Renderer r in ren)
		{
			r.enabled = enable;
		}
		
	}
/*	
	void Update(){
		if(Input.GetKeyDown(KeyCode.F1))
		{
			CityLayerEnable(0,true);
		}
		if(Input.GetKeyDown(KeyCode.F2))
		{
			CityLayerEnable(1,true);
		}
		if(Input.GetKeyDown(KeyCode.F3))
		{
			CityLayerEnable(2,true);
		}
		if(Input.GetKeyDown(KeyCode.F4))
		{
			CityLayerEnable(3,true);
		}
		if(Input.GetKeyDown(KeyCode.F5))
		{
			CityLayerEnable(4,true);
		}
		if(Input.GetKeyDown(KeyCode.F6))
		{
			CityLayerEnable(5,true);
		}
		if(Input.GetKeyDown(KeyCode.F7))
		{
			CityLayerEnable(6,true);
		}
		if(Input.GetKeyDown(KeyCode.F8))
		{
			CityLayerEnable(7,true);
		}
	}
	*/
}
