using UnityEngine;
using System;
using System.Collections.Generic;

namespace BlackTree.Common
{
    // 풀에 가져올 데이터가 없으면 null 을 반환하는 형태
    public class ObjectPool<T> where T : UnityEngine.Object
    {
        public readonly Queue<T> _pool = new Queue<T>();

        public T GetObject()
        {
            T o;

            if (_pool.Count == 0)
            {
                return null;
            }
            else
            {
                o = _pool.Dequeue();
            }

            return o;
        }

        public void PoolObject(T o)
        {
            _pool.Enqueue(o);
        }

        public void ClearAll()
        {
            while (_pool.Count > 0)
            {
                var o = _pool.Dequeue();
                UnityEngine.Object.Destroy(o);
            }
        }
    }


    public class ObjectPool<TKey, TType> where TType : UnityEngine.Object
    {
        readonly Dictionary<TKey, ObjectPool<TType>> _pool = new Dictionary<TKey, ObjectPool<TType>>();


        public TType GetObject(TKey key)
        {
            ObjectPool<TType> op;
            if (_pool.TryGetValue(key, out op))
            {
                return op.GetObject();
            }

            return null;
        }

        public void PoolObject(TKey key, TType o)
        {
            ObjectPool<TType> op;
            if (!_pool.TryGetValue(key, out op))
            {
                op = new ObjectPool<TType>();
                _pool.Add(key, op);
            }

            op.PoolObject(o);
        }

        public void Clear(TKey key)
        {
            ObjectPool<TType> op;
            if (_pool.TryGetValue(key, out op))
            {
                op.ClearAll();
                _pool.Remove(key);
            }
        }

        public void ClearAll()
        {
            foreach (var op in _pool)
            {
                op.Value.ClearAll();
            }

            _pool.Clear();
        }
    }

}
