using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Inventory", menuName = "Inventory/new Inventory")]
public class Inventory:ScriptableObject
{
    public List<Item> itemList = new List<Item>();
}
