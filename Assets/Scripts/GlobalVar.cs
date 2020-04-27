using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVar : MonoBehaviour
{
    public  GameObject[] AllObjects = new GameObject[1]; //все декорации и другие объекты для размещения
    public static GameObject[] Objects = new GameObject[1];

    public GameObject StartLimit, EndLimit; //ограничиваем площадь с помощью кубов
    public GameObject[] AllFish; //здесь хранятся все рыбы, которые есть в проекте (строго по ID)
    public static bool livePaper = false;
    public static float oxygen = 0;// где 0 - это 100% кислорода в аквариуме
    public static double Pollution = 50; // где 100 - это 100% загрязнения

    public static GameObject buildObject; //объект, который размещаем
    public static bool activeBuild; //режим строительства
    public static bool onTriggerObj = false; //проверка на пересечение объектов по коллайдеру (в текущий момент не используется)

    public static string OffWalling = "";
    public static string TrayLog = "";

    public static Vector3 StartLimiterPos, EndLimiterPos; //ограничение пространства

    public static string path; // путь для сохранения файла
    public static Save sv = new Save(); //класс сохранения

    public static GameObject[] Fish = new GameObject[1]; //здесь хранятся все рыбы, которые есть на сцене


    void Start()
    {
        StartLimiterPos = StartLimit.transform.position;
        EndLimiterPos = EndLimit.transform.position;
        StartCoroutine(OxegenMin());
        StartCoroutine(Pollytions());
    }


    void Update()
    {

    }

    IEnumerator OxegenMin() //каждую минуту кислород отнимается на 5%
    {
        while (true)
        {
            yield return new WaitForSeconds(60);
            GlobalVar.oxygen += 5;
        }
    }
    IEnumerator Pollytions() //каждую минуту аквариум загрязняется на 5%
    {
        while (true)
        {
            yield return new WaitForSeconds(60);
            GlobalVar.Pollution += 5;
        }
    }
}
