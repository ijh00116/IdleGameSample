using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree.InGame
{
    public class CharacterPositionControll : MonoBehaviour
    {
        [SerializeField] public List<Transform> PlayerPositionList = new List<Transform>();
        [SerializeField] public List<Transform> DungeonPlayerPositionList = new List<Transform>();

        [HideInInspector] public List<Transform> CurrentPlayerPositionList = new List<Transform>();
    }

}
