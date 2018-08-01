using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AudioClipContainer
{
    public string name;
    public AudioClip[] clips;
}

[RequireComponent(typeof(AudioSource))]
public class AudioClipPlayer : MonoBehaviour
{
    public AudioClipContainer[] audioContainers;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomClip()
    {
        int rand = Random.Range(0, audioContainers.Length);

        audioSource.PlayOneShot(audioContainers[rand].clips[Random.Range(0, audioContainers[rand].clips.Length)]);
    }

    public void PlayRandomClip(string name)
    {
        for (int i = 0; i < audioContainers.Length; i++)
        {
            if (audioContainers[i].name == name)
            {
                audioSource.PlayOneShot(audioContainers[i].clips[Random.Range(0, audioContainers[i].clips.Length)]);
            }
        }
    }

    public void PlayClip(string name, int index)
    {
        for (int i = 0; i < audioContainers.Length; i++)
        {
            if (audioContainers[i].name == name)
            {
                audioSource.PlayOneShot(audioContainers[i].clips[index]);
            }
        }

    }
}

