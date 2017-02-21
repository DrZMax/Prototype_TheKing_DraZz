using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test2_UIController : MonoBehaviour {

    #region Variables publiques

    public GameObject HeartPrefab;                  // Gameobjet contenant un coeur (contient un spriteRenderer et une animation).
    public GameObject PanelHearts;                  // Où on va affiché les coeurs.
    public GameObject PanelDanger;                  // Panel qui va prévenir l'utilisateur qu'il ne reste plus beaucoup de vie.
    public GameObject ImageItemUI;

    //--------------------------------------------------------------------------------
    // DEBUG - À retirer quand on va utiliser la classe pour de vrai ;)
    public Button BtnAddCurrentLife;
    public Button BtnRemoveCurrentLife;
    public Text LblCurrentLife;

    public Button BtnAddMaxLife;
    public Button BtnRemoveMaxLife;
    public Text LblMaxLifes;

    public GameObject PanelDebugPS4;
    public GameObject PanelDebugXBox;
    public GameObject PanelDebugOther;
    //--------------------------------------------------------------------------------

    #endregion

    #region Données membres

    private Test2_GameManager _gameManager;            // Controleur qu'on va utiliser pour affecter la vie du joueur. Mouhahahaha!
    private GameObject[] _arrHeartsObj;             // Contient la liste des coeurs qu'on crée pour afficher la vie du joueur.
    private bool _isDebugDisplayed;                 // Indique si les informations de debug (Touches PS3/PS4 ou XBox ou autres variables). Le joueur peut appuyer sur F3 pour les afficher/cacher.
    private Test2_InputManager _inputManager;
    private Test2_ItemsController _itemsController;

    #endregion

    private void Start()
    {
        _isDebugDisplayed = false;

        _gameManager = GameObject.Find("GameManager").GetComponent<Test2_GameManager>();
        _inputManager = GameObject.Find("GameManager").GetComponent<Test2_InputManager>();
        _itemsController = GameObject.Find("GameManager").GetComponent<Test2_ItemsController>();
        createHeatsOnScreen();
    }
	
	private void Update()
    {
        if (_gameManager != null)
        {
            UpdateHeartsDisplay();
            updateButtonsDisplay();
            ShowDangerUI();

            if (!Test2_GameManager.IsGameFreeze)
            {
                UpdateUIDebug();
            }                
        }

        manageDisplayInfosDebug();
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
        int nbCoeurParRange = Test2_GameManager.NUMBER_MAX_HEARTS / 2;
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

    /// <summary>
    /// Permet d'afficher le bon item sur le bouton
    /// </summary>
    private void updateButtonsDisplay()
    {
        Color colorBlue = new Color32(60, 120, 255, 255);
        Color colorRed = new Color32(255, 0, 0, 255);
        Color colorGreen = new Color32(0, 220, 50, 255);
        Color colorWhite = new Color32(255, 255, 255, 255);
        Color colorInactive = new Color32(165, 165, 165, 255);
        Color colorTransparent = new Color32(255, 255, 255, 0);
        Color colorBlueSemiTransparent = new Color32(60, 120, 255, 125);
        Color colorRedSemiTransparent = new Color32(255, 0, 0, 125);
        Color colorGreenSemiTransparent = new Color32(0, 220, 50, 125);
        Color colorWhiteSemiTransparent = new Color32(255, 255, 255, 125);

        Test2_ItemsController.Item currentItem = _itemsController.GetCurrentItem();

        if (currentItem.ID == -1 || (currentItem.State != Test2_ItemsController.enmEtatItem.Actif && currentItem.State != Test2_ItemsController.enmEtatItem.Inactif))
        {
            // Il n'y a pas d'item courant.
            ImageItemUI.transform.Find("imgItem").GetComponent<Image>().color = colorTransparent;
            ImageItemUI.transform.Find("imgItem/imgBackgroundAmmo").GetComponent<Image>().color = colorTransparent;
            ImageItemUI.transform.Find("imgItem/txtNbAmmo").GetComponent<Text>().color = colorTransparent;
        }
        else
        {
            ImageItemUI.transform.Find("imgItem").GetComponent<Image>().color = colorWhite;
            ImageItemUI.transform.Find("imgItem").GetComponent<Image>().sprite = currentItem.Image;
            ImageItemUI.transform.Find("imgItem/txtNbAmmo").GetComponent<Text>().text = currentItem.NbItem.ToString();

            if (currentItem.NbMaxItem <= 0)
            {
                ImageItemUI.transform.Find("imgItem/imgBackgroundAmmo").GetComponent<Image>().color = colorTransparent;
                ImageItemUI.transform.Find("imgItem/txtNbAmmo").GetComponent<Text>().color = colorTransparent;
            }                
            else
            {
                ImageItemUI.transform.Find("imgItem/imgBackgroundAmmo").GetComponent<Image>().color = (currentItem.State == Test2_ItemsController.enmEtatItem.Actif ? colorWhite : colorWhiteSemiTransparent);
                if (currentItem.NbItem == 0)
                    ImageItemUI.transform.Find("imgItem/txtNbAmmo").GetComponent<Text>().color = (currentItem.State == Test2_ItemsController.enmEtatItem.Actif ? colorRed : colorRedSemiTransparent);
                else if (currentItem.NbItem == currentItem.NbMaxItem)
                    ImageItemUI.transform.Find("imgItem/txtNbAmmo").GetComponent<Text>().color = (currentItem.State == Test2_ItemsController.enmEtatItem.Actif ? colorGreen : colorGreenSemiTransparent);
                else
                    ImageItemUI.transform.Find("imgItem/txtNbAmmo").GetComponent<Text>().color = (currentItem.State == Test2_ItemsController.enmEtatItem.Actif ? colorBlue : colorBlueSemiTransparent);
            }
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
        int maxLifeEver = Test2_GameManager.NUMBER_MAX_HEARTS;

        LblCurrentLife.text = currentPlayerLife.ToString("0.0");
        LblMaxLifes.text = maxPlayerLife.ToString();

        BtnAddCurrentLife.interactable = currentPlayerLife < maxPlayerLife;
        BtnRemoveCurrentLife.interactable = currentPlayerLife > 0;
        BtnAddMaxLife.interactable = maxPlayerLife < maxLifeEver;
        BtnRemoveMaxLife.interactable = maxPlayerLife > 0;
    }

    /// <summary>
    /// Permet d'afficher ou non les informations de debug. On va afficher les informations sur un controlleur seulement si on détecte que ce controlleur est branché.
    /// </summary>
    private void manageDisplayInfosDebug()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            _isDebugDisplayed = !_isDebugDisplayed;
        }

        bool isPS4Pluged = false;
        bool isXBoxPluged = false;
        foreach (string controller in Input.GetJoystickNames())
        {
            if (controller.ToLower().Trim() == "wireless controller")
                isPS4Pluged = true;
            else if (controller.ToLower().Trim().Contains("xbox"))
                isXBoxPluged = true;
        }

        PanelDebugPS4.SetActive(_isDebugDisplayed && isPS4Pluged);
        PanelDebugXBox.SetActive(_isDebugDisplayed && isXBoxPluged);
        PanelDebugOther.SetActive(_isDebugDisplayed);

        if (_isDebugDisplayed && isPS4Pluged)
            displayPS4Debug();

        if (_isDebugDisplayed && isXBoxPluged)
            displayXBoxDebug();
    }

    /// <summary>
    /// Permet d'afficher les informations sur les touches. Permet de savoir s'ils sont bien mappé et voir l'effet de la pression sur certaine touche.
    /// </summary>
    private void displayPS4Debug()
    {
        string strDebugPS4 = "";
        strDebugPS4 += "  -> Button X: " + Input.GetButton("PS4_X").ToString() + "\r\n";
        strDebugPS4 += "  -> Button O: " + Input.GetButton("PS4_O").ToString() + "\r\n";
        strDebugPS4 += "  -> Button Square: " + Input.GetButton("PS4_Square").ToString() + "\r\n";
        strDebugPS4 += "  -> Button Triangle: " + Input.GetButton("PS4_Triangle").ToString() + "\r\n";
        strDebugPS4 += "\r\n";
        strDebugPS4 += "  -> Button L1: " + Input.GetButton("PS4_L1").ToString() + "\r\n";
        strDebugPS4 += "  -> Button L2: " + Input.GetAxis("PS4_L2").ToString("0.0000") + "\r\n";
        strDebugPS4 += "  -> Button L3: " + Input.GetButton("PS4_L3").ToString() + "\r\n";
        strDebugPS4 += "  -> Button R1: " + Input.GetButton("PS4_R1").ToString() + "\r\n";
        strDebugPS4 += "  -> Button R2: " + Input.GetAxis("PS4_R2").ToString("0.0000") + "\r\n";
        strDebugPS4 += "  -> Button R3: " + Input.GetButton("PS4_R3").ToString() + "\r\n";
        strDebugPS4 += "\r\n";
        strDebugPS4 += "  -> DPad (Horizontal): " + Input.GetAxisRaw("PS4_DPadHorizontal").ToString() + "\r\n";
        strDebugPS4 += "  -> DPad (Vertical): " + Input.GetAxisRaw("PS4_DPadVertical").ToString() + "\r\n";
        strDebugPS4 += "\r\n";
        strDebugPS4 += "  -> Left Analog Joystick (Horizontal): " + Input.GetAxis("Horizontal").ToString("0.0000") + "\r\n";
        strDebugPS4 += "  -> Left Analog Joystick (Vertical): " + Input.GetAxis("Vertical").ToString("0.0000") + "\r\n";
        strDebugPS4 += "\r\n";
        strDebugPS4 += "  -> Rigth Analog Joystick (Horizontal): " + Input.GetAxis("PS4_RightAnalogHorizontal").ToString("0.00") + "\r\n";
        strDebugPS4 += "  -> Rigth Analog Joystick (Vertical): " + Input.GetAxis("PS4_RigthAnalogVertical").ToString("0.00") + "\r\n";
        strDebugPS4 += "\r\n";
        strDebugPS4 += "  -> Options: " + Input.GetButton("PS4_Options").ToString() + "\r\n";
        strDebugPS4 += "  -> Share: " + Input.GetButton("PS4_Share").ToString() + "\r\n";
        strDebugPS4 += "  -> PSN: " + Input.GetButton("PS4_PSN").ToString() + "\r\n";
        strDebugPS4 += "  -> Touch pad: " + Input.GetButton("PS4_Touch").ToString() + "\r\n";
        strDebugPS4 += "\r\n";

        PanelDebugPS4.transform.Find("lblDebug").GetComponent<Text>().text = strDebugPS4;
    }

    /// <summary>
    /// Permet d'afficher les informations sur les touches. Permet de savoir s'ils sont bien mappé et voir l'effet de la pression sur certaine touche.
    /// </summary>
    private void displayXBoxDebug()
    {
        string strDebugXBox = "";
        strDebugXBox += "  -> Button A: " + Input.GetButton("XBox_A").ToString() + "\r\n";
        strDebugXBox += "  -> Button B: " + Input.GetButton("XBox_B").ToString() + "\r\n";
        strDebugXBox += "  -> Button X: " + Input.GetButton("XBox_X").ToString() + "\r\n";
        strDebugXBox += "  -> Button Y: " + Input.GetButton("XBox_Y").ToString() + "\r\n";
        strDebugXBox += "\r\n";
        strDebugXBox += "  -> Button LB: " + Input.GetButton("XBox_LB").ToString() + "\r\n";
        strDebugXBox += "  -> Button LT: " + Input.GetAxis("XBox_LT").ToString("0.0000") + "\r\n";
        strDebugXBox += "  -> Button L: " + Input.GetButton("XBox_L").ToString() + "\r\n";
        strDebugXBox += "  -> Button RB: " + Input.GetButton("XBox_RB").ToString() + "\r\n";
        strDebugXBox += "  -> Button RT: " + Input.GetAxis("XBox_RT").ToString("0.0000") + "\r\n";
        strDebugXBox += "  -> Button R: " + Input.GetButton("XBox_R").ToString() + "\r\n";
        strDebugXBox += "\r\n";
        strDebugXBox += "  -> DPad (Horizontal): " + Input.GetAxisRaw("XBox_DPadHorizontal").ToString() + "\r\n";
        strDebugXBox += "  -> DPad (Vertical): " + Input.GetAxisRaw("XBox_DPadVertical").ToString() + "\r\n";
        strDebugXBox += "\r\n";
        strDebugXBox += "  -> Left Analog Joystick (Horizontal): " + Input.GetAxis("Horizontal").ToString("0.0000") + "\r\n";
        strDebugXBox += "  -> Left Analog Joystick (Vertical): " + Input.GetAxis("Vertical").ToString("0.0000") + "\r\n";
        strDebugXBox += "\r\n";
        strDebugXBox += "  -> Rigth Analog Joystick (Horizontal): " + Input.GetAxis("XBox_RightAnalogHorizontal").ToString("0.00") + "\r\n";
        strDebugXBox += "  -> Rigth Analog Joystick (Vertical): " + Input.GetAxis("XBox_RightAnalogVertical").ToString("0.00") + "\r\n";
        strDebugXBox += "\r\n";
        strDebugXBox += "  -> Back: " + Input.GetButton("XBox_Back").ToString() + "\r\n";
        strDebugXBox += "  -> Start: " + Input.GetButton("XBox_Start").ToString() + "\r\n";
        strDebugXBox += "  -> Home: " + Input.GetButton("XBox_Home").ToString() + " (note: Disabled on Windows lol)\r\n";
        strDebugXBox += "\r\n";

        PanelDebugXBox.transform.Find("lblDebug").GetComponent<Text>().text = strDebugXBox;
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
