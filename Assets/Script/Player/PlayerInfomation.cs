using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfomation : MonoBehaviour
{
    [SerializeField] GameObject itemHoldingPosition;
    [SerializeField] GameObject itemHolding;

    public bool IsHoldingItem()
    {
        return itemHolding != null;
    }
    public void PlaceItem(GameObject place)
    {
        itemHolding.transform.parent = place.transform;
        itemHolding.transform.position = place.transform.position;
        place.GetComponent<CounterTop>().SetItemOnTable(itemHolding);
        itemHolding = null;
    }
    public GameObject GetHeldItem()
    {
        return itemHolding;
    }
    public void HoldItem(GameObject item)
    {
        itemHolding = item;
        item.transform.parent = itemHoldingPosition.transform;
        item.transform.position = itemHoldingPosition.transform.position;
        item.transform.rotation = itemHoldingPosition.transform.rotation;
    }
}
