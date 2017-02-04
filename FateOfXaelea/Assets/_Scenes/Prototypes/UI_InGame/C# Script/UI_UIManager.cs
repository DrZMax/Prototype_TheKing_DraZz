using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UIManager : MonoBehaviour {

    // **IMPORTANT - Le nombre de sprites doit correspondre à la constante (+1) utilisé dans UI_GameManager.
    public Sprite[] HeartsSprites;                  // Contient les images des coeurs qu'on peut affiché (À l'indice 0, le coeur est vide et ça se rempli jusqu'à être 
                                                    // plein au dernier indice). À tester, mais je pense qu'on peut mettre jusqu'à 9-10 fragments max.
    public GameObject HeartPrefab;                  // Gameobjet contenant un coueur (contient un spriteRenderer et une animation).
    public GameObject PanelHearts;
    public GameObject PanelDanger;

    //--------------------------------------------------------------------------------
    // DEBUG - À retirer quand on va utiliser la classe pour de vrai ;)
    public Button BtnAddCurrentLife;
    public Button BtnRemoveCurrentLife;
    public Text LblCurrentLife;

    public Button BtnAddMaxLife;
    public Button BtnRemoveMaxLife;
    public Text LblMaxLifes;
    //--------------------------------------------------------------------------------

    private UI_GameManager _gameManager;
    private GameObject[] _arrHeartsObj;

    void Start () {
        _gameManager = GameObject.Find("GameManager").GetComponent<UI_GameManager>();
        createHeatsOnScreen();
    }
	
	void Update () {
        if (_gameManager != null)
        {
            UpdateHeartsDisplay();
            ShowDangerUI();
            UpdateUIDebug();
        }        
    }

    /// <summary>
    /// On va créer tout les gameobjets qu'on a besoin.
    /// </summary>
    private void createHeatsOnScreen()
    {
        if (_arrHeartsObj != null)
        {
            foreach (GameObject obj in _arrHeartsObj)
            {
                Destroy(obj);
            }
        }        

        int espLeft = 25;
        int espTop = 20;
        int espX = 5;
        int espY = 5;

        int width = (int)((RectTransform)HeartPrefab.transform).rect.width;
        int height = (int)((RectTransform)HeartPrefab.transform).rect.height;

        List<GameObject> lstHearts = new List<GameObject>();
        int nbCoueurAAfficher = _gameManager.MaxPlayerLife;
        int nbCoeurParRange = UI_GameManager.NUMBER_MAX_HEARTS_EVER / 2;
        for (int ind = 1; ind <= nbCoueurAAfficher; ind++)
        {
            // On va déterminer la position à laquelle on va afficher le coeur à l'écran.
            int indX = (ind <= nbCoeurParRange ? ind : ind - nbCoeurParRange);
            int indY = (ind <= nbCoeurParRange ? 1 : 2);

            int posX = ((width + espX) * (indX - 1)) + espLeft;
            int posY = (((height + espY) * (indY - 1)) + espTop) * -1;

            GameObject objHeart = Instantiate(HeartPrefab, new Vector3(posX, posY, 1), Quaternion.identity, PanelHearts.transform);
            objHeart.name = "Heart (" + ind.ToString() + ")";
            ((RectTransform)objHeart.transform).localPosition = new Vector3(posX, posY, 1);
            lstHearts.Add(objHeart);
        }

        _arrHeartsObj = lstHearts.ToArray();
    }

    /// <summary>
    /// Permet de changer les sprites sur les coeurs pour représenter la vie du joueur. De plus, on va réinitialiser l'animation de tous les coueurs pour que seul le coeur actif soit animé.
    /// </summary>
    private void UpdateHeartsDisplay()
    {
        for (int ind = 0; ind < _arrHeartsObj.Length; ind++)
        {
            int indHeart = ind + 1;
            GameObject heart = _arrHeartsObj[ind];

            // Étape 1 - On détermine quel coeur on veut affiché. Le coeur vide est à gauche et ça devrait se remplir jusqu'à être complet à droite.
            int posIconInTexture = 0;
            float rest = (float)_gameManager.CurrentPlayerLife - (int)_gameManager.CurrentPlayerLife;

            if (indHeart <= (int)_gameManager.CurrentPlayerLife)
            {
                // C'est sûr que c'est un coeur plein.
                posIconInTexture = UI_GameManager.NUMBER_HEART_PARTS;
            }
            else if (indHeart > (int)_gameManager.CurrentPlayerLife + 1)
            {
                // C'est sûr que c'est un coeur vide.
                posIconInTexture = 0;
            }
            else
            {
                // On va avoir un coeur incomplet, ou même vide. On va aller chercher le reste et multiplier par le nombre de parties (Heart piece).
                // Ex. On veut afficher le 3e coeur. _currentLife est à 2.4.
                //     0.4 * 5 = 2   C'est la 3e image (On commence à 0) dans la texture.
                //posIconInTexture = (int)(rest * UI_GameManager.NUMBER_HEART_PARTS);
                posIconInTexture = Mathf.RoundToInt(rest * UI_GameManager.NUMBER_HEART_PARTS);
            }

            heart.GetComponent<Image>().sprite = HeartsSprites[posIconInTexture];
            heart.GetComponent<Animator>().SetBool("IsCurrentHeart", indHeart == ((int)_gameManager.CurrentPlayerLife + (rest == 0 ? 0 : 1)));
        }
    }

    /// <summary>
    /// Permet d'afficher des barres rouges sur le contour de l'écran pour indiquer que la vie du joueur commence à être bas.
    /// On l'affiche si nécessaire.
    /// </summary>
    private void ShowDangerUI()
    {
        Animator anmDanger = PanelDanger.GetComponent<Animator>();
        float rateHighDanger = _gameManager.MaxPlayerLife * 0.08f;  // < 8%
        float rateLowDanger = _gameManager.MaxPlayerLife * 0.20f;   // < 20%

        if (_gameManager.CurrentPlayerLife <= rateHighDanger)
            anmDanger.SetInteger("EtatDanger", 2);  // Grand danger, on va afficher l'animation rapide.
        else if (_gameManager.CurrentPlayerLife <= rateLowDanger)
            anmDanger.SetInteger("EtatDanger", 1);  // il y a danger, on va afficher l'animation lente.
        else
            anmDanger.SetInteger("EtatDanger", 0);  // état idle.
    }

    #region Debug

    /// <summary>
    /// Permet d'afficher les informations de debug (current life et max life).
    /// </summary>
    private void UpdateUIDebug()
    {
        float currentPlayerLife = _gameManager.CurrentPlayerLife;
        int maxPlayerLife = _gameManager.MaxPlayerLife;
        int maxLifeEver = UI_GameManager.NUMBER_MAX_HEARTS_EVER;

        LblCurrentLife.text = currentPlayerLife.ToString("0.0");
        LblMaxLifes.text = maxPlayerLife.ToString();

        BtnAddCurrentLife.interactable = currentPlayerLife < maxPlayerLife;
        BtnRemoveCurrentLife.interactable = currentPlayerLife > 0;
        BtnAddMaxLife.interactable = maxPlayerLife < maxLifeEver;
        BtnRemoveMaxLife.interactable = maxPlayerLife > 0;
    }

    public void AddCurrentLife()
    {
        _gameManager.HealPlayer();
    }

    public void RemoveCurrentLife()
    {
        _gameManager.PlayerTakeDamage();
    }

    public void AddMaxLife()
    {
        _gameManager.AddLife();
        createHeatsOnScreen();
    }

    public void RemoveMaxLife()
    {
        _gameManager.RemoveLife();
        createHeatsOnScreen();
    }

    #endregion
}
