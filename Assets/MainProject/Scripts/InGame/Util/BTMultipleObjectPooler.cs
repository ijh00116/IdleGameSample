using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.State;
using BlackTree.Common;

namespace BlackTree.InGame
{
    public class BTMultipleObjectPooler : MonoBehaviour
    {
        public GameObject ProjectilePrefab;
        public int objectPoolMaxCount;
        public List<Transform> PooledPosition=new List<Transform>();

        PoolableObject[] PooledObjs;

        protected BTObjectPool<GameObject> objectsParent;
        GameObject poolparent;
        private void Start()
        {
            FillObjects();
        }
        public void FillObjects()
        {
            if (objectsParent == null)
            {
                poolparent = new GameObject(ProjectilePrefab.name + "Objectpool");
                objectsParent = new BTObjectPool<GameObject>();
                objectsParent.PoolingObjects = new List<GameObject>();
            }
            for (int i = 0; i < objectPoolMaxCount; i++)
            {
                CreateObject();
            }

            PooledObjs = new PoolableObject[PooledPosition.Count];
        }

        public PoolableObject[] GetPooledObject()
        {
            for (int i = 0; i < PooledObjs.Length; i++)
                PooledObjs[i] = null;

            int index = 0;
            foreach (var obj in objectsParent.PoolingObjects)
            {
                if (obj.gameObject.activeInHierarchy == false)
                {
                    if (PooledObjs[index] == null)
                    {
                        PooledObjs[index] = obj.GetComponent<PoolableObject>();
                        index++;
                    }
                    if (index >= PooledObjs.Length)
                        break;
                }
            }
            for (int i = 0; i < PooledObjs.Length; i++)
            {
                PooledObjs[i].transform.position = PooledPosition[i].transform.position;
                PooledObjs[i].gameObject.SetActive(true);
            }
            

            return PooledObjs;
        }

        public void CreateObject()
        {
            GameObject obj = Instantiate(ProjectilePrefab);
            obj.transform.SetParent(poolparent.transform);
            obj.transform.position = this.transform.position;
            obj.SetActive(false);
            objectsParent.PoolingObjects.Add(obj);
        }
    }

}
