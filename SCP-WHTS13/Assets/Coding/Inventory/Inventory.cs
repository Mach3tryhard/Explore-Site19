using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory instance;
    void Awake()
    {
        if(instance!= null)
        {
            Debug.LogWarning("More Than One Instance Of Inventory found!");
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged OnItemChangedCallback;
    public Transform PlaceItemHereOnDrop;

    public int space = 8;

    public List<Item> items = new List<Item>();
    #region CARDSCHECK

    public int CheckCardLevel()
    {
        int level=-1;
        foreach (Item Litem in items)
        {
            if(Litem.name=="CardL0" && level<0)level=0;
            if(Litem.name=="CardL1" && level<1)level=1;
            if(Litem.name=="CardL2" && level<2)level=2;
            if(Litem.name=="CardL3" && level<3)level=3;
            if(Litem.name=="CardL4" && level<4)level=4;
            if(Litem.name=="CardL5" && level<5)level=5;
        }
        return level;
    }
    #endregion
    public bool Add (Item item)
    {
        if(items.Count>=space)
        {
            Debug.Log("Not enough room.");
            return false;
        }
        items.Add(item);/// Efect instant la pickup
        if(item.name=="SCP-198")
        {
            item.Use();
        }
        if(OnItemChangedCallback !=null)
        {
            OnItemChangedCallback.Invoke();
        }
        return true;
    }

    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(3);
    }

    public void Remove(Item item,int removetype)
    {
        if(item.name!="SCP-198")/// Nu dau drop la 198
        {
            if(removetype==0)
            {
                if(item.isUsing==true)// sa nu poata folosi itemul dropat
                {
                    item.Use();
                }
                item.gameObjectOfItem.transform.parent = GameObject.FindWithTag("ItemParent").transform;
                item.gameObjectOfItem.transform.position=PlaceItemHereOnDrop.position;
                item.gameObjectOfItem.SetActive(true);
                ///
                items.Remove(item);
            }
            if(removetype==1)
            {
                items.Remove(item);
            }

            if(OnItemChangedCallback !=null)
            {
                OnItemChangedCallback.Invoke();
            }
        }
    }
    
}
