using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/MonsterStat", order = 1)]
public class MonsterStat : Entity
{
    [SerializeField]
    private int giveExp;
    public int GiveExp { get { return giveExp; } set { giveExp = value; } }
}
