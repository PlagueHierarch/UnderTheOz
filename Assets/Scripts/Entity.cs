using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

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