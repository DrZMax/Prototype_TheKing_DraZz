using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    private static LevelManager _instance;       // Singleton - On s'assure d'avoir une seule instance.

    /// <summary>
    /// On va faire en sorte que le LevelManager reste là même quand on change de scène. Mais on en veut juste un ;)
    /// </summary>
    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }            
        else
        {
            Destroy(gameObject);
        }
    }
	
    /// <summary>
    /// Quand on appuie sur ESC, on va retourner au menu principale.
    /// </summary>
	private void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name.ToLower() == "menu")
                Application.Quit();
            else
                LoadScene("Menu");
        }
	}

    /// <summary>
    /// Permet de naviguer vers la scène spécifier.
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.Log("Erreur! Aucun nom de scène n'a été spécifié.");
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
