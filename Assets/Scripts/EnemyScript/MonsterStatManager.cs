using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatManager : MonoBehaviour
{
    [SerializeField] private MonsterStat baseStat;
    public MonsterStat curStat { set { baseStat = value; } }
    

}
