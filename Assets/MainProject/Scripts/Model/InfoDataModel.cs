using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BlackTree.Model
{
    [CreateAssetMenu(fileName = "InformationData", menuName = "GameData/InformationData")]
    public class InfoDataModel : ScriptableObject
    {
        public int MaxChapter;
        public int MaxStage;
        public int MaxRound;

        public int AllKillMonsterNum;
        public int TotalPlayTime;
    }

}
