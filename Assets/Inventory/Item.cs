using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class Item:ScriptableObject
{
    public string itemName;
    public Sprite itemImg;
    public int itemHeld;

    [TextArea]
    public string itemMSG;

    public bool Stackable;
}
