using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test1_UIManager : MonoBehaviour {

    #region Variables publiques

    public GameObject HeartPrefab;                  // Gameobjet contenant un coeur (contient un spriteRenderer et une animation).
    public GameObject PanelHearts;                  // Où on va affiché les coeurs.
    public GameObject PanelDanger;                  // Panel qui va prévenir l'utilisateur qu'il ne reste plus beaucoup de vie.

    //--------------------------------------------------------------------------------
    // DEBUG - À retirer quand on va utiliser la classe pour de vrai ;)
    public Button BtnAddCurrentLife;
    public Button BtnRemoveCurrentLife;
    public Text LblCurrentLife;

    public Button BtnAddMaxLife;
    public Button BtnRemoveMaxLife;
    public Text LblMaxLifes;
    //--------------------------------------------------------------------------------

    #endregion

    #region Données membres

    private Test1_GameManager _gameManager;            // Controleur qu'on va utiliser pour affecter la vie du joueur. Mouhahahaha!
    private GameObject[] _arrHeartsObj;             // Contient la liste des coeurs qu'on crée pour afficher la vie du joueur.

    #endregion

    void Start ()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<Test1_GameManager>();
        createHeatsOnScreen();
    }
	
	void Update ()
    {
        if (_gameManager != null)
        {
            UpdateHeartsDisplay();
            ShowDangerUI();

            if (!Test1_GameManager.IsGameFreeze)
            {
                UpdateUIDebug();
            }                
        }        
    }

    /// <summary>
    /// Pour affiché la vie, on va créer un gameobject pour chaque coeur que le joueur peut avoir. On va ajuster plus tard le sprite affiché.
    /// </summary>
    private void createHeatsOnScreen()
    {
        // On supprime les coeurs qui y sont déjà présent.
        int nbHearts = PanelHearts.transform.childCount;
        for (int ind = 0; ind < nbHearts; ind++)
        {
            Destroy(PanelHearts.transform.GetChild(ind).gameObject);
        }
           

        int espLeft = 45;
        int espTop = 40;
        int espX = 10;
        int espY = 10;

        int width = 48;
        int height = 48;

        List<GameObject> lstHearts = new List<GameObject>();
        int nbCoueurAAfficher = _gameManager.PlayerMaxLife / 10;
        int nbCoeurParRange = Test1_GameManager.NUMBER_MAX_HEARTS / 2;
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
            int nbFullLife = _gameManager.PlayerCurrentLife / 10;
            int nbRest = _gameManager.PlayerCurrentLife - (nbFullLife * 10);

            if (indHeart <= nbFullLife)
            {
                // C'est sûr que c'est un coeur plein.
                posIconInTexture = _gameManager.NumberHeartParts;
            }
            else if (indHeart > (nbFullLife + (nbRest > 0 ? 1 : 0)))
            {
                // C'est sûr que c'est un coeur vide.
                posIconInTexture = 0;
            }
            else
            {
                posIconInTexture = (int)(nbRest / 10.0f * _gameManager.NumberHeartParts);
            }

            heart.GetComponent<Image>().sprite = _gameManager.Heart_Sprites[posIconInTexture];
            heart.GetComponent<Animator>().SetBool("IsCurrentHeart", indHeart == (nbFullLife + (nbRest > 0 ? 1 : 0)));
        }
    }

    /// <summary>
    /// Permet d'afficher des barres rouges sur le contour de l'écran pour indiquer que la vie du joueur commence à être bas.
    /// On l'affiche si nécessaire.
    /// </summary>
    private void ShowDangerUI()
    {
        Animator anmDanger = PanelDanger.GetComponent<Animator>();
        float rateHighDanger = _gameManager.PlayerMaxLife * 0.12f;  // < 12%
        float rateLowDanger = _gameManager.PlayerMaxLife * 0.25f;   // < 25%

        if (_gameManager.PlayerCurrentLife <= rateHighDanger)
        {
            anmDanger.SetInteger("EtatDanger", 2);  // Grand danger, on va afficher l'animation rapide.
            _arrHeartsObj[0].GetComponent<Animator>().speed = 1.8f;
        }            
        else if (_gameManager.PlayerCurrentLife <= rateLowDanger)
        {
            anmDanger.SetInteger("EtatDanger", 1);  // il y a danger, on va afficher l'animation lente.
            _arrHeartsObj[0].GetComponent<Animator>().speed = 1.0f;
        }            
        else
        {
            anmDanger.SetInteger("EtatDanger", 0);  // état idle.
            _arrHeartsObj[0].GetComponent<Animator>().speed = 1.0f;
        }            
    }

    #region Debug

    /// <summary>
    /// Permet d'afficher les informations de debug (current life et max life).
    /// </summary>
    private void UpdateUIDebug()
    {
        float currentPlayerLife = (_gameManager.PlayerCurrentLife / 10.0f);
        int maxPlayerLife = (_gameManager.PlayerMaxLife / 10);
        int maxLifeEver = Test1_GameManager.NUMBER_MAX_HEARTS;

        LblCurrentLife.text = currentPlayerLife.ToString("0.0");
        LblMaxLifes.text = maxPlayerLife.ToString();

        BtnAddCurrentLife.interactable = currentPlayerLife < maxPlayerLife;
        BtnRemoveCurrentLife.interactable = currentPlayerLife > 0;
        BtnAddMaxLife.interactable = maxPlayerLife < maxLifeEver;
        BtnRemoveMaxLife.interactable = maxPlayerLife > 0;
    }

    public void AddCurrentLife()
    {
        _gameManager.PlayerHeal();
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
