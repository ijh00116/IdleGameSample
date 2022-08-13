using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public interface IOnOffValue
    {
        bool enable { get; set; }
    }

    public class UIOptionToggleWithValue
    {

    }

    public class BoolPref : IOnOffValue
    {
        private const int FALSE = 0;
        private const int TRUE = 1;

        private readonly string key;
        private bool cache;
        private int defaultValue;

        public BoolPref(string key)
            : this(key, false) { }

        public BoolPref(string key, bool defaultValue)
        {
            this.key = key;
            this.defaultValue = ToInt(defaultValue);
            this.cache = rawValue;
        }

        public bool enable
        {
            get
            {
                return cache;
            }
            set
            {
                var asInt = ToInt(value);
                PlayerPrefs.SetInt(key, asInt);
                PlayerPrefs.Save();
                cache = value;
            }
        }

        public bool rawValue
        {
            get
            {
                var value = PlayerPrefs.GetInt(key, defaultValue);
                return ToBool(value);
            }
        }

        private static int ToInt(bool value)
        {
            if (value == false)
            {
                return FALSE;
            }
            else
            {
                return TRUE;
            }
        }

        private static bool ToBool(int value)
        {
            return value != FALSE;
        }
    }

}
