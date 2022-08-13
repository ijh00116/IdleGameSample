using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlackTree.InGame;
using Spine;
using Spine.Unity;

namespace BlackTree
{
    public class EnemySkinChanger : MonoBehaviour
    {
        Character _character;
        string Frontpath = "Spine/Monster";
        private void Awake()
        {
            _character = this.GetComponent<Character>();
        }

        private void OnEnable()
        {
            //SetSkindata();
        }
        public void SetSkindata()
        {
            List<StageGroup> stagegroupinfo = null;
            if (_character.playertype == CharacterType.NormalMonster)
            {
                stagegroupinfo = InGameDataTableManager.StageList.group.FindAll(o => o.monster_group == Common.InGameManager.Instance.GetPlayerData.stage_Info.scenario
                && o.monster_type == "normal");
            }
            else if (_character.playertype == CharacterType.Boss)
            {
                stagegroupinfo = InGameDataTableManager.StageList.group.FindAll(o => o.monster_group == Common.InGameManager.Instance.GetPlayerData.stage_Info.scenario
                && o.monster_type == "boss");
            }
            else if (_character.playertype == CharacterType.Mimic)
            {
                stagegroupinfo = InGameDataTableManager.StageList.group.FindAll(o => o.monster_group == Common.InGameManager.Instance.GetPlayerData.stage_Info.scenario
                && o.monster_type == "boss");
            }
            if (stagegroupinfo == null)
            {
                _character._state.ChangeState(eActorState.Idle);
                return;
            }

            int randomindex = Random.Range(0, stagegroupinfo.Count);

            string fullpath = string.Format("{0}/{1}/{2}", Frontpath, stagegroupinfo[randomindex].resource_id,
                stagegroupinfo[randomindex].resource_id+ "_SkeletonData");

            SkeletonDataAsset asset = Resources.Load<SkeletonDataAsset>(fullpath);
            if (asset != null)
            {
                if(_character._Skeletonanimator.skeletonDataAsset!=null)
                    _character._Skeletonanimator.skeletonDataAsset.Clear();

                _character._Skeletonanimator.skeletonDataAsset = asset;
                _character._Skeletonanimator.initialSkinName = stagegroupinfo[randomindex].skinname;


                _character._Skeletonanimator.Initialize(true);
            }
            _character._state.ChangeState(eActorState.Idle);
        }

    
    }

}
