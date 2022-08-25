using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip punchClip;

    [SerializeField] AudioSource ambientSound, battleSound;

    [SerializeField] private float fadeDuration = 2f;

    [SerializeField] private AnimationCurve animCurve;


    #region -----AmbientSound------
    // Start is called before the first frame update
    void ChangerMusique(bool battle)
    {
        
            StartCoroutine(battle ? FadeSound(ambientSound, battleSound) : FadeSound(battleSound, ambientSound));

    }

    IEnumerator FadeSound(AudioSource toStop, AudioSource toPlay)
    {
        toPlay.Play();

        float volume1 = toStop.volume;
        float volume2 = toPlay.volume;

        float t = 0;

        while (t < fadeDuration)
        {
            //Lerp
            toStop.volume = Mathf.Lerp(volume1, 0, t / fadeDuration);
            toPlay.volume = Mathf.Lerp(volume2, 1, t / fadeDuration);


            t += Time.deltaTime;
            yield return null;

        }

        toStop.volume = 0;
        toPlay.volume = 1;

        toStop.Stop();


    }
    
    #endregion
      

    public void PlayPunchSound()
    {

        GameObject punchSound = new GameObject("punchSound");
        AudioSource audioSource = punchSound.AddComponent<AudioSource>();
        audioSource.clip = punchClip;
        audioSource.Play();

        Destroy(punchSound, punchClip.length);

    }
    

}
