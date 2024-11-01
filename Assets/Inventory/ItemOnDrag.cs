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
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name+ " childcount:"+eventData.pointerCurrentRaycast.gameObject.transform.childCount);
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
        else//碰到是slot了
        {
            if (eventData.pointerCurrentRaycast.gameObject.transform.GetChild(0).gameObject.activeSelf)//有物品
            {
                transform.SetParent(oriParent);
                transform.position = oriParent.position;
            }
            else
            {
                GameObject itsItem = eventData.pointerCurrentRaycast.gameObject.transform.GetChild(0).gameObject;
                itsItem.transform.SetParent(oriParent);
                itsItem.transform.position = oriParent.position;

                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
            }
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
