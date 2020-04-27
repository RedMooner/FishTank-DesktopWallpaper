using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePanelMouse : MonoBehaviour
{
  public float distance = 10f;
  public Camera cam;

void OnMouseDrag()
  {
      Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance); // переменной записываються координаты мыши по иксу и игрику
      Vector3 objPosition = cam.ScreenToWorldPoint(mousePosition); // переменной - объекту присваиваеться переменная с координатами мыши
      transform.position = objPosition; // и собственно объекту записываються координаты
  }
}
