using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Timers;

public class FishBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    [Header("Состояние рыбы")]
    public int health = 100;
    public int food = 100;
    public float oxygen = 100;
    public double DeathCount = 0;
    public string name;

    [Header("Настройки рыбы")]
    public Vector3 MaxScale;
    public float SpeedHeight;

    [Header("Глубина рыбы")]
    public float MinDepth; // минимальная глубина для оживление рыбы
    public float MaxDepth; //максимальная глубина для оживление рыбы
    public float SpeedSinkin; //скорость ухода рыбы на глубину

    [HideInInspector]
    public int IDMassiv; // ID в массиве

    [HideInInspector]
    public bool active = false;
    [HideInInspector]
    [SerializeField] private int food_updater = 10;
    [SerializeField]
    [HideInInspector]
    private int food_destroy_count = 5;
    private float randDepth;
//    [HideInInspector]
    public bool StartFish;
    // Update is called once per frame
    void Start()
    {
      if(StartFish){
        randDepth = Random.Range(MinDepth,MaxDepth);
      }
        StartCoroutine(FoodCounter());
        StartCoroutine(UpdateHealth());
    }
    void OnMouseDown()
    {
        active = true;
        Debug.Log("Fish:" + name + "food  is " + food);
    }
    public void Get()
    {

    }
    // TODO: Переделать 42 , 48 итп на метод Kill()
    void Update()
    {
      Height();
      OnScriptFlock();
        if (DeathCount >= 100)
        {
            Debug.Log("Рыба умерла от болезни!");
            Sortirovka();
            Destroy(gameObject);
        }
        if (food <= 0)
        {
            Debug.Log("Рыба умерла от недостатка еды!");
            Sortirovka();
            Destroy(gameObject);
        }
    }
    void OnScriptFlock(){ //включаем скрипт на определенной глубине
      if(StartFish){
      if(gameObject.transform.position.y > randDepth){
        gameObject.transform.position -= new Vector3(0,SpeedSinkin * Time.deltaTime,0);
      }else{
        gameObject.GetComponent<flock>().enabled = true;
        //StartFish = false;
      }
    }
    }

    public void Height(){ //в этом методе, наша рыбка растет
      if(gameObject.transform.localScale.x < MaxScale.x){
        gameObject.transform.localScale += new Vector3(SpeedHeight * Time.deltaTime,0,0);
      }
      if(gameObject.transform.localScale.y < MaxScale.y){
        gameObject.transform.localScale += new Vector3(0,SpeedHeight * Time.deltaTime,0);
      }
      if(gameObject.transform.localScale.z > MaxScale.z){
        gameObject.transform.localScale -= new Vector3(0,0,SpeedHeight * Time.deltaTime);
      }
    }
    public void Kill()
    { // Убъект рыбу
        Sortirovka();
        Destroy(gameObject);
    }

    public void Sortirovka()
    {
        GlobalVar.sv.CountFish -= 1;
        for (int i = IDMassiv; i < GlobalVar.sv.CountFish - 1; i++)
        {
            GlobalVar.Fish[i] = GlobalVar.Fish[i + 1];
        }
    }

    IEnumerator FoodCounter()
    {
        while (true)
        {
            food = food - food_destroy_count;
            yield return new WaitForSeconds(food_updater);
        }
    }
    IEnumerator UpdateHealth() //каждую секунду будет прибавлять болезнь в зависимости от ситуации
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (GlobalVar.Pollution > 0)
            {
                DeathCount += 0.005 * GlobalVar.Pollution;
            }
            if (GlobalVar.oxygen > 0)
            {
                DeathCount += 0.005 * GlobalVar.oxygen;
            }
        }
    }
}
