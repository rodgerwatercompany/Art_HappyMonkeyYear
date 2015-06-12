using UnityEngine;
using System.Collections;

public class SymbWheel_5x3 : MonoBehaviour
{
    public string symbWheelPath = "UI Root/Camera/SoftClip_Panel/Anchor/01_SymbolReel/SymbReel/symbol_Reel_0";
    public string symbolResouece = "/Game/UI/SlotGame/Symbol_14/";
    private ScriptsMgr scriptsMgr;

    public int Pict_X = 120;
    public int Pict_Y = 101;
    public int Pict_Total = 15;
    private int Pict_Max = 15;

    private const float m_TextureScale_Y = 3 / 15f; // m_TextureScale_Y = 1 / 15(圖示數)
    [HideInInspector]
    public float m_OffSetMax = 1 - m_TextureScale_Y;
    [HideInInspector]
    public float m_StopPos = 11f / 15f;
    public int symbWheel;

    [HideInInspector]
    public int iWheelinx = -1;
    private Texture[] m_SymbWheel;
    private Texture2D[] m_SymbWheel_Animation;
    private int[] m_iSymbWheelIdx = new int[15];
    [HideInInspector]
    public GameObject[] symbol_Reel;
    [HideInInspector]
    public float[] offSetCnts;
    [HideInInspector]
    public float[] WeightSpeed;
    [HideInInspector]
    public bool[] IsWheelStop;
    public float m_Speed = 0.02f;
    public float m_DecSpeed = 0.8f;
    public float m_minSpeed = 0.005f;
    public float m_WaitTick = 1f;
    public float m_StopTick;

    [HideInInspector]
    public float m_StopPosDown = 0.5f;
    public float m_StopPosDownSpeed = 0.003f;
    public float m_StopPosUpSpeed = 0.002f;

    [HideInInspector]
    public SymbolWheelRun[] symbolRun;
    [HideInInspector]
    public int roller;

    public AudioClip slotSpin;
    public AudioClip slotStop;
    public AudioClip win_Audio;
    public AudioClip freeGameWin_Audio;
    public AudioClip mega_Audio;
    public AudioClip grand_Audio;

    [HideInInspector]
    public int[] stopState = new int[5];

    public bool[] IsStopDown = new bool[5];

