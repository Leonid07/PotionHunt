using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Transform itemParent; // Контейнер для предметов в UI
    public GameObject itemPrefab; // Префаб UI-элемента для предмета
    public Dictionary<ConsumableScripteble, int> itemDictionary = new Dictionary<ConsumableScripteble, int>();

    public void LoadItemsUI()
    {
        itemDictionary = DataManger.InstanceData.LoadItems();
        // Очистка старых предметов в UI
        foreach (Transform child in itemParent)
        {
            Destroy(child.gameObject);
        }

        // Проход по всем загруженным предметам и создание UI-элементов
        foreach (KeyValuePair<ConsumableScripteble, int> entry in itemDictionary)
        {
            ConsumableScripteble item = entry.Key;
            int quantity = entry.Value;

            // Создание нового элемента UI для предмета
            GameObject newItem = Instantiate(itemPrefab, itemParent);

            ItemSlot itemSlot = newItem.GetComponent<ItemSlot>();

            itemSlot.spriteImage.sprite = item.spriteItem;
            itemSlot.consumableScripteble = item;
            itemSlot.countItem.text = quantity.ToString();
            itemSlot.sellCount.text = item.price.ToString();
        }
    }
}
