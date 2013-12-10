using UnityEngine;
using System.Collections;

public class UI_WaveCount : SingletonMonoBehaviour<UI_WaveCount> {
	
	enum STATE 
	{
		eLbWaveFadeIn_Init = 0,
		eLbWaveFadeIn_Wait,
		
		eLbNumFadeIn_Init,
		eLbNumFadeIn_Wait,
		
		eSpReadyFadeIn_Init,
		eSpReadyFadeIn_Wait,
		
		eSpReadyFadeOut,
		
		eEnumEnd
	}
	
	public	UI_Label_Fade	lbFd_wave;
	public	UI_Label_Fade	lbFd_waveNum;
	public	UI_Sprite_Fade	spFd_ready;
	private	int				count = 0;
	
	private	STATE	state = STATE.eEnumEnd;
	
	void	Update(){
		switch(state)
		{
		case STATE.eLbWaveFadeIn_Init:
			count++;
			lbFd_wave.FadeIn(0.5f, 0.0f, "OnComplete", gameObject);
			state++;
			break;
		case STATE.eLbWaveFadeIn_Wait:
			break;
			
		case STATE.eLbNumFadeIn_Init:
			lbFd_waveNum.GetComponent<UILabel>().text = count.ToString();
			lbFd_waveNum.FadeIn(0.5f, 0.0f, "OnComplete", gameObject);
			state++;
			break;
		case STATE.eLbNumFadeIn_Wait:
			break;
			
		case STATE.eSpReadyFadeIn_Init:
			lbFd_wave.FadeOut(0.4f, 0.0f);
			lbFd_waveNum.FadeOut(0.4f, 0.0f);
			spFd_ready.FadeIn(0.5f, 0.3f , "OnComplete", gameObject);
			state++;
			break;
		case STATE.eSpReadyFadeIn_Wait:
			break;
			
		case STATE.eSpReadyFadeOut:
			spFd_ready.FadeOut(0.5f, 0.5f);
			state++;
			break;
			
		case STATE.eEnumEnd:
			break;
		}
	}
	
	void OnComplete(){
		state++;
	}
	
	public	void Play(){
		if(state == STATE.eEnumEnd)
			state = STATE.eLbWaveFadeIn_Init;
	}
}
