using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using BlackTree;
using BlackTree.Common;

namespace BlackTree
{
    public class TableManager : MonoSingleton<TableManager>
    {
        bool _alreadyLoading = false;
        bool _loadComplete = false;

        readonly Dictionary<System.Type, object> _tables = new Dictionary<Type, object>();

        public T GetTableClass<T>() where T:class
        {
            object table;
            if (_tables.TryGetValue(typeof(T), out table))
                return (T)table;

            return null;
        }

        public IEnumerator Load()
        {
            if (_loadComplete)
                yield break;

            if (_alreadyLoading)
            {
                while (!_loadComplete)
                    yield return null;

                yield break;
            }

            yield return ResourceLoader.Instance.Load<BT_Sound>("Tables/BT_Sound", o => {
                _tables.Add(Type.GetType("BT_Sound"), o);
            });

            _alreadyLoading = false;
            _loadComplete = true;
        }

        public void Clear()
        {
            _tables.Clear();
#if UNITY_EDITOR
            Debug.Log("Clear Tables. - " + GetInstanceID());
#endif
        }
    }
}

