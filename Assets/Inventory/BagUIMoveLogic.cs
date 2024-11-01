using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class BagUIMoveLogic : MonoBehaviour,IDragHandler
{

    public RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rt.anchoredPosition += eventData.delta;
    }
}
