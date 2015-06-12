using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

    public UIButton[] buttons;


    void Awake () 
    {
        buttons = this.GetComponents<UIButton>();
        foreach (UIButton button in buttons)
        {
            button.state = UIButtonColor.State.Disabled;
        }
	}
	
	void Update () 
    {
        foreach (UIButton button in buttons)
        {
            button.state = UIButtonColor.State.Disabled;
        }
    }
}
