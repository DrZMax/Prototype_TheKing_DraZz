using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2_InputManager : MonoBehaviour {

    public enum enmActionController
    {
        InGame_Sword = 0,
        InGame_Item = 1,
        InGame_Action = 2,
        InGame_SwitchItem = 3,
        InGame_Accept = 4,
        InGame_Cancel = 5,
        InGame_Sheild = 6,
        Menu_ChangeToTabLeft = 7,
        Menu_ChangeToTabRight = 8,
        Menu_Save = 9,
        Menu_ZoomIn = 10,
        Menu_ZoomOut = 11,
        Menu_Open = 12,
        Menu_Close = 13,
        Menu_Move = 14,
        Menu_NavigateMap = 15,
        Menu_Apply = 16,
        Menu_DefineCurrentItem = 17,
        DPad_Horizontal = 18,
        DPad_Vertical = 19
    }

    private enum enmPS4Buttons
    {
        PS4_X = 0,
        PS4_O = 1,
        PS4_Triangle = 2,
        PS4_Square = 3,
        PS4_DPad = 4,
        PS4_L1 = 5,
        PS4_L2 = 6,
        PS4_R1 = 7,
        PS4_R2 = 8,
        PS4_LeftJoystick = 9,
        PS4_RightJoystick = 10,
        PS4_Option = 11,
        PS4_Share = 12,
        PS4_TouchPad = 13
    }

    private enum enmXBoxButtons
    {
        XBox_A = 0,
        XBox_B = 1,
        XBox_X = 2,
        XBox_Y = 3,
        XBox_DPad = 4,
        XBox_LB = 5,
        XBox_LT = 6,
        XBox_RB = 7,
        XBox_RT = 8,
        XBox_LeftJoystick = 9,
        XBox_RightJoystick = 10,
        XBox_Back = 11,
        XBox_Start = 12
    }

    private enum enmKeyboardButtons
    {
        Key_0 = 0,
        Key_1 = 1,
        Key_2 = 2,
        Key_3 = 3,
        Key_4 = 4,
        Key_5 = 5,
        Key_6 = 6,
        Key_7 = 7,
        Key_8 = 8,
        Key_9 = 9,
        Key_A = 10,
        Key_B = 11,
        Key_C = 12,
        Key_D = 13,
        Key_E = 14,
        Key_F = 15,
        Key_G = 16,
        Key_H = 17,
        Key_I = 18,
        Key_J = 19,
        Key_K = 20,
        Key_L = 21,
        Key_M = 22,
        Key_N = 23,
        Key_O = 24,
        Key_P = 25,
        Key_Q = 26,
        Key_R = 27,
        Key_S = 28,
        Key_T = 29,
        Key_U = 30,
        Key_V = 31,
        Key_W = 32,
        Key_X = 33,
        Key_Y = 34,
        Key_Z = 35,
        Key_Space = 36,
        Key_Tab = 37,
        Key_Shift = 38,
        Key_Alt = 39,
        Key_Ctrl = 40,
        Key_Delete = 41,
        Key_Backspace = 42,
        Key_Enter = 43,
        Key_Escape = 44,
        Key_Home = 45,
        Key_End = 46,
        Key_Plus = 47,
        Key_PlusLong = 48,
        Key_Minus = 49,
        Key_ArrowLeft = 50,
        Key_ArrowRight = 51,
        Key_ArrowUp = 52,
        Key_ArrowDown = 53,
        Key_BracketLeft = 54,
        Key_BracketRight = 55
    }

    // Note: Les indices utiliser pour ces sprites devraient matcher avec les enums ci-dessus.
    public Sprite[] PS4ControllerButtons;
    public Sprite[] XBoxControllerButtons;
    public Sprite[] KeyBoardButtons;

    private bool _isUsePS4Controller = false;
    private bool _isUseXBox360Controller = false;
    private bool _isUseOnlyKeyboard = false;

    public bool IsUsePS4Controller
    {
        get { return _isUsePS4Controller; }
    }

    public bool IsUseXBoxController
    {
        get { return _isUseXBox360Controller; }
    }

    public bool IsUseKeyBoard
    {
        get { return _isUseOnlyKeyboard; }
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
        bool isPS4Controller = false;
        bool isXBoxController = false;
        bool isAllEmpty = true;
        foreach (string controller in Input.GetJoystickNames())
        {
            if (controller.ToLower().Trim() == "wireless controller")
            {
                isPS4Controller = true;
            }
            else if (controller.ToLower().Trim().Contains("xbox"))
            {
                isXBoxController = true;
            }

            if (!string.IsNullOrEmpty(controller))
                isAllEmpty = false;
        }

        // Just in case, s'il y a une mannette et qu'on s'est pas c'est quoi, on va prendre XBox.
        if (!isAllEmpty && !isPS4Controller)
            isXBoxController = true;

        _isUsePS4Controller = isPS4Controller;
        _isUseXBox360Controller = isXBoxController;
        _isUseOnlyKeyboard = !isPS4Controller && !isXBoxController;


        // Input.GetButton() / GetButtonDown()  - return bool
        // Input.GetKey() / GetKeyDown()        - return bool
        // Input.GetAxis() / GetAxisRaw()       - return float
    }

    /// <summary>
    /// Permet d'obtenir le nom de l'input en fonction de l'action demandé ainsi que du controlleur qui est utilisé.
    /// </summary>
    private void getInputInfos(enmActionController actionController, out KeyCode keyInput, out string nameInput, out Sprite imageInput)
    {
        keyInput = KeyCode.Ampersand;  // Je voulais juste qqc de random puisque ça ne peut pas être null...
        nameInput = "";
        imageInput = null;

        switch (actionController)
        {
            case enmActionController.InGame_Accept:
                {
                    if (_isUsePS4Controller)
                    {
                        nameInput = "PS4_X";
                        imageInput = PS4ControllerButtons[(int)enmPS4Buttons.PS4_X];
                    }
                    else if (_isUseXBox360Controller)
                    {
                        nameInput = "XBox_A";
                        imageInput = XBoxControllerButtons[(int)enmXBoxButtons.XBox_A];
                    }
                    else if (_isUseOnlyKeyboard)
                    {
                        nameInput = "";
                        keyInput = KeyCode.Return;
                        imageInput = KeyBoardButtons[(int)enmKeyboardButtons.Key_Enter];
                    }
                }
                break;
            case enmActionController.InGame_Cancel:
                {
                    if (_isUsePS4Controller)
                    {
                        nameInput = "PS4_O";
                        imageInput = PS4ControllerButtons[(int)enmPS4Buttons.PS4_O];
                    }
                    else if (_isUseXBox360Controller)
                    {
                        nameInput = "XBox_B";
                        imageInput = XBoxControllerButtons[(int)enmXBoxButtons.XBox_B];
                    }
                    else if (_isUseOnlyKeyboard)
                    {
                        nameInput = "";
                        keyInput = KeyCode.Escape;
                        imageInput = KeyBoardButtons[(int)enmKeyboardButtons.Key_Escape];
                    }
                }
                break;
            case enmActionController.InGame_Action:
                {
                    if (_isUsePS4Controller)
                    {
                        nameInput = "PS4_X";
                        imageInput = PS4ControllerButtons[(int)enmPS4Buttons.PS4_X];
                    }
                    else if (_isUseXBox360Controller)
                    {
                        nameInput = "XBox_A";
                        imageInput = XBoxControllerButtons[(int)enmXBoxButtons.XBox_A];
                    }
                    else if (_isUseOnlyKeyboard)
                    {
                        nameInput = "";
                        keyInput = KeyCode.Return;
                        imageInput = KeyBoardButtons[(int)enmKeyboardButtons.Key_Enter];  // À voir
                    }
                }
                break;
            case enmActionController.InGame_Sword:
                {
                    if (_isUsePS4Controller)
                    {
                        nameInput = "PS4_Square";
                        imageInput = PS4ControllerButtons[(int)enmPS4Buttons.PS4_Square];
                    }
                    else if (_isUseXBox360Controller)
                    {
                        nameInput = "XBox_X";
                        imageInput = XBoxControllerButtons[(int)enmXBoxButtons.XBox_X];
                    }
                    else if (_isUseOnlyKeyboard)
                    {
                        nameInput = "";
                        keyInput = KeyCode.LeftShift;
                        imageInput = KeyBoardButtons[(int)enmKeyboardButtons.Key_Shift];  // À voir
                    }
                }
                break;
            case enmActionController.InGame_Item:
                {
                    if (_isUsePS4Controller)
                    {
                        nameInput = "PS4_O";
                        imageInput = PS4ControllerButtons[(int)enmPS4Buttons.PS4_O];
                    }
                    else if (_isUseXBox360Controller)
                    {
                        nameInput = "XBox_B";
                        imageInput = XBoxControllerButtons[(int)enmXBoxButtons.XBox_B];
                    }
                    else if (_isUseOnlyKeyboard)
                    {
                        nameInput = "";
                        keyInput = KeyCode.LeftControl;
                        imageInput = KeyBoardButtons[(int)enmKeyboardButtons.Key_Ctrl];  // À voir
                    }
                }
                break;
            case enmActionController.InGame_SwitchItem:
                {
                    if (_isUsePS4Controller)
                    {
                        nameInput = "";
                        imageInput = PS4ControllerButtons[(int)enmPS4Buttons.PS4_DPad];
                    }
                    else if (_isUseXBox360Controller)
                    {
                        nameInput = "";
                        imageInput = XBoxControllerButtons[(int)enmXBoxButtons.XBox_DPad];
                    }
                    else if (_isUseOnlyKeyboard)
                    {
                        nameInput = "";
                        keyInput = KeyCode.Plus;
                        imageInput = KeyBoardButtons[(int)enmKeyboardButtons.Key_Plus];  // À voir
                    }
                }
                break;
            case enmActionController.Menu_ChangeToTabLeft:
                {
                    if (_isUsePS4Controller)
                    {
                        nameInput = "PS4_L1";
                        imageInput = PS4ControllerButtons[(int)enmPS4Buttons.PS4_L1];
                    }
                    else if (_isUseXBox360Controller)
                    {
                        nameInput = "XBox_LB";
                        imageInput = XBoxControllerButtons[(int)enmXBoxButtons.XBox_LB];
                    }
                    else if (_isUseOnlyKeyboard)
                    {
                        nameInput = "";
                        keyInput = KeyCode.Alpha0;
                        imageInput = KeyBoardButtons[(int)enmKeyboardButtons.Key_0];  // À voir
                    }
                }
                break;
            case enmActionController.Menu_ChangeToTabRight:
                {
                    if (_isUsePS4Controller)
                    {
                        nameInput = "PS4_R1";
                        imageInput = PS4ControllerButtons[(int)enmPS4Buttons.PS4_R1];
                    }
                    else if (_isUseXBox360Controller)
                    {
                        nameInput = "XBox_RB";
                        imageInput = XBoxControllerButtons[(int)enmXBoxButtons.XBox_RB];
                    }
                    else if (_isUseOnlyKeyboard)
                    {
                        nameInput = "";
                        keyInput = KeyCode.Alpha0;
                        imageInput = KeyBoardButtons[(int)enmKeyboardButtons.Key_0];  // À voir
                    }
                }
                break;
            case enmActionController.Menu_Apply:
                {
                    if (_isUsePS4Controller)
                    {
                        nameInput = "PS4_X";
                        imageInput = PS4ControllerButtons[(int)enmPS4Buttons.PS4_X];
                    }
                    else if (_isUseXBox360Controller)
                    {
                        nameInput = "XBox_A";
                        imageInput = XBoxControllerButtons[(int)enmXBoxButtons.XBox_A];
                    }
                    else if (_isUseOnlyKeyboard)
                    {
                        nameInput = "";
                        keyInput = KeyCode.Alpha0;
                        imageInput = KeyBoardButtons[(int)enmKeyboardButtons.Key_0];  // À voir
                    }
                }
                break;
            case enmActionController.Menu_Save:
                {
                    if (_isUsePS4Controller)
                    {
                        nameInput = "PS4_Triangle";
                        imageInput = PS4ControllerButtons[(int)enmPS4Buttons.PS4_Triangle];
                    }
                    else if (_isUseXBox360Controller)
                    {
                        nameInput = "XBox_Y";
                        imageInput = XBoxControllerButtons[(int)enmXBoxButtons.XBox_Y];
                    }
                    else if (_isUseOnlyKeyboard)
                    {
                        nameInput = "";
                        keyInput = KeyCode.Alpha0;
                        imageInput = KeyBoardButtons[(int)enmKeyboardButtons.Key_0];  // À voir
                    }
                }
                break;
            case enmActionController.Menu_Close:
                {
                    if (_isUsePS4Controller)
                    {
                        nameInput = "PS4_O";
                        imageInput = PS4ControllerButtons[(int)enmPS4Buttons.PS4_O];
                    }
                    else if (_isUseXBox360Controller)
                    {
                        nameInput = "XBox_B";
                        imageInput = XBoxControllerButtons[(int)enmXBoxButtons.XBox_B];
                    }
                    else if (_isUseOnlyKeyboard)
                    {
                        nameInput = "";
                        keyInput = KeyCode.Alpha0;
                        imageInput = KeyBoardButtons[(int)enmKeyboardButtons.Key_0];  // À voir
                    }
                }
                break;
            case enmActionController.Menu_Move:
                {
                    if (_isUsePS4Controller)
                    {
                        nameInput = "";
                        imageInput = PS4ControllerButtons[(int)enmPS4Buttons.PS4_DPad];
                    }
                    else if (_isUseXBox360Controller)
                    {
                        nameInput = "";
                        imageInput = XBoxControllerButtons[(int)enmXBoxButtons.XBox_DPad];
                    }
                    else if (_isUseOnlyKeyboard)
                    {
                        nameInput = "";
                        keyInput = KeyCode.Alpha0;
                        imageInput = KeyBoardButtons[(int)enmKeyboardButtons.Key_0];  // À voir
                    }
                }
                break;
            case enmActionController.Menu_DefineCurrentItem:
                {
                    if (_isUsePS4Controller)
                    {
                        nameInput = "PS4_Square";
                        imageInput = PS4ControllerButtons[(int)enmPS4Buttons.PS4_Square];
                    }
                    else if (_isUseXBox360Controller)
                    {
                        nameInput = "XBox_X";
                        imageInput = XBoxControllerButtons[(int)enmXBoxButtons.XBox_X];
                    }
                    else if (_isUseOnlyKeyboard)
                    {
                        nameInput = "";
                        keyInput = KeyCode.Alpha0;
                        imageInput = KeyBoardButtons[(int)enmKeyboardButtons.Key_0];  // À voir
                    }
                }
                break;
            case enmActionController.Menu_ZoomIn:
                {
                    if (_isUsePS4Controller)
                    {
                        nameInput = "PS4_R2";
                        imageInput = PS4ControllerButtons[(int)enmPS4Buttons.PS4_R2];
                    }
                    else if (_isUseXBox360Controller)
                    {
                        nameInput = "XBox_RT";
                        imageInput = XBoxControllerButtons[(int)enmXBoxButtons.XBox_RT];
                    }
                    else if (_isUseOnlyKeyboard)
                    {
                        nameInput = "";
                        keyInput = KeyCode.Alpha0;
                        imageInput = KeyBoardButtons[(int)enmKeyboardButtons.Key_0];  // À voir
                    }
                }
                break;
            case enmActionController.Menu_ZoomOut:
                {
                    if (_isUsePS4Controller)
                    {
                        nameInput = "PS4_L2";
                        imageInput = PS4ControllerButtons[(int)enmPS4Buttons.PS4_L2];
                    }
                    else if (_isUseXBox360Controller)
                    {
                        nameInput = "XBox_LT";
                        imageInput = XBoxControllerButtons[(int)enmXBoxButtons.XBox_LT];
                    }
                    else if (_isUseOnlyKeyboard)
                    {
                        nameInput = "";
                        keyInput = KeyCode.Alpha0;
                        imageInput = KeyBoardButtons[(int)enmKeyboardButtons.Key_0];  // À voir
                    }
                }
                break;
            case enmActionController.Menu_NavigateMap:
                {
                    if (_isUsePS4Controller)
                    {
                        nameInput = "";
                        imageInput = PS4ControllerButtons[(int)enmPS4Buttons.PS4_LeftJoystick];
                    }
                    else if (_isUseXBox360Controller)
                    {
                        nameInput = "";
                        imageInput = XBoxControllerButtons[(int)enmXBoxButtons.XBox_LeftJoystick];
                    }
                    else if (_isUseOnlyKeyboard)
                    {
                        nameInput = "";
                        keyInput = KeyCode.Alpha0;
                        imageInput = KeyBoardButtons[(int)enmKeyboardButtons.Key_0];  // À voir
                    }
                }
                break;
            case enmActionController.Menu_Open:
                {
                    if (_isUsePS4Controller)
                    {
                        nameInput = "PS4_Options";
                        imageInput = PS4ControllerButtons[(int)enmPS4Buttons.PS4_LeftJoystick];
                    }
                    else if (_isUseXBox360Controller)
                    {
                        nameInput = "XBox_Start";
                        imageInput = XBoxControllerButtons[(int)enmXBoxButtons.XBox_LeftJoystick];
                    }
                    else if (_isUseOnlyKeyboard)
                    {
                        nameInput = "";
                        keyInput = KeyCode.Alpha0;
                        imageInput = KeyBoardButtons[(int)enmKeyboardButtons.Key_0];  // À voir
                    }
                } break;
            case enmActionController.DPad_Horizontal:
                {
                    if (_isUsePS4Controller)
                    {
                        nameInput = "PS4_DPadHorizontal";
                        imageInput = PS4ControllerButtons[(int)enmPS4Buttons.PS4_LeftJoystick];
                    }
                    else if (_isUseXBox360Controller)
                    {
                        nameInput = "XBox_DPadHorizontal";
                        imageInput = XBoxControllerButtons[(int)enmXBoxButtons.XBox_LeftJoystick];
                    }
                    else if (_isUseOnlyKeyboard)
                    {
                        nameInput = "";
                        keyInput = KeyCode.Alpha0;
                        imageInput = KeyBoardButtons[(int)enmKeyboardButtons.Key_0];  // À voir
                    }
                } break;
            case enmActionController.DPad_Vertical:
                {
                    if (_isUsePS4Controller)
                    {
                        nameInput = "PS4_DPadVertical";
                        imageInput = PS4ControllerButtons[(int)enmPS4Buttons.PS4_LeftJoystick];
                    }
                    else if (_isUseXBox360Controller)
                    {
                        nameInput = "XBox_DPadVertical";
                        imageInput = XBoxControllerButtons[(int)enmXBoxButtons.XBox_LeftJoystick];
                    }
                    else if (_isUseOnlyKeyboard)
                    {
                        nameInput = "";
                        keyInput = KeyCode.Alpha0;
                        imageInput = KeyBoardButtons[(int)enmKeyboardButtons.Key_0];  // À voir
                    }
                } break;
        }
    }

    public string GetInputName(enmActionController actionController)
    {
        KeyCode inputKeyboard = KeyCode.A;
        string inputName = "";
        Sprite inputSprite = null;
        getInputInfos(actionController, out inputKeyboard, out inputName, out inputSprite);

        return inputName;
    }

    public Sprite GetInputSprite(enmActionController actionController)
    {
        KeyCode keycode = KeyCode.A;
        string nameInput = "";
        Sprite image = null;
        getInputInfos(actionController, out keycode, out nameInput, out image);

        return image;
    }
}
