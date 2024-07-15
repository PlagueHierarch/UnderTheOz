using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomData
{
    public bool objectOnTile;
    public int objeectCount;
    public int tileType;

}
public class TileManager : MonoBehaviour
{
    public Dictionary<Vector3Int, CustomData> dataOnTiles; //Ÿ���� ������
    public Tilemap _tilemap; 
    public Vector3 localPos;
    public Vector3 worldPos;

    public Vector3Int worldToCellPos; //���� ���� �� ��ġ
    public Vector3Int localTocellPos; //Ÿ�ϸ� ���� �� ��ġ

    public Vector3 mousePoint;
    void Start()
    {
        dataOnTiles = new Dictionary<Vector3Int, CustomData>();
        worldToCellPos = _tilemap.WorldToCell(worldPos);
        localTocellPos = _tilemap.LocalToCell(localPos);
            
        foreach(Vector3Int pos in _tilemap.cellBounds.allPositionsWithin)
        {
            if(!_tilemap.HasTile(pos)) continue;
            var tile = _tilemap.GetTile<TileBase>(pos);
            dataOnTiles[pos] = new CustomData();
        }
    }


    void Update()
    {
        mousePoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -Camera.main.transform.position.z));
        localTocellPos = _tilemap.LocalToCell(mousePoint);
        Debug.Log(localTocellPos);
    }

    
}
