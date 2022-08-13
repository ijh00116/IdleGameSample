using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.Common;
using BayatGames.SaveGameFree.Serializers;
using BayatGames.SaveGameFree;

namespace BlackTree
{
    public class Data_local : Singleton<Data_local>
    {
        public readonly BoolPref agreeTerms = new BoolPref("AgreeTerms");



        private readonly ISaveGameSerializer _serializer = new SaveGameBinarySerializer();

        public void SaveLogin(ICredentials credentials)
        {
            var data = ToLoginData(credentials);
            SaveGame.Save("LoginData", data, _serializer);
        }

        public Data_login LoadLogin()
        {
            return SaveGame.Load("LoginData", new Data_login(), _serializer);
        }

        Data_login ToLoginData(ILoginData credentials)
        {
            if (credentials == null)
            {
                return new Data_login();
            }
            else
            {
                return new Data_login()
                {
                    type = credentials.type,
                    loginId = credentials.serialized,
                };
            }
        }

        public void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }
    }



    [System.Serializable]
    public class Data_login
    {
        public CDType type;
        public string loginId;

        public Data_login()
        {
            type = (CDType)(-1);
            loginId = string.Empty;
        }
    }

}
