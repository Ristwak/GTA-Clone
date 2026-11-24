using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int playerMoney;
    public float[] position;
    public bool pistolPrefab;
    public bool m416Prefab;
    public bool akmPrefab;
    public bool Mission1;
    public bool Mission2;
    public bool Mission3;
    public bool Mission4;
    public bool Mission5;

    public PlayerData(GameManager gameManager)
    {
        playerMoney = gameManager.playerMoney;
        Transform playerTransform = gameManager.playerGameObject != null
            ? gameManager.playerGameObject
            : gameManager.transform;
        position = new float[3];
        position[0] = playerTransform.position.x;
        position[1] = playerTransform.position.y;
        position[2] = playerTransform.position.z;
        pistolPrefab = gameManager.pistolPrefab;
        akmPrefab = gameManager.akmPrefab;
        m416Prefab = gameManager.m416Prefab;
        Mission1 = gameManager.Mission1;
        Mission2 = gameManager.Mission2;
        Mission3 = gameManager.Mission3;
        Mission4 = gameManager.Mission4;
        Mission5 = gameManager.Mission5;
    }
}