using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeConsumable : MonoBehaviour
{
    public Slider slider;
    public GameObject buttonTake;

    public bool take = false;

    public ConsumableScripteble[] consumableScripteble;
    public Gmae[] gmaes;
    private void Start()
    {
        buttonTake.SetActive(false);
        for (int i = 0; i < consumableScripteble.Length; i++)
        {
            gmaes[i].consumableScripteble = consumableScripteble[i];
            gmaes[i].imageCell.sprite = consumableScripteble[i].spriteItem;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Con"))
        {
            buttonTake.SetActive(true);
            if (take == true)
            {
                Debug.Log("take");
                take = false;
                buttonTake.SetActive(false);
                DataManger.InstanceData.CollectItem(other.GetComponent<Item>().consumableScripteble);
                for (int i = 0; i < gmaes.Length; i++)
                {
                    if (gmaes[i].consumableScripteble.Name == other.GetComponent<Item>().consumableScripteble.Name)
                    {
                        int count = Convert.ToInt32(gmaes[i].textCell.text);
                        count++;
                        gmaes[i].textCell.text = count.ToString();  
                    }
                }
                Destroy(other.gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Con"))
        {
            buttonTake.SetActive(false);
            slider.gameObject.SetActive(false);
        }
    }
}
