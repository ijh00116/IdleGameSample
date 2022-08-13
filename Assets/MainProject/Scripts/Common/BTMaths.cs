using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace BlackTree.Tools
{
    public static class BTMaths
    {
        public static Vector3 RandomVector3(Vector3 minimum, Vector3 maximum)
        {
            return new Vector3(UnityEngine.Random.Range(minimum.x, maximum.x),
                                             UnityEngine.Random.Range(minimum.y, maximum.y),
                                             UnityEngine.Random.Range(minimum.z, maximum.z));
        }
    }

}

