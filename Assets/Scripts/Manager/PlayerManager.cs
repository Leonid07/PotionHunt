using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager InstancePlayer { get; private set; }
    public GameObject MainCanvas;
    private void Awake()
    {
        if (InstancePlayer != null && InstancePlayer != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstancePlayer = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public AudioSource soundLevelUp;

    [Header("Параметры для скорости сборки")]
    public float collectionSpeed = 10f;
    public string idCollectionSpeed = "speed";

    [Header("Параметры для золота")]
    public int goldCount = 1000;
    public string idGold = "gold";

    [Header("Параметры енергии")]
    public int energyCount = 100;
    public string idEnergy = "energy";
    public int maxEnergy = 100; // Максимальное значение энергии
    public int energyRegenRate = 1;

    [Header("Тесктовые окна")]
    public TMP_Text[] textGold;
    public TMP_Text[] textEnergy;

    private string lastEnergyUpdateKey = "lastEnergyUpdate";

    [Header("Данные для уровней")]
    public GameObject[] buttonLevel;
    public GameObject[] buttonLevelLoad;

    [Header("Уровень игрока")]
    public int level = 1;
    public string idLevel = "level";

    [Header("Улучшение персонажа")]
    public TMP_Text lavelLevelPerson;
    public TMP_Text textSpeedCollection;
    public TMP_Text beforeSpeed;
    public TMP_Text afterSpeed;

    public TMP_Text labelLevel;

    public int priceLevelUp = 500;

    [Header("Панель прокачки персонажа")]
    public GameObject panelUpdatePerson;
    public Button buttonViewPanelUpdate;
    public Button buttonCloseUpdatePanel;
    public Button buttonUpdatePerson;

    [Header("Партикл для улучшения персонажа")]
    public GameObject particleForLevelUpPlayer;
    public Transform person;

    private void Start()
    {
        LoadGold();
        LoadColletionSpeed();
        LoadEnergy();
        LoadLevel();

        // Обновляем энергию на основе времени, прошедшего с момента последнего обновления
        UpdateEnergyBasedOnTime();

        // Запускаем корутину для восстановления энергии каждую минуту
        StartCoroutine(RegenerateEnergyOverTime());

        buttonViewPanelUpdate.onClick.AddListener(() =>
        {
            panelUpdatePerson.SetActive(true);
            CheckLevelUp();
        });
        buttonCloseUpdatePanel.onClick.AddListener(() => { panelUpdatePerson.SetActive(false); });
        buttonUpdatePerson.onClick.AddListener(()=> { LevelUp(); soundLevelUp.Play(); });
    }

    public void CheckLevelUp()
    {
        float speed = collectionSpeed;

        beforeSpeed.text = collectionSpeed.ToString();
        afterSpeed.text = (speed - 0.25f).ToString();
        collectionSpeed -= 0.25f;
    }

    public void LevelUp()
    {

        if (goldCount >= priceLevelUp)
        {
            goldCount -= priceLevelUp;

            level++;

            labelLevel.text = level.ToString();
            textSpeedCollection.text = collectionSpeed.ToString();

            GameObject par = Instantiate(particleForLevelUpPlayer, person.position, Quaternion.identity, person);
            par.transform.Rotate(-90,0,0);

            SaveGold();
            Savelevel();
            CheckLevel();
            SaveColletionSpeed();

            CheckLevelUp();
        }
    }

    public void LoadLevel()
    {
        if (PlayerPrefs.HasKey(idLevel))
        {
            level = PlayerPrefs.GetInt(idLevel);
            labelLevel.text = level.ToString();
            CheckLevel();
        }
    }
    public void CheckLevel()
    {
        for (int i = 0; i < buttonLevel.Length; i++)
        {
            if (level >= 5)
            {
                buttonLevel[0].gameObject.SetActive(false);
                buttonLevelLoad[0].GetComponent<Button>().interactable = true;
            }
            if (level >= 10)
            {
                buttonLevel[1].SetActive(false);
                buttonLevelLoad[1].GetComponent<Button>().interactable = true;
            }
            if (level >= 15)
            {
                buttonLevel[2].SetActive(false);
                buttonLevelLoad[2].GetComponent<Button>().interactable = true;
            }
        }
    }
    public void Savelevel()
    {
        PlayerPrefs.SetInt(idLevel, level);
        PlayerPrefs.Save();
    }

    public void SaveGold()
    {
        PlayerPrefs.SetInt(idGold, goldCount);
        InsertGoldToText(goldCount.ToString());
        PlayerPrefs.Save();
    }

    void InsertGoldToText(string text)
    {
        for (int i = 0; i < textGold.Length; i++)
        {
            textGold[i].text = text;
        }
    }

    public void LoadGold()
    {
        if (PlayerPrefs.HasKey(idGold))
        {
            goldCount = PlayerPrefs.GetInt(idGold);
            InsertGoldToText(goldCount.ToString());
        }
    }

    public void SaveColletionSpeed()
    {
        PlayerPrefs.SetFloat(idCollectionSpeed, collectionSpeed);
        PlayerPrefs.Save();
    }

    public void LoadColletionSpeed()
    {
        if (PlayerPrefs.HasKey(idCollectionSpeed))
        {
            collectionSpeed = PlayerPrefs.GetFloat(idCollectionSpeed);
            textSpeedCollection.text = collectionSpeed.ToString();
        }
    }

    public void SaveEnergy()
    {
        PlayerPrefs.SetInt(idEnergy, energyCount);
        InsertEnergyToText(energyCount.ToString());
        PlayerPrefs.Save();
    }

    void InsertEnergyToText(string energy)
    {
        for (int i = 0; i < textEnergy.Length; i++)
        {
            textEnergy[i].text = energy;
        }
    }

    public void LoadEnergy()
    {
        if (PlayerPrefs.HasKey(idEnergy))
        {
            energyCount = PlayerPrefs.GetInt(idEnergy);
            InsertEnergyToText(energyCount.ToString());
        }
    }
    private void OnApplicationQuit()
    {
        // Сохраняем текущее время при выходе из игры
        PlayerPrefs.SetString(lastEnergyUpdateKey, DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    private void UpdateEnergyBasedOnTime()
    {
        if (PlayerPrefs.HasKey(lastEnergyUpdateKey))
        {
            DateTime lastUpdate = DateTime.Parse(PlayerPrefs.GetString(lastEnergyUpdateKey));
            TimeSpan timePassed = DateTime.Now - lastUpdate;

            int minutesPassed = (int)timePassed.TotalMinutes;
            int energyToRegen = minutesPassed * energyRegenRate;

            if (energyToRegen > 0)
            {
                energyCount = Mathf.Min(energyCount + energyToRegen, maxEnergy);
                SaveEnergy();
            }
        }
    }

    private IEnumerator RegenerateEnergyOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f); // Ждем 60 секунд (1 минута)

            if (energyCount < maxEnergy)
            {
                energyCount += energyRegenRate;
                if (energyCount > maxEnergy)
                {
                    energyCount = maxEnergy;
                }
                SaveEnergy(); // Сохраняем обновленное количество энергии
            }
        }
    }
}
