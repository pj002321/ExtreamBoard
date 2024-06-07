using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player;


public class CrashDetector : MonoBehaviour
{
    [SerializeField] float loadDelay = 1.5f;
    [SerializeField] ParticleSystem crashnEffect;
    [SerializeField] AudioClip crashSFX;

    bool hasCrashed = false;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground" && !hasCrashed)
        {
            hasCrashed = true;
            FindObjectOfType<PlayerController>().DisableControls();
            crashnEffect.Play();
            GetComponent<AudioSource>().PlayOneShot(crashSFX);
            Invoke("ReloadScene", loadDelay);
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
