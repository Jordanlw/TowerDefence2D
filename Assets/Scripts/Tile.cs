using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    static GameObject tileManager;
    public TileManager.ButtonWallState type = TileManager.ButtonWallState.EMPTY;
    public int x, y, g = 1;
    public int h
    {
        get
        {
            return x + y;
        }
    }
    public int f
    {
        get
        {
            return g + h;
        }
    }
    public Tile parent;
    public Sprite wall;
    public Sprite empty;
    static private bool canClick;

    void Start()
    {
        if (tileManager == null)
        {
            tileManager = GameObject.Find("TileManager");
        }
    }
    private void OnMouseEnter()
    {
        canClick = true;
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0) && canClick)
        {
            canClick = false;
            switch (tileManager.GetComponent<TileManager>().buttonWallState)
            {
                case TileManager.ButtonWallState.EMPTY:
                    if (type != TileManager.ButtonWallState.EMPTY)
                    {
                        tileManager.GetComponent<TileManager>().isPathChanged = true;
                        GetComponent<SpriteRenderer>().sprite = empty;
                        type = TileManager.ButtonWallState.EMPTY;
                    }
                    break;
                case TileManager.ButtonWallState.WALL:
                    if (type != TileManager.ButtonWallState.WALL)
                    {
                        TileManager.ButtonWallState previous = type;
                        type = TileManager.ButtonWallState.WALL;
                        if (tileManager.GetComponent<TileManager>().isPathable())
                        {
                            GetComponent<SpriteRenderer>().sprite = wall;
                            tileManager.GetComponent<TileManager>().isPathChanged = true;
                        }
                        else
                        {
                            type = previous;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

}
