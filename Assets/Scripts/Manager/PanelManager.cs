using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public static PanelManager InstancePanel { get; private set; }

    private void Awake()
    {
        if (InstancePanel != null && InstancePanel != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstancePanel = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public InventoryManager inventoryManager;
    public CauldronManager cauldronManager;

    [Header("Кнопки закрытия")]
    public Button[] buttonClose;

    [Space(25)]
    [Header("Кноки открытия")]
    public Button buttonSerring;
    public Button buttonCauldron;
    public Button buttonShop;
    public Button buttonPersonal;
    public Button buttonReward;
    [Header("Переход из магазина на варку")]
    public Button buttonMoveToWarca;

    [Space(25)]
    [Header("Список панелей")]
    public GameObject panelSetting;
    public GameObject panelCauldron;
    public GameObject panelShop;
    public GameObject panelPersonal;
    public GameObject panelReward;
    public GameObject[] panels;

    [Header("Кнопки для уровней")]
    public Button buttonFirstLevel;
    public Button buttonSecondLevel;
    public Button buttonTherdLevel;
    public Button buttonFourLevel;

    [Header("Панель загрузки сцены")]
    private string baseText = "LOADING";
    private int dotCount = 0;
    public float loadingDuration = 5f;

    [Header("Кнопки просмотра рекламы")]
    public Button[] buttonCoin;
    public Button[] buttonEnergy;
    public GameObject panelADSCoin;
    public GameObject panelADSEnergy;
    public Button buttonCloseADS_Energy; 
    public Button buttonCloseADS_Coin;

    [Header("Content for cauldron")]
    public Transform content;

    private void Start()
    {
        for (int i = 0; i < buttonClose.Length; i++)
        {
            int count = i;
            buttonClose[i].onClick.AddListener(() => { ClosePanel(panels); });
        }

        for (int i = 0; i < buttonCoin.Length; i++)
        {
            int count = i;
            buttonCoin[count].onClick.AddListener(() => { OpenPanelADS(0); });
        }
        for (int i = 0; i < buttonEnergy.Length; i++)
        {
            int count = i;
            buttonEnergy[count].onClick.AddListener(() => { OpenPanelADS(1); });
        }

        buttonCloseADS_Coin.onClick.AddListener(() => ClosePanelADS());
        buttonCloseADS_Energy.onClick.AddListener(() => ClosePanelADS());

        buttonSerring.onClick.AddListener(() => { OpenPanel(panelSetting); });
        buttonCauldron.onClick.AddListener(() => { OpenPanel(panelCauldron); cauldronManager.LoadItemsUI(); });
        buttonShop.onClick.AddListener(() => { ClosePanel(panels); OpenPanel(panelShop); inventoryManager.LoadItemsUI(); });
        buttonPersonal.onClick.AddListener(() => { OpenPanel(panelPersonal); });
        buttonReward.onClick.AddListener(() => { OpenPanel(panelReward); });

        buttonMoveToWarca.onClick.AddListener(() =>
        {
            ClosePanel(panels);
            OpenPanel(panelCauldron);
        });

        buttonFirstLevel.onClick.AddListener(() => { LoadLevel(1);});
        buttonSecondLevel.onClick.AddListener(() => { LoadLevel(2); });
        buttonTherdLevel.onClick.AddListener(() => { LoadLevel(3);  });
        buttonFourLevel.onClick.AddListener(() => { LoadLevel(4); });

        buttonNextView.onClick.AddListener(() => { OpenPanel(panelCauldron); cauldronManager.LoadItemsUI(); panel_1_MainMenu.SetActive(false); panel_2_Cauldron.SetActive(true); });
        buttonViewShop.onClick.AddListener(() => { ClosePanel(panels); DataManger.InstanceData.SaveStartGame(); OpenPanel(panelShop); inventoryManager.LoadItemsUI(); });
    }

    public void OpenPanelADS(int variant)
    {
        if (variant == 0)
        {
            panelADSCoin.SetActive(true);
        }
        else
        {
            panelADSEnergy.SetActive(true);
        }
    }
    public void ClosePanelADS()
    {
        panelADSCoin.SetActive(false);
        panelADSEnergy.SetActive(false);
    }

    public void LoadLevel(int sceneIndex)
    {
        switch (sceneIndex)
        {
            case 1:
                int count = PlayerManager.InstancePlayer.energyCount - 5;
                if (count >= 0)
                {
                    PlayerManager.InstancePlayer.energyCount -= 5;
                    PlayerManager.InstancePlayer.SaveEnergy();
                    CanvasLoading.InstanceCanvasLoading.canvasPanel.SetActive(true);
                    StartCoroutine(AnimateLoadingText(sceneIndex));
                    PlayerManager.InstancePlayer.MainCanvas.GetComponent<Canvas>().enabled = false;
                    break;
                }
                break;
            case 2:
                int count2 = PlayerManager.InstancePlayer.energyCount - 10;
                if (count2 >= 0)
                {
                    PlayerManager.InstancePlayer.energyCount -= 10;
                    PlayerManager.InstancePlayer.SaveEnergy();
                    CanvasLoading.InstanceCanvasLoading.canvasPanel.SetActive(true);
                    StartCoroutine(AnimateLoadingText(sceneIndex));
                    PlayerManager.InstancePlayer.MainCanvas.GetComponent<Canvas>().enabled = false;
                    break;
                }
                break;
            case 3:
                int count3 = PlayerManager.InstancePlayer.energyCount - 15;
                if (count3 >= 0)
                {
                    PlayerManager.InstancePlayer.energyCount -= 15;
                    PlayerManager.InstancePlayer.SaveEnergy();
                    CanvasLoading.InstanceCanvasLoading.canvasPanel.SetActive(true);
                    StartCoroutine(AnimateLoadingText(sceneIndex));
                    PlayerManager.InstancePlayer.MainCanvas.GetComponent<Canvas>().enabled = false;
                    break;
                }
                break;
            case 4:
                int count4 = PlayerManager.InstancePlayer.energyCount - 20;
                if (count4 >= 0)
                {
                    PlayerManager.InstancePlayer.energyCount -= 20;
                    PlayerManager.InstancePlayer.SaveEnergy();
                    CanvasLoading.InstanceCanvasLoading.canvasPanel.SetActive(true);
                    StartCoroutine(AnimateLoadingText(sceneIndex));
                    PlayerManager.InstancePlayer.MainCanvas.GetComponent<Canvas>().enabled = false;
                    break;
                }
                break;
        }
    }

    private IEnumerator AnimateLoadingText(int sceneIndex)
    {
        float elapsedTime = 0f;

        while (elapsedTime < loadingDuration)
        {
            dotCount = (dotCount + 1) % 4;
            CanvasLoading.InstanceCanvasLoading.loadingText.text = baseText + new string('.', dotCount); ;
            //loadingText.text = baseText + new string('.', dotCount);
            yield return new WaitForSeconds(0.5f);
            elapsedTime += 0.5f;
        }
        SceneManager.LoadScene(sceneIndex);
    }

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void ClosePanel(GameObject[] panels)
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    [Header("Обучение")]
    public ImageMover[] imageFinger;
    public GameObject[] gameObjectFinger;

    public GameObject panel_1_MainMenu;
    public GameObject panel_2_Cauldron;
    public GameObject panel_3_Cauldron;

    public Button buttonNextView;
    public Button buttonViewShop;

    public Transform panelContent;

    // Действия, которые будут выполняться в зависимости от количества дочерних объектов
    void Update()
    {
        if (DataManger.InstanceData.firstStartCount == 1)
        {
            panel_3_Cauldron.SetActive(false);
            panel_2_Cauldron.SetActive(false);
            panel_1_MainMenu.SetActive(false);
            foreach (GameObject gm in gameObjectFinger)
            {
                gm.SetActive(false);
            }
        }

        if (DataManger.InstanceData.firstStartCount == 0)
        {
            if (panel_1_MainMenu.activeInHierarchy == false)
            {
                switch (panelContent.childCount)
                {
                    case 0:

                        gameObjectFinger[0].SetActive(true);
                        //imageFinger[0].MoveImageRect();
                        //imageFinger[1].StopMoving();

                        gameObjectFinger[1].SetActive(false);
                        gameObjectFinger[2].SetActive(false);
                        gameObjectFinger[3].SetActive(false);


                        panel_2_Cauldron.SetActive(false);
                        panel_3_Cauldron.SetActive(true);

                        break;
                    case 1:

                        gameObjectFinger[1].SetActive(true);
                        //imageFinger[1].MoveImageRect();
                        //imageFinger[2].StopMoving();

                        gameObjectFinger[2].SetActive(false);
                        gameObjectFinger[0].SetActive(false);
                        gameObjectFinger[3].SetActive(false);


                        break;
                    case 2:

                        gameObjectFinger[2].SetActive(true);
                        //imageFinger[2].MoveImageRect();
                        //imageFinger[3].StopMoving();

                        gameObjectFinger[3].SetActive(false);
                        gameObjectFinger[1].SetActive(false);
                        gameObjectFinger[0].SetActive(false);

                        break;
                    case 3:

                        gameObjectFinger[3].SetActive(true);
                        //imageFinger[3].MoveImageRect();

                        break;
                }
            }
        }
    }
}
