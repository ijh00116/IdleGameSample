using BlackTree.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    //캐릭터의 데이터를 가지고있는 클래스 데이터베이스에서 이 데이터를 가지고 있다.->클라이언트에서 저장할것(사용자의 서버에서 저장 안해도 됨-로컬 클라이언트에서 정보 가지고있음)
    [CreateAssetMenu(fileName = "characters", menuName = "Character System/characters")]
    public class CharacterObject : ScriptableObject
    {
        public int UpgradeId=-1;
        public GameObject CharacterPrefab;
        public CharacterType type;
        public Characterdata data;

        [TextArea(15, 20)]
        public string description;
        public Characterdata CreateCharacter()
        {
            Characterdata newcharacter = new Characterdata(this);
            return newcharacter;
        }
    }

    //캐릭터셋이나 세이브시에 저장되는 캐릭터의 가장 간단한 변수(id값으로 characterobject데이터를 찾아서 사용하게 된다)
    //서버에서 저장되야만 하는 변수
    [System.Serializable]
    public class Characterdata
    {
        public string Name;
        public int id;
        public int Level;
        public Characterdata(CharacterObject character)
        {
            Name = character.name;
            id = character.data.id;
            Level = character.data.Level;
        }
    }

}

