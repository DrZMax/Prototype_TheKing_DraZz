using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MenuController : MonoBehaviour {

    private enum enmMenuTab
    {
        Gear = 0,
        Map = 1,
        Item = 2
    }

    public GameObject PanelMenu;                // Le menu en tant que tel.

    private bool _isMenuShown;                  // Indique si le menu est affiché.
    private enmMenuTab _currentTab;             // Indique dans quel menu on est présentement.

	private void Start()
    {
        _isMenuShown = false;
        manageMenuDisplay();
    }
	
	private void Update()
    {
		if (Input.GetKeyDown(KeyCode.F1))
        {
            toggleMenuDisplay();
        }
	}

    private void toggleMenuDisplay()
    {
        _isMenuShown = !_isMenuShown;
        manageMenuDisplay();
    }

    private void manageMenuDisplay()
    {
        PanelMenu.SetActive(_isMenuShown);
    }
}
