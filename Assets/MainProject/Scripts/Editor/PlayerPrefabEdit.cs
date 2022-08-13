using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class PlayerPrefabEdit : MonoBehaviour
{
    [MenuItem("PlayerPrefab/Delete Allprefabs")]
    public static void DeleteAllPrefab()
    {
        PlayerPrefs.DeleteAll();
    }

}

