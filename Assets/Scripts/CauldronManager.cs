using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CauldronManager : MonoBehaviour
{
    public Animator animator;
    public Transform itemParent; // ��������� ��� ��������� � UI
    public GameObject itemPrefab; // ������ UI-�������� ��� ��������
    public HorizontalLayoutGroup horizontalLayoutGroup;

    public List<ConsumableScripteble> consumableScripteble = new List<ConsumableScripteble>();

    public List<ConsumableScripteble> EssenceOfWisdom;
    public List<ConsumableScripteble> ElixirOfInvisibility;
    public List<ConsumableScripteble> PotionOfIceCalm;
    public List<ConsumableScripteble> ElixirOfFieryWrath;
    public List<ConsumableScripteble> TinctureOfMemory;
    public List<ConsumableScripteble> VialOfMoonlight;

    public ConsumableScripteble EssenceOfWisdom_P;
    public ConsumableScripteble ElixirOfInvisibility_P;
    public ConsumableScripteble PotionOfIceCalm_P;
    public ConsumableScripteble ElixirOfFieryWrath_P;
    public ConsumableScripteble TinctureOfMemory_P;
    public ConsumableScripteble VialOfMoonlight_P;

    public Dictionary<ConsumableScripteble, int> itemDictionary = new Dictionary<ConsumableScripteble, int>();

    [Header("������� ������")]
    public Transform spawnPoint;
    public ParticleSystem effectCauldron;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void CheckPoition()
    {
        CheckListsMatch(consumableScripteble, EssenceOfWisdom, "EssenceOfWisdom", PerformEssenceOfWisdomAction);
        CheckListsMatch(consumableScripteble, ElixirOfInvisibility, "ElixirOfInvisibility", PerformElixirOfInvisibilityAction);
        CheckListsMatch(consumableScripteble, PotionOfIceCalm, "PotionOfIceCalm", PerformPotionOfIceCalmAction);
        CheckListsMatch(consumableScripteble, ElixirOfFieryWrath, "ElixirOfFieryWrath", PerformElixirOfFieryWrathAction);
        CheckListsMatch(consumableScripteble, TinctureOfMemory, "TinctureOfMemory", PerformTinctureOfMemoryAction);
        CheckListsMatch(consumableScripteble, VialOfMoonlight, "VialOfMoonlight", PerformVialOfMoonlightAction);
    }

    private void CheckListsMatch(List<ConsumableScripteble> list1, List<ConsumableScripteble> list2, string listName, Action actionIfMatch)
    {
        if (list1.Count == list2.Count)
        {
            bool areEqual = true;

            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] != list2[i])
                {
                    areEqual = false;
                    break;
                }
            }

            if (areEqual)
            {
                Debug.Log($"������ consumableScripteble ��������� � {listName}.");
                actionIfMatch?.Invoke();
            }
        }
        else
        {
            Debug.Log($"������ consumableScripteble � {listName} ����� ������ ���������� ���������.");
        }
    }

    private void PerformEssenceOfWisdomAction()
    {
        DataManger.InstanceData.CollectItem(EssenceOfWisdom_P);
        LoadItemsUI();
        consumableScripteble.Clear();
        animator.StopPlayback();

        Instantiate(effectCauldron, spawnPoint);
    }

    private void PerformElixirOfInvisibilityAction()
    {
        DataManger.InstanceData.CollectItem(ElixirOfInvisibility_P);
        consumableScripteble.Clear();
        LoadItemsUI();
        animator.StopPlayback();
        Instantiate(effectCauldron, spawnPoint);
    }

    private void PerformPotionOfIceCalmAction()
    {
        DataManger.InstanceData.CollectItem(PotionOfIceCalm_P);
        consumableScripteble.Clear();
        LoadItemsUI();
        animator.StopPlayback();
        Instantiate(effectCauldron, spawnPoint);
    }

    private void PerformElixirOfFieryWrathAction()
    {
        DataManger.InstanceData.CollectItem(ElixirOfFieryWrath_P);
        consumableScripteble.Clear();
        LoadItemsUI();
        animator.StopPlayback();
        Instantiate(effectCauldron, spawnPoint);
    }

    private void PerformTinctureOfMemoryAction()
    {
        DataManger.InstanceData.CollectItem(TinctureOfMemory_P);
        consumableScripteble.Clear();
        LoadItemsUI();
        animator.StopPlayback();
        Instantiate(effectCauldron, spawnPoint);
    }

    private void PerformVialOfMoonlightAction()
    {
        DataManger.InstanceData.CollectItem(VialOfMoonlight_P);
        consumableScripteble.Clear();
        LoadItemsUI();
        animator.StopPlayback();
        Instantiate(effectCauldron, spawnPoint);
    }

    public void CleadCatel()
    {
        animator.enabled = false;
        consumableScripteble.Clear();
        animator.enabled = true;
    }

    public void LoadItemsUI()
    {
        itemDictionary = DataManger.InstanceData.LoadItems();

        // ��������, ������� ����� ���������
        string[] excludedItems = { "Essence of Wisdom", "Elixir of Fiery Wrath", "Elixir of Invisibility", "Potion of Ice Calm", "Tincture of Memory", "Vial of Moonlight" };

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

            // ������� ��������� �� ������ ����������
            if (excludedItems.Contains(item.Name))
            {
                continue; // ���������� ���� ������� � ��������� � ����������
            }

            // �������� ������ �������� UI ��� ��������
            GameObject newItem = Instantiate(itemPrefab, itemParent);

            CauldronItem itemSlot = newItem.GetComponent<CauldronItem>();

            itemSlot.consumableScripteble = item;
            itemSlot.imageSprite.sprite = item.spriteItem;
            itemSlot.textCount.text = quantity.ToString();
        }
    }
}
