using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource ambiantSound, battleSound;
    [SerializeField] float fadeDuration = 2f;
    [SerializeField] AudioClip punchClip;
    // Start is called before the first frame update
    
    public void MusicChange(bool menu)
    {
        if (menu)
        ambiantSound.Play();

        StartCoroutine(menu ? FadeSound(ambiantSound, battleSound) : FadeSound(battleSound, ambiantSound));
    }

    IEnumerator FadeSound(AudioSource toStop, AudioSource toPlay)
    {
        toPlay.Play();

        float volume1 = toStop.volume;
        float volume2 = toPlay.volume;
        float t = 0;

        while(t < fadeDuration)
        {
            //LERP
            toStop.volume = Mathf.Lerp(volume1,0, t/fadeDuration);
            toPlay.volume = Mathf.Lerp(volume2,1, t/fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        
        toStop.volume = 0;
        toPlay.volume = 1;
        toStop.Stop();
    }

    public void PunchSound()
    {
        GameObject punchSound = new GameObject("punchSound");
        AudioSource audioSource = punchSound.AddComponent<AudioSource>();
        audioSource.clip = punchClip;
        audioSource.Play();
        Destroy(punchSound, punchClip.length);
    }
}
