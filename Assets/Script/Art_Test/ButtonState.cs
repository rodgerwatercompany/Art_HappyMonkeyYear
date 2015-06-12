using UnityEngine;
using System.Collections;

public class ButtonState : MonoBehaviour
{
    public enum BtnState { Normal, Pressed, Disabled }
    public BtnState btnState;

    public UIButton[] buttons;

    void Awake()
    {
        buttons = this.GetComponents<UIButton>();
    }

    void Update()
    {

    }

    public void ChangeBtnState()
    {
        switch (btnState)
        {
            case BtnState.Normal:
                foreach (UIButton button in buttons)
                {
                    button.state = UIButton.State.Normal;
                }
                this.GetComponent<BoxCollider>().enabled = true;
                break;
            case BtnState.Pressed:
                foreach (UIButton button in buttons)
                {
                    button.state = UIButton.State.Pressed;
                }
                break;
            case BtnState.Disabled:
                foreach (UIButton button in buttons)
                {
                    button.state = UIButton.State.Disabled;
                }
                this.GetComponent<BoxCollider>().enabled = false;
                break;
            default:
                break;
        }
    }
}
