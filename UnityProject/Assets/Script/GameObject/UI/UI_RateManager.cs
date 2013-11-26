using UnityEngine;
using System.Collections;

public class UI_RateManager : SingletonMonoBehaviour<UI_RateManager> {
	
	public UISprite[]	spRates;
	UI_Percentage		percentage;
	int					nowAnimating = -1;
	

	void Start () {
		percentage = GetComponentInChildren<UI_Percentage>();
	}
	

	void Update () {
		if(nowAnimating>-1)
		{
			spRates[nowAnimating].alpha -= Time.deltaTime * 0.5f;
			if(spRates[nowAnimating].alpha <= 0.0f)
			{
				nowAnimating = -1;
			}
		}
/*		
		if(Input.GetKeyDown(KeyCode.F))
		{
			SetRate(50);
		}
		if(Input.GetKeyDown(KeyCode.G))
		{
			SetRate(80);
		}
		if(Input.GetKeyDown(KeyCode.H))
		{
			SetRate(95);
		}
		if(Input.GetKeyDown(KeyCode.J))
		{
			SetRate(100);
		}
*/
	}
	
	
	public void SetRate(float per){
		int rate = -1;
		
		if(per < 75.0f)
		{
			rate = 0;
		}else if(per < 85.0f){
			rate = 1;
		}else if(per < 95.0f){
			rate = 2;
		}else if(per <= 100.0f){
			rate = 3;
		}
		
		nowAnimating = Mathf.Clamp(rate, 0, spRates.Length);
		spRates[rate].alpha = 1.0f;
		spRates[rate].transform.localPosition = new Vector3(0,-200,0);
		spRates[rate].gameObject.GetComponent<iTweenEvent>().Play();
		
		percentage.Activate(per);
		
		UI_TentionGauge.Instance.SetResult(per);
		UI_ScoreCounter.Instance.SetResult(per);
	}
}
