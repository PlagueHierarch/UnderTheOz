using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/PlayerStat", order = 0)]
public class PlayerStat : Entity
{
    [SerializeField]
    private int exp;
    public int Exp { get { return exp; } set { exp = value; } }
    [SerializeField]
    private int level;
    public int Level { get { return level; } set { level = value; } }
    [SerializeField]
    private int lvupExp;
    public int LvUpExp { get { return lvupExp; } set { lvupExp = value; } }

}