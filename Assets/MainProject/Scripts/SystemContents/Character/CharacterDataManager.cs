using BlackTree.Common;
using BlackTree.InGame;
using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public class CharacterDataManager : MonoSingleton<CharacterDataManager>
    {
        [HideInInspector]public Data_Character PlayerCharacterdata;

        protected override void Init()
        {
            base.Init();
            
        }

        protected override void OnDestroy()
        {
            PlayerCharacterdata.ReleaseBaseData();
        }
      

        public IEnumerator LoadCharacterData()
        {
            //현재는 캐릭터가 플레이어 하나라 이렇게 하나만 단일 세팅 하지만 만약적이라든지
            //펫이라든지 캐릭터 세팅하게 되면 ingamemanager에서 캐릭터 목록 불러와서 세팅을 해주는식으로 가게 될것.

            //캐릭터 정보세팅만 따로 매니저를 두고 playerdatamodel과 별개로 두는 이유는 캐릭터 데이터는 전역으로 처리하기 싫기 때문!!
            //왜냐면 나중에 펫등의 캐릭터 정보를 개별로 세팅해줄수 도 있기 때문에...
            PlayerCharacterdata = new Data_Character();
            PlayerCharacterdata.InitBaseData();
            

            yield break;
        }
    }

}
