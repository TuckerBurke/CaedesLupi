using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicPlayer : MonoBehaviour
{

    private AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        music = GetComponent<AudioSource>();
        PlayMusic();

        StopMenuMusic();
    }

    void Awake()
    {
        GameObject[] musicPlayers = GameObject.FindGameObjectsWithTag("Music");
        if (musicPlayers.Length <= 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            for (int i = 1; i < musicPlayers.Length; i++)
            {
                Destroy(musicPlayers[i]);
            }
        }
    }

    public void PlayMusic()
    {
        if (music.isPlaying) return;
        music.Play();
    }

    public void StopMenuMusic()
    {
        Destroy(GameObject.Find("MenuMusicPlayer"));
    }

    public void StopMusic()
    {
        music.Stop();
    }
}