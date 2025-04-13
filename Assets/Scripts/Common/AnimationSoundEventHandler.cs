using System.Collections.Generic;
using UnityEngine;

namespace ZeroStats.Common
{
    public class AnimationSoundEventHandler : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource = default!;
        [SerializeField] private List<AudioClip> clip = new();

        public void PlaySound()
        {
            audioSource.clip = clip[Random.Range(0, clip.Count)];
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();
        }
        
    }
}