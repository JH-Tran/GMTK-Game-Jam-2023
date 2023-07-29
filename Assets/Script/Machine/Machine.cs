using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    protected MachineType machineType;
    protected CounterTop counterTop;
    protected Dictionary<Color, PictureColor> colorDictionary = new Dictionary<Color, PictureColor>
    {
        { Color.white, PictureColor.White },
        { Color.red, PictureColor.Red },
        { Color.blue, PictureColor.Blue},
        { Color.green, PictureColor.Green }
    };
    protected Color[] colorList = { Color.white, Color.red, Color.blue, Color.green };
    protected SpriteRenderer machineSprite;

    public virtual void UseMachine(PlayerInfomation player) { }
    public virtual void PlaceDropItem(PlayerInfomation player)
    {
        if (counterTop == null) { return; }
        if (!player.IsHoldingItem() && counterTop.IsItemOnTable())
        {
            player.HoldItem(counterTop.TakeItemOnTable());
        }
        else if (player.IsHoldingItem() && !counterTop.IsItemOnTable())
        {
            player.PlaceItem(counterTop.GiveItem());
        }
    }
    protected virtual void SetMachineComponents()
    {
        counterTop = GetComponent<CounterTop>();
    }
}
public enum MachineType
{
    Canvas,
    Shape,
    Colour,
    Deliver,
    Button,
    Bin
}

public enum ScreenSide
{
    None,
    Left,
    Right
}
