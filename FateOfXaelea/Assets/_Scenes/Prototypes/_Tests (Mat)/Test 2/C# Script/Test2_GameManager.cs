using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2_GameManager: MonoBehaviour {

    public Sprite[] Heart_Sprites;                  // Array contenant les sprites utilisées pour afficher la vie du joueur. Doit avoir une seule rangée, le premier doit être le coeur vide et ça se rempli jusqu'au dernier indice (coeur plein).
    public static bool IsGameFreeze = false;        // Lorsqu'on affiche le menu ou qu'on veut montrer qqc, on veut faire en sorte que la game gèle. Le joueur fait ces affaires dans le menu et la vie reprends.

    #region Données membres - UI

    public const int NUMBER_MAX_HEARTS = 12;        // On va afficher jusqu'à 12 coeurs au maximum à l'écran (2 rangées de 6).

    private int _playerCurrentLife;                 // Vie du joueur en int, toujours *10. Donc 16 équivaut à 1 coeur et 6/10 -> 3 parties de coeur ayant 5 fragments.
    private int _playerMaxLife;                     // Vie maximale que le joueur peut atteindre lorsqu'il est en santé (heal).

    #endregion

    #region Accesseurs - UI

    public int NumberHeartParts
    {
        get { return Heart_Sprites.Length - 1; }
    }

    public int PlayerCurrentLife
    {
        get { return _playerCurrentLife; }
    }

    public int PlayerMaxLife
    {
        get { return _playerMaxLife; }
    }

    #endregion


    //private static UI_GameManager _instance;
    // On va s'en servir plus tard ;)
    ///// <summary>
    ///// On va faire en sorte que le LevelManager reste là même quand on change de scène. Mais on en veut juste un ;)
    ///// </summary>
    //private void Awake()
    //{
    //    if (!_instance)
    //    {
    //        _instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    private void Start ()
    {
        _playerCurrentLife = ensureValidAmount(16);
        _playerMaxLife = 30;
    }

    #region UI

    /// <summary>
    /// On va baisser la vie du joueur.
    /// </summary>
    /// <param name="amount">Si on ne passe pas de paramètre, on va appliquer un unité de dégât. S'il y a 5 fragments de coeurs, on va réduire la vie de 0.2.</param>
    public void PlayerTakeDamage(int amount = int.MinValue)
    {
        amount = ensureValidAmount(amount);

        _playerCurrentLife -= amount;

        if (_playerCurrentLife < 0)
        {
            // On est mort!
            _playerCurrentLife = 0;
        }
    }

    /// <summary>
    /// On va augmenter la vie du joueur.
    /// </summary>
    /// <param name="amount">Si on ne passe pas de paramètre, on va appliquer un unité de dégât. S'il y a 5 fragments de coeurs, on va réduire la vie de 0.2.</param>
    public void PlayerHeal(int amount = int.MinValue)
    {
        amount = ensureValidAmount(amount);

        _playerCurrentLife += amount;

        // On n'est pas invinsible... On ne peut pas dépasser le max.
        if (_playerCurrentLife > _playerMaxLife)
            _playerCurrentLife = _playerMaxLife;
    }

    /// <summary>
    /// Le joueur n'a plus de vie. Noooooooo! Il est mort! Max qu'est-ce qu'on va faire??
    /// </summary>
    public void PlayerDiying()
    {
        // TODO - Éventuellement.
    }

    /// <summary>
    /// Permet d'ajouter un coeur de plus à la vie du joueur.
    /// </summary>
    public void AddLife()
    {
        _playerMaxLife += 10;
        if (_playerMaxLife > NUMBER_MAX_HEARTS * 10)
            _playerMaxLife = NUMBER_MAX_HEARTS;
    }

    /// <summary>
    /// Permet de retirer un coueur à la vie du joueur. Devrait être utilisé qu'en debug...
    /// </summary>
    public void RemoveLife()
    {
#if (DEBUG)
        // On retire un coeur complet.
        _playerMaxLife -= 10;

        // On ne peut pas avoir de vie... sinon c'est pas jouable!
        if (_playerMaxLife < 10)
            _playerMaxLife = 10;

        // Le joueur ne peut pas avoir plus de vie que le max...
        if (_playerCurrentLife > _playerMaxLife)
            _playerCurrentLife = _playerMaxLife;
#endif
    }

    /// <summary>
    /// Permet de s'assurer que le montant qu'on s'apprête à ajouter/retirer de la vie du joueur est un bon chiffre. Si on incrémente par des 0.2f, on ne peut ajouter ou retire 0.3f.
    /// </summary>
    private int ensureValidAmount(int amount)
    {
        if (amount == int.MinValue)
        {
            return (int)(1.0f / NumberHeartParts * 10);
        }

        if (isValideAmount(amount))
        {
            return amount;
        }
        else
        {
            int cpt = 0;
            while (!isValideAmount(amount) && cpt < 20)
            {
                cpt++;
                amount--;
            }

            return (amount > 0 ? amount : 0);
        }
    }

    private bool isValideAmount(int amount)
    {
        int mod = (int)(1.0f / NumberHeartParts * 10);
        return amount % mod == 0;
    }

    #endregion
}
