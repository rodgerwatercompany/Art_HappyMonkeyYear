using UnityEngine;
using System.Collections;

public class FillAmount : MonoBehaviour {

    public bool fill;


	void Update () 
    {
	    if (fill)
        {
            if (this.GetComponent<UI2DSprite>().fillAmount < 1)
                this.GetComponent<UI2DSprite>().fillAmount += (Time.deltaTime*2);
            else
                fill = false;
        }
	}

    public void FillAni()
    {
        this.GetComponent<UI2DSprite>().fillAmount = 0;
        fill = true;
    }
}
