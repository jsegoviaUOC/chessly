using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OnlineMoveData
{
    public DataMove data;
}

[System.Serializable]
public class DataMove
{
    public int id;
    public int xCurrent;
    public int yCurrent;
    public int xTarget;
    public int yTarget;
    public int playerId;
    public int gameId;
    public string createdAt;
}