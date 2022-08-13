using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="aaa",menuName ="testtest")]
public class RewardsDatabase : ScriptableObject
{
    public Reward[] rewards;

    public int rewardsCount
    {
        get { return rewards.Length; }
    }

    public Reward GetReward(int index)
    {
        return rewards[index];
    }
}
