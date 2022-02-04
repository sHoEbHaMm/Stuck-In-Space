using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] private AudioClip ExplosionSound;
    [SerializeField] private AudioClip VictorySound;
    [SerializeField] private ParticleSystem successParticles;
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private float delayTime;
    bool isTransitioning = false;
    bool collisionDisabled = false;

    void Start() 
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() 
    {
        CheatCode();
    }

    void CheatCode()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(collisionDisabled) {return;}
        
        switch(other.gameObject.tag)
        {
            case "Friendly" :
                break;
            case "Finish" :
                StartLoadNextLevelSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }        
    }

    void StartCrashSequence()
    {
        if(!isTransitioning)
        {
            isTransitioning = true;
            explosionParticles.Play();
            audioSource.Stop();
            audioSource.PlayOneShot(ExplosionSound);
            GetComponent<Movement>().enabled = false;
            Invoke("ReloadLevel", delayTime);
        } 
    }

    void StartLoadNextLevelSequence()
    {
        if(!isTransitioning)
        {
            isTransitioning = true;
            successParticles.Play();
            audioSource.PlayOneShot(VictorySound);
            GetComponent<Movement>().enabled = false;
            Invoke("LoadNextLevel", delayTime);
        }
    }

    void ReloadLevel()
    {
        int index;
        index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }

    void LoadNextLevel()
    {
        int nextLevelIndex;
        nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if(nextLevelIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextLevelIndex = 0;
        }
        SceneManager.LoadScene(nextLevelIndex);
    }
}
