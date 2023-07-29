using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteMachine : Machine
{
    public void Awake()
    {
        SetMachineComponents();
    }

    public override void PlaceDropItem(PlayerInfomation player)
    {
        if (player.IsHoldingItem() && !counterTop.IsItemOnTable())
        {
            player.PlaceItem(counterTop.GiveItem());
            counterTop.DeleteHeldItem();
        }
    }
}
