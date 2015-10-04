﻿using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	public float fadeSpeed = 0.4f;
	public float maxVolumn_Rising = 1f;

	public GameObject player;

	public AudioSource title;
	public AudioSource bgmMain;
	public AudioSource bgmRise;
	public AudioSource fellowDialog;


	public AudioClip bgmRace;

	public AudioClip[] dialogTutorial;

	public AudioClip sfxCrashTree;
	public AudioClip sfxCrashSpiderWeb;

	bool isRising = false;
	bool isFalling = false;


	//test
	AudioSource testAS;
	GameObject testGO;

	// Use this for initialization
	void Start () {
//		Debug.Log("Sound Manager start working");
//		mainThemeStart ();
	}
	
	// Update is called once per frame
	void Update () {

		//test
		if (Input.GetKeyDown ("up"))	RisingMusicStart ();
		if (Input.GetKeyDown ("down"))	FallingMusicStart ();
		if (Input.GetKeyDown ("left"))	RisingOrFallingMusicStop ();
		if (Input.GetKeyDown ("m"))	mainThemeStop ();
		if (Input.GetKeyDown ("n"))	mainThemeStart ();
		if (Input.GetKeyDown ("r"))	raceThemeStart ();
		if (Input.GetKeyDown ("e"))	raceThemeStop ();



		if (Input.GetKeyDown ("0")) titleMusicStart ();
		if (Input.GetKeyDown ("1")) {
			dialog01AHello ();
			dialog01BTurnAroundLookAtMe ();
		}
		if (Input.GetKeyDown ("2")) dialog02LetsGo2SP ();
		if (Input.GetKeyDown ("3")) {
			dialog03AUseDandelion2Fly ();
			dialog03BComeOnWait4U ();
			dialog03CHurryUp4lowMe();
		}
		if (Input.GetKeyDown ("4")) dialog04TurnRight ();
		if (Input.GetKeyDown ("5")) {
			dialog05GJOnReachStart ();
			dialog06NowLetsStartRaceReady321 ();
			dialog07Go ();
		}
		if (Input.GetKeyDown ("6")) {
			dialog08Congra ();
			dialog09A1st ();
			dialog09B2nd ();
			dialog09C3rd ();
		}
	}

	public void titleMusicStart(){
		title.Play ();
	}

	public void mainThemeStart(){
//		Debug.Log ("Start Main Theme");
		StartCoroutine(MusicFadeIn (bgmMain, 1f,1f));
		bgmRise.Play ();
	}

	//Call when the guest arrive starting point (hit the ground)
	//Beware!!!! This function must be called after the mainTheme fade in is COMPLETELY DONE (volumn = 1)!
	//Cause I'm too lazy to adjust it XD
	public void mainThemeStop(){
		StartCoroutine(MusicFadeOut (bgmMain,1f));
		StartCoroutine(MusicFadeOut (bgmRise,1f));
	}

	//Call when the race begins
	public void raceThemeStart(){
		bgmMain.clip = bgmRace;
		bgmMain.volume = 1f;
		bgmMain.Play ();
		bgmRise.Play ();
	}

	public void raceThemeStop(){
		StartCoroutine(MusicFadeOut (bgmMain,1f));
		StartCoroutine(MusicFadeOut (bgmRise,1f));
	}

	//Call when the guest starts to rise
	public void RisingMusicStart(){ 
//		Debug.Log("Start Rising");
		FallingMusicStop();
		isRising = true;
		StartCoroutine(FRMusicFadeIn(bgmRise,maxVolumn_Rising));
	}

	//Call when the guest starts to fall or drop
	public void FallingMusicStart(){
//		Debug.Log("Start Falling");
		GameObject go;
		AudioSource audios;
		go = GameObject.Find("bgmRise");
		audios = go.GetComponent<AudioSource> ();
		RisingMusicStop();
		isFalling = true;
		StartCoroutine(FRMusicFadeOut(bgmRise));
	}

	//Call when the guest stay stable (not rising, not falling)
	//If the guest switch from rising to falling (or oposite) abruptly, this function is no need to call.
	public void RisingOrFallingMusicStop(){
		RisingMusicStop ();
		FallingMusicStop ();
	}


	//Dialogs
	public void dialog01AHello(){
		StartCoroutine(dialogPlay(0,0.5f));
	}

	public void dialog01BTurnAroundLookAtMe(){
		StartCoroutine(dialogPlay(1,0.5f));
	}

	public void dialog02LetsGo2SP(){
		StartCoroutine(dialogPlay(2,0.5f));
	}

	public void dialog03AUseDandelion2Fly(){
		StartCoroutine(dialogPlay(3,0.5f));
	}

	public void dialog03BComeOnWait4U(){
		StartCoroutine(dialogPlay(4,0.7f));
	}

	public void dialog03CHurryUp4lowMe(){
		StartCoroutine(dialogPlay(5,0.7f));
	}

	public void dialog04TurnRight(){
		StartCoroutine(dialogPlay(6,0.7f));
	}

	public void dialog05GJOnReachStart(){
		StartCoroutine(dialogPlay(7,0.5f));
	}

	public void dialog06NowLetsStartRaceReady321(){
		StartCoroutine(dialogPlay(8,0.5f));
	}

	public void dialog07Go(){
		StartCoroutine(dialogPlay(9,0.5f));
	}

	public void dialog08Congra(){
		StartCoroutine(dialogPlay(10,0.8f));
	}

	public void dialog09A1st(){
		StartCoroutine(dialogPlay(11,1f));
	}

	public void dialog09B2nd(){
		StartCoroutine(dialogPlay(12,1f));
	}

	public void dialog09C3rd(){
		StartCoroutine(dialogPlay(13,1f));
	}

	IEnumerator dialogPlay(int dialogNum,float dialogVol){
		while(fellowDialog.isPlaying)
			yield return new WaitForSeconds(Time.deltaTime);
		fellowDialog.clip = dialogTutorial [dialogNum];
		fellowDialog.volume = dialogVol;
		fellowDialog.Play ();
		yield return null;
	}



	//AudioManager
	/// Plays a sound by creating an empty game object with an AudioSource
	/// and attaching it to the given transform (so it moves with the transform). Destroys it after it finished playing.
	AudioSource Play(AudioClip clip, Transform emitter, float volume, float pitch)
	{
		//Create an empty game object
		GameObject go = new GameObject ("Audio: " + clip.name);
		go.transform.position = emitter.position;
		go.transform.parent = emitter;
		
		//Create the source
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volume;
		source.pitch = pitch;
		source.Play ();
		Destroy (go, clip.length);
		return source;
	}

	void RisingMusicStop(){
//		Debug.Log("Stop Rising or Falling.");
		isRising = false;
	}

	void FallingMusicStop(){
//		Debug.Log("Stop Rising or Falling.");
		isFalling = false;
	}

	IEnumerator FRMusicFadeIn(AudioSource audios, float maxVolumn){
		//		Debug.Log("Start fade in." + "isRisingOrFalling = " + isRising);
		if(audios.isPlaying)
			while (audios.volume <= maxVolumn && isRising){
				//			Debug.Log("volumn up");
				audios.volume += fadeSpeed * Time.deltaTime;
				yield return new WaitForSeconds(Time.deltaTime);
			}
	}

	IEnumerator FRMusicFadeOut(AudioSource audios){
		//		Debug.Log("Start fade out." + "isRisingOrFalling = " + isFalling);
		if(audios.isPlaying)
			while (audios.volume >= 0.01 && isFalling){
				//			Debug.Log("volumn down");
				audios.volume -= fadeSpeed * Time.deltaTime;
				yield return new WaitForSeconds(Time.deltaTime);
			}
	}



	IEnumerator MusicFadeIn(AudioSource audios, float maxVolumn){
//		Debug.Log("Start fade in.");
		audios.Play();
		while (audios.volume < maxVolumn){
			audios.volume += fadeSpeed * Time.deltaTime;
			yield return new WaitForSeconds(Time.deltaTime);
		}
//		Debug.Log("Complete fade in.");
	}

	IEnumerator MusicFadeIn(AudioSource audios, float maxVolumn, float fadeInSpeed){
		//		Debug.Log("Start fade in.");
		audios.Play ();
		while (audios.volume < maxVolumn){
			audios.volume += fadeInSpeed * Time.deltaTime;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		//		Debug.Log("Complete fade in.");
	}


	IEnumerator MusicFadeOut(AudioSource audios){
//		Debug.Log("Start fade out.");
		while (audios.volume >= 0.01){
			audios.volume -= fadeSpeed * Time.deltaTime;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		audios.Stop ();
	}

	IEnumerator MusicFadeOut(AudioSource audios, float fadeOutSpeed){
		//		Debug.Log("Start fade out.");
		while (audios.volume >= 0.01){
			audios.volume -= fadeOutSpeed * Time.deltaTime;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		audios.Stop ();
	}

}
