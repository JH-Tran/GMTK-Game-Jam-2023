using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMachine : Machine
{
    private float buttonTimer = 0;
    private bool isButtonTimerOn = false;
    private float buttonCoolDown = 30;
    [SerializeField] private Sprite ButtonOnSprite;
    [SerializeField] private Sprite ButtonOffSprite;

    [SerializeField] private GoalManager goalManager;
    [SerializeField] private ScreenSide screenSide;

    private void FixedUpdate()
    {
        if (isButtonTimerOn && buttonTimer > 0)
        {
            machineSprite.sprite = ButtonOffSprite;
            buttonTimer -= Time.deltaTime;
        }
        else if (isButtonTimerOn && buttonTimer <= 0)
        {
            machineSprite.sprite = ButtonOnSprite;
            isButtonTimerOn = false;
        }
    }

    public override void UseMachine(PlayerInfomation player)
    {
        if (isButtonTimerOn == false)
        {
            goalManager.AddTime(screenSide);
            buttonTimer = buttonCoolDown;
            isButtonTimerOn = true;
        }
    }

    public override void PlaceDropItem(PlayerInfomation player) { }

}
