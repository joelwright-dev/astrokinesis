using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private float randomSoundCooldown = 1f;
    private Dictionary<string, List<AudioSource>> activeSoundEffects = new Dictionary<string, List<AudioSource>>();
    private bool isRandomSoundOnCooldown = false;


    private void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume) {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        if(!activeSoundEffects.ContainsKey(audioClip.name)) {
            activeSoundEffects[audioClip.name] = new List<AudioSource>();
        }

        activeSoundEffects[audioClip.name].Add(audioSource);

        StartCoroutine(RemoveSoundEffectAfterPlay(audioSource, audioClip.name));
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioArray, Transform spawnTransform, float volume) {
        if(isRandomSoundOnCooldown) {
            return;
        }
        
        int rand = Random.Range(0, audioArray.Length);

        AudioClip audioClip = audioArray[rand];
        
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        if(!activeSoundEffects.ContainsKey(audioClip.name)) {
            activeSoundEffects[audioClip.name] = new List<AudioSource>();
        }

        activeSoundEffects[audioClip.name].Add(audioSource);

        StartCoroutine(RemoveSoundEffectAfterPlay(audioSource, audioClip.name));
        StartCoroutine(RandomSoundCooldown());
    }

    private IEnumerator RandomSoundCooldown()
    {
        isRandomSoundOnCooldown = true;
        yield return new WaitForSeconds(randomSoundCooldown);
        isRandomSoundOnCooldown = false;
    }

    private IEnumerator RemoveSoundEffectAfterPlay(AudioSource audioSource, string clipName) {
        yield return new WaitForSeconds(audioSource.clip.length);
        
        if(activeSoundEffects.ContainsKey(clipName)) {
            activeSoundEffects[clipName].Remove(audioSource);
            if(activeSoundEffects[clipName].Count == 0) {
                activeSoundEffects.Remove(clipName);
            }
        }
        
        Destroy(audioSource.gameObject);
    }

    public void StopSoundFX(string clipName)
    {
        if (activeSoundEffects.ContainsKey(clipName))
        {
            foreach (AudioSource audioSource in activeSoundEffects[clipName])
            {
                audioSource.Stop();
                Destroy(audioSource.gameObject);
            }
            activeSoundEffects.Remove(clipName);
        }
    }

    public void StopAllSoundFX()
    {
        foreach (var kvp in activeSoundEffects)
        {
            foreach (AudioSource audioSource in kvp.Value)
            {
                audioSource.Stop();
                Destroy(audioSource.gameObject);
            }
        }
        activeSoundEffects.Clear();
    }
}
