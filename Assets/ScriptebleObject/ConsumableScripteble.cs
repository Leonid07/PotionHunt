using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Consumable")]
public class ConsumableScripteble : ScriptableObject
{
    public string Name;
    public Sprite spriteItem;
    public int price;
    public int quantity;
}
