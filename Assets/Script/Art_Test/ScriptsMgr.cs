using UnityEngine;
using System.Collections;

public class ScriptsMgr : MonoBehaviour
{
    public enum GameType { Normal, FreeGame, Wild, Jackpot, MegaWin, SuperWin, BigWin }
    public GameType gameType;

    public string openSymbol = "0,1,2,3,4,5,6,7,8,9,10,11,12,13,1,2"; //開獎結果

    private GameObject btnState;
    private GameObject spinBtn;
    private GameObject stopBtn;
    private GameObject scoreBtn;
    private GameObject autoBtn;
    private GameObject symbReel;
    private GameObject jackpot;
    private GameObject selectWheel;
    private GameObject dollarBtn;
    private GameObject maxBetBtn;
    private GameObject cashChangeBtn;
    private GameObject freeBtn;
    private UILabel freeTime_lab;
    private GameObject freeGameBG;
    private GameObject megaWin;
    private GameObject jackpotGrp;
    private GameObject lanternLight;

    [HideInInspector]
    public bool ifCloseWheel;

    public bool winOrLose = false;

    private float longTapDuration = 2.0f;
    private float lastTap;
    private bool tapFlg;
    private float autoWait;
    [HideInInspector]
    public bool isAuto = false;
    [HideInInspector]
    public bool isJackpot = false;
    [HideInInspector]
    public bool isFreeGame = false;
    public int freeTimes;
    [HideInInspector]
    public bool isWild = false;
    private BetLineRotate betLineRotate;
    private UpdateScore updateScore;

    public int BetLine;
    public int[] winScore;
    private int win_int;
    public int jackpotScore;

    private DemoControl demoConreol; //DemoContorl

    public AudioClip normal_audio;
    public AudioClip freeGame_Audio;

    void Awake()
    {
        //初始化畫面尺寸
        Screen.SetResolution(1440, 900, false);
        //初始化語系
        Localization.language = "简体中文";

        //DemoControl
        demoConreol = this.GetComponent<DemoControl>();

        symbReel = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/01_SymbolReel/SymbReel");
        spinBtn = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/MainUI_Panel/Button/SpinBtn");
        stopBtn = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/MainUI_Panel/Button/StopBtn");
        scoreBtn = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/MainUI_Panel/Button/ScoreBtn");
        autoBtn = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/MainUI_Panel/Button/AutoBtn");
        jackpot = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/Jackpot_Grp");
        selectWheel = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/SelectWheel/BetLine");
        selectWheel.GetComponent<TweenPosition>().from = selectWheel.transform.localPosition;
        selectWheel.GetComponent<TweenPosition>().to = new Vector3(selectWheel.transform.localPosition.x + 235f,selectWheel.transform.localPosition.y,selectWheel.transform.localPosition.z);
        betLineRotate = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/SelectWheel/BetLine/Bet_Selection").GetComponent<BetLineRotate>();
        dollarBtn = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/MainUI_Panel/Button/DollarBtn");
        maxBetBtn = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/MainUI_Panel/Button/MaxBetBtn");
        cashChangeBtn = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/MainUI_Panel/Button/CashChangeBtn");
        freeBtn = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/MainUI_Panel/Button/FreeBtn");
        freeTime_lab = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/MainUI_Panel/Button/FreeBtn/button/free_label").GetComponent<UILabel>();
        freeGameBG = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/0_BG/Free_Game_BG");
        freeGameBG.SetActive(false);
        megaWin = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/02_Stage/Particle_Grp/Mega_Win");
        updateScore = this.GetComponent<UpdateScore>();
        jackpotGrp = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/02_Stage/Particle_Grp/Jackpot_Grp");
        lanternLight = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/02_Stage/Lantern_light_Grp");
        btnState = spinBtn;

        stopBtn.SetActive(false);
        scoreBtn.SetActive(false);
        autoBtn.SetActive(false);
        freeBtn.SetActive(false);
    }

    void Update()
    {
        if (tapFlg && Time.realtimeSinceStartup - lastTap > longTapDuration)
        {
            isAuto = true;
            Auto();
            tapFlg = false;
        }
    }

