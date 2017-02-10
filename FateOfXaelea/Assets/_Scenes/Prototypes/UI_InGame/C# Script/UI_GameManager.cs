using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameManager : MonoBehaviour {

    // **IMPORTANT - Ça doit correspondre à la texture utilisé dans UI_UIManager.
    public const int NUMBER_HEART_PARTS = 5;       // Indique combien de fragment contient un coeur.
    public const int NUMBER_MAX_HEARTS_EVER = 12;   // Indique combien de coeurs ont va afficher au maximum. Ça va être utile à l'affichage pour savoir combien de coeurs ont affiche par rangée.

    // *** On maintient ces informations en int, on fait donc *10  (16 -> 1.6 coeurs).
    private int _currentPlayerLife;               // Vie du joueur, indique combien de coeurs il lui reste. 1.6f donne donc un coueur complet et 3/5 d'un deuxième coeur.
    private int _maxPlayerLife;                     // Nombre de coeur total dont le joueur dispose. On va afficher tous ces coeurs à l'écran, mais certains pourront être vide.

    //private static UI_GameManager _instance;

    public int CurrentPlayerLife
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
        _currentPlayerLife = ensureValidAmount(16);
        _maxPlayerLife = 30;
    }

    /// <summary>
    /// On va baisser la vie du joueur.
    /// </summary>
    /// <param name="amount">Si on ne passe pas de paramètre, on va appliquer un unité de dégât. S'il y a 5 fragments de coeurs, on va réduire la vie de 0.2.</param>
    public void PlayerTakeDamage(int amount = int.MinValue)
    {
        amount = ensureValidAmount(amount);

        _currentPlayerLife -= amount;

        if (_currentPlayerLife < 0)
            _currentPlayerLife = 0;
    }

    /// <summary>
    /// On va augmenter la vie du joueur.
    /// </summary>
    /// <param name="amount">Si on ne passe pas de paramètre, on va appliquer un unité de dégât. S'il y a 5 fragments de coeurs, on va réduire la vie de 0.2.</param>
    public void HealPlayer(int amount = int.MinValue)
    {
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
        _maxPlayerLife += 10;
        if (_maxPlayerLife > NUMBER_MAX_HEARTS_EVER * 10)
            _maxPlayerLife = NUMBER_MAX_HEARTS_EVER;
    }

    /// <summary>
    /// Permet de retirer un coueur à la vie du joueur. Devrait être utilisé qu'en debug...
    /// </summary>
    public void RemoveLife()
    {
#if (DEBUG)
        _maxPlayerLife -= 10;
        if (_maxPlayerLife < 10)
            _maxPlayerLife = 10;

        if (_currentPlayerLife > _maxPlayerLife)
            _currentPlayerLife = _maxPlayerLife;
#endif
    }

    /// <summary>
    /// Permet de s'assurer que le montant qu'on s'apprête à ajouter/retirer de la vie du joueur est un bon chiffre. Si on incrémente par des 0.2f, on ne peut ajouter ou retire 0.3f.
    /// </summary>
    private int ensureValidAmount(int amount)
    {
        if (amount == int.MinValue)
        {
            return (int)(1.0f / NUMBER_HEART_PARTS * 10);
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
        int mod = (int)(1.0f / NUMBER_HEART_PARTS * 10);
        return amount % mod == 0;
    }
}
