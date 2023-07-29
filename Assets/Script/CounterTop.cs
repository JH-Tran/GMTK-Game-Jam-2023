using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterTop : MonoBehaviour
{
    [SerializeField] private GameObject itemOnCounter;

    public bool IsItemOnTable()
    {
        return itemOnCounter != null;
    }
    public Picture GetItemOnCounterPicture()
    {
        if (itemOnCounter == null) return null;
        return itemOnCounter.GetComponent<Picture>();
    }
    public GameObject GetItemOnCounter()
    {
        return itemOnCounter;
    }
    public GameObject GiveItem()
    {
        return gameObject;
    }
    public GameObject TakeItemOnTable()
    {
        GameObject temp = itemOnCounter;
        itemOnCounter = null;
        return temp;
    }
    public void ChangeItemColour(Color colour)
    {
        if (itemOnCounter.transform.childCount > 0)
        {
            itemOnCounter.GetComponentsInChildren<SpriteRenderer>()[1].GetComponent<SpriteRenderer>().color = colour;
            return;
        }
        itemOnCounter.GetComponentInChildren<SpriteRenderer>().color = colour;
    }
    public void ChangeItemShape(Sprite shape, GameObject createShapeItem = null)
    {
        if (createShapeItem != null)
        {
            if (itemOnCounter == null) { Debug.LogError("Item on counter missing!"); return; }
            //Make ex object to child by changing its properties
            createShapeItem.GetComponentInChildren<SpriteRenderer>().sprite = shape;
            createShapeItem.GetComponent<Picture>().SetColour(itemOnCounter.GetComponent<Picture>().GetColor());
            createShapeItem.GetComponent<Picture>().SetShape(itemOnCounter.GetComponent<Picture>().GetShape());
            createShapeItem.transform.SetParent(itemOnCounter.transform);
            createShapeItem.transform.SetPositionAndRotation(itemOnCounter.transform.position, itemOnCounter.transform.rotation);
            itemOnCounter.GetComponent<Picture>().SetColour(PictureColor.White);
        }
        else
        {
            itemOnCounter.GetComponentsInChildren<SpriteRenderer>()[1].sprite = shape;
        }
    }
    public void SetItemOnTable(GameObject item)
    {
        itemOnCounter = item;
        itemOnCounter.transform.rotation = Quaternion.identity;
    }

    public void DeleteHeldItem()
    {
        Destroy(itemOnCounter);
        itemOnCounter = null;
    }
}
