using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform oriParent;//slot

    public void OnBeginDrag(PointerEventData eventData)
    {
        oriParent = transform.parent;
        transform.SetParent(transform.parent.parent);
        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject == null || eventData.pointerCurrentRaycast.gameObject.tag != "Slots")
        {
            transform.SetParent(oriParent);
            transform.position = oriParent.position;
        }
        else if(eventData.pointerCurrentRaycast.gameObject.name == "ItemImage")
        {
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.position;

            
            eventData.pointerCurrentRaycast.gameObject.transform.parent.position = oriParent.position;
            eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(oriParent);
        }
        else
        {
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
