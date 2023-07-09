using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureMachine : MonoBehaviour
{
    [SerializeField] private MachineType machineType;
    [SerializeField] private GameObject pictureCanvas;
    [SerializeField] private GoalManager goalManager;
    [SerializeField] private ScreenSide screenSide;
    private CounterTop counterTop;
    private Dictionary<Color, PictureColor> colorDictionary = new Dictionary<Color, PictureColor>
    {
        { Color.white, PictureColor.White },
        { Color.red, PictureColor.Red },
        { Color.blue, PictureColor.Blue},
        { Color.green, PictureColor.Green }
    };
    private Color[] colorList = { Color.white, Color.red, Color.blue, Color.green };
    [SerializeField] private Sprite[] shapeList;
    int colorListCount = 0;
    int shapeListCount = 0;

    private float buttonTimer = 0;
    private bool isButtonTimerOn = false;
    private float buttonCoolDown = 30;

    private SpriteRenderer machineSprite;
    [SerializeField] private Sprite ButtonOnSprite;
    [SerializeField] private Sprite ButtonOffSprite;

    private void Start()
    {
        counterTop = gameObject.GetComponent<CounterTop>();
        machineSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (machineType == MachineType.Button)
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
    }

    public void UseMachine(PlayerInfomation player)
    {
        if (machineType == MachineType.Colour)
        {
            if (counterTop.IsItemOnTable())
            {
                counterTop.ChangeItemColour(colorList[colorListCount]);
                counterTop.GetItemOnCounterPicture().SetColour(colorDictionary.GetValueOrDefault(colorList[colorListCount]));                
                colorListCount++;
                if (colorListCount >= colorList.Length) colorListCount = 0; 
            }
        }
        else if (machineType == MachineType.Shape)
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
        else if (machineType == MachineType.Button)
        {
            if (isButtonTimerOn == false)
            {
                goalManager.AddTime(screenSide);
                buttonTimer = buttonCoolDown;
                isButtonTimerOn = true;
            }
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

    public void PlaceDropItem(PlayerInfomation player)
    {
        if (machineType == MachineType.Canvas)
        {
            if (!player.IsHoldingItem())
            {
                player.HoldItem(Instantiate(pictureCanvas, new Vector3(0, 0, 0), player.transform.rotation));
            }
        }
        if (counterTop == null) { return; }
        if (!player.IsHoldingItem() && counterTop.IsItemOnTable())
        {
            //Debug.Log("Item Missing from Table");
            player.HoldItem(counterTop.TakeItemOnTable());
        }
        else if (player.IsHoldingItem() && !counterTop.IsItemOnTable())
        {
            //Debug.Log("Place item");
            player.PlaceItem(counterTop.GiveItem());
            if (machineType == MachineType.Bin)
            {
                counterTop.DeleteHeldItem();
            }
            else if (machineType == MachineType.Deliver)
            {
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
