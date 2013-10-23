using UnityEngine;
using System.Collections;

public class GenerateCity : MonoBehaviour_Extends {
	enum TENTION
	{
		ePurple=0,
		eBlue,
		eSkyBlue,
		eGreen,
		eLimeGreen,
		eYellow,
		eOrange,
		eRed
	}
	
	public	GameObject[]	Layers;
	public	GameObject[]	Buildings;
	public	Material[]		TentionMaterial;
	private	Color[]			TentionColor;
	private const	float	CS_HALF			= 0.498039215803146f;
	private const	float	CS_MAX			= 1.0f;
	
	void Start(){
		TentionColor = 	new Color[]{	new Color(	CS_MAX,		0,			CS_MAX, 	CS_MAX),	// purple
										new Color(	0,			0,			CS_MAX, 	CS_MAX),	// blue
										new Color(	0,			CS_MAX,		CS_MAX, 	CS_MAX),	// skyblue
										new Color(	0,			CS_HALF,	0, 			CS_MAX),	// green
										new Color(	0,			CS_MAX,		0, 			CS_MAX),	// limegreen
										new Color(	CS_MAX,		CS_MAX,		0, 			CS_MAX),	// yellow
										new Color(	CS_MAX,		CS_HALF,	0, 			CS_MAX),	// orange
										new Color(	CS_MAX,		0,			0, 			CS_MAX),	// red
									};
		
		GenerateFromBitmap();
	}
	
	// Update is called once per frame
	void Update () {
	/*	if(Input.GetKeyDown(KeyCode.A))
		{
			GenerateFromBitmap();
		}*/
		
		if(Input.GetKeyDown(KeyCode.Alpha0))
		{
			Layers[0].SetActive(!Layers[0].activeInHierarchy);
		}
		
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			Layers[1].SetActive(!Layers[1].activeInHierarchy);
		}
		
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			Layers[2].SetActive(!Layers[2].activeInHierarchy);
		}
		
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			Layers[3].SetActive(!Layers[3].activeInHierarchy);
		}
		
		if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			Layers[4].SetActive(!Layers[4].activeInHierarchy);
		}
		
		if(Input.GetKeyDown(KeyCode.Alpha5))
		{
			Layers[5].SetActive(!Layers[5].activeInHierarchy);
		}
		
		if(Input.GetKeyDown(KeyCode.Alpha6))
		{
			Layers[6].SetActive(!Layers[6].activeInHierarchy);
		}
		
		if(Input.GetKeyDown(KeyCode.Alpha7))
		{
			Layers[7].SetActive(!Layers[7].activeInHierarchy);
		}
	}
	
	void GenerateFromBitmap(){
		Texture2D map = (Texture2D)Resources.Load("Textures/stage2");
		int w = map.width;
		int h = map.height;
		
		int x = 0,y = 0;
		
		float u = 1.0f/w;
		float v = 1.0f/h;
		float uHalf = u*(w*0.5f-0.5f);
		float vHalf = v*(h*0.5f-0.5f);
		
		for(int i=h-1; i>-1; i--)
		{
			y=i;
			for(int j=0; j<w; j++)
			{
				x=j;
				
				Color pixel = map.GetPixel(j,i);
				
				if(pixel == TentionColor[(int)TENTION.ePurple])
				{
					Generate(0, u*x - uHalf, v*y - vHalf);
				}else
				if(pixel == TentionColor[(int)TENTION.eBlue])
				{
					Generate(1, u*x - uHalf, v*y - vHalf);
				}else
				if(pixel == TentionColor[(int)TENTION.eSkyBlue])
				{
					Generate(2, u*x - uHalf, v*y - vHalf);
				}else
				if(pixel == TentionColor[(int)TENTION.eGreen])
				{
					Generate(3, u*x - uHalf, v*y - vHalf);
				}else
				if(pixel == TentionColor[(int)TENTION.eLimeGreen])
				{
					Generate(4, u*x - uHalf, v*y - vHalf);
				}else
				if(pixel == TentionColor[(int)TENTION.eYellow])
				{
					Generate(5, u*x - uHalf, v*y - vHalf);
				}else
				if(pixel == TentionColor[(int)TENTION.eOrange])
				{
					Generate(6, u*x - uHalf, v*y - vHalf);
				}else
				if(pixel == TentionColor[(int)TENTION.eRed])
				{
					Generate(7, u*x - uHalf, v*y - vHalf);
				}
			}
			
			//y++;
		}
	}
	
	void Generate(int tentionLevel, float u, float v){
		GameObject building = InstantiateGameObject(Buildings[tentionLevel]);
		building.transform.localScale	 = new Vector3((0.6f+0.075f*tentionLevel)/2, (2.25f+Random.Range(0.075f,0.1125f)*tentionLevel)/2, (0.6f+0.075f*tentionLevel)/2);
		int n = Random.Range(0,10);
		if(n>8 && tentionLevel > 2)
		{
			float heightPlus = 0;
			
			switch(tentionLevel)
			{
			case 3:
				heightPlus = Random.Range(1.0f, 2.5f);
				break;
			case 4:
				heightPlus = Random.Range(1.0f, 3.5f);
				break;
			case 5:
				heightPlus = Random.Range(2.0f, 4.0f);
				break;
			case 6:
				heightPlus = Random.Range(3.0f, 6.5f);
				break;
			case 7:
				heightPlus = Random.Range(4.0f, 9.0f);
				break;
			}
			
/*			if(tentionLevel == 3)
				heightPlus = Random.Range(1.0f, 2.5f);
			else if(tentionLevel < 5)
				heightPlus = Random.Range(2.0f,4.0f);
			else
				heightPlus = Random.Range(3.0f,8.0f);*/
			building.transform.localScale = new Vector3(building.transform.localScale.x, building.transform.localScale.y+heightPlus, building.transform.localScale.z);
		}else{
			building.transform.localScale = new Vector3(building.transform.localScale.x, building.transform.localScale.y+Random.Range(-1.5f,0.5f), building.transform.localScale.z);
		}
		building.transform.eulerAngles	 = new Vector3(0,Random.Range(0,360),0);
		building.transform.parent		 = Layers[tentionLevel].transform;
		building.transform.localPosition = new Vector3(u, building.transform.localScale.y/2.0f, v);
		building.transform.position		 = new Vector3(	building.transform.position.x + Random.Range(-0.1f,0.1f),
														building.transform.position.y,
														building.transform.position.z + Random.Range(-0.1f,0.1f));
		
		building.renderer.material = TentionMaterial[tentionLevel];
//		building.renderer.material.color = TentionColor[tentionLevel];
		//building.transform.localScale = new Vector3(1.0f/64.0f, transform.localScale.y, 1.0f/64.0f);
	}
	
	void OnGUI(){
		//GUI.color = Color.black;
		//GUI.Label(new Rect(10, 10, 1000, 100), "Press 'A' Key to Generate A City.");
		for(int i=0; i<8; i++)
		{
			GUI.color = TentionColor[i];
			GUI.Label(new Rect(10,10 + 20*i, 100, 100), "Layer"+i+":"+Layers[i].activeInHierarchy);
		}
	}
}
