using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bonus : MonoBehaviour
{
    public static Bonus InstanceBonus { get; private set; }
    private void Awake()
    {
        if (InstanceBonus != null && InstanceBonus != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceBonus = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public AudioSource soundChest;

    public TMP_Text dailyBonusText;
    public TMP_Text weeklyBonusText;
    public TMP_Text hourlyBonusText; // Текст для часового бонуса

    [Space(10)]
    public TMP_Text hourlyText; // Текст для часового бонуса
    public TMP_Text weeklyText; // Текст для часового бонуса
    public TMP_Text dailyText; // Текст для часового бонуса
    [Space(10)]
    public Button dailyBonusButton;
    public Button weeklyBonusButton;
    public Button hourlyBonusButton; // Кнопка для часового бонуса

    [Header("Анимации кнопок")]
    public Animator animHourly;
    public Animator animDaily;
    public Animator animWeekly;

    public GameObject dailyFG;
    public GameObject weeklyFG;
    public GameObject hourlyFG; // Графический объект для часового бонуса

    private const string DailyBonusTimeKey = "daily_bonus_time";
    private const string WeeklyBonusTimeKey = "weekly_bonus_time";
    private const string HourlyBonusTimeKey = "hourly_bonus_time"; // Ключ для сохранения времени часового бонуса

    public int HourlyBonusCooldownInSeconds = 3600; // 1 час
    public int DailyBonusCooldownInSeconds = 86400; // 24 часа
    public int WeeklyBonusCooldownInSeconds = 604800; // 7 дней

    public int countHourly = 5; // Количество награды за часовой бонус
    public int countDaily = 25;
    public int countWeekly = 50;

    private void Start()
    {
        hourlyBonusButton.onClick.AddListener(() => ClaimHourlyBonus());
        dailyBonusButton.onClick.AddListener(() => ClaimDailyBonus());
        weeklyBonusButton.onClick.AddListener(() => ClaimWeeklyBonus());

        StartCoroutine(UpdateBonusTextsRoutine());
    }

    private IEnumerator UpdateBonusTextsRoutine()
    {
        while (true)
        {
            UpdateBonusTexts();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void UpdateBonusTexts()
    {
        string dailyBonusTimeStr = PlayerPrefs.GetString(DailyBonusTimeKey, "0");
        string weeklyBonusTimeStr = PlayerPrefs.GetString(WeeklyBonusTimeKey, "0");
        string hourlyBonusTimeStr = PlayerPrefs.GetString(HourlyBonusTimeKey, "0");

        long dailyBonusTime = long.Parse(dailyBonusTimeStr);
        long weeklyBonusTime = long.Parse(weeklyBonusTimeStr);
        long hourlyBonusTime = long.Parse(hourlyBonusTimeStr);

        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

        long dailyCooldown = dailyBonusTime + DailyBonusCooldownInSeconds - currentTimestamp;
        long weeklyCooldown = weeklyBonusTime + WeeklyBonusCooldownInSeconds - currentTimestamp;
        long hourlyCooldown = hourlyBonusTime + HourlyBonusCooldownInSeconds - currentTimestamp;

        dailyBonusText.text = FormatTimeDaily(dailyCooldown);
        weeklyBonusText.text = FormatTimeWeekly(weeklyCooldown);
        hourlyBonusText.text = FormatTimeHourly(hourlyCooldown);

        dailyBonusButton.interactable = dailyCooldown <= 0;
        weeklyBonusButton.interactable = weeklyCooldown <= 0;
        hourlyBonusButton.interactable = hourlyCooldown <= 0;
    }

    private string FormatTimeDaily(long seconds)
    {
        if (seconds <= 0)
        {
            //dailyFG.SetActive(false);
            dailyText.gameObject.SetActive(true);
            return "DAILY";
        }
        //dailyFG.SetActive(true);
        //dailyText.gameObject.SetActive(false);
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    private string FormatTimeWeekly(long seconds)
    {
        if (seconds <= 0)
        {
            //weeklyFG.SetActive(false);
            weeklyText.gameObject.SetActive(true);
            return "WEEKLY";
        }
        //weeklyFG.SetActive(true);
        //weeklyText.gameObject.SetActive(false);
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        int totalHours = (int)timeSpan.TotalHours;
        return string.Format("{0:D2}:{1:D2}:{2:D2}", totalHours, timeSpan.Minutes, timeSpan.Seconds);
    }

    private string FormatTimeHourly(long seconds)
    {
        if (seconds <= 0)
        {
            //hourlyFG.SetActive(false);
            hourlyText.gameObject.SetActive(true);
            return "WELCOME";
        }
        //hourlyFG.SetActive(true);
        //hourlyText.gameObject.SetActive(false);
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    private void ClaimDailyBonus()
    {
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        PlayerManager.InstancePlayer.goldCount += countDaily;
        PlayerManager.InstancePlayer.SaveGold();
        animDaily.Play("Reward");
        soundChest.Play();
        PlayerPrefs.SetString(DailyBonusTimeKey, currentTimestamp.ToString());
        PlayerPrefs.Save();

        Debug.Log("Daily Bonus Claimed!");
        Debug.Log($"New Daily Bonus Time: {currentTimestamp}");
    }

    private void ClaimWeeklyBonus()
    {
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        PlayerManager.InstancePlayer.goldCount += countWeekly;
        PlayerManager.InstancePlayer.SaveGold();
        animWeekly.Play("Reward");
        soundChest.Play();
        PlayerPrefs.SetString(WeeklyBonusTimeKey, currentTimestamp.ToString());
        PlayerPrefs.Save();

        Debug.Log("Weekly Bonus Claimed!");
        Debug.Log($"New Weekly Bonus Time: {currentTimestamp}");
    }

    private void ClaimHourlyBonus()
    {
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        animHourly.Play("Reward");
        soundChest.Play();
        PlayerManager.InstancePlayer.goldCount += countHourly;
        PlayerManager.InstancePlayer.SaveGold();
        PlayerPrefs.SetString(HourlyBonusTimeKey, currentTimestamp.ToString());
        PlayerPrefs.Save();

        Debug.Log("Hourly Bonus Claimed!");
        Debug.Log($"New Hourly Bonus Time: {currentTimestamp}");
    }
}
