using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Inventory bag;

    public GameObject slotGrid;
    //public Slot slotPrefab;

    public GameObject emptySlot;

    public Text itemMSG;

    static InventoryManager instance;

    public List<GameObject> slotList = new List<GameObject>();




    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemMSG.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        RefreshItem();
    }

    public static void UpdateTtemMSG(string msg)
    {
        instance.itemMSG.text = msg;
    }


   


    //public static void CreatNewItem(Item item)
    //{
    //    Slot newItem = Instantiate(instance.slotPrefab,instance.slotGrid.transform.position,Quaternion.identity);
    //    newItem.gameObject.transform.SetParent(instance.slotGrid.transform);
    //    newItem.slotItem = item;

    //    newItem.image.sprite = item.itemImg;
    //    Debug.Log("Item count:" + item.itemHeld);
    //    newItem.countText.text = item.itemHeld.ToString();
    //}

    public static void RefreshItem()
    {
        instance.slotList.Clear();
        for(int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
        }

        for(int i = 0;i<instance.bag.itemList.Count;i++)
        {
            instance.slotList.Add(Instantiate(instance.emptySlot));
            instance.slotList[i].transform.SetParent(instance.slotGrid.transform);
            instance.slotList[i].GetComponent<Slot>().SetSlots(instance.bag.itemList[i]);
            //CreatNewItem(instance.bag.itemList[i]);
        }

    }

}
