using UnityEngine;

public class ItemOnScene : MonoBehaviour
{
    public int itemOnSceneCount = 1;

    public Item thisItem;

    public Inventory playInventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AddNewItem();
           // playInventory.AddItem(thisItem);
            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AddNewItem();
            // playInventory.AddItem(thisItem);
            Destroy(gameObject);
        }
    }


    void AddNewItem()
    {
        thisItem.itemHeld += itemOnSceneCount;
        if (!playInventory.itemList.Contains(thisItem))
        {
            for(int i = 0; i < playInventory.itemList.Count; i++)
            {
                if(playInventory.itemList[i] == null)
                {
                    playInventory.itemList[i] = thisItem;
                    break;
                }
            }


            //playInventory.itemList.Add(thisItem);
           // InventoryManager.CreatNewItem(thisItem);
        }
        InventoryManager.RefreshItem();
    }

}
