using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image spriteImage;
    public TMP_Text countItem;
    public TMP_Text sellCount;
    public ConsumableScripteble consumableScripteble;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(()=> 
        {
            DataManger.InstanceData.RemoveItem(consumableScripteble);
            PanelManager.InstancePanel.inventoryManager.LoadItemsUI();
        });
    }
}