    void Start()
    {
        scriptsMgr = GameObject.Find("ScriptsMgr").GetComponent<ScriptsMgr>();

        m_SymbWheel = new Texture2D[Pict_Total];
        m_SymbWheel_Animation = new Texture2D[symbWheel];
        symbol_Reel = new GameObject[symbWheel];
        offSetCnts = new float[symbWheel];
        WeightSpeed = new float[symbWheel];
        IsWheelStop = new bool[symbWheel];
        symbolRun = new SymbolWheelRun[symbWheel];

        for (int i = 0; i < symbol_Reel.Length; i++)
        {
            symbol_Reel[i] = GameObject.Find(symbWheelPath + (i + 1));
            symbol_Reel[i].AddComponent<SymbolWheelRun>();
            symbolRun[i] = symbol_Reel[i].GetComponent<SymbolWheelRun>();
            symbolRun[i].roller = i;
        }

        LoadTexture(m_SymbWheel, symbolResouece, "symbol_");

        for (int i = 0; i < symbol_Reel.Length; i++)
        {
            symbolRun[i].wheelState = SymbolWheelRun.WheelState.Wheel_Normal_State;
        }
        m_StopPosDown = m_StopPos + (1f / (float)Pict_Max) / 2f;
        ChangeWheels();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spin()
    {
        for (int i = 0; i < symbol_Reel.Length; i++)
        {
            symbolRun[i].wheelState = SymbolWheelRun.WheelState.Wheel_Run_State;
            symbolRun[i].wheelStop = false;
            stopState[i] = 0;
            IsStopDown[i] = false;
            offSetCnts[i] = m_StopPos;
        }
        //--為不接Sever
        iWheelinx = 0;
        ChangeWheels();
        StartCoroutine("WheelStop");
        //--

        this.GetComponent<AudioSource>().clip = slotSpin;
        this.GetComponent<AudioSource>().Play();
        this.GetComponent<AudioSource>().loop = true;
    }
    //Stop按鈕
    public void StopBtn()
    {

        for (int i = 0; i < symbol_Reel.Length; i++)
        {
            symbolRun[i].wheelState = SymbolWheelRun.WheelState.Wheel_Stop_State;
        }

        for (int i = 0; i < symbol_Reel.Length; i++)
        {
            IsWheelStop[i] = true;
        }
    }

    void ChangeWheels()
    {
        RestartSeed();
        for (int i = 0; i < symbol_Reel.Length; i++)
        {
            offSetCnts[i] = 0;
            ReSetSymWheel(i);
            symbol_Reel[i].GetComponent<Renderer>().material.SetTexture("_MainTex", m_SymbWheel_Animation[i]);
            IsWheelStop[i] = false;
            WeightSpeed[i] = 1;
        }
    }

    # region WheelStop

    public void StopWheel(int inx)
    {
        symbolRun[inx].wheelState = SymbolWheelRun.WheelState.Wheel_Stop_State;
        IsWheelStop[inx] = true;
    }

    IEnumerator WheelStop()
    {
        yield return new WaitForSeconds(m_WaitTick);
        StopWheel(0);
    }

    public void AllWheelStop()
    {
        if (scriptsMgr.winOrLose)
        {
            switch (scriptsMgr.gameType)
            {
                case ScriptsMgr.GameType.Normal:
                    this.GetComponent<AudioSource>().clip = win_Audio;
                    break;
                case ScriptsMgr.GameType.FreeGame:
                    this.GetComponent<AudioSource>().clip = freeGameWin_Audio;
                    break;
                case ScriptsMgr.GameType.Wild:
                    break;
                case ScriptsMgr.GameType.Jackpot:
                    this.GetComponent<AudioSource>().clip = grand_Audio;
                    break;
                case ScriptsMgr.GameType.MegaWin:
                    this.GetComponent<AudioSource>().clip = mega_Audio;
                    break;
                case ScriptsMgr.GameType.SuperWin:
                    break;
                case ScriptsMgr.GameType.BigWin:
                    break;
                default:
                    break;
            }
            this.GetComponent<AudioSource>().Play();
        }
        scriptsMgr.WinOrLose();
    }

    #endregion

    void RestartSeed()
    {
        string[] openResult = scriptsMgr.openSymbol.Split(',');
        for (int i = 0; i < m_iSymbWheelIdx.Length; i++)
        {
            m_iSymbWheelIdx[i] = int.Parse(openResult[i + 1]) - 1;
        }
    }

    void ReSetSymWheel(int index)
    {

        if (m_SymbWheel_Animation[index] == null)
            m_SymbWheel_Animation[index] = new Texture2D(Pict_X, Pict_Y * Pict_Max, TextureFormat.RGBA32, false);
        {
            Texture2D FirstSymb = (m_SymbWheel[UnityEngine.Random.Range(0, Pict_Total - 1)] as Texture2D);
            for (int i = 0; i < Pict_Max; i++)
            {
                if (i == Pict_Max - 1 || i == 0)
                {
                    m_SymbWheel_Animation[index].SetPixels(0, Pict_Y * i, Pict_X, Pict_Y, FirstSymb.GetPixels(0, 0, Pict_X, Pict_Y));
                }
                else
                {
                    m_SymbWheel_Animation[index].SetPixels(0, Pict_Y * i, Pict_X, Pict_Y, (m_SymbWheel[UnityEngine.Random.Range(0, Pict_Total - 1)] as Texture2D).GetPixels(0, 0, Pict_X, Pict_Y));
                }
            }
            Texture2D pic;
            for (int i = 0, j = 0; i < 3; i++, j++)
            {
                pic = m_SymbWheel[m_iSymbWheelIdx[index * 3 + j]] as Texture2D;
                m_SymbWheel_Animation[index].SetPixels(0, Pict_Y * (Pict_Max - 2 - i), Pict_X, Pict_Y, pic.GetPixels(0, 0, Pict_X, Pict_Y));

            }
            m_SymbWheel_Animation[index].Apply();
        }
        offSetCnts[index] = 0;
        symbol_Reel[index].GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(1, m_TextureScale_Y));
        symbol_Reel[index].GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, 0));
    }


    private void LoadTexture(Texture[] AnimationTexture, string sPathName, string FileName)
    {
        #region ...
        for (int i = 0; i < AnimationTexture.Length; i++)
        {
            AnimationTexture[i] = LoadTexture(sPathName + FileName + (i + 1).ToString("000"));
        }
        #endregion
    }

    private Texture LoadTexture(string name)
    {
        Texture temp = null;
        temp = Resources.Load<Texture>(name);
        return temp;
    }
}
