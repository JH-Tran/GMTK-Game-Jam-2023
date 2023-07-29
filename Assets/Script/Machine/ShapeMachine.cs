using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeMachine : Machine
{
    [SerializeField] private GameObject pictureCanvas;
    [SerializeField] private Sprite[] shapeList;
    int shapeListCount = 0;

    public void Awake()
    {
        SetMachineComponents();
    }

    public override void UseMachine(PlayerInfomation player)
    {
        if (!counterTop.IsItemOnTable()) return;
        if (counterTop.GetItemOnCounter().transform.childCount > 0)
        {
            counterTop.ChangeItemShape(shapeList[shapeListCount]);
            UpdateItemShape();
            shapeListCount++;
            if (shapeListCount >= shapeList.Length) shapeListCount = 0;
        }
        else
        {
            GameObject tempItem = Instantiate(pictureCanvas, new Vector2(0, 0), Quaternion.identity);
            counterTop.ChangeItemShape(shapeList[shapeListCount], tempItem);
            UpdateItemShape();
        }
    } 
    private void UpdateItemShape()
    {
        if (shapeListCount == 0)
        {
            counterTop.GetItemOnCounterPicture().SetShape(PictureShape.Triangle);

        }
        else if (shapeListCount == 1)
        {
            counterTop.GetItemOnCounterPicture().SetShape(PictureShape.Square);

        }
        else if (shapeListCount == 2)
        {
            counterTop.GetItemOnCounterPicture().SetShape(PictureShape.Circle);
        }
    }
}
