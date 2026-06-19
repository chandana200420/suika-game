using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeEffectManager : MonoBehaviour
{
    public static MergeEffectManager instance;
    public ParticleSystem mergeEffect;
    public AudioClip mergeSound;
    private AudioSource audioSource;

    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayMergeEffect(Vector3 position)
    {
        if (mergeEffect != null)
        {
            ParticleSystem effect = Instantiate(mergeEffect, position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, 2f);
        }

        if (mergeSound != null && audioSource != null)
        {
            audioSource.Stop();  // stop any previous sound (prevents overlapping)
            audioSource.PlayOneShot(mergeSound);
            StartCoroutine(StopMergeSound());
        }
    }

    private IEnumerator StopMergeSound()
    {
        yield return new WaitForSeconds(0.5f); // adjust fade time
        audioSource.Stop();
    }

}
