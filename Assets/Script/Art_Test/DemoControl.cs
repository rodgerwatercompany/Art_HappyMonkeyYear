using UnityEngine;
using System.Collections;

public class DemoControl : MonoBehaviour
{

    public string[] openSymbol; //開獎結果
    public int spinResult = 0;
    public string[] openLine;   //中獎線
    private GameObject Frame_Grp;
    public string[] openFrame;  //中獎框
    private int bingoResult = 0;

    private ScriptsMgr scriptsMgr;

    private GameObject[] line;  //Bingo Line
    private GameObject[] frame; //Bingo Frame

    private string title;   //freegame & wild title

    void Awake()
    {
        scriptsMgr = GameObject.Find("ScriptsMgr").GetComponent<ScriptsMgr>();
        GameObject Line_Panel = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/02_Stage/Line_Panel");
        Frame_Grp = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/02_Stage/Frame_Grp");
        line = new GameObject[Line_Panel.transform.childCount];
        for (int i = 0; i < line.Length; i++)
        {
            line[i] = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/02_Stage/Line_Panel/line_" + (i + 1).ToString("00"));
            line[i].AddComponent<FillAmount>();
        }
        frame = new GameObject[Frame_Grp.transform.childCount];
        for (int i = 0; i < frame.Length; i++)
        {
            frame[i] = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/02_Stage/Frame_Grp/frame_" + (i + 1).ToString("00"));
        }
        HideLineFrame();
    }

    public void GameResult()
    {
        scriptsMgr.openSymbol = openSymbol[spinResult];
        string[] openResult = openSymbol[spinResult].Split(',');

        switch (int.Parse(openResult[0]))
        {
            case 0: //lose
                scriptsMgr.winOrLose = false;
                scriptsMgr.gameType = ScriptsMgr.GameType.Normal;
                break;
            case 1: //win
                scriptsMgr.winOrLose = true;
                scriptsMgr.gameType = ScriptsMgr.GameType.Normal;
                break;
            case 2: //free game
                scriptsMgr.winOrLose = true;
                scriptsMgr.freeTimes = 3;
                scriptsMgr.gameType = ScriptsMgr.GameType.FreeGame;
                title = "free_game.PNG";
                break;
            case 3: //wild
                scriptsMgr.winOrLose = true;
                scriptsMgr.isWild = true;
                scriptsMgr.gameType = ScriptsMgr.GameType.Wild;
                title = "wild.PNG";
                break;
            case 4: //jackpot
                scriptsMgr.winOrLose = true;
                scriptsMgr.isJackpot = true;
                scriptsMgr.gameType = ScriptsMgr.GameType.Jackpot;
                break;
            case 5: //megawin
                scriptsMgr.winOrLose = true;
                scriptsMgr.gameType = ScriptsMgr.GameType.MegaWin;
                break;
            default:
                break;
        }

        if (spinResult < openSymbol.Length - 1)
            spinResult++;
        else
            spinResult = 0;
    }

    void Update()
    {

    }

    public void ShowLineFrame()
    {
        if (scriptsMgr.gameType == ScriptsMgr.GameType.Normal)
        {
            Frame_Grp.GetComponent<AudioSource>().Play();
        }

        string[] showLine = openLine[bingoResult].Split(',');
        for (int i = 0; i < showLine.Length; i++)
        {
            line[(int.Parse(showLine[i])) - 1].SetActive(true);
            line[(int.Parse(showLine[i])) - 1].GetComponent<FillAmount>().FillAni();
        }
        string[] showFrame = openFrame[bingoResult].Split(',');
        for (int i = 0; i < showFrame.Length; i++)
        {
            frame[(int.Parse(showFrame[i])) - 1].SetActive(true);
            frame[(int.Parse(showFrame[i])) - 1].GetComponent<TweenScale>().PlayForward();
            frame[(int.Parse(showFrame[i])) - 1].transform.GetChild(0).GetComponent<UISprite>().spriteName = title;
            frame[(int.Parse(showFrame[i])) - 1].GetComponent<UIPlayTween>().Play(true);
        }
        if (bingoResult < openLine.Length - 1)
            bingoResult++;
        else
            bingoResult = 0;
    }

    public void HideLineFrame()
    {
        title = "null.PNG";

        for (int i = 0; i < line.Length; i++)
        {
            line[i].SetActive(false);
        }

        for (int i = 0; i < frame.Length; i++)
        {
            frame[i].GetComponent<TweenScale>().ResetToBeginning();
            frame[i].transform.GetChild(0).GetComponent<UISprite>().spriteName = title;
            frame[i].SetActive(false);
        }
    }
}
