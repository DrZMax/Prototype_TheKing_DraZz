/* Février 2017
 * Auteur: Mathieu Tremblay
 * 
 * Ce qu'il y a à améliorer:
 *   -> Le controlleur d'Item devrait s'occuper de contrôler tous ce qui a rapport à l'affichage du menu et à l'intéraction du joueur.
 *   -> C'est le GameManager qui devrait savoir quels items le joueur a en sa possession, mais c'est le Controlleur qui devrait savoir quels sont les items.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MenuController : MonoBehaviour {

    private static enmMenuTab _lastTabOpen;                             // Quand l'utilisateur va ouvrir le menu, on va lui afficher le dernier onglet qu'il avait consulté. On veut 
                                                                        //   donc garder cette information en mémoire.

    public GameObject PanelMenu;                                        // Le menu en tant que tel.

    // Pour le menu Items
    public GameObject Items_ItemTop;
    public GameObject Items_ItemLeft;
    public GameObject Items_ItemRight;
    public GameObject Items_ItemDown;
    public GameObject[] Items_Items;

    /// <summary>
    /// Représente tous les onglets du menu.
    /// </summary>
    private enum enmMenuTab
    {
        N_A = 0,
        Gear = 1,
        Map = 2,
        Item = 3
    }

    /// <summary>
    /// Le Save est un peu particulier, parce que le popup apparaît par dessus le menu. On veut s'assurer de gérer les bons Input aux bonnes places (dans le Save ou dans le menu?).
    /// </summary>
    private enum enmMenuStep
    {
        N_A = 0,                            // Le menu n'est pas affiché.
        MenuShow = 1,
        Save_AskConfirm = 2,
        Save_ShowLoading = 3,
        Save_ShowSuccess= 4
    }

    

    private bool _isMenuShown;                  // Indique si le menu est affiché.
    private enmMenuTab _currentTab;             // Indique dans quel menu on est présentement.
    private enmMenuStep _currentStep;
    private GameObject _tabGear;
    private GameObject _tabMap;
    private GameObject _tabItems;
    private GameObject _pnnSave;
    private Text _lblInfosDebugPS4;
    private Text _lblInfosDebugVariables;

    // Indique si on est en train de changer de tab (si on maintient L1 enfoncer, on veut pas changer de tab à chaque tick de l'Update()).
    private bool _isSaveShown = false;
    private bool _isDPadXButtonPressed = false;
    private bool _isDPadYButtonPressed = false;
    private float _timeRemainSave = 0.0f;

    // Section Items
    private int _indItemWithCursor = 0;                 // Indice de l'item où se trouve le curseur. -1 = Haut, -2 = Bas, -3 = Gauche, -4 = Droite, >=0 = Sur un item dans la section de droite. Par défaut, le premier item de 
                                                        //    la section de droite est sélectionné.
    private int _indItemSelect1 = int.MinValue;         // Indice de l'item sélectionner en premier. C'est les mêmes indices, mais int.MinValue ça signifie qu'il n'y a aucun item de sélectionner.
    private int _indItemSelect2 = int.MinValue;         // Indice de l'item sélectionner en deuxième. C'est les mêmes indices, mais int.MinValue ça signifie qu'il n'y a aucun item de sélectionner.

    private UI_MenuItemsController _itemsController;
    


    private void Awake()
    {
        // Pas sûr de mon affaire ici, à tester! Le but c'est que si on change de scène et qu'on crée une nouvelle instance de MenuController, vu que la variable est static, ça devrait s'en souvenir...
        if (_lastTabOpen == enmMenuTab.N_A)
            _lastTabOpen = enmMenuTab.Gear;

        _currentStep = enmMenuStep.N_A;

        _currentTab = _lastTabOpen;
    }

    private void Start()
    {
        _isMenuShown = false;

        _tabGear = PanelMenu.transform.Find("Gear").gameObject;
        _tabMap = PanelMenu.transform.Find("Map").gameObject;
        _tabItems = PanelMenu.transform.Find("Items").gameObject;
        _pnnSave = PanelMenu.transform.Find("Save").gameObject;
        _lblInfosDebugPS4 = GameObject.Find("lblInfosDebugPS4").GetComponent<Text>();
        _lblInfosDebugVariables = GameObject.Find("lblInfosDebugVariables").GetComponent<Text>();
        _itemsController = GameObject.Find("ItemsController").GetComponent<UI_MenuItemsController>();
    }
	
	private void Update()
    {
        manageShowHideMenu();

        // Si le menu est affiché, on va vérifier les touches appuyer par l'utilisateur, pour naviguer dans le menu.
        if (_isMenuShown)
        {
            managePlayerChangeTab();

            managePlayerSave();

            switch (_currentTab)
            {
                case enmMenuTab.Gear: manageMenuGear(); break;
                case enmMenuTab.Item: manageMenuItems(); break;
                case enmMenuTab.Map: manageMenuMap(); break;
            }
        }

        manageMenuDisplay();

        LogInfoDebug();
    }

    private void toggleMenuDisplay()
    {
        _isMenuShown = !_isMenuShown;

        _currentStep = (_isMenuShown ? enmMenuStep.MenuShow : enmMenuStep.N_A);

        manageMenuDisplay();
    }

    

    private void changeToNextTab()
    {
        switch (_currentTab)
        {
            default:
            case enmMenuTab.Gear: { _currentTab = enmMenuTab.Map; } break;
            case enmMenuTab.Map: { _currentTab = enmMenuTab.Item; } break;
            case enmMenuTab.Item: { _currentTab = enmMenuTab.Gear; } break;
        }

        _indItemWithCursor = 0;
    }

    private void changeToPreviousTab()
    {
        switch (_currentTab)
        {
            default:
            case enmMenuTab.Gear: { _currentTab = enmMenuTab.Item; } break;
            case enmMenuTab.Map: { _currentTab = enmMenuTab.Gear; } break;
            case enmMenuTab.Item: { _currentTab = enmMenuTab.Map; } break;
        }

        _indItemWithCursor = 0;
    }

    /// <summary>
    /// On va s'assurer d'afficher ou non le menu et d'afficher le bon onglet.
    /// </summary>
    private void manageMenuDisplay()
    {
        if (_tabGear != null && _tabItems != null && _tabMap != null)
        {
            PanelMenu.SetActive(_isMenuShown);

            if (_isMenuShown)
            {
                _tabGear.SetActive(_currentTab == enmMenuTab.Gear || _currentTab == enmMenuTab.N_A);  // Just in case... Question qu'on affiche au moins un menu.
                _tabItems.SetActive(_currentTab == enmMenuTab.Item);
                _tabMap.SetActive(_currentTab == enmMenuTab.Map);
            }

            _pnnSave.SetActive(_isSaveShown);
            if (_isSaveShown)
            {
                _pnnSave.transform.Find("pnnAskConfirmation").gameObject.SetActive(_currentStep == enmMenuStep.Save_AskConfirm);
                _pnnSave.transform.Find("pnnSaving").gameObject.SetActive(_currentStep == enmMenuStep.Save_ShowLoading);
                _pnnSave.transform.Find("pnnSaveSuccessful").gameObject.SetActive(_currentStep == enmMenuStep.Save_ShowSuccess);
            }        
        }
    }

    /// <summary>
    /// Permet de vérifier si le joueur veut afficher ou cacher le menu en appuyant sur Options (ou F1).
    /// Note: Si on affiche le popup de save, on doit terminé ce popup, donc on ne peut pas close le menu tant que le popup de save est affiché.
    /// </summary>
    private void manageShowHideMenu()
    {
        bool isCheckButton = !_isMenuShown || (_currentStep == enmMenuStep.N_A || _currentStep == enmMenuStep.MenuShow);
        
        if (isCheckButton)
        {
            if (Input.GetButtonDown("PS4_Options") || (_isMenuShown && Input.GetButtonDown("PS4_O")))
            {
                toggleMenuDisplay();
            }            
        }
    }

    /// <summary>
    /// On va vérifier si le joueur veut changer de tab en appuyant sur L1 ou R1 (- ou =). Comme on appel Input.GetAxis() et non GetKeyDown(), il faut s'assurer de ne pas changer de menu à chaque tick de l'update, puisque 
    ///   GetAxis retourne une valeur tant que le bouton est enfoncé.
    /// </summary>
    private void managePlayerChangeTab()
    {
        // L'utilisateur appuie sur le bouton pour changer d'onglet (vers la droite). R1 sur PS4, = sur clavier.
        if (Input.GetButtonDown("PS4_R1"))
        {
            changeToNextTab();
        }

        // L'utilisateur appuie sur le bouton pour changer d'onglet (vers la gauche). L1 sur PS4, - sur clavier.
        if (Input.GetButtonDown("PS4_L1"))
        {
            changeToPreviousTab();
        }
    }

    /// <summary>
    /// Lorsqu'on est dans le menu, le joueur peut appuyer sur Triangle pour sauvegarder la partie. On va donc lui afficher une confirmation. S'il appuie sur X, il accepte, on affiche le loading. Sinon, on ferme le popup.
    /// </summary>
    private void managePlayerSave()
    {
        // Si c'est le menu, on va vérifier si on veut afficher le popup de save.
        if (_currentStep == enmMenuStep.MenuShow || _currentStep == enmMenuStep.N_A)
        {
            if (Input.GetAxisRaw("PS4_Triangle") > 0)
            {
                if (!_isSaveShown)
                {
                    _currentStep = enmMenuStep.Save_AskConfirm;
                }

                _isSaveShown = true;
            }
        }
        else if (_currentStep == enmMenuStep.Save_AskConfirm)
        {
            if (Input.GetButtonDown("PS4_X"))
            {
                _currentStep = enmMenuStep.Save_ShowLoading;
                _timeRemainSave = 3.0f;
            }
            else if (Input.GetButtonDown("PS4_O"))
            {
                _isSaveShown = false;
                _currentStep = enmMenuStep.MenuShow;
            }
        }
        else if (_currentStep == enmMenuStep.Save_ShowLoading)
        {
            _timeRemainSave -= Time.deltaTime;
            if (_timeRemainSave <= 0)
            {
                _timeRemainSave = 0.0f;
                _currentStep = enmMenuStep.Save_ShowSuccess;
            }
        }
        else if (_currentStep == enmMenuStep.Save_ShowSuccess)
        {
            if (Input.GetButtonDown("PS4_X"))
            {
                _isSaveShown = false;
                _currentStep = enmMenuStep.MenuShow;
            }
        }     
    }

    private void manageMenuGear()
    {

    }

    /// <summary>
    /// On va supporter deux façons de faire (parce qu'on est cool).
    ///   1) Tu cliques sur un des items dans la section de gauche (Haut, Bas, Droite ou Gauche) et sélectionne un item dans la section de droite ou vice-versa.
    ///   2) Tu cliques seulement sur l'item dans la section de droite, et en maintenant le bouton X, tu appuies sur une des flèches pour assigner l'item à l'emplacement voulu.
    /// </summary>
    private void manageMenuItems()
    {
        if (_itemsController.LstGameItems != null && _itemsController.LstGameItems.Count > 0)
        {
            // On veut se déplacer (mais pas dans le cas qu'on est en train d'appuyer sur X et qu'il y a un item sélectionné).
            if (!(_indItemSelect1 > 0 && _indItemSelect2 == int.MinValue && Input.GetAxisRaw("PS4_X") > 0))
            {
                if (!_isDPadXButtonPressed)
                {
                    if (Input.GetAxisRaw("PS4_DPadHorizontal") < 0 || Input.GetAxisRaw("Horizontal") < 0)
                    {
                        moveCursorToLeft();
                    }
                    else if (Input.GetAxisRaw("PS4_DPadHorizontal") > 0 || Input.GetAxisRaw("Horizontal") > 0)
                    {
                        moveCursorToRight();
                    }
                }
                if (!_isDPadYButtonPressed)
                {
                    if (Input.GetAxisRaw("PS4_DPadVertical") < 0 || Input.GetAxisRaw("Vertical") < 0)
                    {
                        moveCursorVerticaly(1);
                    }
                    else if (Input.GetAxisRaw("PS4_DPadVertical") > 0 || Input.GetAxisRaw("Vertical") > 0)
                    {
                        moveCursorVerticaly(-1);
                    }
                }
            }            

            
            // On sélectionne un item
            if (Input.GetButtonDown("PS4_X"))
            {
                if (_indItemSelect1 == int.MinValue)
                {
                    // Il n'y a aucun item qui est sélectionner pour l'instant. On le sélectionne!
                    _indItemSelect1 = _indItemWithCursor;
                }
                else
                {
                    // Il y avait déjà un item de sélectionner. On a une deuxième sélection.
                    _indItemSelect2 = _indItemWithCursor;
                }

                if (_indItemSelect1 != int.MinValue && _indItemSelect2 != int.MinValue)
                {
                    if (_indItemSelect1 == _indItemSelect2)
                    {
                        // Il a choisi 2x la même affaire, on désélectionne tout.
                        _indItemSelect1 = int.MinValue;
                        _indItemSelect2 = int.MinValue;
                    }
                    else
                    {
                        if (_indItemSelect1 >= 0 && _indItemSelect2 >= 0)
                        {
                            // L'utilisateur a sélectionné deux items. On garde uniquement la dernière sélection.
                            _indItemSelect1 = _indItemWithCursor;
                            _indItemSelect2 = int.MinValue;
                        }
                        else if (_indItemSelect1 < 0 && _indItemSelect2 < 0)
                        {
                            // L'utilisateur a sélectionné deux directions. On va switcher les ID.
                            int IDItem1 = _itemsController.GetItem((UI_MenuItemsController.enmDirection)_indItemSelect1).ID;
                            int IDItem2 = _itemsController.GetItem((UI_MenuItemsController.enmDirection)_indItemSelect2).ID;

                            _itemsController.SetItem((UI_MenuItemsController.enmDirection)_indItemSelect1, IDItem2);
                            _itemsController.SetItem((UI_MenuItemsController.enmDirection)_indItemSelect2, IDItem1);

                            _indItemSelect1 = int.MinValue;
                            _indItemSelect2 = int.MinValue;
                        }
                        else
                        {
                            // L'utilisateur a sélectionné une direction et un item. On veut donc associer un item à une direction.
                            int indDirection = (_indItemSelect1 < 0 ? _indItemSelect1 : _indItemSelect2);
                            int idItem = (_indItemSelect1 >= 0 ? _indItemSelect1 : _indItemSelect2); // Pour l'instant j'assume que l'indice dans le teableau c'est l'ID.

                            _itemsController.SetItem((UI_MenuItemsController.enmDirection)indDirection, idItem);
                            _indItemSelect1 = int.MinValue;
                            _indItemSelect2 = int.MinValue;
                        }
                    }
                }
            }


            // Si le bouton X est toujours enfoncé, c'est qu'on veut assigner l'item à une direction.
            if (_indItemSelect1 >= 0 && _indItemSelect2 == int.MinValue && Input.GetAxisRaw("PS4_X") > 0)
            {
                // L'utilisateur a sélectionner un item. On va vérifier s'il appuie sur une des touches directionnelles.
                if (!_isDPadXButtonPressed && !_isDPadYButtonPressed)
                {
                    bool isChangement = false;
                    if (Input.GetAxisRaw("PS4_DPadHorizontal") < 0)
                    {
                        // On assigne l'item sélectionné à la flèche de gauche.
                        _itemsController.SetItem(UI_MenuItemsController.enmDirection.Left, _indItemSelect1);
                        isChangement = true;
                    }
                    else if (Input.GetAxisRaw("PS4_DPadHorizontal") > 0)
                    {
                        // On assigne l'item sélectionné à la flèche de droite.
                        _itemsController.SetItem(UI_MenuItemsController.enmDirection.Right, _indItemSelect1);
                        isChangement = true;
                    }
                    else if (Input.GetAxisRaw("PS4_DPadVertical") < 0)
                    {
                        // On assigne l'item sélectionné à la flèche du bas.
                        _itemsController.SetItem(UI_MenuItemsController.enmDirection.Down, _indItemSelect1);
                        isChangement = true;
                    }
                    else if (Input.GetAxisRaw("PS4_DPadVertical") > 0)
                    {
                        // On assigne l'item sélectionné à la flèche du haut.
                        _itemsController.SetItem(UI_MenuItemsController.enmDirection.Up, _indItemSelect1);
                        isChangement = true;
                    }

                    if (isChangement)
                    {
                        _indItemWithCursor = _indItemSelect1;
                        _indItemSelect1 = int.MinValue;
                        _indItemSelect2 = int.MinValue;
                    }
                }                
            }
        }

        _isDPadXButtonPressed = Input.GetAxisRaw("PS4_DPadHorizontal") != 0 || Input.GetAxisRaw("Horizontal") != 0;
        _isDPadYButtonPressed = Input.GetAxisRaw("PS4_DPadVertical") != 0 || Input.GetAxisRaw("Vertical") != 0;

        manageMenuItemsDisplay();
    }

    /// <summary>
    /// On gère l'affichage du menu Items.
    /// </summary>
    private void manageMenuItemsDisplay()
    {
        Color colorGreyInaccessible = new Color32(175, 175, 175, 255);

        if (_itemsController.LstGameItems == null || _itemsController.LstGameItems.Count == 0)
        {
            Items_ItemTop.GetComponent<Image>().color = colorGreyInaccessible;
            Items_ItemDown.GetComponent<Image>().color = colorGreyInaccessible;
            Items_ItemLeft.GetComponent<Image>().color = colorGreyInaccessible;
            Items_ItemRight.GetComponent<Image>().color = colorGreyInaccessible;

            PanelMenu.transform.Find("Items/pnnRightPart_Item").gameObject.SetActive(false);
            PanelMenu.transform.Find("Items/pnnRightPart_NoItem").gameObject.SetActive(true);
        }
        else
        {
            //----------------------------------------------------------------------------
            // Left Part
            Items_ItemTop.GetComponent<Image>().color = Color.white;
            Items_ItemDown.GetComponent<Image>().color = Color.white;
            Items_ItemLeft.GetComponent<Image>().color = Color.white;
            Items_ItemRight.GetComponent<Image>().color = Color.white;

            Color colorCursor = GameObject.Find("TitleBorder").gameObject.GetComponent<Image>().color;
            Color colorCursorActive = colorCursor;
            colorCursorActive.a = 1;
            Color colorCursorInactive = colorCursor;
            colorCursorInactive.a = 0;

            Color colorAlreadySelectActive = new Color32(96, 136, 255, 255);
            Color colorAlreadySelectInactive = new Color32(96, 136, 255, 0);

            UI_MenuItemsController.GameItem itemLeft = _itemsController.GetItem(UI_MenuItemsController.enmDirection.Left);
            UI_MenuItemsController.GameItem itemRight = _itemsController.GetItem(UI_MenuItemsController.enmDirection.Right);
            UI_MenuItemsController.GameItem itemTop = _itemsController.GetItem(UI_MenuItemsController.enmDirection.Up);
            UI_MenuItemsController.GameItem itemDown = _itemsController.GetItem(UI_MenuItemsController.enmDirection.Down);

            // On gère l'affichage du curseur sur l'item.
            Items_ItemTop.transform.Find("imgCursorOnIt").gameObject.GetComponent<Image>().color = (_indItemWithCursor == -1 ? colorCursorActive : colorCursorInactive);
            Items_ItemTop.transform.Find("imgItem").gameObject.GetComponent<Image>().sprite = (itemTop.ID != -1 ? itemTop.ImageItem : null);
            Items_ItemTop.transform.Find("imgItem").gameObject.GetComponent<Image>().color = (itemTop.ID != -1 ? new Color32(255, 255, 255, 255) : new Color32(255, 255, 255, 0));
            Items_ItemTop.transform.Find("imgSelectedItem").gameObject.GetComponent<Image>().color = (_indItemSelect1 == -1 || _indItemSelect2 == -1 ? colorCursorActive : colorCursorInactive);

            Items_ItemDown.transform.Find("imgCursorOnIt").gameObject.GetComponent<Image>().color = (_indItemWithCursor == -2 ? colorCursorActive : colorCursorInactive);
            Items_ItemDown.transform.Find("imgItem").gameObject.GetComponent<Image>().sprite = (itemDown.ID != -1 ? itemDown.ImageItem : null);
            Items_ItemDown.transform.Find("imgItem").gameObject.GetComponent<Image>().color = (itemDown.ID != -1 ? new Color32(255, 255, 255, 255) : new Color32(255, 255, 255, 0));
            Items_ItemDown.transform.Find("imgSelectedItem").gameObject.GetComponent<Image>().color = (_indItemSelect1 == -2 || _indItemSelect2 == -2 ? colorCursorActive : colorCursorInactive);

            Items_ItemLeft.transform.Find("imgCursorOnIt").gameObject.GetComponent<Image>().color = (_indItemWithCursor == -3 ? colorCursorActive : colorCursorInactive);
            Items_ItemLeft.transform.Find("imgItem").gameObject.GetComponent<Image>().sprite = (itemLeft.ID != -1 ? itemLeft.ImageItem : null);
            Items_ItemLeft.transform.Find("imgItem").gameObject.GetComponent<Image>().color = (itemLeft.ID != -1 ? new Color32(255, 255, 255, 255) : new Color32(255, 255, 255, 0));
            Items_ItemLeft.transform.Find("imgSelectedItem").gameObject.GetComponent<Image>().color = (_indItemSelect1 == -3 || _indItemSelect2 == -3 ? colorCursorActive : colorCursorInactive);

            Items_ItemRight.transform.Find("imgCursorOnIt").gameObject.GetComponent<Image>().color = (_indItemWithCursor == -4 ? colorCursorActive : colorCursorInactive);
            Items_ItemRight.transform.Find("imgItem").gameObject.GetComponent<Image>().sprite = (itemRight.ID != -1 ? itemRight.ImageItem : null);
            Items_ItemRight.transform.Find("imgItem").gameObject.GetComponent<Image>().color = (itemRight.ID != -1 ? new Color32(255, 255, 255, 255) : new Color32(255, 255, 255, 0));
            Items_ItemRight.transform.Find("imgSelectedItem").gameObject.GetComponent<Image>().color = (_indItemSelect1 == -4 || _indItemSelect2 == -4 ? colorCursorActive : colorCursorInactive);



            //----------------------------------------------------------------------------
            // Rigth Part
            PanelMenu.transform.Find("Items/pnnRightPart_Item").gameObject.SetActive(true);
            PanelMenu.transform.Find("Items/pnnRightPart_NoItem").gameObject.SetActive(false);

            // Pour chaque item dans la section de droite, on va afficher l'image de l'item.
            for (int ind = 0; ind < Items_Items.Length; ind++)
            {
                Items_Items[ind].SetActive(true);

                // On a un item!
                if (ind < _itemsController.LstGameItems.Count && _itemsController.LstGameItems[ind].ImageItem != null)
                {
                    // L'emplacement de l'item sera affiché en blanc.
                    Items_Items[ind].GetComponent<Image>().color = new Color32(255, 255, 255, 255);

                    // Si le curseur se trouve sur l'item, on va afficher le curseur bleu (sinon on ne l'affiche pas).
                    Items_Items[ind].transform.Find("imgCursorOnIt").gameObject.GetComponent<Image>().color = (_indItemWithCursor == ind ? colorCursorActive : colorCursorInactive);

                    // On affiche l'item!
                    Items_Items[ind].transform.Find("imgItem").gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 255);
                    Items_Items[ind].transform.Find("imgItem").gameObject.GetComponent<Image>().sprite = _itemsController.LstGameItems[ind].ImageItem;

                    // On a sélectionner l'item, mais on n'a pas décider encore ce qu'on va faire avec.
                    Items_Items[ind].transform.Find("imgSelectedItem").gameObject.GetComponent<Image>().color = (ind == _indItemSelect1 || ind == _indItemSelect2 ? colorCursorActive : colorCursorInactive);

                    // Si l'item est déjà assigner à une direction, on va affiché un background bleu, sinon pas de background.
                    Items_Items[ind].transform.Find("imgItemAlreadySelected").gameObject.GetComponent<Image>().color = (_itemsController.IsItemChoose(_itemsController.LstGameItems[ind].ID) ? colorAlreadySelectActive : colorAlreadySelectInactive);
                }
                // On n'a pas d'item à cet emplacement.
                else if (ind < _itemsController.LstGameItems.Count)
                {
                    Items_Items[ind].GetComponent<Image>().color = colorGreyInaccessible;

                    Items_Items[ind].transform.Find("imgCursorOnIt").gameObject.GetComponent<Image>().color = colorCursorInactive;

                    Items_Items[ind].transform.Find("imgItem").gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0);
                    Items_Items[ind].transform.Find("imgItem").gameObject.GetComponent<Image>().sprite = null;

                    Items_Items[ind].transform.Find("imgSelectedItem").gameObject.GetComponent<Image>().color = colorCursorInactive;

                    Items_Items[ind].transform.Find("imgItemAlreadySelected").gameObject.GetComponent<Image>().color = colorAlreadySelectInactive;
                }
                else
                {
                    Items_Items[ind].SetActive(false);
                }
            }
        }
    }

    private void manageMenuMap()
    {

    }

    private void LogInfoDebug()
    {
        string strDebugPS4 = "PS4 Controller\r\n";
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
        strDebugPS4 += "  -> DPad (Horizontal): " + Input.GetButton("PS4_DPadHorizontal").ToString() + "\r\n";
        strDebugPS4 += "  -> DPad (Vertical): " + Input.GetButton("PS4_DPadVertical").ToString() + "\r\n";
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
        
        string strDebugVariable = "Variables\r\n";
        strDebugVariable += "isMenuShow: " + _isMenuShown.ToString() + "\r\n";
        strDebugVariable += "\r\n";
        strDebugVariable += "_currentTab: " + _currentTab.ToString() + "\r\n";
        strDebugVariable += "_currentStep: " + _currentStep.ToString() + "\r\n";
        strDebugVariable += "\r\n";
        strDebugVariable += "_isSaveShown: " + _isSaveShown.ToString() + "\r\n";
        strDebugVariable += "_timeRemainSave: " + _timeRemainSave.ToString() + "\r\n";
        strDebugVariable += "_isDPadXButtonPressed: " + _isDPadXButtonPressed.ToString() + "\r\n";
        strDebugVariable += "_indItemWithCursor: " + _indItemWithCursor.ToString() + "\r\n";
        strDebugVariable += "_indItemSelect1: " + _indItemSelect1.ToString() + "\r\n";
        strDebugVariable += "_indItemSelect2: " + _indItemSelect2.ToString() + "\r\n";

        _lblInfosDebugPS4.text = strDebugPS4;
        _lblInfosDebugVariables.text = strDebugVariable;
    }

    private void moveCursorToLeft()
    {
        if (_indItemWithCursor == (int)UI_MenuItemsController.enmDirection.Left)
        {
            // dernier item
            for (int ind = _itemsController.LstGameItems.Count - 1; ind >= 0; ind--)
            {
                if (_itemsController.LstGameItems[ind].ImageItem != null)
                {
                    _indItemWithCursor = ind;
                    break;
                }
            }
        }
        else if (_indItemWithCursor == 0)
        {
            _indItemWithCursor = (int)UI_MenuItemsController.enmDirection.Right;
        }
        else if (_indItemWithCursor < 0)
        {
            _indItemWithCursor = (int)UI_MenuItemsController.enmDirection.Left;
        }
        else
        {
            // item précédent
            int indItem = -1;
            for (int ind = _indItemWithCursor - 1; ind >= 0; ind--)
            {
                if (_itemsController.LstGameItems[ind].ImageItem != null)
                {
                    indItem = ind;
                    break;
                }
            }

            if (indItem != -1)
                _indItemWithCursor = indItem;
            else
                _indItemWithCursor = (int)UI_MenuItemsController.enmDirection.Right;
        }
    }

    private void moveCursorToRight()
    {
        if (_indItemWithCursor == (int)UI_MenuItemsController.enmDirection.Right)
        {
            // premier item
            for (int ind = 0; ind < _itemsController.LstGameItems.Count - 1; ind++)
            {
                if (_itemsController.LstGameItems[ind].ImageItem != null)
                {
                    _indItemWithCursor = ind;
                    break;
                }
            }
        }
        else if (_indItemWithCursor == _itemsController.LstGameItems.Count - 1)
        {
            _indItemWithCursor = (int)UI_MenuItemsController.enmDirection.Left;
        }
        else if (_indItemWithCursor < 0)
        {
            _indItemWithCursor = (int)UI_MenuItemsController.enmDirection.Right;
        }
        else
        {
            // item suivant
            int indItem = -1;
            for (int ind = _indItemWithCursor + 1; ind < _itemsController.LstGameItems.Count - 1; ind++)
            {
                if (_itemsController.LstGameItems[ind].ImageItem != null)
                {
                    indItem = ind;
                    break;
                }
            }

            if (indItem != -1)
                _indItemWithCursor = indItem;
            else
                _indItemWithCursor = (int)UI_MenuItemsController.enmDirection.Left;
        }
    }

    private void moveCursorVerticaly(int facteurVertical)
    {
        if (_indItemWithCursor >= 0)
        {
            int rangeActuel = _indItemWithCursor / 6;
            int colonneActuel = _indItemWithCursor % 6;

            int rangeAChercher = rangeActuel + facteurVertical;
            if (rangeAChercher < 0)
                rangeAChercher = (_itemsController.LstGameItems.Count - 1) / 6;

            int facteurColonne = 0;
            int indTemp = 0;

            int indItemTrouve = -1;
            int cpt = 0;

            int indDebutLigne = -1;
            int indFinLigne = -1;
            while (rangeAChercher != rangeActuel && indItemTrouve == -1)
            {
                cpt++;
                if (cpt > 300)
                    break;

                indDebutLigne = 6 * rangeAChercher;
                indFinLigne = indDebutLigne + 5;

                // S'assurer que si on est sur la dernière ligne, on continue la recherche sur la première ligne.
                if (_itemsController.LstGameItems.Count < 6 * rangeAChercher)
                {
                    rangeAChercher = 0;
                    if (rangeAChercher == rangeActuel)
                        break;
                }

                if (facteurColonne == 0)
                {
                    // C'est l'item directement en dessous.
                    indTemp = rangeAChercher * 6 + colonneActuel;
                    if (indTemp < _itemsController.LstGameItems.Count && _itemsController.LstGameItems[indTemp].ID != -1)
                    {
                        // On a trouvé!
                        indItemTrouve = indTemp;
                        break;
                    }
                    facteurColonne++;
                }
                else
                {
                    indTemp = rangeAChercher * 6 + colonneActuel - (facteurColonne * facteurVertical);
                    if (indTemp >= indDebutLigne && indTemp <= indFinLigne)
                    {
                        if (indTemp < _itemsController.LstGameItems.Count && _itemsController.LstGameItems[indTemp].ID != -1)
                        {
                            // On a trouvé!
                            indItemTrouve = indTemp;
                            break;
                        }
                    }

                    indTemp = rangeAChercher * 6 + colonneActuel + (facteurColonne * facteurVertical);
                    if (indTemp >= indDebutLigne && indTemp <= indFinLigne)
                    {
                        if (indTemp < _itemsController.LstGameItems.Count && _itemsController.LstGameItems[indTemp].ID != -1)
                        {
                            // On a trouvé!
                            indItemTrouve = indTemp;
                            break;
                        }
                    }

                    facteurColonne++;
                    if (facteurColonne >= 6)
                    {
                        facteurColonne = 0;
                        if (facteurVertical < 0)
                            rangeAChercher--;
                        else
                            rangeAChercher++;
                    }
                }
            }

            if (indItemTrouve != -1)
            {
                _indItemWithCursor = indItemTrouve;
            }
        }       
        else
        {
            if (facteurVertical == -1)
            {
                // On veut aller en haut.
                if (_indItemWithCursor == (int)UI_MenuItemsController.enmDirection.Up)
                    _indItemWithCursor = (int)UI_MenuItemsController.enmDirection.Down;
                else
                    _indItemWithCursor = (int)UI_MenuItemsController.enmDirection.Up;
            }
            else
            {
                // On veut aller en bas.
                if (_indItemWithCursor == (int)UI_MenuItemsController.enmDirection.Down)
                    _indItemWithCursor = (int)UI_MenuItemsController.enmDirection.Up;
                else
                    _indItemWithCursor = (int)UI_MenuItemsController.enmDirection.Down;
            }
        } 
    }
}
