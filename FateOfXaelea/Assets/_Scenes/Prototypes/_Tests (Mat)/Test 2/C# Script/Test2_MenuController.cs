using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test2_MenuController : MonoBehaviour {

    public Image IconeTabGear;
    public Image IconeTabMap;
    public Image IconeTabItem;
    public Text TitreTab;

    public GameObject PanelTabGear;
    public GameObject PanelTabMap;
    public GameObject PanelTabItem;

    public GameObject PanelButtonsOption;

    private enum enmMenuTab
    {
        Gear = 0,
        Map = 1,
        Item = 2
    }

    private bool _isMenuDisplayed = false;
    private bool _isSavingProcess = false;
    private enmMenuTab _currentTabDisplayed = enmMenuTab.Item;
    private enmMenuTab _lastTabDisplayed = enmMenuTab.Item;
    private GameObject _menuContainer;
    private Test2_InputManager _inputManager;

    public bool IsMenuDisplayed
    {
        get { return _isMenuDisplayed; }
    }

    public bool IsSavingProcess
    {
        get { return _isSavingProcess; }
    }

    private void Start()
    {
        _isMenuDisplayed = false;
        _menuContainer = gameObject.transform.Find("Canvas/MenuContainer").gameObject;
        _inputManager = GameObject.Find("GameManager").GetComponent<Test2_InputManager>();
    }

    private void Update()
    {
        string openInputName = _inputManager.GetInputName(Test2_InputManager.enmActionController.Menu_Open);
        string closeInputName = _inputManager.GetInputName(Test2_InputManager.enmActionController.Menu_Close);

        // Si le joueur appuie sur la touche "Options" (PS4), on affiche ou cache le menu.
        if (!string.IsNullOrEmpty(openInputName) && !string.IsNullOrEmpty(closeInputName))
        {
            if (Input.GetButtonDown(openInputName) || (_isMenuDisplayed && Input.GetButtonDown(closeInputName)))
            {
                _isMenuDisplayed = !_isMenuDisplayed;
                _currentTabDisplayed = _lastTabDisplayed;
            }
        }            

        // On affiche (ou pas) le menu.
        _menuContainer.SetActive(_isMenuDisplayed);

        // Si le menu est affiché, on met la game en pause.
        Test2_GameManager.IsGameFreeze = _isMenuDisplayed;

        checkForChangeTab();
        updateUIDisplay();
    }

    /// <summary>
    /// On vérifie si le joueur veut changer d'onglet (L1 et R1).
    /// </summary>
    private void checkForChangeTab()
    {
        string changeTabLeftInputName = _inputManager.GetInputName(Test2_InputManager.enmActionController.Menu_ChangeToTabLeft);
        string changeTabRightInputName = _inputManager.GetInputName(Test2_InputManager.enmActionController.Menu_ChangeToTabRight);

        if (!string.IsNullOrEmpty(changeTabLeftInputName) && !string.IsNullOrEmpty(changeTabRightInputName))
        {
            if (Input.GetButtonDown(changeTabLeftInputName))
            {
                // L'utilisateur veut changer d'onglet, vers la gauche.
                switch (_currentTabDisplayed)
                {
                    case enmMenuTab.Gear: _currentTabDisplayed = enmMenuTab.Item; break;
                    case enmMenuTab.Map: _currentTabDisplayed = enmMenuTab.Gear; break;
                    case enmMenuTab.Item: _currentTabDisplayed = enmMenuTab.Map; break;
                }
            }
            else if (Input.GetButtonDown(changeTabRightInputName))
            {
                // L'utilisateur veut changer d'onglet, vers la droite.
                switch (_currentTabDisplayed)
                {
                    case enmMenuTab.Gear: _currentTabDisplayed = enmMenuTab.Map; break;
                    case enmMenuTab.Map: _currentTabDisplayed = enmMenuTab.Item; break;
                    case enmMenuTab.Item: _currentTabDisplayed = enmMenuTab.Gear; break;
                }
            }
        }           
    }

    /// <summary>
    /// Permet d'afficher les boutons à partir des inputs, ainsi que d'afficher le tab sélectionner (icône et titre).
    /// </summary>
    private void updateUIDisplay()
    {
        PanelTabGear.SetActive(_currentTabDisplayed == enmMenuTab.Gear);
        PanelTabMap.SetActive(_currentTabDisplayed == enmMenuTab.Map);
        PanelTabItem.SetActive(_currentTabDisplayed == enmMenuTab.Item);

        PanelButtonsOption.transform.Find("pnnItemOptions").gameObject.SetActive(_currentTabDisplayed == enmMenuTab.Item);
        PanelButtonsOption.transform.Find("pnnMapOptions").gameObject.SetActive(_currentTabDisplayed == enmMenuTab.Map);
        PanelButtonsOption.transform.Find("pnnGearOptions").gameObject.SetActive(_currentTabDisplayed == enmMenuTab.Gear);

        TitreTab.text = _currentTabDisplayed.ToString().ToUpper();

        Color colorBlue = new Color32(65, 120, 255, 255);
        IconeTabGear.color = (_currentTabDisplayed == enmMenuTab.Gear ? colorBlue : Color.white);
        IconeTabMap.color = (_currentTabDisplayed == enmMenuTab.Map ? colorBlue : Color.white);
        IconeTabItem.color = (_currentTabDisplayed == enmMenuTab.Item ? colorBlue : Color.white);
    }
}
