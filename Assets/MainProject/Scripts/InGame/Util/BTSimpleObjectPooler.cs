using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.State;
using BlackTree.Common;

namespace BlackTree.InGame
{
    public class BTSimpleObjectPooler : MonoBehaviour
    {
        public GameObject ProjectilePrefab;
        public int objectPoolMaxCount;

        protected BTObjectPool<GameObject> objectsParent;
        GameObject poolparent;
        private void Start()
        {
            FillObjects();
        }
        public void FillObjects()
        {
            if(objectsParent==null)
            {
                poolparent = new GameObject(ProjectilePrefab.name + "Objectpool");
                objectsParent = new BTObjectPool<GameObject>();
                objectsParent.PoolingObjects = new List<GameObject>();
            }
            for(int i=0; i<objectPoolMaxCount; i++)
            {
                CreateObject();
            }
        }

        public PoolableObject GetPooledObject()
        {
            PoolableObject _projectile=null;
            foreach(var obj in objectsParent.PoolingObjects)
            {
                if(obj.gameObject.activeInHierarchy==false)
                {
                    _projectile = obj.GetComponent<PoolableObject>();
                    break;
                }
            }
            _projectile.transform.position = this.transform.position;
            _projectile.gameObject.SetActive(true);

            return _projectile;
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
