using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CauldronItem : MonoBehaviour, /*IBeginDragHandler, IDragHandler, IEndDragHandler,*/ IPointerClickHandler
{
    public Image imageSprite;
    public TMP_Text textCount;
    public ConsumableScripteble consumableScripteble;
    public Dictionary<ConsumableScripteble, int> itemDictionary = new Dictionary<ConsumableScripteble, int>();

    private RectTransform rectTransform;
    private Canvas canvas;

    private Vector2 initialPosition;

    public RectTransform movingObject; // UI ������, ������� ����� �������
    public float moveSpeed = 10f; // �������� �����������

    Canvas thisCanvas;

    public bool wwww = true;

    private void Start()
    {
        movingObject = GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        initialPosition = rectTransform.localPosition;
    }

    private float CanvasScaleFactor()
    {
        return canvas.scaleFactor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (wwww == true)
        {
            thisCanvas = gameObject.AddComponent<Canvas>();
            thisCanvas.overrideSorting = true;
            thisCanvas.sortingOrder = 50;

            StartCoroutine(MoveObjectToCenter());
        }
    }

    private IEnumerator MoveObjectToCenter()
    {
        // ������� ����� ������� � ������� �����������
        Vector3 canvasCenter = GetCanvasCenter();

        qwe(false);

        // ���� ������������, ���� ������ �� ��������� ������ �������
        while (Vector3.Distance(movingObject.position, canvasCenter) > 0.01f)
        {
            // ������� ������������� ������� ������� �� ����������� � ������ �������
            movingObject.position = Vector3.MoveTowards(movingObject.position, canvasCenter, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject.GetComponent<Canvas>());

        // ����� ����������� ����� ��������� �������������� ��������, ���� �����
        PanelManager.InstancePanel.cauldronManager.animator.Play("AnimaCauldron");
        if (PanelManager.InstancePanel.cauldronManager.consumableScripteble.Count > 4)
        {
            PanelManager.InstancePanel.cauldronManager.animator.enabled = false;
            PanelManager.InstancePanel.cauldronManager.consumableScripteble.Clear();
            PanelManager.InstancePanel.cauldronManager.animator.enabled = true;
        }
        PanelManager.InstancePanel.cauldronManager.horizontalLayoutGroup.enabled = false;
        PanelManager.InstancePanel.cauldronManager.horizontalLayoutGroup.enabled = true;
        PanelManager.InstancePanel.cauldronManager.consumableScripteble.Add(consumableScripteble);
        DataManger.InstanceData.RemoveItem(consumableScripteble);
        PanelManager.InstancePanel.cauldronManager.CheckPoition();
        PanelManager.InstancePanel.cauldronManager.LoadItemsUI();

        qwe(true);
    }

    public void qwe(bool isq = false)
    {
        if (isq == false)
        {
            foreach (Transform child in PanelManager.InstancePanel.content)
            {
                child.GetComponent<CauldronItem>().wwww = false;
                //child.GetComponent<CanvasRenderer>().cullTransparentMesh = false;
                //child.GetComponent<Image>().raycastTarget = false;
                //child.GetComponent<Button>().enabled = false;
            }
        }
        else
        {
            foreach (Transform child in PanelManager.InstancePanel.content)
            {
                child.GetComponent<CauldronItem>().wwww = true;
                //child.GetComponent<CanvasRenderer>().cullTransparentMesh = true;
                //child.GetComponent<Image>().raycastTarget = true;
                //child.GetComponent<Button>().enabled = true;
            }
        }
    }

    // ����� ��� ��������� ������ ������� � ������� �����������
    private Vector3 GetCanvasCenter()
    {
        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();

        // ������� ����� ������� � ��������� �����������, ����� ����������� �� � �������
        Vector3 canvasCenterLocal = new Vector3(0, 0, 0); // ����� �������
        return canvasRectTransform.TransformPoint(canvasCenterLocal);
    }
}