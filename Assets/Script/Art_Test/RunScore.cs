using UnityEngine;
using System.Collections;

public class RunScore : MonoBehaviour {

    public EffectManager score;
    public EffectManager text;
    public int totalScore;
    private int scorePlus;
    private int score_i;

    private GameObject tweenObject;
    private GameObject scriptMar;
    private UpdateScore updateScore;
    private bool run;

    void Awake()
    {
        tweenObject = this.transform.GetChild(0).gameObject;
        scriptMar = GameObject.Find("ScriptsMgr");
        updateScore = scriptMar.GetComponent<UpdateScore>();
        tweenObject.SetActive(false);
        run = false;
    }

    void Update()
    {
        if (run)
        {
            if (score_i < totalScore)
            {
                score.Text = "" + (score_i += scorePlus).ToString("N0");
            }
            else
            {
                score.Text = totalScore.ToString("N0");
                StartCoroutine("UpdateWin");
                run = false;
                scriptMar.GetComponent<UpdateScore>().PlusWinScore(totalScore);
                this.GetComponent<AudioSource>().Pause();
            }
        }
    }

    public void StartCount()
    {
        text.PlayAnimation();
        score.Text = "11";
        scorePlus = (int)(totalScore * 0.01f);
        score_i = 0;
        run = true;
        tweenObject.SetActive(true);
        this.GetComponent<AudioSource>().Play();
    }

    IEnumerator  UpdateWin()
    {
        yield return new WaitForSeconds(3);
        tweenObject.SetActive(false);
        scriptMar.GetComponent<ScriptsMgr>().ScoreRunEnd();
    }
}
