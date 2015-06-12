using UnityEngine;
using System.Collections;

public class SymbolWheelRun : MonoBehaviour
{

    private SymbWheel_5x3 symbolMar;
    private GameObject symbReel;

    public enum WheelState
    {
        Wheel_Normal_State,
        Wheel_Run_State,
        Wheel_Stop_State,
        Wheel_Win_State,
        Wheel_Auto_State,
    }
    public WheelState wheelState;

    public int roller;
    [HideInInspector]
    public float m_Speed;
    [HideInInspector]
    public float m_OffSetMax;
    [HideInInspector]
    public float m_DecSpeed;
    [HideInInspector]
    public bool wheelStop;
    void Awake()
    {
        symbolMar = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/01_SymbolReel/SymbReel").GetComponent<SymbWheel_5x3>();
        symbReel = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/01_SymbolReel/SymbReel");
        m_Speed = symbolMar.m_Speed;
        m_OffSetMax = symbolMar.m_OffSetMax;
        m_DecSpeed = symbolMar.m_DecSpeed;
    }

    void Update()
    {
        switch (wheelState)
        {
            case WheelState.Wheel_Run_State:
                RunWheel();
                break;
            case WheelState.Wheel_Stop_State:
                StopWheel();
                break;
            default:
                break;
        }
    }

    public void RunWheel()
    {
        float frame = 1 / Time.deltaTime;
        float tick = 60 / frame;

        this.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, symbolMar.offSetCnts[roller]));
        symbolMar.offSetCnts[roller] += m_Speed * tick;
        if (symbolMar.offSetCnts[roller] >= m_OffSetMax)
            symbolMar.offSetCnts[roller] -= m_OffSetMax;
    }

    void StopWheel()
    {
        float frame = 1 / Time.deltaTime;
        float tick = 60 / frame;
        float speed;// = m_Speed * tick * WeightSpeed[inx];


        if (symbolMar.WeightSpeed[roller] < 1 && symbolMar.IsStopDown[roller] == true) //offSetCnts[inx] == m_StopPos)
        {
            if (symbolMar.stopState[roller] == 1)
            {
                speed = symbolMar.m_StopPosDownSpeed * tick;
                symbolMar.symbol_Reel[roller].GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, symbolMar.offSetCnts[roller]));
                symbolMar.offSetCnts[roller] += speed;
                if (symbolMar.offSetCnts[roller] >= symbolMar.m_StopPosDown)
                {
                    symbolMar.stopState[roller] = 2;
                    symbolMar.offSetCnts[roller] = symbolMar.m_StopPosDown;
                    symbolMar.symbol_Reel[roller].GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, symbolMar.offSetCnts[roller]));
                }
            }
            else if (symbolMar.stopState[roller] == 2)
            {
                speed = symbolMar.m_StopPosUpSpeed * tick;
                symbolMar.offSetCnts[roller] -= speed;
                symbolMar.symbol_Reel[roller].GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, symbolMar.offSetCnts[roller]));
                if (symbolMar.offSetCnts[roller] <= symbolMar.m_StopPos)
                {
                    symbReel.GetComponent<AudioSource>().clip = symbolMar.slotStop;
                    symbReel.GetComponent<AudioSource>().loop = false;
                    symbReel.GetComponent<AudioSource>().Play();

                    symbolMar.stopState[roller] = 3;
                    symbolMar.iWheelinx++;
                    symbolMar.offSetCnts[roller] = symbolMar.m_StopPos;
                    symbolMar.symbol_Reel[roller].GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, symbolMar.offSetCnts[roller]));
                }
            }
            if (symbolMar.stopState[roller] == 3)
            {
                wheelState = WheelState.Wheel_Normal_State;

                StartCoroutine("AllWheelStop");
            }
            return;
        }

        speed = m_Speed * tick * symbolMar.WeightSpeed[roller];
        if (speed < symbolMar.m_minSpeed) speed = symbolMar.m_minSpeed;

        symbolMar.symbol_Reel[roller].GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, symbolMar.offSetCnts[roller]));
        symbolMar.offSetCnts[roller] += speed;


        if (symbolMar.WeightSpeed[roller] == 1 || symbolMar.IsWheelStop[roller] == false)
        {
            if (symbolMar.offSetCnts[roller] >= m_OffSetMax)
            {
                symbolMar.offSetCnts[roller] -= m_OffSetMax;
                if (symbolMar.IsWheelStop[roller] == true)
                    symbolMar.WeightSpeed[roller] *= m_DecSpeed;
            }
        }
        else
        {
            symbolMar.WeightSpeed[roller] *= m_DecSpeed;
            if (symbolMar.offSetCnts[roller] >= symbolMar.m_StopPos)
            {
                symbolMar.offSetCnts[roller] = symbolMar.m_StopPos;
                symbolMar.stopState[roller] = 1;
                symbolMar.symbol_Reel[roller].GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, symbolMar.offSetCnts[roller]));
                symbolMar.iWheelinx++;
                symbolMar.IsStopDown[roller] = true;
            }
            if (roller < symbolMar.symbWheel - 1 && !wheelStop)
            {
                wheelStop = true;
                symbolMar.StopWheel(roller + 1);
            }
        }
    }

    IEnumerator AllWheelStop()
    {
        yield return new WaitForSeconds(0.5f);
        if (symbolMar.symbWheel == roller + 1)
        {
            symbolMar.AllWheelStop();
        }
    }

}