    public void SpinBtnState(GameObject switchBtn)
    {
        btnState.SetActive(false);
        switchBtn.SetActive(true);
        btnState = switchBtn;
    }

    #region ButtonLock
    void SpinLock()
    {
        spinBtn.GetComponent<ButtonState>().btnState = ButtonState.BtnState.Disabled;
        spinBtn.GetComponent<ButtonState>().ChangeBtnState();
    }

    void SpinUnLock()
    {
        spinBtn.GetComponent<ButtonState>().btnState = ButtonState.BtnState.Normal;
        spinBtn.GetComponent<ButtonState>().ChangeBtnState();
    }

    void MaxLock()
    {
        maxBetBtn.GetComponent<ButtonState>().btnState = ButtonState.BtnState.Disabled;
        maxBetBtn.GetComponent<ButtonState>().ChangeBtnState();
    }

    void MaxUnLock()
    {
        maxBetBtn.GetComponent<ButtonState>().btnState = ButtonState.BtnState.Normal;
        maxBetBtn.GetComponent<ButtonState>().ChangeBtnState();
    }

    void AllButtonLock()
    {
        btnState.GetComponent<ButtonState>().btnState = ButtonState.BtnState.Disabled;
        btnState.GetComponent<ButtonState>().ChangeBtnState();
        dollarBtn.GetComponent<ButtonState>().btnState = ButtonState.BtnState.Disabled;
        dollarBtn.GetComponent<ButtonState>().ChangeBtnState();
    }

    void AllButtonUnLock()
    {
        btnState.GetComponent<ButtonState>().btnState = ButtonState.BtnState.Normal;
        btnState.GetComponent<ButtonState>().ChangeBtnState();
        dollarBtn.GetComponent<ButtonState>().btnState = ButtonState.BtnState.Normal;
        dollarBtn.GetComponent<ButtonState>().ChangeBtnState();
    }
    #endregion

    public void WinOrLose()
    {
        switch (gameType)
        {
            case GameType.Normal:
                if(isAuto)
                {
                    if (winOrLose)
                    {
                        //DemoControl
                        demoConreol.ShowLineFrame();
                        StartCoroutine("AutoWin");
                        lanternLight.GetComponent<Animation>().Play();
                        autoWait = 1.2f;
                    }
                    else
                    {
                        StartCoroutine("AutoSpin");
                        autoWait = 0.5f;
                    }

                }else if (winOrLose)
                {
                    SpinBtnState(scoreBtn);
                    UpdateWinScore();
                    //DemoControl
                    demoConreol.ShowLineFrame();
                    lanternLight.GetComponent<Animation>().Play();
                }
                else
                {
                    SpinBtnState(spinBtn);
                    selectWheel.GetComponent<TweenPosition>().PlayReverse();
                    MaxUnLock();
                }

                break;
            case GameType.FreeGame:
                if (!isFreeGame)
                {
                    isFreeGame = true;
                    SpinBtnState(freeBtn);
                    this.GetComponent<AudioSource>().clip = freeGame_Audio;
                    this.GetComponent<AudioSource>().Play();
                }
                FreeGame();
                if (winOrLose)
                {
                    //DemoControl
                    winOrLose = false;
                    demoConreol.ShowLineFrame();
                    lanternLight.GetComponent<Animation>().Play();
                }
                break;
            case GameType.Wild:
                break;
            case GameType.Jackpot:
                if (!isAuto)
                {
                    SpinBtnState(scoreBtn);
                }
                else
                {
                    SpinBtnState(autoBtn);
                }
                AllButtonLock();
                lanternLight.GetComponent<Animation>().Play();
                jackpotGrp.GetComponent<RunScore>().totalScore = jackpotScore;
                jackpotGrp.GetComponent<RunScore>().StartCount();
                demoConreol.ShowLineFrame();
                break;
            case GameType.MegaWin:
                lanternLight.GetComponent<Animation>().Play();
                demoConreol.ShowLineFrame();
                megaWin.GetComponent<Animation>().Play();
                if (!isAuto)
                {
                    SpinBtnState(scoreBtn);
                }
                else
                {
                    SpinBtnState(autoBtn);
                }
                AllButtonLock();
                UpdateWinScore();
                break;
            default:
                break;
        }
        if (isFreeGame && !winOrLose)
        {
            if (freeTimes == 0)
            {
                FreeGameEnd();
            }
        }
    }

