using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MenuItemsController : MonoBehaviour {

    public Sprite Item_ImageBow;
    public Sprite Item_ImageBomb;
    public Sprite Item_ImageBombchu;
    public Sprite Item_ImageDekuStick;
    public Sprite Item_ImageGrappin;
    public Sprite Item_ImageTruth;
    public Sprite Item_ImageBoomerang;
    public Sprite Item_ImageBottle;

    public int IDItemLeft = -1;
    public int IDItemTop = -1;
    public int IDItemDown = -1;
    public int IDItemRight = -1;

    public struct GameItem
    {
        public int ID;
        public string NameItem;
        public Sprite ImageItem;
    }

    public enum enmDirection
    {
        Up = -1,
        Down = -2,
        Left = -3,
        Right = -4
    }

    List<GameItem> _lstGameItems;

    private void Awake()
    {
        _lstGameItems = new List<GameItem>();

        GameItem itemBow = new GameItem();
        itemBow.ID = 0;
        itemBow.NameItem = "Bow";
        itemBow.ImageItem = Item_ImageBow;
        _lstGameItems.Add(itemBow);

        GameItem item1 = new GameItem();
        item1.ID = -1;
        _lstGameItems.Add(item1);

        GameItem itemBomb = new GameItem();
        itemBomb.ID = 2;
        itemBomb.NameItem = "Bomb";
        itemBomb.ImageItem = Item_ImageBomb;
        _lstGameItems.Add(itemBomb);

        GameItem item3 = new GameItem();
        item3.ID = -1;
        _lstGameItems.Add(item3);

        GameItem item4 = new GameItem();
        item4.ID = -1;
        _lstGameItems.Add(item4);

        GameItem item5 = new GameItem();
        item5.ID = -1;
        _lstGameItems.Add(item5);

        GameItem itemGrappin = new GameItem();
        itemGrappin.ID = 6;
        itemGrappin.NameItem = "Grappin";
        itemGrappin.ImageItem = Item_ImageGrappin;
        _lstGameItems.Add(itemGrappin);

        GameItem item7 = new GameItem();
        item7.ID = -1;
        _lstGameItems.Add(item7);

        GameItem itemBoomerang = new GameItem();
        itemBoomerang.ID = 8;
        itemBoomerang.NameItem = "Boomerang";
        itemBoomerang.ImageItem = Item_ImageBoomerang;
        _lstGameItems.Add(itemBoomerang);

        GameItem item9 = new GameItem();
        item9.ID = -1;
        _lstGameItems.Add(item9);

        GameItem item10 = new GameItem();
        item10.ID = -1;
        _lstGameItems.Add(item10);

        GameItem item11 = new GameItem();
        item11.ID = -1;
        _lstGameItems.Add(item11);

        GameItem item12 = new GameItem();
        item12.ID = -1;
        _lstGameItems.Add(item12);

        GameItem itemTruth = new GameItem();
        itemTruth.ID = 13;
        itemTruth.NameItem = "Truth";
        itemTruth.ImageItem = Item_ImageTruth;
        _lstGameItems.Add(itemTruth);

        GameItem item14 = new GameItem();
        item14.ID = -1;
        _lstGameItems.Add(item14);

        GameItem itemDekuStick = new GameItem();
        itemDekuStick.ID = 15;
        itemDekuStick.NameItem = "Deku stick";
        itemDekuStick.ImageItem = Item_ImageDekuStick;
        _lstGameItems.Add(itemDekuStick);

        GameItem item16 = new GameItem();
        item16.ID = -1;
        _lstGameItems.Add(item16);

        GameItem itemBombchu = new GameItem();
        itemBombchu.ID = 17;
        itemBombchu.NameItem = "Bombchu";
        itemBombchu.ImageItem = Item_ImageBombchu;
        _lstGameItems.Add(itemBombchu);



        GameItem itemBottle = new GameItem();
        itemBottle.ID = 18;
        itemBottle.NameItem = "Bottle";
        itemBottle.ImageItem = Item_ImageBottle;
        _lstGameItems.Add(itemBottle);

        GameItem item19 = new GameItem();
        item19.ID = -1;
        _lstGameItems.Add(item19);

        GameItem item20 = new GameItem();
        item20.ID = -1;
        _lstGameItems.Add(item20);

        GameItem item21 = new GameItem();
        item21.ID = -1;
        _lstGameItems.Add(item21);
    }

    public List<GameItem> LstGameItems
    {
        get { return _lstGameItems; }
    }

    public bool IsItemChoose(int IDItem)
    {
        return (IDItemDown == IDItem || IDItemLeft == IDItem || IDItemRight == IDItem || IDItemTop == IDItem);
    }

    public GameItem GetItem(enmDirection Direction)
    {
        int idItem = -1;

        switch (Direction)
        {
            case enmDirection.Up: idItem = IDItemTop; break;
            case enmDirection.Down: idItem = IDItemDown; break;
            case enmDirection.Left: idItem = IDItemLeft; break;
            case enmDirection.Right: idItem = IDItemRight; break;
        }

        if (idItem == -1)
        {
            GameItem item = new GameItem();
            item.ID = -1;
            item.ImageItem = null;
            item.NameItem = "";

            return item;
        }
        else
        {
            GameItem item = new GameItem();
            foreach (GameItem tmpItem in _lstGameItems)
            {
                if (tmpItem.ID == idItem)
                {
                    item = tmpItem;
                    break;
                }
            }

            return item;
        }
    }

    /// <summary>
    /// L'utilisateur a assigné un item à une direction. On va cependant vérifier si celui-ci est déjà associer à une autre direction. Si c'est le cas, on les switch!
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="idNouvelItem"></param>
    public void SetItem(enmDirection direction, int idNouvelItem)
    {
        int idItemActuel = -1;

        switch (direction)
        {
            case enmDirection.Up:
                {
                    idItemActuel = IDItemTop;

                    if (IDItemDown == idNouvelItem)
                        IDItemDown = idItemActuel;
                    else if (IDItemLeft == idNouvelItem)
                        IDItemLeft = idItemActuel;
                    else if (IDItemRight == idNouvelItem)
                        IDItemRight = idItemActuel;

                    IDItemTop = idNouvelItem;
                } break;
            case enmDirection.Down:
                {
                    idItemActuel = IDItemDown;

                    if (IDItemTop == idNouvelItem)
                        IDItemTop = idItemActuel;
                    else if (IDItemLeft == idNouvelItem)
                        IDItemLeft = idItemActuel;
                    else if (IDItemRight == idNouvelItem)
                        IDItemRight = idItemActuel;

                    IDItemDown = idNouvelItem;
                } break;
            case enmDirection.Left:
                {
                    idItemActuel = IDItemLeft;

                    if (IDItemDown == idNouvelItem)
                        IDItemDown = idItemActuel;
                    else if (IDItemTop == idNouvelItem)
                        IDItemTop = idItemActuel;
                    else if (IDItemRight == idNouvelItem)
                        IDItemRight = idItemActuel;

                    IDItemLeft = idNouvelItem;
                } break;
            case enmDirection.Right:
                {
                    idItemActuel = IDItemRight;

                    if (IDItemDown == idNouvelItem)
                        IDItemDown = idItemActuel;
                    else if (IDItemLeft == idNouvelItem)
                        IDItemLeft = idItemActuel;
                    else if (IDItemTop == idNouvelItem)
                        IDItemTop = idItemActuel;

                    IDItemRight = idNouvelItem;
                } break;
        }
    }
}
