using UnityEngine;

namespace BlackTree.Common
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T:MonoSingleton<T>
    {
        static T _instance = null;

        public static T Instance
        {
            get
            {
                if(_instance==null)
                {
                    _instance = GameObject.FindObjectOfType(typeof(T)) as T;
                    if(_instance==null)
                    {
                        var Obj = new GameObject(typeof(T).ToString());
                        _instance = Obj.AddComponent<T>();
                    }
                    else
                    {
                        _instance.Init();
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if(_instance==null)
            {
                _instance = this as T;
                _instance.Init();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            Release();
        }

        protected virtual void Init()
        {

        }

        protected virtual void Release()
        {

        }
    }

    public class Singleton<T>where T:new()
    {
        private static System.Lazy<T> lazy = new System.Lazy<T>(() => new T());

        public static T Instance { get { return lazy.Value; } }

        protected Singleton()
        {
        }

    }
}