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
        // ������������� ������������ �������� �������� �� 100
        slider.maxValue = 100f;
        slider.value = 0f;
        slider.gameObject.SetActive(false);
    }

    // ����� ���������� ��� ������� �� ������
    public void OnPointerDown(PointerEventData eventData)
    {
        // ���� �������� ��� ��������, �� ��������� �� ��������
        if (fillCoroutine == null)
        {
            slider.gameObject.SetActive(true);
            fillCoroutine = StartCoroutine(FillSlider());
        }
    }

    // ����� ���������� ��� ���������� ������
    public void OnPointerUp(PointerEventData eventData)
    {
        // ������������� ����������
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

        // ���� ������� �� ���������� ���������
        while (elapsedTime < fillTime)
        {
            elapsedTime += Time.deltaTime;

            // ������������ ����� �������� ��������
            slider.value = Mathf.Clamp(elapsedTime / fillTime * slider.maxValue, 0f, slider.maxValue);

            // ���� ������� �������� �� ���������
            if (slider.value >= slider.maxValue)
            {
                // ������������� �������� � ������������ �������
                slider.value = 0;
                slider.gameObject.SetActive(false);
                takeConsumable.take = true;
                takeConsumable.buttonTake.SetActive(false);
                fillCoroutine = null;
                yield break;
            }

            yield return null; // ���� ��������� ����
        }
    }
}