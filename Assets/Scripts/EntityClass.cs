using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EntityClass : MonoBehaviour
{   
}

public class Entity : ScriptableObject
{
    [SerializeField]
    private int hp;
    public int HP { get { return hp; } set { hp = value; } }
    [SerializeField]
    private int maxhp;
    public int MaxHP { get {  return maxhp; } set {  maxhp = value; } }
    [SerializeField]
    private int dmg;
    public int Dmg { get {  return dmg; } set { dmg = value; } }
}


[CreateAssetMenu(menuName = "ScriptableObjects/PlayerStats", order = 0)]
public class PlayerStat : Entity
{
    [SerializeField]
    private int exp;
    public int Exp { get { return exp; } set { exp = value; } }
    [SerializeField]
    private int level;
    public int Level { get { return level; } set { level = value; } }
    [SerializeField]
    private float lvupExp;
    public float LvUpExp { get { return lvupExp; } set { lvupExp = value; } }
    
}
