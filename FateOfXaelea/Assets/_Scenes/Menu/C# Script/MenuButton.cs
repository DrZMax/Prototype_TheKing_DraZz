using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public string SceneName;                // Nom de la scène vers laquelle l'utilisateur sera redirigé s'il appuie sur le bouton.
    public string Title;                    // Titre qui sera affiché dans le panel.
    public string Description;              // Description qui sera affichée dans le panel.

    private MenuController _menuController; // Permet d'interagir avec le UI (afficher titre et description, naviguer vers la scène).

    private void Start()
    {
        _menuController = GameObject.Find("MenuController").GetComponent<MenuController>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _menuController.ShowInfos(Title, Description);
    }

    public void OnPointerExit(PointerEventData evenData)
    {
        _menuController.HideInfos();
    }

    public void OnPointerClick(PointerEventData evenData)
    {
        _menuController.GoToScene(SceneName);
    }
}
