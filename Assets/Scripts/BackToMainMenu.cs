using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToMainMenu : MonoBehaviour
{
    public GameObject panelWin;
    public TMP_Text textWIN;
    public string textWin = "ALL RIGHT!";

    public bool qe = false;

    private void Start()
    {
        if (qe == false)
        {
            GetComponent<Button>().onClick.AddListener(SetActivePanelWin);
        }
        else
        {
            GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene(0); PlayerManager.InstancePlayer.MainCanvas.GetComponent<Canvas>().enabled = true; });
        }
    }
    public void SetActivePanelWin()
    {
        panelWin.SetActive(true);
        textWIN.text = textWin;
    }
}
