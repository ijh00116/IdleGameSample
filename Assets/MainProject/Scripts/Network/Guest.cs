using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public class Guest : ICredentials
    {
        public Guest(string serialized)
        {
            if (string.IsNullOrEmpty(serialized) == true)
            {
                serialized = CreateNewGuest();
            }

            this.serialized = serialized;
        }

        public void Login(System.Action<bool> onLogin)
        {
            onLogin(true);
        }

        public void Logout()
        {
            // do nothing;
        }

        private static string CreateNewGuest()
        {
            var obj = new Credential_Guest()
            {
                acut = System.Guid.NewGuid().ToString(),
                //피씨버전 테스트로 임시계정 발금
                //acut = "임시계정" + Random.Range(0, 1000).ToString(),
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public CDType type
        {
            get
            {
                return CDType.Guest;
            }
        }

        public string serialized { get; private set; }
    }
}