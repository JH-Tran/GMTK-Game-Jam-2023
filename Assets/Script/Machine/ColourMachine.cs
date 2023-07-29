using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourMachine : Machine
{
    int colorListCount = 0;

    public void Awake()
    {
        counterTop = GetComponent<CounterTop>();
    }

    public override void UseMachine(PlayerInfomation player)
    {
        if (counterTop.IsItemOnTable())
        {
            counterTop.ChangeItemColour(colorList[colorListCount]);
            counterTop.GetItemOnCounterPicture().SetColour(colorDictionary.GetValueOrDefault(colorList[colorListCount]));
            colorListCount++;
            if (colorListCount >= colorList.Length) colorListCount = 0;
        }
    }
}
