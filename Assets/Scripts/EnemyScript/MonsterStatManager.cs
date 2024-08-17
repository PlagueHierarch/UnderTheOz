using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatManager : MonoBehaviour
{
    [SerializeField] private MonsterStat baseStat;
    public MonsterStat curStat { set { baseStat = value; } }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
