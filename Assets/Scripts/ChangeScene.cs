using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private AudioClip clickClip;
    public void GoToGame() {
        SoundFXManager.instance.PlaySoundFXClip(clickClip, Camera.main.transform, 1f);
        SceneManager.LoadScene("Game");
    }
}
