using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Consumable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Slider slider;
    public float fillTime;

    public TakeConsumable takeConsumable;

    private Coroutine fillCoroutine;

    private void Start()
    {
        fillTime = PlayerManager.InstancePlayer.collectionSpeed / 3;
        // Устанавливаем максимальное значение слайдера на 100
        slider.maxValue = 100f;
        slider.value = 0f;
        slider.gameObject.SetActive(false);
    }

    // Метод вызывается при нажатии на кнопку
    public void OnPointerDown(PointerEventData eventData)
    {
        // Если корутина уже запущена, не запускаем ее повторно
        if (fillCoroutine == null)
        {
            slider.gameObject.SetActive(true);
            fillCoroutine = StartCoroutine(FillSlider());
        }
    }

    // Метод вызывается при отпускании кнопки
    public void OnPointerUp(PointerEventData eventData)
    {
        // Останавливаем заполнение
        if (fillCoroutine != null)
        {
            slider.gameObject.SetActive(false);
            StopCoroutine(fillCoroutine);
            fillCoroutine = null;
        }
    }

    private IEnumerator FillSlider()
    {
        float elapsedTime = 0f;

        // Пока слайдер не заполнится полностью
        while (elapsedTime < fillTime)
        {
            elapsedTime += Time.deltaTime;

            // Рассчитываем новое значение слайдера
            slider.value = Mathf.Clamp(elapsedTime / fillTime * slider.maxValue, 0f, slider.maxValue);

            // Если слайдер заполнен до максимума
            if (slider.value >= slider.maxValue)
            {
                // Останавливаем корутину и деактивируем слайдер
                slider.value = 0;
                slider.gameObject.SetActive(false);
                takeConsumable.take = true;
                takeConsumable.buttonTake.SetActive(false);
                fillCoroutine = null;
                yield break;
            }

            yield return null; // Ждем следующий кадр
        }
    }
}