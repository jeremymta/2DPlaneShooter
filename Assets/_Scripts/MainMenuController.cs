using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip startPlayMusic;

    public Button playButton;
    public Button quitButton;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //audioSource.PlayOneShot(startPlayMusic);
        PlayStartMusic();

        playButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);

    }
    public void PlayStartMusic()
    {
        audioSource.clip = startPlayMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void PlayGameButton()
    {
        //Application.LoadLevel("GamePlay");
        audioSource.Stop();
        SceneManager.LoadScene("GamePlay");
        
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }

}
