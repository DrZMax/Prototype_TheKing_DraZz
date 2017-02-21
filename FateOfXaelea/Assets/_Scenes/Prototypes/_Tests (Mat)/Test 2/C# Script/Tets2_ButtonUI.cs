using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tets2_ButtonUI : MonoBehaviour {

    public Test2_InputManager.enmActionController ButtonType;

    private Image _imageButton; 
    private Test2_InputManager _inputManager;

	private void Start ()
    {
        _imageButton = gameObject.GetComponent<Image>();
        _inputManager = GameObject.Find("GameManager").GetComponent<Test2_InputManager>();
    }
	
	private void Update ()
    {
        _imageButton.sprite = _inputManager.GetInputSprite(ButtonType);
    }
}
