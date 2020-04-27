using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Drag2d : MonoBehaviour, IDragHandler
{

    #region IDragHandler implementation

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 calc = new Vector3(eventData.delta.x,eventData.delta.y,0);
        this.transform.position += new Vector3(calc.x,calc.y,0);
    }

    #endregion
}
