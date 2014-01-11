using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {
	public Texture2D pauseIcon;
	public Texture2D unpauseIcon;
	public AudioClip pauseSound;
	public AudioClip unpauseSound;
	private bool paused = false;

	// http://docs.unity3d.com/Documentation/Components/GUIScriptingGuide.html
	void OnGUI() {
		// http://answers.unity3d.com/questions/46158/how-to-create-a-transparent-button.html
		GUI.backgroundColor = Color.clear;
		Rect buttonRect = new Rect(0, 0, 50, 50);
		if (paused) {
			paused = !GUI.Button(buttonRect, unpauseIcon);
			if (!paused) {
				audio.PlayOneShot(unpauseSound);
				Time.timeScale = 1;
			}
		}
		else {
			paused = GUI.Button(buttonRect, pauseIcon);
			if (paused) {
				audio.PlayOneShot(pauseSound);
				// http://answers.unity3d.com/questions/7544/how-do-i-pause-my-game.html
				// Prevents fruits from falling, and also seems to prevent matching on click.
				Time.timeScale = 0;
			}
		}
	}
}
