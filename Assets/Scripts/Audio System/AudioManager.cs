using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			//DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	private void Update()
	{
		//Pause sounds if gameplay is paused
		if (GameManager.instance.gameState == GameManager.GameState.Paused)
		{
			for (int i = 0; i < sounds.Length; i++)
			{
				sounds[i].source.Pause();
			}
		}
		//Resume sounds if gameplay is unpaused
		else if (GameManager.instance.gameState == GameManager.GameState.Gameplay)
		{
            for (int i = 0; i < sounds.Length; i++)
            {
                sounds[i].source.UnPause();
            }
        }
		//If any other gameState, stop all sounds
		else
		{
            for (int i = 0; i < sounds.Length; i++)
            {
                sounds[i].source.Stop();
            }
        }

		//Stop processing if not in gameplay
		if (GameManager.instance.gameState != GameManager.GameState.Gameplay)
			return;

		//If player is not interacting, stop all interacting sounds
		if (!Input.GetMouseButton(0))
		{
			Stop("Water Pour");
		}
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

    public void ExclusivePlay(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

		//If the sound is already playing, don't play again
		if (s.source.isPlaying)
			return;

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

		s.source.Stop();
    }
}
