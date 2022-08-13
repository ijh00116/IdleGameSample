using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace BlackTree
{
    
    public class EnumExtention
    {
        public static int ParseToInt<T>(string uiName)where T:struct
        {
            return (int)Enum.Parse(typeof(T), uiName,true);
        }

        public static T ParseToEnum<T>(string uiName) where T : struct
        {
            return (T)Enum.Parse(typeof(T), uiName, true);
        }
    }

    public static class DebugExtention
    {
        public static void ColorLog(string color, string log)
        {
#if UNITY_EDITOR
            Debug.Log(string.Format("<color={0}>{1}</color>",color,log));
#endif
        }
    }

    public static class GameObjectExtention
    {
        public static void SetLayerInChildren(this GameObject parent, int layer)
        {
            parent.layer = layer;
            if (parent.transform.childCount <= 0)
                return;
            for(int i=0; i<parent.transform.childCount; i++ )
            {
                var obj = parent.transform.GetChild(i).gameObject;
                obj.layer = layer;
                if(obj.transform.childCount>0)
                {
                    obj.SetLayerInChildren(layer);
                }
            }
        }
    }

    public static class AnimatorExtentions
    {
        public static bool HasAnimatorParameter(this Animator self,string paramname, AnimatorControllerParameterType paramtype)
        {
            if (string.IsNullOrEmpty(paramname)) { return false; }
            AnimatorControllerParameter[] paramlist=self.parameters;
            foreach(var param in paramlist)
            {
                if (param.type == paramtype && param.name == paramname)
                    return true;
            }
            return false;
        }
    }
    public class BTLayers
    {
        public static bool LayerInLayerMask(int layer,LayerMask layermask)
        {
            if(((1<<layer)&layermask)!=0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}
