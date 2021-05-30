using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OnlineGameData
{
    public DataGame data;
}

[System.Serializable]
public class DataGame
{
    public int id;
    public int xAxis;
    public int yAxis;
    public int creatorId;
    public int playerId;
    public int winnerId;
    public string type;
    public string colorCreator;
    public string pieces;
}