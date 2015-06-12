using UnityEngine;
using System.Collections;

public class UpdateScore : MonoBehaviour
{
    private ScriptsMgr scriptMgr;

    public int betScore = 0;
    private int currentBet;
    public int nowCredit = 8888888;
    private int currentCredit;
    public int winScore;
    private int currentWin;
    public string board = "12345-67890-12345-37880";

    [HideInInspector]public bool runScore;

    private int fromScore;
    public int toScore;
    private int scorePlus;
    private string updateText;
    private UILabel Text_lab;

    private UILabel betScore_lab;
    private UILabel nowCredit_lab;
    private UILabel winScore_lab;
    private UILabel board_lab;

    void Awake()
    {
        scriptMgr = this.GetComponent<ScriptsMgr>();
        betScore_lab = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/MainUI_Panel/Info/BetScore/betscore_label").GetComponent<UILabel>();
        nowCredit_lab = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/MainUI_Panel/Info/NowCredit/nowcredit_label").GetComponent<UILabel>();
        winScore_lab = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/MainUI_Panel/Info/WinScore/winscore_label").GetComponent<UILabel>();
        board_lab = GameObject.Find("UI Root/Camera/SoftClip_Panel/Anchor/03_UIButton/MainUI_Panel/Info/Board/board_label_lock/board_label").GetComponent<UILabel>();

        betScore_lab.text = betScore.ToString("N0");
        nowCredit_lab.text = nowCredit.ToString("N0");
        winScore_lab.text = winScore.ToString("N0");
        board_lab.text = board;

        RefreshScore();
    }

    void RefreshScore()
    {
        currentBet = betScore;
        currentCredit = nowCredit;
        currentWin = winScore;
    }

    void Update()
    {
        if (runScore)
        {
            if (fromScore < toScore)
            {
                updateText = "" + (fromScore += scorePlus).ToString("N0");
            }
            else
            {
                updateText = toScore.ToString("N0");
                RefreshScore();
                runScore = false;
            }
            Text_lab.text = updateText;
        }
    }

    #region ScoreUpdate
    public void UpdateBetScore(int bet)
    {
        betScore_lab.text = bet.ToString("N0");
        betScore = bet;
        RefreshScore();
    }

    public void SubtracCredit(int bet)
    {
        if (runScore)
        {
            toScore -= bet;
            nowCredit = toScore;
        }
        else
        {
            nowCredit -= bet;
            nowCredit_lab.text = nowCredit.ToString("N0");
        }
        RefreshScore();
    }

    public void UpdateNowCredit()
    {
        fromScore = currentCredit;
        int win = (int)decimal.Parse(winScore_lab.text);
        nowCredit += win;
        toScore = nowCredit;
        scorePlus = (int)(win * 0.01f) + 2;
        runScore = true;
        Text_lab = nowCredit_lab;
        winScore = 0;
        UpdateWinScore(winScore);
        RefreshScore();
    }

    public void UpdateWinScore(int win)
    {
        winScore = win;
        winScore_lab.text = win.ToString("N0");
        RefreshScore();
    }

    public void PlusWinScore(int win)
    {
        winScore_lab.text = ((int)decimal.Parse(winScore_lab.text) + win).ToString("N0");
        RefreshScore();
    }

    #endregion
}

