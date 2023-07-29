using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMachine: Machine
{
    [SerializeField] private GameObject pictureCanvas;

    public override void UseMachine(PlayerInfomation player)
    {
        Debug.Log("Use Canvas Machine");
        if (!player.IsHoldingItem())
        {
            player.HoldItem(Instantiate(pictureCanvas, new Vector3(0, 0, 0), player.transform.rotation));
        }
    }
    public override void PlaceDropItem(PlayerInfomation player) { }
}
