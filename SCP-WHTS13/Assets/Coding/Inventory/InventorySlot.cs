using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public Image icon;

    public Button removeButton;

    Item item;
    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }
    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item,0);
    }

    public void UseItem()
    {
        bool isOneTimeUse = item.oneTimeUse;
        if(item!=null && isOneTimeUse==true)
        {
            item.Use();
            Inventory.instance.Remove(item,1);
        }
        if(item!=null && isOneTimeUse==false)
        {
            item.Use();
        }
    }
}
