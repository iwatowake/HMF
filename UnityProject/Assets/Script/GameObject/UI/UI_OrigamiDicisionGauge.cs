using UnityEngine;
using System.Collections;

/* 折り紙折る時用の決断ゲージ */
public class UI_OrigamiDicisionGauge : SingletonMonoBehaviour<UI_OrigamiDicisionGauge> {
	
	//! モード
	public enum FILLMODE 
	{
		AutoIncrease,	// 自動加算
		AutoDecrease,	// 自動減算
		Fixed			// 勝手に動かない
	}
	
	public enum SPRITETYPE
	{
		Select,
		Cancel
	}
	
	[HideInInspector]
	public			float			fillTime  	= 1.5f;				//!<	ゲージが0から最大までたまるまでの時間(秒)
	
	private 		UIFilledSprite	sprite;							//!<	ゲージのスプライト
	
	private			FILLMODE		fillMode  	= FILLMODE.Fixed;	//!<	モード
	
	public			GameObject		GaugeLasrWaveEffect;
	public			GameObject		GaugeAppearWaveEffect;
	public			UISprite		sp_cancel;
	
	/* 初期化 */
	void Start(){
		sprite = gameObject.GetComponent<UIFilledSprite>();
		sp_cancel.alpha = 0;
	}
	
	/* 現在のゲージ値を時間(秒)に変換して取得  範囲は0～fillTime */
	public	float	GetNowAmount(){
		return sprite.fillAmount * fillTime;
	}
	
	/* 現在のゲージ値を時間(秒)に変換してセット */
	public	void	SetNowAmount(float time){
		sprite.fillAmount = time / fillTime;
	}
	
	/* 現在のゲージ値を0～1で取得 */
	public	float	GetNowAmount01(){
		return sprite.fillAmount;
	}
	
	/* 現在のゲージ値を0～1でセット */
	public	void	SetNowAmount01(float amount){
		sprite.fillAmount = amount;
	}	
	
	/* 再生 */
	public	void	Play(){
		spriteEnable(true);
		fillMode = FILLMODE.AutoIncrease;
		//Instantiate(GaugeAppearWaveEffect,gameObject.transform.position,Quaternion.identity);
	}
	
	/* 逆再生 */
	public	void	PlayInverse(){
		spriteEnable(true);
		fillMode = FILLMODE.AutoDecrease;
	}
	
	/* ゼロから再生 */
	public	void	PlayAtZero(){
		sprite.fillAmount = 0;
		spriteEnable(true);
		fillMode = FILLMODE.AutoIncrease;
		//Instantiate(GaugeAppearWaveEffect,gameObject.transform.position,Quaternion.identity);
	}
	
	public	void	SetSpriteType(SPRITETYPE type){
		switch(type)
		{
		case SPRITETYPE.Select:
			sprite.spriteName = "Gauge_min";
			sp_cancel.alpha = 0;
			break;
		case SPRITETYPE.Cancel:
			sprite.spriteName = "Gauge_max";
			sp_cancel.alpha = 1;
			break;
		}
	}
	
	/* 一時停止 */
	public	void	Pause(){
		fillMode = FILLMODE.Fixed;
	}
	
	/* 停止(ゲージ値はゼロになる) */
	public	void	Stop(){
		sprite.fillAmount = 0;
		fillMode = FILLMODE.Fixed;
	}
	
	/* 更新 */
	void Update () {
		// モードごとの動作
		switch(fillMode)
		{
			// 自動加算モード ゲージが最大になったら一時停止する
		case FILLMODE.AutoIncrease:
			sprite.fillAmount += Time.deltaTime / fillTime;
			if(sprite.fillAmount >= 1)
			{
				fillMode = FILLMODE.Fixed;
				sp_cancel.alpha = 0;
			}
			break;
			
			// 自動減算モード ゲージが0になったら一時停止する
		case FILLMODE.AutoDecrease:
			sprite.fillAmount -= Time.deltaTime / fillTime;
			if(sprite.fillAmount <= 0)
			{
				fillMode = FILLMODE.Fixed;
			}
			break;
			
			// 勝手に動かないモード　Setは反映される
		case FILLMODE.Fixed:
			break;
		}
	}
	
	/* ゲージたまったらtrue返すやつ */
	public bool isFilled(){
		if(sprite.fillAmount >= 1.0f)
		{
			Instantiate(GaugeLasrWaveEffect,gameObject.transform.position,Quaternion.identity);
			return true;
		}else{
			return false;
		}
	}
	
	/* 表示非表示切り替え */
	public void spriteEnable(bool b){
		if( b )
		{
			sprite.alpha = 1.0f;
		}else{
			sprite.alpha = 0.0f;
		}
	}
}
