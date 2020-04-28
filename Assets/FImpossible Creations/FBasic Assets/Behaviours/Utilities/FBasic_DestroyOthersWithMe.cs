using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.Basics
{
    /// <summary>
    /// FM: Basic class, destroys target objects when base gameOBject is destroyed.
    /// Good for objects which can't be parented in one transform but are related to each other.
    /// </summary>
    public class FBasic_DestroyOthersWithMe : MonoBehaviour
    {
        private List<GameObject> ToDestroyAfterDestroy;

        public void AddToDestroy(GameObject obj)
        {
            if (ToDestroyAfterDestroy == null) ToDestroyAfterDestroy = new List<GameObject>();

            ToDestroyAfterDestroy.Add(obj);
        }

        void OnDestroy()
        {
            for (int i = 0; i < ToDestroyAfterDestroy.Count; i++)
            {
                if (ToDestroyAfterDestroy[i] != null)
                {
                    GameObject.Destroy(ToDestroyAfterDestroy[i]);
                }
            }
        }
    }
}
