using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LiveWallpaperCore
{
    public class DecorCondition : MonoBehaviour
    {
        // Start is called before the first frame update
        public bool EditMode = false;
        private Vector3 mOffset;
        private float mZCoord;
        //public WallpaperManager manager;
        public GameObject Manager;
        public Decoration decoration;

        [SerializeField] private int rotationDeg = 10;
        public void Sell()
        {
            int price = decoration.price;
            price = price / 2;
            int player_money = SaveSystem.Global.global_money;
            Debug.Log("Продано: " + decoration.name);
            player_money = player_money + price;
            SaveSystem.Global.global_money = player_money;
            Destroy(this.gameObject);
            EditMode = false;
        }
        void Awake()
        {
            Manager = GameObject.FindGameObjectWithTag("manager");
        }
        void Update()
        {

            if (Manager.GetComponent<WallpaperManager>().flag == false)
            {
                if (EditMode == true)
                {
                    if (Input.GetKeyDown(KeyCode.Delete))
                        Sell();
                    float mw = Input.GetAxis("Mouse ScrollWheel");
                    if (mw > 0.1)
                    {
                        gameObject.transform.Rotate(new Vector3(0, gameObject.transform.rotation.y + rotationDeg, 0));
                    }
                    if (mw < -0.1)
                    {
                        gameObject.transform.Rotate(new Vector3(0, gameObject.transform.rotation.y - rotationDeg, 0));
                    }
                }

            }


        }
        void OnMouseDown()
        {
            // Decoration
            GameObject[] decorations = GameObject.FindGameObjectsWithTag("Decoration");
            foreach (GameObject respawn in decorations)
            {
                respawn.GetComponent<DecorCondition>().EditMode = false;
            }
            EditMode = true;
            Debug.Log("Dragging...");
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            mOffset = gameObject.transform.position - GetMouseWorldPos();
        }
        void OnMouseUp()
        {
            // If your mouse hovers over the GameObject with the script attached, output this message
            Debug.Log("Drag ended!");
            // EditMode = false;
        }
        private Vector3 GetMouseWorldPos()
        {
            Vector3 mousePoint = Input.mousePosition;

            mousePoint.z = mZCoord;
            return Camera.main.ScreenToWorldPoint(mousePoint);
        }
        void OnMouseDrag()
        {
            if (Manager.GetComponent<WallpaperManager>().flag == false)
            {
                if (EditMode)
                {
                    transform.position = GetMouseWorldPos() + mOffset;
                }
            }
        }

    }
}