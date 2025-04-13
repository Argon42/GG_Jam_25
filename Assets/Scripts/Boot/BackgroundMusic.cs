using System;
using UnityEngine;

namespace ZeroStats
{
    public class BackgroundMusic : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource = default!;
        private static BackgroundMusic _instance = default!;

        private void Start()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            _instance = this;
            _audioSource.Play();
        }
    }
}