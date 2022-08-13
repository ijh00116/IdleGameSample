using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public interface ILoginData
    {
        CDType type { get; }
        string serialized { get; }
    }

    public interface ICredentials : ILoginData
    {
        void Login(System.Action<bool> onLogin);
        void Logout();
    }
    public static class ILoginDataExtension
    {
        public static string Dump(this ILoginData source)
        {
            return string.Format("type : {0}, {1} / serialized : {2}", source.type, source.GetType(), source.serialized);
        }
    }

    public class None : ICredentials
    {
        public string serialized
        {
            get
            {
                return string.Empty;
            }
        }

        public CDType type
        {
            get
            {
                return (CDType)(-1);
            }
        }

        public void Login(System.Action<bool> onLogin)
        {
            onLogin(false);
        }

        public void Logout()
        {
            // do nothing
        }
    }
}
