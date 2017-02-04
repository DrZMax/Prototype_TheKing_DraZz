using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameManager : MonoBehaviour {

    // **IMPORTANT - Ça doit correspondre à la texture utilisé dans UI_UIManager.
    public const int NUMBER_HEART_PARTS = 5;       // Indique combien de fragment contient un coeur.
    public const int NUMBER_MAX_HEARTS_EVER = 12;   // Indique combien de coeurs ont va afficher au maximum. Ça va être utile à l'affichage pour savoir combien de coeurs ont affiche par rangée.

    private float _currentPlayerLife;               // Vie du joueur, indique combien de coeurs il lui reste. 1.6f donne donc un coueur complet et 3/5 d'un deuxième coeur.
    private int _maxPlayerLife;                     // Nombre de coeur total dont le joueur dispose. On va afficher tous ces coeurs à l'écran, mais certains pourront être vide.

    //private static UI_GameManager _instance;

    public float CurrentPlayerLife
    {
        get { return _currentPlayerLife; }
    }

    public int MaxPlayerLife
    {
        get { return _maxPlayerLife; }
    }

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
    
    private void Start () {
        _currentPlayerLife = ensureValidAmount(1.6f);
        _maxPlayerLife = 3;
    }

    /// <summary>
    /// On va baisser la vie du joueur.
    /// </summary>
    /// <param name="amount">Si on ne passe pas de paramètre, on va appliquer un unité de dégât. S'il y a 5 fragments de coeurs, on va réduire la vie de 0.2f.</param>
    public void PlayerTakeDamage(float amount = float.MinValue)
    {
        if (amount == float.MinValue)
            amount = 1.0f / NUMBER_HEART_PARTS;

        amount = ensureValidAmount(amount);

        _currentPlayerLife -= amount;

        if (_currentPlayerLife < 0)
            _currentPlayerLife = 0;
    }

    /// <summary>
    /// On va augmenter la vie du joueur.
    /// </summary>
    /// <param name="amount">Si on ne passe pas de paramètre, on va appliquer un unité de dégât. S'il y a 5 fragments de coeurs, on va réduire la vie de 0.2f.</param>
    public void HealPlayer(float amount = float.MinValue)
    {
        if (amount == float.MinValue)
            amount = 1.0f / NUMBER_HEART_PARTS;

        amount = ensureValidAmount(amount);

        _currentPlayerLife += amount;

        if (_currentPlayerLife > _maxPlayerLife)
            _currentPlayerLife = _maxPlayerLife;
    }

    /// <summary>
    /// Permet d'ajouter un coeur de plus à la vie du joueur.
    /// </summary>
    public void AddLife()
    {
        _maxPlayerLife++;
        if (_maxPlayerLife > NUMBER_MAX_HEARTS_EVER)
            _maxPlayerLife = NUMBER_MAX_HEARTS_EVER;
    }

    /// <summary>
    /// Permet de retirer un coueur à la vie du joueur. Devrait être utilisé qu'en debug...
    /// </summary>
    public void RemoveLife()
    {
#if (DEBUG)
        _maxPlayerLife--;
        if (_maxPlayerLife < 1)
            _maxPlayerLife = 1;

        if (_currentPlayerLife > _maxPlayerLife)
            _currentPlayerLife = _maxPlayerLife;
#endif
    }

    /// <summary>
    /// Permet de s'assurer que le montant qu'on s'apprête à ajouter/retirer de la vie du joueur est un bon chiffre. Si on incrémente par des 0.2f, on ne peut ajouter ou retire 0.3f.
    /// </summary>
    private float ensureValidAmount(float amount)
    {
        float newAmount = amount * (float)NUMBER_HEART_PARTS;
        if (newAmount - (int)newAmount == 0)
        {
            return amount;
        }

        int cpt = 0;
        while ((newAmount - (int)newAmount != 0) && cpt < 20)
        {
            cpt++;
            amount -= 0.1f;
            newAmount = amount * (float)NUMBER_HEART_PARTS;
        }

        return amount;
    }
}
