using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip startPlayMusic;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(startPlayMusic);
    }
    public void PlayGameButton()
    {
        //Application.LoadLevel("GamePlay");
        SceneManager.LoadScene("GamePlay");
        //AudioManager.Instance.PlayBackgroundMusic();
        //audioSource.PlayOneShot(startPlayMusic);
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }


}
