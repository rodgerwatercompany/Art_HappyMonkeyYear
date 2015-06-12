using UnityEngine;
using System.Collections;

public class BetLineRotate : MonoBehaviour {
	private Transform target ;
	private float axisX;   
	private float axisY;  
	private float cXY;
	//是否拖曳旗標
	private int flag_Drag;
	//是否到達定位旗標
	public int flag_move;
	//底盤旋轉角度
	private int rotate_Angles = 0;
	private string[] NumberObj = new string[20];
	private Vector2 currentMousePos;
	private Vector3 lastWorldMousePos;
	private Vector3 currentWorldMousePos;
	private Vector2 deltaWorldMousePosY;
	private Vector2 MouseSpeed;
	private bool keepSliding = false;
	private bool slidingWay = false;
	private int slidingFrames;
	private float moveSpeed;
	private UILabel Number_Obj_1 ;
	private UILabel Number_Obj_2 ;
	private UILabel Number_Obj_3 ;
	private UILabel Number_Obj_4 ;
	private UILabel Number_Obj_5 ;
	private UILabel Number_Obj_6 ;
	private UILabel Number_Obj_7 ;
	private UILabel Number_Obj_8 ;
	private BetLineRotateNumber m_Rotate1 ;
	private BetLineRotateNumber m_Rotate2 ;
	private BetLineRotateNumber m_Rotate3 ;
	private BetLineRotateNumber m_Rotate4 ;
	private BetLineRotateNumber m_Rotate5 ;
	private BetLineRotateNumber m_Rotate6 ;
	private BetLineRotateNumber m_Rotate7 ;
	private BetLineRotateNumber m_Rotate8 ;
	private int Count_Number1 = 0;
	private int Count_Number2 = 0;
	private int Count_Number3 = 0;
	private int Count_Number4 = 0;
	private int Count_Number5 = 0;
	private int Count_Number6 = 0;
	private int Count_Number7 = 0;
	private int Count_Number8 = 0;
	private int Tmp_Number = 0;
	private UIFont m_font_R1 ;
	private float m_stopSpeed = 140;
	private Camera m_camera = null;
	private int returnBet = 0;
	private ScriptsMgr m_ScriptsMgr = null;
	void Awake(){
		m_ScriptsMgr = GameObject.Find ("ScriptsMgr").GetComponent<ScriptsMgr>();
        target = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/SelectWheel/BetLine/Bet_Selection/Bet_Wheel/bet_bg/select_bg").transform;
		for (int i = 0; i < NumberObj.Length; i++) {
						NumberObj [i] = ((i * 50)+50).ToString ("00");
            //Debug.Log("i :"+NumberObj [i]);		
		}
        Number_Obj_1 = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/SelectWheel/BetLine/Bet_Selection/Bet_Wheel/bet_bg/bet_label/bet_label_01").gameObject.GetComponent<UILabel>();
		Number_Obj_1.gameObject.AddComponent<BetLineRotateNumber>();
        Number_Obj_2 = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/SelectWheel/BetLine/Bet_Selection/Bet_Wheel/bet_bg/bet_label/bet_label_02").gameObject.GetComponent<UILabel>();
//		Number_Obj_2.gameObject.AddComponent<BetLineRotateNumber>();
        Number_Obj_3 = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/SelectWheel/BetLine/Bet_Selection/Bet_Wheel/bet_bg/bet_label/bet_label_03").gameObject.GetComponent<UILabel>();
//		Number_Obj_3.gameObject.AddComponent<BetLineRotateNumber>();
        Number_Obj_4 = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/SelectWheel/BetLine/Bet_Selection/Bet_Wheel/bet_bg/bet_label/bet_label_04").gameObject.GetComponent<UILabel>();
//		Number_Obj_4.gameObject.AddComponent<BetLineRotateNumber>();
        Number_Obj_5 = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/SelectWheel/BetLine/Bet_Selection/Bet_Wheel/bet_bg/bet_label/bet_label_05").gameObject.GetComponent<UILabel>();
//		Number_Obj_5.gameObject.AddComponent<BetLineRotateNumber>();
        Number_Obj_6 = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/SelectWheel/BetLine/Bet_Selection/Bet_Wheel/bet_bg/bet_label/bet_label_06").gameObject.GetComponent<UILabel>();
//		Number_Obj_6.gameObject.AddComponent<BetLineRotateNumber>();
        Number_Obj_7 = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/SelectWheel/BetLine/Bet_Selection/Bet_Wheel/bet_bg/bet_label/bet_label_07").gameObject.GetComponent<UILabel>();
//		Number_Obj_7.gameObject.AddComponent<BetLineRotateNumber>();
        Number_Obj_8 = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/SelectWheel/BetLine/Bet_Selection/Bet_Wheel/bet_bg/bet_label/bet_label_08").gameObject.GetComponent<UILabel>();
//		Number_Obj_8.gameObject.AddComponent<BetLineRotateNumber>();

		m_Rotate1 = Number_Obj_1.gameObject.GetComponent<BetLineRotateNumber>();
		m_Rotate2 = Number_Obj_2.gameObject.GetComponent<BetLineRotateNumber>();
		m_Rotate3 = Number_Obj_3.gameObject.GetComponent<BetLineRotateNumber>();
		m_Rotate4 = Number_Obj_4.gameObject.GetComponent<BetLineRotateNumber>();
		m_Rotate5 = Number_Obj_5.gameObject.GetComponent<BetLineRotateNumber>();
		m_Rotate6 = Number_Obj_6.gameObject.GetComponent<BetLineRotateNumber>();
		m_Rotate7 = Number_Obj_7.gameObject.GetComponent<BetLineRotateNumber>();
		m_Rotate8 = Number_Obj_8.gameObject.GetComponent<BetLineRotateNumber>();

		m_camera  = GameObject.Find ("UI Root/Camera").GetComponent<Camera>();
		//取顏色
		m_font_R1 = Number_Obj_2.bitmapFont;


	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0)) {
			axisY = Input.GetAxis ("Mouse Y");
			currentMousePos = Input.mousePosition;

			lastWorldMousePos = m_camera.ScreenToWorldPoint( currentMousePos );


		}else if(Input.GetMouseButton(0)){
			axisY = Input.GetAxis ("Mouse Y");
			currentMousePos = Input.mousePosition;
			currentWorldMousePos = m_camera.ScreenToWorldPoint( currentMousePos );
			deltaWorldMousePosY = new Vector2( 0.0f, currentWorldMousePos.y - lastWorldMousePos.y );

		    float dist = deltaWorldMousePosY.magnitude;
			moveSpeed = dist / Time.deltaTime * 1.5f;

			lastWorldMousePos = currentWorldMousePos;
			if (UICamera.hoveredObject != null && UICamera.hoveredObject.name != ("bet_bg_touch"))
				return;

			flag_move = 0;
			//已轉輪盤，無法關
			m_ScriptsMgr.ifCloseWheel = true;
			keepSliding = true;
			if(moveSpeed < 10)
				moveSpeed = 3;
			if(axisY > 0){
				//往上滾
//**				target.transform.Rotate(0,0,-moveSpeed * 0.99f);

				m_Rotate1.Rotate_Up(moveSpeed * 50);//速率必須一樣，滑動與慣性滑動
				slidingWay = true;
			}else if(axisY < 0){
				//往下滾
//**				target.transform.Rotate(0,0,moveSpeed * 0.99f);
				m_Rotate1.Rotate_Down(moveSpeed * 50);//速率必須一樣，滑動與慣性滑動
				slidingWay = false;
			}

		}else if(keepSliding){
			--moveSpeed;
//			Debug.Log("RRR :"+moveSpeed);
			if(moveSpeed < 0){
				moveSpeed = 0;
				keepSliding = false;

				//底盤角度
				if(target.transform.eulerAngles.z < 45f && target.transform.eulerAngles.z > -45f)
					target.transform.eulerAngles = new Vector3(0,0,0);
				if(target.transform.eulerAngles.z < 135f && target.transform.eulerAngles.z > 45f)
					target.transform.eulerAngles = new Vector3(0,0,90);
				if(target.transform.eulerAngles.z < 225f && target.transform.eulerAngles.z > 135f)
					target.transform.eulerAngles = new Vector3(0,0,-180);
				if(target.transform.eulerAngles.z < 270f && target.transform.eulerAngles.z > 225f)
					target.transform.eulerAngles = new Vector3(0,0,-90);
				if(target.transform.eulerAngles.z < 360f && target.transform.eulerAngles.z > 270f)
					target.transform.eulerAngles = new Vector3(0,0,0);
//				Debug.Log ("theta :"+m_Rotate1.theta);
				m_stopSpeed = 140;
				flag_move = 1;
				//可以關輪盤
				m_ScriptsMgr.ifCloseWheel = false;
				return;
			}

			if(slidingWay){
//**				target.transform.Rotate(0,0,-1f-(moveSpeed*1.1f));
				m_Rotate1.Rotate_Up((moveSpeed * 50));//速率必須一樣，滑動與慣性滑動
			}else{
//**				target.transform.Rotate(0,0,1f+(moveSpeed*1.1f));
				m_Rotate1.Rotate_Down((moveSpeed * 50));//速率必須一樣，滑動與慣性滑動
			}
		}
		if(flag_move == 1){
			if(Number_Obj_1.bitmapFont == m_font_R1){
				SetAllGridNumber(1);
				flag_move = 0;
				m_Rotate1.theta = 180f;
				m_Rotate1.Rotate_Up(1f);
				returnBet = int.Parse(Number_Obj_1.text);
			}
			if(Number_Obj_2.bitmapFont == m_font_R1){
				SetAllGridNumber(2);
				flag_move = 0;
				m_Rotate1.theta = 135f;
				m_Rotate1.Rotate_Up(1f);
				returnBet = int.Parse(Number_Obj_2.text);
			}
			if(Number_Obj_3.bitmapFont == m_font_R1){
				SetAllGridNumber(3);
				flag_move = 0;
				m_Rotate1.theta = 90f;
				m_Rotate1.Rotate_Up(1f);
				returnBet = int.Parse(Number_Obj_3.text);
			}
			if(Number_Obj_4.bitmapFont == m_font_R1){
				SetAllGridNumber(4);
				flag_move = 0;
				m_Rotate1.theta = 45f;
				m_Rotate1.Rotate_Up(1f);
				returnBet = int.Parse(Number_Obj_4.text);
			}
			if(Number_Obj_5.bitmapFont == m_font_R1){
				SetAllGridNumber(5);
				flag_move = 0;
				m_Rotate1.theta = 0f;
				m_Rotate1.Rotate_Up(1f);
				returnBet = int.Parse(Number_Obj_5.text);
			}
			if(Number_Obj_6.bitmapFont == m_font_R1){
				SetAllGridNumber(6);
				flag_move = 0;
				m_Rotate1.theta = 315f;
				m_Rotate1.Rotate_Up(1f);
				returnBet = int.Parse(Number_Obj_6.text);
			}
			if(Number_Obj_7.bitmapFont == m_font_R1){
				SetAllGridNumber(7);
				flag_move = 0;
				m_Rotate1.theta = 270f;
				m_Rotate1.Rotate_Up(1f);
				returnBet = int.Parse(Number_Obj_7.text);
			}
			if(Number_Obj_8.bitmapFont == m_font_R1){
				SetAllGridNumber(8);
				flag_move = 0;
				m_Rotate1.theta = 225f;
				m_Rotate1.Rotate_Up(1f);
				returnBet = int.Parse(Number_Obj_8.text);
			}
			m_ScriptsMgr.BetLine = returnBet;
			//Kenny Fix
            m_ScriptsMgr.UpdateBetScore();

		}
	}
	private void SetAllGridNumber(int int_tmp){

		switch(int_tmp){
		case 1:
			Change_Number_Down("bet_label_08");
			Change_Number_Down("bet_label_07");
			Change_Number("bet_label_02");
			Change_Number("bet_label_03");
			break;
		case 2:
			Change_Number_Down("bet_label_01");
			Change_Number_Down("bet_label_08");
			Change_Number("bet_label_03");
			Change_Number("bet_label_04");
			break;
		case 3:
			Change_Number_Down("bet_label_02");
			Change_Number_Down("bet_label_01");
			Change_Number("bet_label_04");
			Change_Number("bet_label_05");
			break;
		case 4:
			Change_Number_Down("bet_label_03");
			Change_Number_Down("bet_label_02");
			Change_Number("bet_label_05");
			Change_Number("bet_label_06");
			break;
		case 5:
			Change_Number_Down("bet_label_04");
			Change_Number_Down("bet_label_03");
			Change_Number("bet_label_06");
			Change_Number("bet_label_07");
			break;
		case 6:
			Change_Number_Down("bet_label_05");
			Change_Number_Down("bet_label_04");
			Change_Number("bet_label_07");
			Change_Number("bet_label_08");
			break;
		case 7:
			Change_Number_Down("bet_label_06");
			Change_Number_Down("bet_label_05");
			Change_Number("bet_label_08");
			Change_Number("bet_label_01");
			break;
		case 8:
			Change_Number_Down("bet_label_07");
			Change_Number_Down("bet_label_06");
			Change_Number("bet_label_01");
			Change_Number("bet_label_02");
			break;
		}
	}

	//換數字
	public void Change_Number(string str_name){
	
		switch(str_name){
		case "bet_label_01":
			Count_Number1 = int.Parse(Number_Obj_8.text)/50 + 1;
			if(Count_Number1 > NumberObj.Length)
				Count_Number1 = 1;
			//取陣列鍵值-1
			Number_Obj_1.text = NumberObj[Count_Number1 - 1].ToString();
			break;
		case "bet_label_02":
			Count_Number2 = int.Parse(Number_Obj_1.text)/50 + 1;
			if(Count_Number2 > NumberObj.Length)
				Count_Number2 = 1;
			//取陣列鍵值-1
			Number_Obj_2.text = NumberObj[Count_Number2 - 1].ToString();
			break;
		case "bet_label_03":
			Count_Number3 = int.Parse(Number_Obj_2.text)/50 + 1;
			if(Count_Number3 > NumberObj.Length)
				Count_Number3 = 1;
			//取陣列鍵值-1
			Number_Obj_3.text = NumberObj[Count_Number3 - 1].ToString();
			break;
		case "bet_label_04":
			Count_Number4 = int.Parse(Number_Obj_3.text)/50 + 1;
			if(Count_Number4 > NumberObj.Length)
				Count_Number4 = 1;
			//取陣列鍵值-1
			Number_Obj_4.text = NumberObj[Count_Number4 - 1].ToString();
			break;
		case "bet_label_05":
			Count_Number5 = int.Parse(Number_Obj_4.text)/50 + 1;
			if(Count_Number5 > NumberObj.Length)
				Count_Number5 = 1;
			//取陣列鍵值-1
			Number_Obj_5.text = NumberObj[Count_Number5 - 1].ToString();
			break;
		case "bet_label_06":
			Count_Number6 = int.Parse(Number_Obj_5.text)/50 + 1;
			if(Count_Number6 > NumberObj.Length)
				Count_Number6 = 1;
			//取陣列鍵值-1
			Number_Obj_6.text = NumberObj[Count_Number6 - 1].ToString();
			break;
		case "bet_label_07":
			Count_Number7 = int.Parse(Number_Obj_6.text)/50 + 1;
			if(Count_Number7 > NumberObj.Length)
				Count_Number7 = 1;
			//取陣列鍵值-1
			Number_Obj_7.text = NumberObj[Count_Number7 - 1].ToString();
			break;
		case "bet_label_08":
			Count_Number8 = int.Parse(Number_Obj_7.text)/50 + 1;
			if(Count_Number8 > NumberObj.Length)
				Count_Number8 = 1;
			//取陣列鍵值-1
			Number_Obj_8.text = NumberObj[Count_Number8 - 1].ToString();
			break;
		}
	}
	public void Change_Number_Down(string str_name){

		switch(str_name){
		case "bet_label_01":
			Count_Number1 = int.Parse(Number_Obj_2.text)/50 - 1;
			if(Count_Number1 == 0)
				Count_Number1 = NumberObj.Length;
			//取陣列鍵值-1
			Number_Obj_1.text = NumberObj[Count_Number1 - 1].ToString();
			break;
		case "bet_label_02":
			Count_Number2 = int.Parse(Number_Obj_3.text)/50 - 1;
			if(Count_Number2 == 0)
				Count_Number2 = NumberObj.Length;
			//取陣列鍵值-1
			Number_Obj_2.text = NumberObj[Count_Number2 - 1].ToString();
			break;
		case "bet_label_03":
			Count_Number3 = int.Parse(Number_Obj_4.text)/50 - 1;
			if(Count_Number3 == 0)
				Count_Number3 = NumberObj.Length;
			//取陣列鍵值-1
			Number_Obj_3.text = NumberObj[Count_Number3 - 1].ToString();
			break;
		case "bet_label_04":
			Count_Number4 = int.Parse(Number_Obj_5.text)/50 - 1;
			if(Count_Number4 == 0)
				Count_Number4 = NumberObj.Length;
			//取陣列鍵值-1
			Number_Obj_4.text = NumberObj[Count_Number4 - 1].ToString();
			break;
		case "bet_label_05":
			Count_Number5 = int.Parse(Number_Obj_6.text)/50 - 1;
			if(Count_Number5 == 0)
				Count_Number5 = NumberObj.Length;
			//取陣列鍵值-1
			Number_Obj_5.text = NumberObj[Count_Number5 - 1].ToString();
			break;
		case "bet_label_06":
			Count_Number6 = int.Parse(Number_Obj_7.text)/50 - 1;
			if(Count_Number6 == 0)
				Count_Number6 = NumberObj.Length;
			//取陣列鍵值-1
			Number_Obj_6.text = NumberObj[Count_Number6 - 1].ToString();
			break;
		case "bet_label_07":
			Count_Number7 = int.Parse(Number_Obj_8.text)/50 - 1;
			if(Count_Number7 == 0)
				Count_Number7 = NumberObj.Length;
			//取陣列鍵值-1
			Number_Obj_7.text = NumberObj[Count_Number7 - 1].ToString();
			break;
		case "bet_label_08":
			Count_Number8 = int.Parse(Number_Obj_1.text)/50 - 1;
			if(Count_Number8 == 0)
				Count_Number8 = NumberObj.Length;
			//取陣列鍵值-1
			Number_Obj_8.text = NumberObj[Count_Number8 - 1].ToString();
			break;
		}
	}
	public void MaxBet_Bet(){
        /*	if (Number_Obj_1.bitmapFont == m_font_R1) 
                initNumber (NumberObj.Length,01,02,03,04,NumberObj.Length - 3,NumberObj.Length - 2,NumberObj.Length - 1);
            if (Number_Obj_2.bitmapFont == m_font_R1) 
                initNumber (NumberObj.Length - 1,NumberObj.Length,01,02,03,04,NumberObj.Length - 3,NumberObj.Length - 2);
            if (Number_Obj_3.bitmapFont == m_font_R1) 
                initNumber (NumberObj.Length - 2,NumberObj.Length - 1,NumberObj.Length,01,02,03,04,NumberObj.Length - 3);
            if (Number_Obj_4.bitmapFont == m_font_R1) 
                initNumber (NumberObj.Length - 3,NumberObj.Length - 2,NumberObj.Length - 1,NumberObj.Length,01,02,03,04);
            if (Number_Obj_5.bitmapFont == m_font_R1) 
                initNumber (04,NumberObj.Length - 3,NumberObj.Length - 2,NumberObj.Length - 1,NumberObj.Length,01,02,03);
            if (Number_Obj_6.bitmapFont == m_font_R1) 
                initNumber (03,04,NumberObj.Length - 3,NumberObj.Length - 2,NumberObj.Length - 1,NumberObj.Length,01,02);
            if (Number_Obj_7.bitmapFont == m_font_R1) 
                initNumber (02,03,04,NumberObj.Length - 3,NumberObj.Length - 2,NumberObj.Length - 1,NumberObj.Length,01);
            if (Number_Obj_8.bitmapFont == m_font_R1) 
                initNumber (01,02,03,04,NumberObj.Length - 3,NumberObj.Length - 2,NumberObj.Length - 1,NumberObj.Length);
            */
        m_ScriptsMgr.BetLine = int.Parse(NumberObj[NumberObj.Length-1]);
	}
	void initNumber(int int_1,int int_2,int int_3,int int_4,int int_5,int int_6,int int_7,int int_8){
/*		Number_Obj_1.text = int_1.ToString("00");
		Number_Obj_2.text = int_2.ToString("00");
		Number_Obj_3.text = int_3.ToString("00");
		Number_Obj_4.text = int_4.ToString("00");
		Number_Obj_5.text = int_5.ToString("00");
		Number_Obj_6.text = int_6.ToString("00");
		Number_Obj_7.text = int_7.ToString("00");
		Number_Obj_8.text = int_8.ToString("00");*/
	}
}
