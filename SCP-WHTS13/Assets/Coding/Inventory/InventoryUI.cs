using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;

    Inventory inventory;

    InventorySlot[] slots;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.OnItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Inventory"))
        {
            GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().canLook = !GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().canLook;
            GameObject.FindWithTag("Document").GetComponent<Image>().enabled=false;
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().canInteract = !GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().canInteract;
            if(Cursor.lockState==CursorLockMode.Locked)Cursor.lockState = CursorLockMode.None;
            else Cursor.lockState = CursorLockMode.Locked;
            if(Cursor.visible == false)Cursor.visible = true;
            else Cursor.visible = false;
        }
    }

    void UpdateUI()
    {
        for(int i=0;i<slots.Length;i++)
        {
            if(i<inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
        Debug.Log("Update UI");
    }
}
