using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public Text txtTitle;                   // Élément où on va afficher le titre du bouton (dans le panel prévu à cette effet).
    public Text txtDescription;             // Élément où on va afficher la description, si existante.
    public LevelManager LevelManager;       // Permet de changer de scène!

    private void Start()
    {
        ShowDefaultText();
    }

    /// <summary>
    /// Lorsque l'utilisateur passe la souris sur un des boutons, on devrait afficher le titre et la description du bouton dans le panel prévu à cet effet.
    /// </summary>
    public void ShowInfos(string title, string description)
    {
        txtTitle.text = title;
        txtDescription.text = description;
    }

    /// <summary>
    /// Lorsque le curseur quitter le bouton, on va remettre le texte par défaut.
    /// </summary>
    public void HideInfos()
    {
        ShowDefaultText();
    }

    /// <summary>
    /// Permet de naviguer vers la scène spécifier.
    /// </summary>
    /// <param name="sceneName"></param>
    public void GoToScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.Log("Erreur! Aucun nom de scène n'a été spécifié.");
        }
        else
        {
            txtTitle.text = "Affichage de la scène \"" + sceneName + "\"";
            txtDescription.text = "Un instant svp...";

            LevelManager.LoadScene(sceneName);
        }
    }

    /// <summary>
    /// Permet d'afficher une instruction quand le curseur ne se trouve pas sur un bouton.
    /// </summary>
    private void ShowDefaultText()
    {
        txtTitle.text = "";
        txtDescription.text = "Cliquer sur un bouton pour aller vers cette scène.";
    }
}
