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

    public RectTransform movingObject; // UI объект, который будем двигать
    public float moveSpeed = 10f; // Скорость перемещения

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
        // Находим центр канваса в мировых координатах
        Vector3 canvasCenter = GetCanvasCenter();

        qwe(false);

        // Цикл продолжается, пока объект не достигнет центра канваса
        while (Vector3.Distance(movingObject.position, canvasCenter) > 0.01f)
        {
            // Линейно интерполируем позицию объекта по направлению к центру канваса
            movingObject.position = Vector3.MoveTowards(movingObject.position, canvasCenter, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject.GetComponent<Canvas>());

        // После перемещения можно выполнить дополнительные действия, если нужно
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

    // Метод для получения центра канваса в мировых координатах
    private Vector3 GetCanvasCenter()
    {
        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();

        // Находим центр канваса в локальных координатах, затем преобразуем их в мировые
        Vector3 canvasCenterLocal = new Vector3(0, 0, 0); // Центр канваса
        return canvasRectTransform.TransformPoint(canvasCenterLocal);
    }
}