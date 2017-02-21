using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test2_SwitchItemController : MonoBehaviour {

    public GameObject ImageItemLeft;
    public GameObject ImageItemRight;
    public GameObject ImageItemTop;    
    public GameObject ImageItemDown;

    public Image ImageArrowLeft;
    public Image ImageArrowRight;
    public Image ImageArrowTop;    
    public Image ImageArrowDown;
    public GameObject PanelSwitchItem;
    public Text TextDebug;

    private Test2_ItemsController _itemController;
    private bool _isPanelSwitchItemShown = false;

    private Test2_ItemsController.Item _itemLeft;
    private Test2_ItemsController.Item _itemRight;
    private Test2_ItemsController.Item _itemTop;    
    private Test2_ItemsController.Item _itemDown;
    private float _remainTime = float.MinValue;

    private Test2_ItemsController.enmDirectionItem _currentDirectionAssignedItem;   // Direction officiel à laquelle est assigné l'item courant.
    private Test2_ItemsController.enmDirectionItem _tempDirectionAssignedItem;      // Direction temporaire (le joueur enfonce une direction, mais ne l'a pas relâché encore).

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

    private Test2_InputManager _inputManager;

    private void Start()
    {
        _isPanelSwitchItemShown = false;
        PanelSwitchItem.SetActive(false);
        _itemController = GameObject.Find("GameManager").GetComponent<Test2_ItemsController>();
        _inputManager = GameObject.Find("GameManager").GetComponent<Test2_InputManager>();
    }
	
	private void Update()
    {
        _itemLeft = _itemController.GetItem(Test2_ItemsController.enmDirectionItem.Left);
        _itemRight = _itemController.GetItem(Test2_ItemsController.enmDirectionItem.Right);
        _itemTop = _itemController.GetItem(Test2_ItemsController.enmDirectionItem.Up);        
        _itemDown = _itemController.GetItem(Test2_ItemsController.enmDirectionItem.Down);

        updateUISwitchItem();

        if (Test2_GameManager.IsGameFreeze)
        {
            PanelSwitchItem.SetActive(false);
        }

        PanelSwitchItem.SetActive(_isPanelSwitchItemShown);

        manageDisplaySwitchItem();
        displayDebug();
    }

    private void updateUISwitchItem()
    {
        _currentDirectionAssignedItem = _itemController.GetDirectionCurrentItem();

        for (int ind = -1; ind >= -4; ind--)
        {
            Test2_ItemsController.enmDirectionItem direction = (Test2_ItemsController.enmDirectionItem)ind;
            GameObject ObjAssignedItem = null;
            Image imageArrow = null;
            switch (direction)
            {
                case Test2_ItemsController.enmDirectionItem.Left:
                    {
                        ObjAssignedItem = ImageItemLeft;
                        imageArrow = ImageArrowLeft;
                    } break;
                case Test2_ItemsController.enmDirectionItem.Right:
                    {
                        ObjAssignedItem = ImageItemRight;
                        imageArrow = ImageArrowRight;
                    } break;
                case Test2_ItemsController.enmDirectionItem.Up:
                    {
                        ObjAssignedItem = ImageItemTop;
                        imageArrow = ImageArrowTop;
                    } break;
                case Test2_ItemsController.enmDirectionItem.Down:
                    {
                        ObjAssignedItem = ImageItemDown;
                        imageArrow = ImageArrowDown;
                    } break;
            }

            Test2_ItemsController.Item item = _itemController.GetItem(direction);

            if (item.ID == -1)
            {
                // Il n'y a aucun item qui est associé à cette direction. On n'affiche pas d'item.
                ObjAssignedItem.transform.Find("imgItem").GetComponent<Image>().color = _colorTransparent;
                ObjAssignedItem.transform.Find("imgItem/imgBackgroundAmmo").GetComponent<Image>().color = _colorTransparent;
                ObjAssignedItem.transform.Find("imgItem/txtNbAmmo").GetComponent<Text>().color = _colorTransparent;
                ObjAssignedItem.transform.Find("imgItemAssigned").GetComponent<Image>().color = _colorTransparent;
            }
            else
            {
                // Il y a un item dans cette direction.

                // On affiche l'image.
                ObjAssignedItem.transform.Find("imgItem").GetComponent<Image>().color = (item.State == Test2_ItemsController.enmEtatItem.Actif ? _colorWhite : _colorWhiteSemiTransparent);

                ObjAssignedItem.transform.Find("imgItem").GetComponent<Image>().sprite = item.Image;
                ObjAssignedItem.transform.Find("imgItem/txtNbAmmo").GetComponent<Text>().text = item.NbItem.ToString();

                // On affiche le nombre d'items (munitions)
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

                ObjAssignedItem.transform.Find("imgItemAssigned").GetComponent<Image>().color = (direction == _itemController.GetDirectionCurrentItem() && item.ID != -1 ? _colorBlue : _colorTransparent);
            }

            // On affiche la couleur des flèches.
            if (item.ID == -1 || (item.State != Test2_ItemsController.enmEtatItem.Actif && item.State != Test2_ItemsController.enmEtatItem.Inactif))
                imageArrow.color = _colorInactive;
            else
                imageArrow.color = (_tempDirectionAssignedItem == direction ? _colorBlue : _colorWhite);
        }
    }

    private void manageDisplaySwitchItem()
    {
        if (!Test2_GameManager.IsGameFreeze)
        {
            if (_isPanelSwitchItemShown)
            {
                if (Input.GetAxisRaw(_inputManager.GetInputName(Test2_InputManager.enmActionController.DPad_Horizontal)) < 0 && (_itemLeft.ID != -1 && _itemLeft.State == Test2_ItemsController.enmEtatItem.Actif))
                    _tempDirectionAssignedItem = Test2_ItemsController.enmDirectionItem.Left;
                else if (Input.GetAxisRaw(_inputManager.GetInputName(Test2_InputManager.enmActionController.DPad_Horizontal)) > 0 && (_itemRight.ID != -1 && _itemRight.State == Test2_ItemsController.enmEtatItem.Actif))
                    _tempDirectionAssignedItem = Test2_ItemsController.enmDirectionItem.Right;
                else if (Input.GetAxisRaw(_inputManager.GetInputName(Test2_InputManager.enmActionController.DPad_Vertical)) > 0 && (_itemTop.ID != -1 && _itemTop.State == Test2_ItemsController.enmEtatItem.Actif))
                    _tempDirectionAssignedItem = Test2_ItemsController.enmDirectionItem.Up;
                else if (Input.GetAxisRaw(_inputManager.GetInputName(Test2_InputManager.enmActionController.DPad_Vertical)) < 0 && (_itemDown.ID != -1 && _itemDown.State == Test2_ItemsController.enmEtatItem.Actif))
                    _tempDirectionAssignedItem = Test2_ItemsController.enmDirectionItem.Down;

                if (Input.GetAxisRaw(_inputManager.GetInputName(Test2_InputManager.enmActionController.DPad_Horizontal)) == 0 && Input.GetAxisRaw(_inputManager.GetInputName(Test2_InputManager.enmActionController.DPad_Vertical)) == 0)
                {
                    if (_remainTime == float.MinValue)
                    {
                        _remainTime = 0.5f;
                        _itemController.SetCurrentItem(_tempDirectionAssignedItem);
                    }                        
                }
                else
                {
                    _remainTime = float.MinValue;
                }
            }
            else
            {
                if (_itemController.NbItemsAssigned() > 0)
                {
                    if (Input.GetAxisRaw(_inputManager.GetInputName(Test2_InputManager.enmActionController.DPad_Horizontal)) != 0 || Input.GetAxisRaw(_inputManager.GetInputName(Test2_InputManager.enmActionController.DPad_Vertical)) != 0)
                    {
                        if (!_isPanelSwitchItemShown)
                        {
                            // On affiche le panneau de sélection d'item.
                            _tempDirectionAssignedItem = _itemController.GetDirectionCurrentItem();
                        }

                        _isPanelSwitchItemShown = true;
                        _remainTime = float.MinValue;
                    }
                }                
            }

            if (_remainTime != float.MinValue)
            {
                _remainTime -= Time.deltaTime;

                if (_remainTime < 0)
                {
                    _remainTime = float.MinValue;
                    _isPanelSwitchItemShown = false;
                }
            }
        }
    }

    private void displayDebug()
    {
        string strDebug = "";

        strDebug += "IsGameFreeze: " + Test2_GameManager.IsGameFreeze.ToString() + "\r\n";
        strDebug += "IsPanelSwitchItemShown: " + _isPanelSwitchItemShown.ToString() + "\r\n";
        strDebug += "_currentDirectionAssignedItem: " + _currentDirectionAssignedItem.ToString() + "\r\n";
        strDebug += "_tempDirectionAssignedItem: " + _tempDirectionAssignedItem.ToString() + "\r\n";
        strDebug += "_itemLeft: " + _itemLeft.ID.ToString() + " (" + _itemLeft.State.ToString() + ")\r\n";
        strDebug += "_itemRight: " + _itemRight.ID.ToString() + " (" + _itemRight.State.ToString() + ")\r\n";
        strDebug += "_itemTop: " + _itemTop.ID.ToString() + " (" + _itemTop.State.ToString() + ")\r\n";
        strDebug += "_itemDown: " + _itemDown.ID.ToString() + " (" + _itemDown.State.ToString() + ")\r\n";

        TextDebug.text = strDebug;
    }
}
