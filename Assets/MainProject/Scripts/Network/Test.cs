using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public class Test : ICredentials
    {
        public Test(string id, CDType type)
        {
            var protocol = CreateStructure(id, type);
            Debug.Assert(protocol != null);

            this.serialized = Newtonsoft.Json.JsonConvert.SerializeObject(protocol);
            this.type = type;
        }

        public CDType type { get; private set; }
        public string serialized { get; private set; }

        private static object CreateStructure(string id, CDType type)
        {
            switch (type)
            {
                case CDType.GooglePlay:
                    return new Credential_GooglePlay()
                    {
                        acut = id,
                    };

                case CDType.IOS_GameCenter:
                    return new Credential_GameCenter()
                    {
                        acut = id,
                    };

                default:
                    return null;
            }
        }

        public void Login(System.Action<bool> onLogin)
        {
            onLogin(true);
        }

        public void Logout()
        {
            // do nothing
        }
    }

}
