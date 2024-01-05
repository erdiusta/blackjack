using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Deal Card")]
    [SerializeField] AudioClip dealCardClip;
    [SerializeField] [Range(0f, 1f)] float dealCardVolume = 1f;

    [Header("Hit")]
    [SerializeField] AudioClip hitClip;
    [SerializeField] [Range(0f, 1f)] float hitVolume = 1f;

    [Header("Stand")]
    [SerializeField] AudioClip standClip;
    [SerializeField] [Range(0f, 1f)] float standVolume = 1f;

    [Header("Bet")]
    [SerializeField] AudioClip betClip;
    [SerializeField] [Range(0f, 1f)] float betVolume = 1f;

    [Header("RoundOVer")]
    [SerializeField] AudioClip roundOverClip;
    [SerializeField] [Range(0f, 1f)] float roundOverVolume = 1f;
   

    public static AudioManager instance;

    void Awake()
    {
        ManageSingleton();
    }

    void ManageSingleton() 
    {
        if(instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }   

    public void PlayDealCardClip() 
    {
        PlayClip(dealCardClip, dealCardVolume);
    }

    public void PlayHitClip()
    {
        PlayClip(hitClip, hitVolume);
    }

    public void PlayStandClip()
    {
        PlayClip(standClip, standVolume);
    }

    public void PlayBetClip()
    {
        PlayClip(betClip, betVolume);
    }
    public void PlayRoundOverClip()
    {
        PlayClip(roundOverClip, roundOverVolume);
    }
    void PlayClip(AudioClip clip, float volume)
    {
        if(clip != null)
        {
            Vector3 cameraPos = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(clip, cameraPos, volume);
        }
    }
}
