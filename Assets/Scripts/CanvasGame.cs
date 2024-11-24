using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasGame : MonoBehaviour
{
    public Button buttonReloadGame;
    public Button buttonBackToMainMenu;

    public TMP_Text textCountReload;
    public int CountEnergy;

    private void Start()
    {
        buttonBackToMainMenu.onClick.AddListener(BackToMainMenu);
        buttonReloadGame.onClick.AddListener(ReloadLevel);
    }
    public void ReloadLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        textCountReload.text = CountEnergy.ToString();
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
