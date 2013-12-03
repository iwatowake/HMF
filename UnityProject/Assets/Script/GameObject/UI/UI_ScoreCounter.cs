using UnityEngine;
using System.Collections;

public class UI_ScoreCounter : SingletonMonoBehaviour<UI_ScoreCounter> {
	
	private			int			score			= 0;
	private			int			combo			= 0;
	private			float		scoreCounter	= 0;
	private			UILabel		label;
	
	private	const	float		SC_COUNTSPEED 	= 600.0f;
	
	public	int		Score{
		get{return score;}
	}
	
	public	int		Combo{
		get{return combo;}
	}
	
	public	void	SetResult(float	per){
		Debug.Log("score:"+per);
		
		float	per_floored = StaticMath.ToRoundDown(per, 1);
		
		if(per < Define_Rate.Good)
			combo = 0;
		
		int	scoreAdds = Mathf.FloorToInt( (per * 10) * (1 + (float)combo * 0.5f) );

		score += scoreAdds;
		
		if(score > 99999)
			score = 99999;
		
		if(combo > 0)
			UI_Combo.Instance.SetCombo(combo);
		
		if(per >= Define_Rate.Good)
			combo++;
		
		UI_ScoreAdds.Instance.SetScore(scoreAdds);
	}
	
	private	void	Start(){
		label = gameObject.GetComponent<UILabel>();
	}
	
	private	void	Update(){
		if(score > scoreCounter)
		{
			scoreCounter += Time.deltaTime * SC_COUNTSPEED;
			label.text = Mathf.FloorToInt(scoreCounter).ToString("00000");
		}else if(score < scoreCounter)
		{
			scoreCounter = score;
			label.text = score.ToString("00000");
		}
	}
}
