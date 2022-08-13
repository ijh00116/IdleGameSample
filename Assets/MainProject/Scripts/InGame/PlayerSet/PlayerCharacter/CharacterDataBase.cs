using BlackTree.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    [CreateAssetMenu(fileName ="CharacterDataBase",menuName ="Character System/CharacterDatabase")]
    public class CharacterDataBase : ScriptableObject,ISerializationCallbackReceiver
    {
        public CharacterType monsterType;
        public CharacterObject[] Characters;
        public Dictionary<int, CharacterObject> GetCharacter = new Dictionary<int, CharacterObject>();
        public void OnAfterDeserialize()
        {
            //GetCharacter = new Dictionary<int, CharacterObject>();
            //for(int i=0; i< Characters.Length; i++)
            //{
            //    if (Characters[i] == null)
            //        continue;
            //    Characters[i].id = i;
            //    GetCharacter.Add(i, Characters[i]);
            //}
        }

        public void Loaddata()
        {
            GetCharacter = new Dictionary<int, CharacterObject>();
            for (int i = 0; i < Characters.Length; i++)
            {
                if (Characters[i] == null)
                    continue;
                //Characters[i].id = i;
                GetCharacter.Add(Characters[i].data.id, Characters[i]);
            }
        }

        public void OnBeforeSerialize()
        {
            //GetCharacter = new Dictionary<int, CharacterObject>();
        }

    }

}
