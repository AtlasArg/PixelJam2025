using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlimeData
{
    public float attackRate;
    public Vector3 scale;
    public float moveSpeed;
    public float jumpForce;
}

[CreateAssetMenu(fileName = "MonsterDataPerState", menuName = "Scriptable Objects/MonsterDataPerState")]
public class MonsterDataPerState : ScriptableObject
{
    [SerializeField] private List<SlimeData> SlimeData;

    public Dictionary<int, SlimeData> GetSlimeDataPerLevel()
    {
        Dictionary<int, SlimeData> slimeDataPerLevel = new Dictionary<int, SlimeData>();
        int i = 1;
        foreach (SlimeData data in SlimeData)
        {
            slimeDataPerLevel.Add(i, data);
            i++;
        }

        return slimeDataPerLevel;
    }
}
