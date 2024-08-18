using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    private AudioSource _bgm;
    private AudioSource _sfx;
    private GameObject rootSound;

    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        rootSound = GameObject.Find("Sound");
        _bgm = rootSound.transform.Find("Bgm").GetComponent<AudioSource>();
        _sfx = rootSound.transform.Find("Effect").GetComponent<AudioSource>();
    }

    public void BgmControl()
    {
        _bgm.volume = bgmSlider.value;
    }

    public void SfxControl()
    {
        _sfx.volume = sfxSlider.value;
    }
}
