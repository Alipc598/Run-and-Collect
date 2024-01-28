using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playMusic : MonoBehaviour {

    public AudioSource audioSource;

    public AudioClip btnSound;
	// Use this for initialization
	void Awake()
    {
        GameObject[] musicPlayer = GameObject.FindGameObjectsWithTag("Music");

        /*if(musicPlayer.Length > 1)  //if more than one is created when going back to main menu
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject); *///allows the music to keep playing by keeping the game object throughout scenes
    }

    public void btnSoundPlay()
    {
        audioSource.PlayOneShot(btnSound);
    }

}