    void StartSpin()
    {
        if (isFreeGame)
        {
            freeTimes--;
            freeTime_lab.text = freeTimes.ToString();
        }


        winOrLose = false;
        //DemoControl
        demoConreol.GameResult();
        //DemoControl
        MaxLock();
        symbReel.GetComponent<SymbWheel_5x3>().Spin();
        selectWheel.GetComponent<TweenPosition>().PlayForward();
        if (!isFreeGame)
        { 
            updateScore.SubtracCredit(updateScore.betScore);
        }
    }

    IEnumerator AutoSpin()
    {
        yield return new WaitForSeconds(autoWait);
        StartSpin();
    }
    IEnumerator AutoScore()
    {
        yield return new WaitForSeconds(1);
        UpdateNowCredit();
        //DemoControl
        demoConreol.HideLineFrame();
        StartCoroutine("AutoSpin");
        StopLanternLightAni();
    }
    IEnumerator AutoWin()
    {
        yield return new WaitForSeconds(1);
        UpdateWinScore();
        StartCoroutine("AutoScore");
    }

    #region Button
    public void Spin()
    {
        if (!isAuto)
        {
            StartSpin();
            SpinBtnState(stopBtn);
        }
    }

    public void Score()
    {
        StopLanternLightAni();

        SpinBtnState(spinBtn);
        UpdateNowCredit();
        selectWheel.GetComponent<TweenPosition>().PlayReverse();
        MaxUnLock();
        //DemoControl
        if (win_int < winScore.Length - 1)
        {
            win_int++;
        }
        else
            win_int = 0;
        demoConreol.HideLineFrame();
        //DemoControl
    }
    public void Auto()
    {
        SpinBtnState(autoBtn);
        StartSpin();
    }

    public void StopAuto()
    {
        if (isAuto)
        {
            isAuto = false;
            gameType = GameType.Normal;
        }
    }

    public void Stop()
    {

    }

    public void Maxbet()
    {
        betLineRotate.MaxBet_Bet();
        UpdateBetScore();
    }

    #endregion


    public void OnPress()
    {
        tapFlg = true;
        lastTap = Time.realtimeSinceStartup;
    }


    public void OnRelease()
    {
        tapFlg = false;
    }

    void FreeGame()
    {
        freeTime_lab.text = freeTimes.ToString();
        if (winOrLose)
            StartCoroutine("AutoWin");
        else
            StartCoroutine("AutoScore");
        freeGameBG.SetActive(true);
    }

    void FreeGameEnd()
    {
        isFreeGame = false;

        freeGameBG.SetActive(false);
        this.GetComponent<AudioSource>().clip = normal_audio;
        this.GetComponent<AudioSource>().Play();
    }

    public void ScoreRunEnd()
    {
        isJackpot = false;
        AllButtonUnLock();
        if (isAuto)
        {
            StartCoroutine("AutoScore");
            SpinBtnState(autoBtn);
            autoBtn.GetComponent<ButtonState>().btnState = ButtonState.BtnState.Normal;
            autoBtn.GetComponent<ButtonState>().ChangeBtnState();
        }
        if (isFreeGame)
        {
            if (freeTimes == 0)
            {
                FreeGameEnd();
            }
        }
    }

    void StopLanternLightAni()
    {
        lanternLight.GetComponent<Animation>().Stop();
        for (int i = 0; i < lanternLight.transform.childCount; i++)
        {
            lanternLight.transform.GetChild(i).GetComponent<UISprite>().color = new Vector4(1, 1, 1, 5f / 225f);
        }
    }

    public void UpdateBetScore()
    {
        updateScore.UpdateBetScore(BetLine);
    }

    public void UpdateNowCredit()
    {
        updateScore.UpdateNowCredit();
    }

    public void UpdateWinScore()
    {
        updateScore.UpdateWinScore(winScore[win_int]);
        if (isAuto)
        {
            if (win_int < winScore.Length - 1)
            {
                win_int++;
            }
            else
                win_int = 0;
        }
    }
}
