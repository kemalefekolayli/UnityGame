using System.Collections.Generic;

using UnityEngine;

public class LevelObject 
{
    public int level_number;
    public int grid_width;
    public int grid_height;
    public int move_count;  // <-- match JSON exactly
    public List<string> grid;

    public int GetMoveCount()
    {
        return  move_count;
    }

    public int GetGridWidth()
    {
        return  grid_width;
    }

    public int GetGridHeight()
    {
        return  grid_height;
    }
    
    public List<string> GetGridVector()
    {
        return  grid;
    }

}