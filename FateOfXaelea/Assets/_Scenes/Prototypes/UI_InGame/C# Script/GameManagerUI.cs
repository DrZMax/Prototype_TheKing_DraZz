using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerUI : MonoBehaviour
{
    public Texture TextureHearts;                   // On reçoit du designer la texture pour les coeurs.

    private float _currentLife = 1.6f;              // Ex. 3.2f -> 3 Coeurs complets et 1 / 5 d'un autre coeur.
    private int _maxLifes = 3;                      // Indique combien de vies au total le joueur pourrait avoir.

    public Button BtnAddLife;
    public Button BtnRemoveLife;
    public Text LblLifes;

    /// <summary>
    /// On initialise nos variables.
    /// </summary>
	private void Start()
    {
        HeartDisplayConfigs.WidthHeartInTexture = TextureHearts.height;
        HeartDisplayConfigs.NumberHeartsInTexture = TextureHearts.width / TextureHearts.height;  // Assumons que la texture comporte tous les coeurs sur la même rangée.

        HeartDisplayConfigs.WidthHeartOnScreen = 32;
        HeartDisplayConfigs.HeightHeartOnScreen = 32;
        HeartDisplayConfigs.SpaceBetweenHearts = 10;
    }

    private void Update()
    {
        AjustDisplayDebug();
    }

    /// <summary>
    /// UI - On gère l'affichage de tout ce qui doit s'afficher à l'écran.
    /// </summary>
    private void OnGUI()
    {
        //ShowPlayerLife();
        ShowWarningLowLife();
    }

    /// <summary>
    /// En haut à gauche de l'écran, on affiche la vie du personnage.
    /// </summary>
    private void ShowPlayerLife()
    {
        float rest = -1;
        int posIconInTexture = -1;
        int posX = -1;
        int posY = -1;

        if (_currentLife >= 0)
        {
            // Pour chaque coeur qu'on peut afficher (si le joueur perd un coeur de vie, on va quand même afficher le contour du coeur).
            for (int ind = 1; ind <= _maxLifes; ind++)
            {
                // Étape 1 - On détermine quel coeur on veut affiché. Le coeur vide est à gauche et ça devrait se remplir jusqu'à être complet à droite.
                if (ind <= (int)_currentLife)
                {
                    // C'est sûr que c'est un coeur plein.
                    posIconInTexture = HeartDisplayConfigs.NumberHeartsInTexture - 1;
                }
                else if (ind > (int)_currentLife + 1)
                {
                    // C'est sûr que c'est un coeur vide.
                    posIconInTexture = 0;
                }
                else
                {
                    // On va avoir un coeur incomplet, ou même vide. On va aller chercher le reste et multiplier par le nombre de parties (Heart piece).
                    // Ex. On veut afficher le 3e coeur. _currentLife est à 2.4.
                    //     0.4 * 5 = 2   C'est la 3e image (On commence à 0) dans la texture.
                    rest = (float)_currentLife - (int)_currentLife;
                    posIconInTexture = (int)(rest * HeartDisplayConfigs.NumberHeartsInTexture);
                }


                // Étape 2 - On détermine la position de la sprite à afficher dans le texture.
                Rect posInTexture = new Rect((float)posIconInTexture * (float)HeartDisplayConfigs.WidthHeartInTexture / (float)TextureHearts.width, 0, 
                    (float)HeartDisplayConfigs.WidthHeartInTexture / (float)TextureHearts.width, 1);


                // Étape 3 - On détermine la position à afficher dans l'écran.
                posX = HeartDisplayConfigs.SpaceBetweenHearts + ((ind - 1) * HeartDisplayConfigs.SpaceBetweenHearts) + ((ind - 1) * HeartDisplayConfigs.WidthHeartOnScreen);
                posY = HeartDisplayConfigs.SpaceBetweenHearts;
                Rect posOnScreen = new Rect(posX, posY, HeartDisplayConfigs.WidthHeartOnScreen, HeartDisplayConfigs.HeightHeartOnScreen);


                // Étape 4 - On affiche!
                GUI.DrawTextureWithTexCoords(posOnScreen, TextureHearts, posInTexture);                
            }
        }
    }

    /// <summary>
    /// Quand la vie du joueur atteint un certain seuil, on va afficher un indicateur tout le tour de l'écran pour le stresser!! MOUHAHAHAHA
    /// </summary>
    private void ShowWarningLowLife()
    {
        // Mettons si reste < 15% de la vie max?
    }

    #region Debug

    public void HealPlayer()
    {
        _currentLife += 1.0f / (float)(HeartDisplayConfigs.NumberHeartsInTexture - 1);

        if (_currentLife > _maxLifes)
            _currentLife = _maxLifes;
    }

    public void PlayerTakeDamage()
    {
        _currentLife -= 1.0f / (float)(HeartDisplayConfigs.NumberHeartsInTexture - 1);

        if (_currentLife < 0)
            _currentLife = 0;
    }

    private void AjustDisplayDebug()
    {
        LblLifes.text = _currentLife.ToString("0.0");
        BtnAddLife.interactable = _currentLife < _maxLifes;
        BtnRemoveLife.interactable = _currentLife > 0;
    }

    #endregion
}
