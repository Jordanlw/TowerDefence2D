using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public enum ButtonWallState
    {
        EMPTY,
        WALL
    }
    public ButtonWallState buttonWallState = ButtonWallState.EMPTY;
    public GameObject tile;
    public int size = 40;
    public List<Tile> tiles = new List<Tile>();
    public Tile enemySpawn, enemyDestination;
    public bool updatePath;

    // Use this for initialization
    void Start()
    {
        float offset = size * tile.GetComponent<SpriteRenderer>().size.x / 2;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Tile tileSpawned = Instantiate(tile, new Vector3(tile.GetComponent<SpriteRenderer>().size.x * x - offset, tile.GetComponent<SpriteRenderer>().size.y * y - offset), Quaternion.identity).GetComponent<Tile>();
                tileSpawned.x = x;
                tileSpawned.y = y;
                tiles.Add(tileSpawned);
                if (x == 0 && y == 0)
                {
                    enemyDestination = tileSpawned;
                }
                else if (x == size - 1 && y == size - 1)
                {
                    enemySpawn = tileSpawned;
                }
            }
        }
    }

    void Update()
    {
        if(updatePath && Input.GetMouseButtonUp(0))
        {
            updatePath = false;
            List<Tile> path = new List<Tile>(FindPath().ToArray());
            if (path[path.Count - 1].Equals(enemyDestination))
            {
                //print("Path to enemyDestination found");
            }
            foreach (var tile in tiles)
            {
                tile.GetComponent<SpriteRenderer>().color = Color.white;
            }
            foreach (var tile in path)
            {
                tile.GetComponent<SpriteRenderer>().color = Color.blue;
            }
        }
    }

    public void SetButtonState(string state)
    {
        switch (state)
        {
            case "Empty":
                buttonWallState = ButtonWallState.EMPTY;
                break;
            case "Wall":
                buttonWallState = ButtonWallState.WALL;
                break;
            default:
                break;
        }
    }

    public Stack<Tile> FindPath()
    {
        HashSet<Tile> open = new HashSet<Tile>();
        HashSet<Tile> closed = new HashSet<Tile>();
        open.Add(enemySpawn);
        enemySpawn.g = 1;
        foreach (var tile in tiles)
        {
            tile.g = 1;
        }
        while (open.Count > 0)
        {
            Tile current = FindLowestF(open);
            open.Remove(current);
            closed.Add(current);
            //print("Size of closed: " + closed.Count);
            foreach (var selection in tiles)
            {
                if (!closed.Contains(selection) && selection.type == ButtonWallState.EMPTY)
                {
                    if (selection.x + 1 == current.x && selection.y == current.y)
                    {
                        calculateTile(current, selection, open);
                    }
                    else if (selection.x - 1 == current.x && selection.y == current.y)
                    {
                        calculateTile(current, selection, open);
                    }
                    else if (selection.x == current.x && selection.y + 1 == current.y)
                    {
                        calculateTile(current, selection, open);
                    }
                    else if (selection.x == current.x && selection.y - 1 == current.y)
                    {
                        calculateTile(current, selection, open);
                    }
                }
            }
            if (closed.Contains(enemyDestination))
            {
                //print("closed contains enemyDestination");
                return MakePath();
            }
        }
        //print("open is empty");
        return MakePath();
    }

    void calculateTile(Tile current, Tile selection, HashSet<Tile> open)
    {
        if (!open.Contains(selection))
        {
            open.Add(selection);
            selection.parent = current;
            selection.g += current.g;
        }
        else if (selection.g - selection.parent.g + current.g < selection.g)
        {
            //print("Changed parent of selection");
            selection.g -= selection.parent.g;
            selection.g += current.g;
            selection.parent = current;
        }
    }

    Stack<Tile> MakePath()
    {
        Stack<Tile> path = new Stack<Tile>();
        path.Push(enemyDestination);
        //print("Making Path");
        while (!path.Contains(enemySpawn))
        {
            path.Push(path.Peek().parent);
        }
        return path;
    }

    Tile FindLowestF(HashSet<Tile> open)
    {
        Tile lowest = null;
        foreach (var tile in open)
        {
            if (lowest == null || tile.f < lowest.f)
            {
                lowest = tile;
            }
        }
        return lowest;
    }
}
