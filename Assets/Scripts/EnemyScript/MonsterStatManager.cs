using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatManager : MonoBehaviour
{
    [SerializeField] private MonsterStat baseStat;
    public MonsterStat curStat { get; private set; }

    private void StatInit()
    {
        curStat.HP = baseStat.HP;
        curStat.MaxHP = baseStat.MaxHP;
        curStat.Dmg = baseStat.Dmg;
        curStat.GiveExp = baseStat.GiveExp;
    }
    private void Awake()
    {
        curStat = ScriptableObject.CreateInstance<MonsterStat>();
        StatInit();
    }
    private void Update()
    {
        if(curStat.HP <= 0)
        {
            GameManager.instance.monsters.RemoveAt(0);
            Destroy(gameObject);
            PlayerStatManager.instance.GetExp(curStat.GiveExp);
        }
       // if(curStat.HP != baseStat.HP) { StatInit(); }
    }

    public void GetDamaged(int dmg)
    {
        curStat.HP -= dmg;
        Debug.Log(curStat.HP);
    }
}
