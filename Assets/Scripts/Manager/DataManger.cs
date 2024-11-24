using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public int quantity;
}


public class DataManger : MonoBehaviour
{
    public static DataManger InstanceData { get; private set; }

    private void Awake()
    {
        if (InstanceData != null && InstanceData != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceData = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public List<ConsumableScripteble> items; // Список предметов

    private string filePath;

    [Header("Первый запуск")]
    public ConsumableScripteble[] item;

    public int firstStartCount = 0; // 0 первый старт 1 второй запуск
    public string firstStart = "firstStart";

    public void FirstStartGame()
    {
        if (PlayerPrefs.HasKey(firstStart))
        {
            firstStartCount = PlayerPrefs.GetInt(firstStart);
            return;
        }
        else
        {
            if (firstStartCount == 0)
            {
                foreach (ConsumableScripteble it in item)
                {
                    CollectItem(it);
                }
            }
        }
    }
    public void SaveStartGame()
    {
        firstStartCount++;
        PlayerPrefs.SetInt(firstStart, firstStartCount);
    }

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "items.json");
        Debug.Log("File path: " + filePath);
        CreateEmptyFileIfNotExists();

        FirstStartGame();
    }
    private void CreateEmptyFileIfNotExists()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "{\r\n    \"items\": []\r\n}"); // Создаем пустой JSON-массив
            Debug.Log("Created empty file at: " + filePath);
        }
    }

public void SaveItems(Dictionary<ConsumableScripteble, int> itemDictionary)
    {
        List<ItemData> itemList = new List<ItemData>();
        foreach (KeyValuePair<ConsumableScripteble, int> entry in itemDictionary)
        {
            ConsumableScripteble item = entry.Key;
            int quantity = entry.Value;

            ItemData data = new ItemData
            {
                itemName = item.Name,
                iconPath = item.spriteItem != null ? item.spriteItem.name : "", // сохраняем только имя иконки
                price = item.price,
                quantity = quantity // сохраняем количество
            };
            itemList.Add(data);
        }

        string json = JsonUtility.ToJson(new Serialization<ItemData>(itemList), true);
        File.WriteAllText(filePath, json);
        Debug.Log("Items saved to: " + filePath);
    }

    public Dictionary<ConsumableScripteble, int> itemDictionary = new Dictionary<ConsumableScripteble, int>();

    // Загрузка массива ScriptableObject из JSON
    public Dictionary<ConsumableScripteble, int> LoadItems()
    {
        //Dictionary<ConsumableScripteble, int> itemDictionary = new Dictionary<ConsumableScripteble, int>();

        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                Serialization<ItemData> dataList = JsonUtility.FromJson<Serialization<ItemData>>(json);

                foreach (ItemData data in dataList.items)
                {
                    ConsumableScripteble item = items.Find(x => x.Name == data.itemName);
                    if (item == null)
                    {
                        Debug.LogWarning("Item not found: " + data.itemName);
                        continue;
                    }

                    itemDictionary[item] = data.quantity;
                }

                Debug.Log("Items loaded from: " + filePath);
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to load items: " + ex.Message);
            }
        }
        else
        {
            Debug.LogWarning("Save file not found!");
        }

        return itemDictionary;
    }

    public void CollectItem(ConsumableScripteble item)
    {
        if (itemDictionary.ContainsKey(item))
        {
            itemDictionary[item]++;
        }
        else
        {
            itemDictionary[item] = 1;
        }

        // Вызываем сохранение после сбора предмета
        SaveItems(itemDictionary);
    }
    public void RemoveItem(ConsumableScripteble item)
    {
        if (itemDictionary.ContainsKey(item))
        {
            int currentQuantity = itemDictionary[item];
            currentQuantity--; // Уменьшаем количество

            PlayerManager.InstancePlayer.goldCount += item.price;
            PlayerManager.InstancePlayer.SaveGold();

            if (currentQuantity <= 0)
            {
                itemDictionary.Remove(item);
            }
            else
            {
                itemDictionary[item] = currentQuantity; // Обновляем количество в словаре
            }

            SaveItems(itemDictionary);
        }
        else
        {
            Debug.LogWarning("Item not found in the inventory: " + item.Name);
        }
    }
}

// Вспомогательный класс для сериализации массива
[System.Serializable]
public class Serialization<T>
{
    public T[] items;

    public Serialization(List<T> items)
    {
        this.items = items.ToArray();
    }
}