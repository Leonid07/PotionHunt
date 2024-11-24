using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Transform itemParent; // ��������� ��� ��������� � UI
    public GameObject itemPrefab; // ������ UI-�������� ��� ��������
    public Dictionary<ConsumableScripteble, int> itemDictionary = new Dictionary<ConsumableScripteble, int>();

    public void LoadItemsUI()
    {
        itemDictionary = DataManger.InstanceData.LoadItems();
        // ������� ������ ��������� � UI
        foreach (Transform child in itemParent)
        {
            Destroy(child.gameObject);
        }

        // ������ �� ���� ����������� ��������� � �������� UI-���������
        foreach (KeyValuePair<ConsumableScripteble, int> entry in itemDictionary)
        {
            ConsumableScripteble item = entry.Key;
            int quantity = entry.Value;

            // �������� ������ �������� UI ��� ��������
            GameObject newItem = Instantiate(itemPrefab, itemParent);

            ItemSlot itemSlot = newItem.GetComponent<ItemSlot>();

            itemSlot.spriteImage.sprite = item.spriteItem;
            itemSlot.consumableScripteble = item;
            itemSlot.countItem.text = quantity.ToString();
            itemSlot.sellCount.text = item.price.ToString();
        }
    }
}
