using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.State;
using BlackTree.Common;

namespace BlackTree.InGame
{
    public class BTObjectPool<T>
    {
        [BTReadOnly]
        public List<T> PoolingObjects;
    }

}
