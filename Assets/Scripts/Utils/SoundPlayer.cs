using UnityEngine;

public class SoundPlayer {

	public static void PlaySound(AudioSource source, AudioClip clip) {
		if(clip != null) {
			if(source != null) {
				source.clip = clip;
				source.Play();
			}
		}
	}

}
