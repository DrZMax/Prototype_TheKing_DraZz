using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2_ItemsController : MonoBehaviour {

    /// <summary>
    /// Représente un item dans le UI et dans le menu.
    /// </summary>
	public struct Item
    {
        public int ID;
        public string Name;
        public string Description;
        public Sprite Image;
        public enmEtatItem State;
        public int NbItem;
        public int NbMaxItem;
        //ItemPrefab -> Lien vers le prefab de l'item
    }

    public enum enmEtatItem
    {
        Actif = 0,              // Cet item est affiché et peut être sélectionné par l'utilisateur.
        Inactif = 1,            // Cet item a été découvert, mais ne peut être utilisé pour le moment. On l'affiche grisé.
        PasDecouvert = 2,       // N'est pas affiché pour le moment, mais on va affiché un espace réservé pour cet item.
        JamaisUtilise = 3       // Cet item ne sera jamais affiché. On n'affichera donc pas d'emplacement réservé.
    }

    public enum enmDirectionItem
    {
        N_A = 0,
        Right = -1,
        Down = -2,
        Up = -3,
        Left = -4
    }

    public Sprite Item_ImageBow;
    public Sprite Item_ImageBomb;
    public Sprite Item_ImageBombchu;
    public Sprite Item_ImageDekuStick;
    public Sprite Item_ImageGrappin;
    public Sprite Item_ImageTruth;
    public Sprite Item_ImageBoomerang;
    public Sprite Item_ImageBottle;

    private List<Item> _lstItems;
    private Item _itemAssignToLeft;
    private Item _itemAssignToRight;
    private Item _itemAssignToTop;
    private Item _itemAssignToDown;

    private enmDirectionItem _directionItemCurrent;
    private Item _itemEmpty;

    public List<Item> LstItems
    {
        get { return _lstItems; }
    }

    /// <summary>
    /// On va construire notre liste d'item.
    /// </summary>
    private void Awake()
    {
        initialiseItemAssigned();
        initialiseListeItems();
    }

    /// <summary>
    /// On intialise les items assignés.
    /// </summary>
    private void initialiseItemAssigned()
    {
        _directionItemCurrent = enmDirectionItem.N_A;

        _itemEmpty = new Item();
        _itemEmpty.ID = -1;
        _itemEmpty.State = enmEtatItem.PasDecouvert;

        _itemAssignToLeft = _itemEmpty;
        _itemAssignToRight = _itemEmpty;
        _itemAssignToTop = _itemEmpty;
        _itemAssignToDown = _itemEmpty;

        // Si on veut en mettre un par défaut...
        Item item0 = new Item();
        item0.ID = 0;
        item0.Name = "Arc";
        item0.Description = "Permet de lancer des flèches";
        item0.Image = Item_ImageBow;
        item0.State = enmEtatItem.Actif;
        item0.NbItem = 23;
        item0.NbMaxItem = 25;
        _itemAssignToLeft = item0;
        
        _directionItemCurrent = enmDirectionItem.Left;
    }

    /// <summary>
    /// Permet de créer la liste des items qui sera afficher dans le menu (Items).
    /// </summary>
    private void initialiseListeItems()
    {
        _lstItems = new List<Item>();

        // Arc
        Item item0 = new Item();
        item0.ID = 0;
        item0.Name = "Arc";
        item0.Description = "Permet de lancer des flèches";
        item0.Image = Item_ImageBow;
        item0.State = enmEtatItem.Actif;
        item0.NbItem = 23;
        item0.NbMaxItem = 25;
        _lstItems.Add(item0);

        // Cet item n'est pas disponible pour le moment.
        Item item1 = new Item();
        item1.ID = 1;
        item1.State = enmEtatItem.PasDecouvert;
        _lstItems.Add(item1);

        // Cet item n'est pas disponible pour le moment.
        Item item2 = new Item();
        item2.ID = 2;
        item2.State = enmEtatItem.PasDecouvert;
        _lstItems.Add(item2);

        // Bombe
        Item item3 = new Item();
        item3.ID = 3;
        item3.Name = "Bombe";
        item3.Description = "Permet de faire exploser des choses";
        item3.Image = Item_ImageBomb;
        item3.State = enmEtatItem.Actif;
        item3.NbItem = 10;
        item3.NbMaxItem = 10;
        _lstItems.Add(item3);

        // Bombchu
        Item item4 = new Item();
        item4.ID = 4;
        item4.Name = "Bombchu";
        item4.Description = "Permet de faire exploser des choses à distance";
        item4.Image = Item_ImageBombchu;
        item4.State = enmEtatItem.Inactif;
        item4.NbItem = 0;
        item4.NbMaxItem = 10;
        _lstItems.Add(item4);

        // Cet item n'est pas disponible pour le moment.
        Item item5 = new Item();
        item5.ID = 5;
        item5.State = enmEtatItem.PasDecouvert;
        _lstItems.Add(item5);

        // Deku stick
        Item item6 = new Item();
        item6.ID = 6;
        item6.Name = "Deku stick";
        item6.Description = "Peut-être brûlé, peut attaqué...";
        item6.Image = Item_ImageDekuStick;
        item6.State = enmEtatItem.Actif;
        item6.NbItem = 0;
        item6.NbMaxItem = 10;
        _lstItems.Add(item6);

        // Cet item n'est pas disponible pour le moment.
        Item item7 = new Item();
        item7.ID = 7;
        item7.State = enmEtatItem.PasDecouvert;
        _lstItems.Add(item7);

        // Cet item n'est pas disponible pour le moment.
        Item item8 = new Item();
        item8.ID = 8;
        item8.State = enmEtatItem.PasDecouvert;
        _lstItems.Add(item8);

        // Grappin
        Item item9 = new Item();
        item9.ID = 9;
        item9.Name = "Grappin";
        item9.Description = "Permet de monter ou d'agripper qqc";
        item9.Image = Item_ImageGrappin;
        item9.State = enmEtatItem.Inactif;
        item9.NbItem = 0;
        item9.NbMaxItem = 0;
        _lstItems.Add(item9);

        // Cet item n'est pas disponible pour le moment.
        Item item10 = new Item();
        item10.ID = 10;
        item10.State = enmEtatItem.PasDecouvert;
        _lstItems.Add(item10);

        // Truth
        Item item11 = new Item();
        item11.ID = 11;
        item11.Name = "Monocle de vérité";
        item11.Description = "Permet de voir la vérité";
        item11.Image = Item_ImageTruth;
        item11.State = enmEtatItem.Inactif;
        item11.NbItem = 0;
        item11.NbMaxItem = 0;
        _lstItems.Add(item11);

        // Cet item n'est pas disponible pour le moment.
        Item item12 = new Item();
        item12.ID = 12;
        item12.State = enmEtatItem.PasDecouvert;
        _lstItems.Add(item12);

        // Boomerang
        Item item13 = new Item();
        item13.ID = 13;
        item13.Name = "Boomerang";
        item13.Description = "Lance-le et il reviendra dans ta face!";
        item13.Image = Item_ImageBoomerang;
        item13.State = enmEtatItem.Actif;
        item13.NbItem = 0;
        item13.NbMaxItem = 0;
        _lstItems.Add(item13);

        // Cet item ne sera jamais disponible
        Item item14 = new Item();
        item14.ID = 14;
        item14.State = enmEtatItem.JamaisUtilise;
        _lstItems.Add(item14);

        // Cet item ne sera jamais disponible
        Item item15 = new Item();
        item15.ID = 15;
        item15.State = enmEtatItem.JamaisUtilise;
        _lstItems.Add(item15);

        // Cet item ne sera jamais disponible
        Item item16 = new Item();
        item16.ID = 16;
        item16.State = enmEtatItem.JamaisUtilise;
        _lstItems.Add(item16);

        // Cet item ne sera jamais disponible
        Item item17 = new Item();
        item17.ID = 17;
        item17.State = enmEtatItem.JamaisUtilise;
        _lstItems.Add(item17);



        // Bouteille 1
        Item item18 = new Item();
        item18.ID = 18;
        item18.Name = "Bouteille 1";
        item18.Description = "Permet de prendre et de transporter des trucs";
        item18.Image = Item_ImageBottle;
        item18.State = enmEtatItem.Actif;
        item18.NbItem = 0;
        item18.NbMaxItem = 0;
        _lstItems.Add(item18);

        // Bouteille 2
        Item item19 = new Item();
        item19.ID = 19;
        item19.Name = "Bouteille 2";
        item19.Description = "Permet de prendre et de transporter des trucs";
        item19.Image = Item_ImageBottle;
        item19.State = enmEtatItem.Actif;
        item19.NbItem = 0;
        item19.NbMaxItem = 0;
        _lstItems.Add(item19);

        // Cet item n'est pas disponible pour le moment.
        Item item20 = new Item();
        item20.ID = 20;
        item20.State = enmEtatItem.PasDecouvert;
        _lstItems.Add(item20);

        // Cet item n'est pas disponible pour le moment.
        Item item21 = new Item();
        item21.ID = 21;
        item21.State = enmEtatItem.PasDecouvert;
        _lstItems.Add(item21);

        // Cet item ne sera jamais disponible
        Item item22 = new Item();
        item22.ID = 22;
        item22.State = enmEtatItem.JamaisUtilise;
        _lstItems.Add(item22);

        // Cet item ne sera jamais disponible
        Item item23 = new Item();
        item23.ID = 23;
        item23.State = enmEtatItem.JamaisUtilise;
        _lstItems.Add(item23);
    }

    /// <summary>
    /// Permet d'obtenir l'item courant (ex. l'item qui est à droite si la direction demandé est la droite).
    /// </summary>
    /// <returns></returns>
    public Item GetCurrentItem()
    {
        return GetItem(_directionItemCurrent);
    }

    /// <summary>
    /// Permet d'obtenir quel item est associer à une direction en particulier.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public Item GetItem(enmDirectionItem direction)
    {
        switch (direction)
        {
            default:
            case enmDirectionItem.Left: return _itemAssignToLeft;
            case enmDirectionItem.Right: return _itemAssignToRight;
            case enmDirectionItem.Up: return _itemAssignToTop;
            case enmDirectionItem.Down: return _itemAssignToDown;
        }
    }

    public Item GetItem(int idItem)
    {
        foreach (Item item in _lstItems)
        {
            if (item.ID == idItem)
                return item;
        }

        return _itemEmpty;
    }

    public enmDirectionItem GetDirectionCurrentItem()
    {
        return _directionItemCurrent;
    }

    public void SetCurrentItem(enmDirectionItem direction)
    {
        _directionItemCurrent = direction;
    }

    public void AssignItem(enmDirectionItem direction, int idItem)
    {
        int idItemActuel = -1;

        switch (direction)
        {
            case enmDirectionItem.Up:
                {
                    idItemActuel = _itemAssignToTop.ID;

                    if (_itemAssignToDown.ID == idItem)
                        _itemAssignToDown = GetItem(idItemActuel);
                    else if (_itemAssignToLeft.ID == idItem)
                        _itemAssignToLeft = GetItem(idItemActuel);
                    else if (_itemAssignToRight.ID == idItem)
                        _itemAssignToRight = GetItem(idItemActuel);

                    _itemAssignToTop = GetItem(idItem);
                }
                break;
            case enmDirectionItem.Down:
                {
                    idItemActuel = _itemAssignToDown.ID;

                    if (_itemAssignToTop.ID == idItem)
                        _itemAssignToTop = GetItem(idItemActuel);
                    else if (_itemAssignToLeft.ID == idItem)
                        _itemAssignToLeft = GetItem(idItemActuel);
                    else if (_itemAssignToRight.ID == idItem)
                        _itemAssignToRight = GetItem(idItemActuel);

                    _itemAssignToDown = GetItem(idItem);
                }
                break;
            case enmDirectionItem.Left:
                {
                    idItemActuel = _itemAssignToLeft.ID;

                    if (_itemAssignToTop.ID == idItem)
                        _itemAssignToTop = GetItem(idItemActuel);
                    else if (_itemAssignToDown.ID == idItem)
                        _itemAssignToDown = GetItem(idItemActuel);
                    else if (_itemAssignToRight.ID == idItem)
                        _itemAssignToRight = GetItem(idItemActuel);

                    _itemAssignToLeft = GetItem(idItem);
                }
                break;
            case enmDirectionItem.Right:
                {
                    idItemActuel = _itemAssignToRight.ID;

                    if (_itemAssignToTop.ID == idItem)
                        _itemAssignToTop = GetItem(idItemActuel);
                    else if (_itemAssignToDown.ID == idItem)
                        _itemAssignToDown = GetItem(idItemActuel);
                    else if (_itemAssignToLeft.ID == idItem)
                        _itemAssignToLeft = GetItem(idItemActuel);

                    _itemAssignToRight = GetItem(idItem);
                }
                break;
        }

        if (_directionItemCurrent == enmDirectionItem.N_A)
            _directionItemCurrent = direction;

        Item currentItem = GetCurrentItem();
        if (currentItem.ID == -1)
            _directionItemCurrent = direction;
    }

    public bool IsItemAssigned(int iDItem)
    {
        bool isAssigned = false;

        if (_itemAssignToDown.ID == iDItem || _itemAssignToTop.ID == iDItem || _itemAssignToLeft.ID == iDItem || _itemAssignToRight.ID == iDItem)
            isAssigned = true;

        return isAssigned;
    }

    public int NbItemsAssigned()
    {
        int nbItemsAssgned = 0;

        if (_itemAssignToDown.ID != -1)
            nbItemsAssgned++;
        if (_itemAssignToTop.ID != -1)
            nbItemsAssgned++;
        if (_itemAssignToLeft.ID != -1)
            nbItemsAssgned++;
        if (_itemAssignToRight.ID != -1)
            nbItemsAssgned++;

        return nbItemsAssgned;
    }

    public int GetNbItemDiscover()
    {
        int nbItem = 0;

        foreach (Item item in _lstItems)
        {
            if (item.State == enmEtatItem.Actif || item.State == enmEtatItem.Inactif)
                nbItem++;
        }

        return nbItem;
    }
}