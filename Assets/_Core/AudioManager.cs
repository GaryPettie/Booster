using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static void Play2DClipAtPoint (AudioClip clip, Vector3 position, float volume) {
		GameObject temp = new GameObject("PointAudioClip");
		temp.transform.position = position;
		AudioSource tempAudio = temp.AddComponent<AudioSource>();
		tempAudio.clip = clip;
		tempAudio.volume = volume;
		tempAudio.spatialBlend = 0;
		tempAudio.Play();
		Destroy(temp, clip.length);
	}
}
