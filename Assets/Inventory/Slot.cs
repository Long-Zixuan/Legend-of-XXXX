using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item slotItem;
    public Image image;
    public Text countText;

    public string slotMSG;

    public GameObject itemSlots;


    public void SetSlots(Item item)
    {
        if(item == null)
        {
            itemSlots.SetActive(false);
            return;
        }
        slotItem = item;
        image.sprite = item.itemImg;
        countText.text = item.itemHeld.ToString();
        slotMSG = item.itemMSG;
    }


    public void OnItemClick()
    {
        if (slotItem.itemMSG == null || slotItem.itemMSG == "")
        {
            InventoryManager.UpdateTtemMSG("MSGNotFound!");
        }
        else
        {
            InventoryManager.UpdateTtemMSG(slotMSG);
        }
    }

    //private void Start()
    //{
    //    if(image == null)
    //    {
    //        image = GetComponent<Image>();
    //    }
    //    if (countText == null) 
    //    {
    //        countText = GetComponent<Text>();
    //    }
    //}

}
