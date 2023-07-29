using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalMachine : Machine
{
    [SerializeField] private GoalManager goalManager;
    [SerializeField] private ScreenSide screenSide;

    public void Awake()
    {
        SetMachineComponents();
    }

    public override void PlaceDropItem(PlayerInfomation player)
    {
        if (player.IsHoldingItem() && !counterTop.IsItemOnTable())
        {
            player.PlaceItem(counterTop.GiveItem());
            if (goalManager == null) { Debug.LogError($"{gameObject.name} is missing goal manager!"); return; }
            if (goalManager.VerifyRequirement(counterTop.gameObject.GetComponentsInChildren<Picture>(), screenSide))
            {
                counterTop.DeleteHeldItem();
            }
            else
            {
                Debug.Log("Loser!");
                counterTop.DeleteHeldItem();
            }
        }
    }
}
