
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flock : MonoBehaviour
{
    // Start is called before the first frame update
   // public GameObject StartTarget,EndTarget;
    public float speed = 0.001f;
    public int ID;
    float rotationSpeed = 3.0f;
    float minSpeed = 0.5f; // 0.8
    float maxSpeed = 2.0f; // 2.0
    Vector3 averageHeading;
    Vector3 averagePosition;
    float neighbourDistance = 3.0f;
    public Vector3 newGoalPos;
    [SerializeField] private bool relaxMode = false;
    bool turning = false;

    private float timer;
    private bool Relaxation = false;
    private float TekSpeed = 0;


    bool rotate = true;

    void Start()
    {
        if(gameObject.GetComponent<FishBehavior>().StartFish){
          StartCoroutine(RotatiFishStart());
        }
        speed = Random.Range(minSpeed, maxSpeed);
        timer = Random.Range(5, 16);
        TekSpeed = speed;
        //   this.GetComponent<Animation>()["Motion"].speed = speed;

    }
    void OnTriggerEnter(Collider other)
    {
        if (!turning)
        {
            newGoalPos = this.transform.position - other.gameObject.transform.position;
        }
        turning = true;
    }
    void OnTriggerExit(Collider other)
    {
        //        Debug.Log("Trg is exit");
        turning = false;
    }
    // Update is called once per frame



    void Update()
    {

        Relaxations(); //метод отдых для рыб
        CheckPosition(); //ограничение рыбы в пространстве

          if (Vector3.Distance(transform.position, Vector3.zero) >= globalFlock.tankSize)
          {
             turning = true;
          }
         else
             turning = false;
        // TODO Отвых рыбаам'


        //end
        if (turning)
        {
            Vector3 direction = newGoalPos - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(direction),
            rotationSpeed * Time.deltaTime);
            //speed = Random.Range(minSpeed, maxSpeed);
            //this.GetComponent<Animation>()["Motion"].speed = speed;
        }
        else
        {
            if (Random.Range(0, 10) < 1)
                ApplyRules();
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void CheckPosition()
    {

            if (gameObject.transform.position.y <= GlobalVar.StartLimiterPos.y) //если рыба находится ниже аквариума, то...
            {
                //gameObject.transform.Rotate(0, 0, 180);
            }
            else if (gameObject.transform.position.y >= GlobalVar.EndLimiterPos.y) //если рыба находится выше аквариума, то...
            {
                 //gameObject.transform.Rotate(0, 0, 180);
            }
            else if (gameObject.transform.position.x <= GlobalVar.StartLimiterPos.x) //если рыба выплывает за аквариум по x, то...
            {
                 //gameObject.transform.Rotate(0, 180, 0);
            }
            else if (gameObject.transform.position.x >= GlobalVar.EndLimiterPos.x) //если рыба выплывает за аквариум по x, то...
            {
                 //gameObject.transform.Rotate(0, 180, 0);
            }
            else if (gameObject.transform.position.z <= GlobalVar.StartLimiterPos.z) //если рыба выплывает за аквариум по z, то...
            {
                 //gameObject.transform.Rotate(180, 0, 0);
            }
            else if (gameObject.transform.position.z >= GlobalVar.EndLimiterPos.z) //если рыба выплывает за аквариум по z, то...
            {
                 //gameObject.transform.Rotate(180, 0, 0);
            }
    }



    void Relaxations()
    {
        if (!Relaxation) //Если рыба находится в активном режиме
        {
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                if (TekSpeed / 2 <= speed)
                {
                    speed -= 0.001f;
                }
                else
                {
                    timer = Random.Range(10, 20); //задаем время пасивного режима
                    TekSpeed = speed;
                    Relaxation = true;
                }

            }
        }
        else //Если рыба находится в пасивном режиме
        {
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                if (TekSpeed * 2 >= speed)
                {
                    speed += 0.001f;
                }
                else
                {
                    timer = Random.Range(5, 10); //задаем время активного режима
                    TekSpeed = speed;
                    Relaxation = false;
                }
            }
        }
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = GlobalVar.Fish;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.1f;

        Vector3 goalPos = globalFlock.goalPos;

        float dist;

        int groupSize = 0;
        foreach (GameObject go in gos)
        {
            if ((go != this.gameObject)&&(go != null))
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if (dist <= neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    if (dist < 2.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }

                    flock anotherFlock = go.GetComponent<flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
        if (groupSize > 0)
        {
            vcentre = vcentre / groupSize + (goalPos - this.transform.position);
           // speed = gSpeed / groupSize;
            //  this.GetComponent<Animation>()["Motion"].speed = speed;
            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction),
                rotationSpeed * Time.deltaTime);
        }
    }

    IEnumerator RotatiFishStart()
    {
          turning = true;
          yield return new WaitForSeconds(50);
          gameObject.GetComponent<FishBehavior>().StartFish = false;
          turning = false;
    }
}
