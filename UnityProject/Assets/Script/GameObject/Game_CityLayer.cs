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
			CityLayerEnable(i,false);
		}
	}
	
	public	void	CityLayerEnable_WithTween(int tention){
		Renderer[] ren = Layers[tention].GetComponentsInChildren<Renderer>();
		
		foreach(Renderer r in ren)
		{
			r.enabled = true;
			//iTween.ColorTo(r.gameObject, Color.white, 0.5f);
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
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.F1))
		{
			//CityLayerEnable(0,true);
			CityLayerEnable_WithTween(0);
		}
		if(Input.GetKeyDown(KeyCode.F2))
		{
			//CityLayerEnable(1,true);
			CityLayerEnable_WithTween(1);
		}
		if(Input.GetKeyDown(KeyCode.F3))
		{
			//CityLayerEnable(2,true);
			CityLayerEnable_WithTween(2);
		}
		if(Input.GetKeyDown(KeyCode.F4))
		{
			//CityLayerEnable(3,true);
			CityLayerEnable_WithTween(3);
		}
		if(Input.GetKeyDown(KeyCode.F5))
		{
			//CityLayerEnable(4,true);
			CityLayerEnable_WithTween(4);
		}
		if(Input.GetKeyDown(KeyCode.F6))
		{
			//CityLayerEnable(5,true);
			CityLayerEnable_WithTween(5);
		}
		if(Input.GetKeyDown(KeyCode.F7))
		{
			//CityLayerEnable(6,true);
			CityLayerEnable_WithTween(6);
		}
		if(Input.GetKeyDown(KeyCode.F8))
		{
			//CityLayerEnable(7,true);
			CityLayerEnable_WithTween(7);
		}
	}
	
}
