using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    GameObject tileManager;
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

    void Start()
    {
        tileManager = GameObject.Find("TileManager");
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            switch (tileManager.GetComponent<TileManager>().buttonWallState)
            {
                case TileManager.ButtonWallState.EMPTY:
                    GetComponent<SpriteRenderer>().sprite = empty;
                    type = TileManager.ButtonWallState.EMPTY;
                    break;
                case TileManager.ButtonWallState.WALL:
                    tileManager.GetComponent<TileManager>().updatePath = true;
                    GetComponent<SpriteRenderer>().sprite = wall;
                    type = TileManager.ButtonWallState.WALL;
                    break;
                default:
                    break;
            }
        }
    }

}
