using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePoint : MonoBehaviour
{
    RaycastHit _hit;
    public GameObject _target;//объект, который перемещаем
    float raycastLeight = 50; // длина луча
    float SpeedRotation = 1000f; //скорость поворота
    float rotation;
    int player_money;//текущие деньги

    void Start()
    {
        player_money = SaveSystem.Global.global_money;
    }


    void Update()
    {
        float Zoom = Input.GetAxis("Mouse ScrollWheel");
        if (GlobalVar.activeBuild)
        {
            _target = GameObject.FindWithTag("CreateObj"); // цель
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit, raycastLeight))
            {
                if ((_hit.collider.name == "floor")&&(_target != null)||(_hit.collider.tag== "CompleteObj"))
                {
                    _target.transform.position = _hit.point; //перемещаем объект по сцене
                }
            }
            if (Zoom > 0) //поворот объекта по часовой
            {
                TargetRotation(_target, -SpeedRotation);
            }
            if (Zoom < 0)//поворот объекта против часовой
            {
                TargetRotation(_target, SpeedRotation);
            }
            if((Input.GetMouseButton(0))&&(player_money >= _target.GetComponent<Object>().Price)){ //построить объект
                player_money = player_money - _target.GetComponent<Object>().Price;
                SaveSystem.Global.global_money = player_money;
                GlobalVar.activeBuild = false;

                System.Array.Resize(ref GlobalVar.Objects, GlobalVar.Objects.Length + 1);
                GlobalVar.Objects[GlobalVar.sv.CountObjects] = _target;
                GlobalVar.sv.CountObjects++;
                _target = null;
                SaveFile.Saving();

                TargetRotation(null, 0);
            }
            if (Input.GetMouseButton(1)) //отмена строительста
            {
                GlobalVar.activeBuild = false;
                Destroy(_target);
            }
        }
    }

    void TargetRotation(GameObject _target,float _sr) //метод для поворота объекта
    {
        rotation = _sr * Time.deltaTime;
        _target.transform.Rotate(transform.eulerAngles.x * 0, transform.eulerAngles.y + rotation, transform.eulerAngles.z);
    }
}
