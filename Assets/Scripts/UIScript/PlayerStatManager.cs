using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatManager : MonoBehaviour
{
    [SerializeField] private PlayerStat baseStats;
    [SerializeField] private PlayerStat statsModifier;
    public PlayerStat curStats  { get; private set; } 
    public float expOffset;

    public List<Image> bars;
    public List<TMP_Text> texts;

    public void initStat()
    {
        curStats.HP = baseStats.HP;
        curStats.MaxHP = baseStats.MaxHP;
        curStats.Dmg = baseStats.Dmg;
        curStats.Exp = baseStats.Exp;
        curStats.Level = baseStats.Level;
        curStats.LvUpExp = baseStats.LvUpExp;
    }

    private void LevelUp()
    {
        curStats.HP += statsModifier.HP;
        curStats.MaxHP += statsModifier.MaxHP;
        curStats.Dmg += statsModifier.Dmg;
        curStats.Exp = 0 + (int)expOffset;
        curStats.Level += 1;
        curStats.LvUpExp += statsModifier.LvUpExp;
        texts[1].text = "LV."+curStats.Level.ToString();
        texts[0].text = $"{curStats.HP} / {curStats.MaxHP}";
    }

    private void UpdateBar()
    {
        bars[0].fillAmount = (float)(curStats.HP / curStats.MaxHP);
        bars[1].fillAmount = (float)(curStats.Exp / curStats.LvUpExp);
        Debug.Log("hp" + curStats.HP +"/"+ curStats.MaxHP + "=" +  bars[0].fillAmount);
        Debug.Log("Exp" + curStats.Exp + "/" + curStats.LvUpExp + "=" + bars[1].fillAmount);
        texts[0].text = $"{curStats.HP} / {curStats.MaxHP}";
    }

    private void Awake()
    {
        curStats = ScriptableObject.CreateInstance<PlayerStat>();
        texts[0].text = $"{baseStats.HP} / {baseStats.MaxHP}";
        initStat();
    }

    private void Update()
    {
        if(curStats.Exp >= curStats.LvUpExp)
        {
            expOffset = curStats.Exp - curStats.LvUpExp;
            if(expOffset < 0 ) expOffset = 0;
            LevelUp();
        }

        UpdateBar();

    }

}
