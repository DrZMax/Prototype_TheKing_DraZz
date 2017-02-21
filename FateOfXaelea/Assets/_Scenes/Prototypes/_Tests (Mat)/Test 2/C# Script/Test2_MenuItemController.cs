using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test2_MenuItemController : MonoBehaviour {

    public GameObject PanelItems;
    public GameObject PanelNoItem;
    public GameObject[] LstobjItem;
    public GameObject ObjItemAssignedLeft;
    public GameObject ObjItemAssignedRight;
    public GameObject ObjItemAssignedTop;
    public GameObject ObjItemAssignedDown;
    public GameObject ObjImageItemsAssigned;
    public GameObject PanelButtons;
    public GameObject PanelMenu;

    private Test2_ItemsController _itemController;
    private Color _colorBlue = new Color32(60, 120, 255, 255);
    private Color _colorRed = new Color32(255, 0, 0, 255);
    private Color _colorGreen = new Color32(0, 220, 50, 255);
    private Color _colorWhite = new Color32(255, 255, 255, 255);
    private Color _colorInactive = new Color32(165, 165, 165, 255);
    private Color _colorTransparent = new Color32(255, 255, 255, 0);
    private Color _colorBlueSemiTransparent = new Color32(60, 120, 255, 125);
    private Color _colorRedSemiTransparent = new Color32(255, 0, 0, 125);
    private Color _colorGreenSemiTransparent = new Color32(0, 220, 50, 125);
    private Color _colorWhiteSemiTransparent = new Color32(255, 255, 255, 125);

    private int _indItemCursor;
    private int _idItemSelect1 = int.MinValue;
    private int _idItemSelect2 = int.MinValue;
    private Test2_MenuController _menuController;

    private bool _isDPadXButtonPressed = false;
    private bool _isDPadYButtonPressed = false;

    private Test2_InputManager _inputManager;

    private void Start()
    {
        _itemController = GameObject.Find("GameManager").GetComponent<Test2_ItemsController>();
        _menuController = PanelMenu.GetComponent<Test2_MenuController>();
        _inputManager = GameObject.Find("GameManager").GetComponent<Test2_InputManager>();
    }
	
	private void Update()
    {
        if (gameObject.activeSelf && _menuController.IsMenuDisplayed)
        {
            displayItemsAssigns();
            displayItems();

            manageInput_MenuItems();
            displayButtonsInfos();
        }        
    }

    /// <summary>
    /// On va afficher les items dans la partie de gauche (items qui ont été assignés à une direction, s'il a lieu).
    /// </summary>
    private void displayItemsAssigns()
    {
        for (int ind = -1; ind >= -4; ind--)
        {
            Test2_ItemsController.enmDirectionItem direction = (Test2_ItemsController.enmDirectionItem)ind;
            GameObject ObjAssignedItem = null;
            Image imgArrow = null;
            switch (direction)
            {
                case Test2_ItemsController.enmDirectionItem.Left:
                    {
                        ObjAssignedItem = ObjItemAssignedLeft;
                        imgArrow = ObjImageItemsAssigned.transform.Find("ArrowLeft").GetComponent<Image>();
                    } break;
                case Test2_ItemsController.enmDirectionItem.Right:
                    {
                        ObjAssignedItem = ObjItemAssignedRight;
                        imgArrow = ObjImageItemsAssigned.transform.Find("ArrowRight").GetComponent<Image>();
                    } break;
                case Test2_ItemsController.enmDirectionItem.Up:
                    {
                        ObjAssignedItem = ObjItemAssignedTop;
                        imgArrow = ObjImageItemsAssigned.transform.Find("ArrowTop").GetComponent<Image>();
                    } break;
                case Test2_ItemsController.enmDirectionItem.Down:
                    {
                        ObjAssignedItem = ObjItemAssignedDown;
                        imgArrow = ObjImageItemsAssigned.transform.Find("ArrowDown").GetComponent<Image>();
                    } break;
            }

            Test2_ItemsController.Item item = _itemController.GetItem(direction);
            ObjAssignedItem.transform.Find("imgCursorOnIt").GetComponent<Image>().color = (_indItemCursor == ind ? _colorBlue : _colorTransparent);
            
            if (item.ID == -1)
            {
                // Il n'y a aucun item qui est associé à cette direction. On n'affiche pas d'item.
                ObjAssignedItem.transform.Find("imgSelectedItem").GetComponent<Image>().color = _colorTransparent;
                ObjAssignedItem.transform.Find("imgItem").GetComponent<Image>().color = _colorTransparent;
                ObjAssignedItem.transform.Find("imgItem/imgBackgroundAmmo").GetComponent<Image>().color = _colorTransparent;
                ObjAssignedItem.transform.Find("imgItem/txtNbAmmo").GetComponent<Text>().color = _colorTransparent;
            }
            else
            {
                ObjAssignedItem.transform.Find("imgSelectedItem").GetComponent<Image>().color = ((int)direction == _idItemSelect1 || (int)direction == _idItemSelect2 ? _colorGreen : _colorTransparent);
                ObjAssignedItem.transform.Find("imgItem").GetComponent<Image>().color = (item.State == Test2_ItemsController.enmEtatItem.Actif ? _colorWhite : _colorWhiteSemiTransparent);

                ObjAssignedItem.transform.Find("imgItem").GetComponent<Image>().sprite = item.Image;
                ObjAssignedItem.transform.Find("imgItem/txtNbAmmo").GetComponent<Text>().text = item.NbItem.ToString();

                if (item.NbMaxItem <= 0)
                    ObjAssignedItem.transform.Find("imgItem/imgBackgroundAmmo").GetComponent<Image>().color = _colorTransparent;
                else
                    ObjAssignedItem.transform.Find("imgItem/imgBackgroundAmmo").GetComponent<Image>().color = (item.State == Test2_ItemsController.enmEtatItem.Actif ? _colorWhite : _colorWhiteSemiTransparent);

                if (item.NbMaxItem <= 0)
                    ObjAssignedItem.transform.Find("imgItem/txtNbAmmo").GetComponent<Text>().color = _colorTransparent;                 
                else if (item.NbItem == 0)
                    ObjAssignedItem.transform.Find("imgItem/txtNbAmmo").GetComponent<Text>().color = (item.State == Test2_ItemsController.enmEtatItem.Actif ? _colorRed : _colorRedSemiTransparent);
                else if (item.NbItem == item.NbMaxItem)
                    ObjAssignedItem.transform.Find("imgItem/txtNbAmmo").GetComponent<Text>().color = (item.State == Test2_ItemsController.enmEtatItem.Actif ? _colorGreen : _colorGreenSemiTransparent);
                else
                    ObjAssignedItem.transform.Find("imgItem/txtNbAmmo").GetComponent<Text>().color = (item.State == Test2_ItemsController.enmEtatItem.Actif ? _colorBlue : _colorBlueSemiTransparent);
            }

            Test2_ItemsController.enmDirectionItem directionCurrentItem = _itemController.GetDirectionCurrentItem();
            if (item.ID == -1 || item.State != Test2_ItemsController.enmEtatItem.Actif)
            {
                imgArrow.color = _colorInactive;
            }
            else if (_itemController.GetDirectionCurrentItem() == direction)
            {
                imgArrow.color = _colorBlue;
            }
            else
            {
                imgArrow.color = _colorWhite;
            }
        }
    }

    /// <summary>
    /// On va afficher les items dans la partie de droite (la liste des items, s'il a lieu).
    /// </summary>
    private void displayItems()
    {
        if (_itemController.GetNbItemDiscover() == 0)
        {
            // Il n'y a aucun item qui a été découvert. On va afficher le panel NoItem.
            PanelNoItem.SetActive(true);
            PanelItems.SetActive(false);
        }
        else
        {
            // Il y a au moins un item ayant été découvert. On affiche les items :)
            PanelNoItem.SetActive(false);
            PanelItems.SetActive(true);

            bool isItemUse = false;
            Test2_ItemsController.Item itemInList;
            GameObject objItem;
            for (int ind = 0; ind < _itemController.LstItems.Count; ind++)
            {
                if (LstobjItem.Length > ind)
                {
                    objItem = LstobjItem[ind];
                    itemInList = _itemController.LstItems[ind];

                    isItemUse = itemInList.State != Test2_ItemsController.enmEtatItem.JamaisUtilise;
                    objItem.SetActive(isItemUse);

                    // Si le joueur n'a pas découvert encore l'item, on n'aura pas d'item à afficher. On va quand même afficher l'emplacement, mais grisé.
                    if (itemInList.State == Test2_ItemsController.enmEtatItem.PasDecouvert || itemInList.ID == -1)
                    {
                        // L'item n'est pas encore découvert. On n'affiche uniquement l'emplacement grisé.
                        objItem.GetComponent<Image>().color = _colorInactive;
                        objItem.transform.Find("imgSelectedItem").GetComponent<Image>().color = _colorTransparent;
                        objItem.transform.Find("imgCursorOnIt").GetComponent<Image>().color = _colorTransparent;
                        objItem.transform.Find("imgItemAssigned").GetComponent<Image>().color = _colorTransparent;
                        objItem.transform.Find("imgItem").GetComponent<Image>().color = _colorTransparent;
                        objItem.transform.Find("imgItem/imgBackgroundAmmo").GetComponent<Image>().color = _colorTransparent;
                        objItem.transform.Find("imgItem/txtNbAmmo").GetComponent<Text>().color = _colorTransparent;
                    }
                    else if (itemInList.State == Test2_ItemsController.enmEtatItem.Actif || itemInList.State == Test2_ItemsController.enmEtatItem.Inactif)
                    {
                        // L'item a déjà été découvert. On va l'afficher!
                        objItem.GetComponent<Image>().color = (itemInList.State == Test2_ItemsController.enmEtatItem.Actif ? _colorWhite : _colorInactive);
                        objItem.transform.Find("imgCursorOnIt").GetComponent<Image>().color = (ind == _indItemCursor ? _colorBlue : _colorTransparent);
                        objItem.transform.Find("imgSelectedItem").GetComponent<Image>().color = (itemInList.ID == _idItemSelect1 || itemInList.ID == _idItemSelect2 ? _colorGreen : _colorTransparent);

                        if (_itemController.IsItemAssigned(itemInList.ID))
                            objItem.transform.Find("imgItemAssigned").GetComponent<Image>().color = (itemInList.State == Test2_ItemsController.enmEtatItem.Actif ? _colorBlue : _colorWhiteSemiTransparent);
                        else
                            objItem.transform.Find("imgItemAssigned").GetComponent<Image>().color = _colorTransparent;

                        objItem.transform.Find("imgItem").GetComponent<Image>().color = (itemInList.State == Test2_ItemsController.enmEtatItem.Actif ? _colorWhite : _colorWhiteSemiTransparent);
                        objItem.transform.Find("imgItem").GetComponent<Image>().sprite = itemInList.Image;
                        objItem.transform.Find("imgItem/txtNbAmmo").GetComponent<Text>().text = itemInList.NbItem.ToString();

                        if (itemInList.NbMaxItem <= 0)
                            objItem.transform.Find("imgItem/imgBackgroundAmmo").GetComponent<Image>().color = _colorTransparent;
                        else
                            objItem.transform.Find("imgItem/imgBackgroundAmmo").GetComponent<Image>().color = (itemInList.State == Test2_ItemsController.enmEtatItem.Actif ? _colorWhite : _colorWhiteSemiTransparent);

                        if (itemInList.NbMaxItem <= 0)
                            objItem.transform.Find("imgItem/txtNbAmmo").GetComponent<Text>().color = _colorTransparent;
                        else if (itemInList.NbItem == 0)
                            objItem.transform.Find("imgItem/txtNbAmmo").GetComponent<Text>().color = (itemInList.State == Test2_ItemsController.enmEtatItem.Actif ? _colorRed : _colorRedSemiTransparent);
                        else if (itemInList.NbItem == itemInList.NbMaxItem)
                            objItem.transform.Find("imgItem/txtNbAmmo").GetComponent<Text>().color = (itemInList.State == Test2_ItemsController.enmEtatItem.Actif ? _colorGreen : _colorGreenSemiTransparent);
                        else
                            objItem.transform.Find("imgItem/txtNbAmmo").GetComponent<Text>().color = (itemInList.State == Test2_ItemsController.enmEtatItem.Actif ? _colorBlue : _colorBlueSemiTransparent);
                    }
                }               
            }
        }
    }

    /// <summary>
    /// Permet au joueur d'intéragir dans le menu. On va lui permettre de bouger le curseur, sélectionner l'item A et l'item B ou sélectionner l'item A et tout de suite l'assigner.
    /// </summary>
    private void manageInput_MenuItems()
    {
        // Si on est en train de sauvegarder, c'est le menu controlleur qui doit s'en chargé. La fenêtre est freeze tant que le processus de sauvegarde n'est pas terminé.
        if (!_menuController.IsSavingProcess)
        {
            if (_indItemCursor == int.MinValue)
                _indItemCursor = 0; // On veut que le curseur soit sur un item en tout temps.

            if (_itemController.LstItems != null && _itemController.LstItems.Count > 0)
            {
                // On veut se déplacer (mais pas dans le cas qu'on est en train d'appuyer sur X et qu'il y a un item sélectionné).
                if (!(_idItemSelect1 >= 0 && _idItemSelect2 == int.MinValue && Input.GetAxisRaw(_inputManager.GetInputName(Test2_InputManager.enmActionController.Menu_Apply)) > 0))
                {
                    if (!_isDPadXButtonPressed)
                    {
                        if (Input.GetAxisRaw(_inputManager.GetInputName(Test2_InputManager.enmActionController.DPad_Horizontal)) < 0 || Input.GetAxis("Horizontal") < 0)
                        {
                            moveCursorToLeft();
                        }
                        else if (Input.GetAxisRaw(_inputManager.GetInputName(Test2_InputManager.enmActionController.DPad_Horizontal)) > 0 || Input.GetAxis("Horizontal") > 0)
                        {
                            moveCursorToRight();
                        }
                    }
                    if (!_isDPadYButtonPressed)
                    {
                        if (Input.GetAxisRaw(_inputManager.GetInputName(Test2_InputManager.enmActionController.DPad_Vertical)) < 0 || Input.GetAxis("Vertical") < 0)
                        {
                            moveCursorVerticaly(1);
                        }
                        else if (Input.GetAxisRaw(_inputManager.GetInputName(Test2_InputManager.enmActionController.DPad_Vertical)) > 0 || Input.GetAxis("Vertical") > 0)
                        {
                            moveCursorVerticaly(-1);
                        }
                    }
                }


                // On sélectionne un item
                int currentItemID = (_indItemCursor > 0 ? _itemController.LstItems[_indItemCursor].ID : _indItemCursor);
                if (Input.GetButtonDown(_inputManager.GetInputName(Test2_InputManager.enmActionController.Menu_Apply)))
                {
                    if (_idItemSelect1 == int.MinValue)
                    {
                        // Il n'y a aucun item qui est sélectionner pour l'instant. On le sélectionne!
                        _idItemSelect1 = currentItemID;
                    }
                    else
                    {
                        // Il y avait déjà un item de sélectionner. On a une deuxième sélection.
                        _idItemSelect2 = currentItemID;
                    }

                    if (_idItemSelect1 != int.MinValue && _idItemSelect2 != int.MinValue)
                    {
                        if (_idItemSelect1 == _idItemSelect2)
                        {
                            // Il a choisi 2x la même affaire, on désélectionne tout.
                            _idItemSelect1 = int.MinValue;
                            _idItemSelect2 = int.MinValue;
                        }
                        else
                        {
                            if (_idItemSelect1 >= 0 && _idItemSelect2 >= 0)
                            {
                                // L'utilisateur a sélectionné deux items. On garde uniquement la dernière sélection.
                                _idItemSelect1 = currentItemID;
                                _idItemSelect2 = int.MinValue;
                            }
                            else if (_idItemSelect1 < 0 && _idItemSelect2 < 0)
                            {
                                // L'utilisateur a sélectionné deux directions. On va switcher les ID.
                                int IDItem1 = _itemController.GetItem((Test2_ItemsController.enmDirectionItem)_idItemSelect1).ID;
                                int IDItem2 = _itemController.GetItem((Test2_ItemsController.enmDirectionItem)_idItemSelect2).ID;

                                _itemController.AssignItem((Test2_ItemsController.enmDirectionItem)_idItemSelect1, IDItem2);
                                _itemController.AssignItem((Test2_ItemsController.enmDirectionItem)_idItemSelect2, IDItem1);

                                _idItemSelect1 = int.MinValue;
                                _idItemSelect2 = int.MinValue;
                            }
                            else
                            {
                                // L'utilisateur a sélectionné une direction et un item. On veut donc associer un item à une direction.
                                int indDirection = (_idItemSelect1 < 0 ? _idItemSelect1 : _idItemSelect2);
                                int idItem = (_idItemSelect1 >= 0 ? _idItemSelect1 : _idItemSelect2);

                                _itemController.AssignItem((Test2_ItemsController.enmDirectionItem)indDirection, idItem);
                                _idItemSelect1 = int.MinValue;
                                _idItemSelect2 = int.MinValue;
                            }
                        }
                    }
                }


                // Si le bouton X est toujours enfoncé, c'est qu'on veut assigner l'item à une direction.
                if (_idItemSelect1 >= 0 && _idItemSelect2 == int.MinValue && Input.GetAxisRaw(_inputManager.GetInputName(Test2_InputManager.enmActionController.Menu_Apply)) > 0)
                {
                    // L'utilisateur a sélectionner un item. On va vérifier s'il appuie sur une des touches directionnelles.
                    if (!_isDPadXButtonPressed && !_isDPadYButtonPressed)
                    {
                        bool isChangement = false;
                        if (Input.GetAxisRaw(_inputManager.GetInputName(Test2_InputManager.enmActionController.DPad_Horizontal)) < 0)
                        {
                            // On assigne l'item sélectionné à la flèche de gauche.
                            _itemController.AssignItem(Test2_ItemsController.enmDirectionItem.Left, _idItemSelect1);
                            isChangement = true;
                        }
                        else if (Input.GetAxisRaw(_inputManager.GetInputName(Test2_InputManager.enmActionController.DPad_Horizontal)) > 0)
                        {
                            // On assigne l'item sélectionné à la flèche de droite.
                            _itemController.AssignItem(Test2_ItemsController.enmDirectionItem.Right, _idItemSelect1);
                            isChangement = true;
                        }
                        else if (Input.GetAxisRaw(_inputManager.GetInputName(Test2_InputManager.enmActionController.DPad_Vertical)) < 0)
                        {
                            // On assigne l'item sélectionné à la flèche du bas.
                            _itemController.AssignItem(Test2_ItemsController.enmDirectionItem.Down, _idItemSelect1);
                            isChangement = true;
                        }
                        else if (Input.GetAxisRaw(_inputManager.GetInputName(Test2_InputManager.enmActionController.DPad_Vertical)) > 0)
                        {
                            // On assigne l'item sélectionné à la flèche du haut.
                            _itemController.AssignItem(Test2_ItemsController.enmDirectionItem.Up, _idItemSelect1);
                            isChangement = true;
                        }

                        if (isChangement)
                        {
                            _indItemCursor = _idItemSelect1;
                            _idItemSelect1 = int.MinValue;
                            _idItemSelect2 = int.MinValue;
                        }
                    }
                }

                if (Input.GetButtonDown(_inputManager.GetInputName(Test2_InputManager.enmActionController.Menu_DefineCurrentItem)) && _indItemCursor < 0 && _indItemCursor != int.MinValue)
                {
                    Test2_ItemsController.enmDirectionItem direction = (Test2_ItemsController.enmDirectionItem)_indItemCursor;
                    if (_itemController.GetItem(direction).ID != -1 && _itemController.GetItem(direction).State == Test2_ItemsController.enmEtatItem.Actif)
                    {
                        _itemController.SetCurrentItem(direction);
                    }
                }
            }

            _isDPadXButtonPressed = Input.GetAxisRaw(_inputManager.GetInputName(Test2_InputManager.enmActionController.DPad_Horizontal)) != 0 || Input.GetAxisRaw("Horizontal") != 0;
            _isDPadYButtonPressed = Input.GetAxisRaw(_inputManager.GetInputName(Test2_InputManager.enmActionController.DPad_Vertical)) != 0 || Input.GetAxisRaw("Vertical") != 0;
        }
    }

    /// <summary>
    /// On va aider l'utilisateur en affichant les boutons par rapport aux actions disponibles.
    /// </summary>
    private void displayButtonsInfos()
    {
        // Lorsque le curseur est sur un item dans la partie de gauche, on va permettre à l'utilisateur de spécifier que l'item courant est tel direction.
        Color colorButtonCurrentItem = _colorTransparent;
        Color colorTextCurrentItem = _colorTransparent;
        if (_indItemCursor < 0 && _indItemCursor != int.MinValue)
        {
            Test2_ItemsController.Item itemAssigned = _itemController.GetItem((Test2_ItemsController.enmDirectionItem)_indItemCursor);
            if (itemAssigned.ID != -1)
            {
                colorButtonCurrentItem = _colorWhite;
                colorTextCurrentItem = Color.black;
            }                
        }
        PanelButtons.transform.Find("btnCurrentItem").GetComponent<Image>().color = colorButtonCurrentItem;
        PanelButtons.transform.Find("lblCurrentItem").GetComponent<Text>().color = colorTextCurrentItem;

        // On affiche une info différente lorsque le joueur peut assigner directement un item.
        if (Input.GetButton(_inputManager.GetInputName(Test2_InputManager.enmActionController.Menu_Apply)) && _idItemSelect1 != int.MinValue && _idItemSelect2 == int.MinValue)
        {
            PanelButtons.transform.Find("lblChoose").GetComponent<Text>().text = "Assigner";
            PanelButtons.transform.Find("lblSelect").GetComponent<Text>().text = "(maintenir)";
        }
        else
        {
            PanelButtons.transform.Find("lblChoose").GetComponent<Text>().text = "Déplacer";
            PanelButtons.transform.Find("lblSelect").GetComponent<Text>().text = "Sélectionner";
        }
    }

    private void moveCursorToLeft()
    {
        if (_indItemCursor == (int)Test2_ItemsController.enmDirectionItem.Left)
        {
            // dernier item
            for (int ind = _itemController.LstItems.Count - 1; ind >= 0; ind--)
            {
                if (_itemController.LstItems[ind].ID != -1 && _itemController.LstItems[ind].State == Test2_ItemsController.enmEtatItem.Actif)
                {
                    _indItemCursor = ind;
                    break;
                }
            }
        }
        else if (_indItemCursor == 0)
        {
            _indItemCursor = (int)Test2_ItemsController.enmDirectionItem.Right;
        }
        else if (_indItemCursor < 0)
        {
            _indItemCursor = (int)Test2_ItemsController.enmDirectionItem.Left;
        }
        else
        {
            // item précédent
            int indItem = -1;
            for (int ind = _indItemCursor - 1; ind >= 0; ind--)
            {
                if (_itemController.LstItems[ind].ID != -1 && _itemController.LstItems[ind].State == Test2_ItemsController.enmEtatItem.Actif)
                {
                    indItem = ind;
                    break;
                }
            }

            if (indItem != -1)
                _indItemCursor = indItem;
            else
                _indItemCursor = (int)Test2_ItemsController.enmDirectionItem.Right;
        }
    }

    private void moveCursorToRight()
    {
        if (_indItemCursor == (int)Test2_ItemsController.enmDirectionItem.Right)
        {
            // premier item
            for (int ind = 0; ind < _itemController.LstItems.Count - 1; ind++)
            {
                if (_itemController.LstItems[ind].ID != -1 && _itemController.LstItems[ind].State == Test2_ItemsController.enmEtatItem.Actif)
                {
                    _indItemCursor = ind;
                    break;
                }
            }
        }
        else if (_indItemCursor == _itemController.LstItems.Count - 1)
        {
            _indItemCursor = (int)Test2_ItemsController.enmDirectionItem.Left;
        }
        else if (_indItemCursor < 0)
        {
            _indItemCursor = (int)Test2_ItemsController.enmDirectionItem.Right;
        }
        else
        {
            // item suivant
            int indItem = -1;
            for (int ind = _indItemCursor + 1; ind < _itemController.LstItems.Count - 1; ind++)
            {
                if (_itemController.LstItems[ind].ID != -1 && _itemController.LstItems[ind].State == Test2_ItemsController.enmEtatItem.Actif)
                {
                    indItem = ind;
                    break;
                }
            }

            if (indItem != -1)
                _indItemCursor = indItem;
            else
                _indItemCursor = (int)Test2_ItemsController.enmDirectionItem.Left;
        }
    }

    private void moveCursorVerticaly(int facteurVertical)
    {
        if (_indItemCursor >= 0)
        {
            int rangeActuel = _indItemCursor / 6;
            int colonneActuel = _indItemCursor % 6;

            int rangeAChercher = rangeActuel + facteurVertical;
            if (rangeAChercher < 0)
                rangeAChercher = (_itemController.LstItems.Count - 1) / 6;

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
                if (_itemController.LstItems.Count < 6 * rangeAChercher)
                {
                    rangeAChercher = 0;
                    if (rangeAChercher == rangeActuel)
                        break;
                }

                if (facteurColonne == 0)
                {
                    // C'est l'item directement en dessous.
                    indTemp = rangeAChercher * 6 + colonneActuel;
                    if (indTemp < _itemController.LstItems.Count && _itemController.LstItems[indTemp].ID != -1 && _itemController.LstItems[indTemp].State == Test2_ItemsController.enmEtatItem.Actif)
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
                        if (indTemp < _itemController.LstItems.Count && _itemController.LstItems[indTemp].ID != -1 && _itemController.LstItems[indTemp].State == Test2_ItemsController.enmEtatItem.Actif)
                        {
                            // On a trouvé!
                            indItemTrouve = indTemp;
                            break;
                        }
                    }

                    indTemp = rangeAChercher * 6 + colonneActuel + (facteurColonne * facteurVertical);
                    if (indTemp >= indDebutLigne && indTemp <= indFinLigne)
                    {
                        if (indTemp < _itemController.LstItems.Count && _itemController.LstItems[indTemp].ID != -1 && _itemController.LstItems[indTemp].State == Test2_ItemsController.enmEtatItem.Actif)
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
                _indItemCursor = indItemTrouve;
            }
        }
        else
        {
            if (facteurVertical == -1)
            {
                // On veut aller en haut.
                if (_indItemCursor == (int)Test2_ItemsController.enmDirectionItem.Up)
                    _indItemCursor = (int)Test2_ItemsController.enmDirectionItem.Down;
                else
                    _indItemCursor = (int)Test2_ItemsController.enmDirectionItem.Up;
            }
            else
            {
                // On veut aller en bas.
                if (_indItemCursor == (int)Test2_ItemsController.enmDirectionItem.Down)
                    _indItemCursor = (int)Test2_ItemsController.enmDirectionItem.Up;
                else
                    _indItemCursor = (int)Test2_ItemsController.enmDirectionItem.Down;
            }
        }
    }
}
