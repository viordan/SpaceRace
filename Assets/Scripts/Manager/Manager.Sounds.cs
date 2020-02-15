using System.Collections;
using UnityEngine;

public partial class Manager : MonoBehaviour {
	// define the audio clips
	[Header("Sounds")]
	public AudioClip clippop;

	public AudioSource[] popSounds;
	AudioSource pop1;
	AudioSource pop2;
	AudioSource pop3;
	AudioSource pop4;
	AudioSource pop5;

	public AudioClip musicClip;
	AudioSource musicSource;
	void InitilizeSounds() {
		//setup sounds
		pop1 = AddAudio(clippop, true, 1f, .4f, false);
		pop2 = AddAudio(clippop, true, 1f, .5f, false);
		pop3 = AddAudio(clippop, true, 1f, .6f, false);
		pop4 = AddAudio(clippop, true, 1f, .7f, false);
		pop5 = AddAudio(clippop, true, 1f, .8f, false);

		musicSource = AddAudio(musicClip, true, 1f, 1f, true);
		popSounds = new AudioSource[] { pop1, pop2, pop3, pop4, pop5 };
		if (settingsDataInMemory.music) musicSource.Play();
	}

	AudioSource AddAudio(AudioClip clip, bool playAwake, float vol, float _pitch, bool _loop) {
		AudioSource newAudio = gameObject.AddComponent<AudioSource>();
		newAudio.clip = clip;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol;
		newAudio.pitch = _pitch;
		newAudio.loop = _loop;
		return newAudio;
	}

	public void ToggleMusic() {
		settingsDataInMemory.music = !settingsDataInMemory.music;
		if (settingsDataInMemory.music) musicSource.Play(); else musicSource.Stop();
	}

	public void ToggleSounds() {
		settingsDataInMemory.soundEffects = !settingsDataInMemory.soundEffects;
	}

	public void PlaySound() {
		if (settingsDataInMemory.soundEffects) popSounds[Random.Range(0, 4)].Play();
	}
}

