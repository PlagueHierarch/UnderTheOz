using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Properties;
using UnityEngine.Windows;

namespace TreeExpansion
{
    public class TreeNode
    {
        public TreeNode leftTree;
        public TreeNode rightTree;
        public TreeNode parentTree;
        public Vector3Int startTile;
        public int direction;
        public RectInt roomRect;

        public Dictionary<Vector2Int, int> roomList = new Dictionary<Vector2Int, int>(); //0 : 빈 공간, 1 : 바닥 타일, 2 : 벽 타일, 3 : 천장 타일, 4 : 벽 + 천장
        public Dictionary<Vector2Int, bool> wallList = new Dictionary<Vector2Int, bool>();
        public Dictionary<Vector2Int, int> ceilingList = new Dictionary<Vector2Int, int>();
        
        public TreeNode(Vector3Int position, int _direction)
        {
            startTile = position;
            direction = _direction;
        }
    }

    public class TreeExpansion : MonoBehaviour
    {
        //[SerializeField] private GameObject[] enemyTiles;
        [SerializeField] private GameObject[] floorTiles;
        [SerializeField] private GameObject[] wallTiles;
        [SerializeField] private GameObject[] CeilingTiles;
        [SerializeField] private GameObject[] VoidTile;
        [SerializeField] private GameObject[] ExitTile;
        [SerializeField] private GameObject[] Player;

        [SerializeField] private Vector2Int mapSize;

        [SerializeField] private int maxNode;
        [SerializeField] private int maxRoomSize;
        [SerializeField] private int minRoomSize;

        [SerializeField] private GameObject line;
        [SerializeField] private Transform lineHolder;
        [SerializeField] private GameObject rectangle;


        public Dictionary<Vector3Int, bool> FloorPosition = new Dictionary<Vector3Int, bool>(); //바닥타일 좌표만 저장

        private BoardManager _boardManager;
        private enum Tile_num //0 : 빈 공간, 1 : 바닥 타일, 2 : 벽 타일, 3 : 천장 타일, 4 : 벽 + 천장
        {
            None,
            Floor,
            Wall,
        }
        private int None = (int)Tile_num.None;
        private int Floor = (int)Tile_num.Floor;
        private int Wall = (int)Tile_num.Wall;

        private int up = 0;
        private int right = 1;
        private int down = 2;
        private int left = 3;
        void ReferenceComponent()
        {
            _boardManager = GetComponent<BoardManager>();
        }
        private void Awake()
        {
            TreeNode rootNode = new TreeNode(new Vector3Int(0, 0, 0), right);
            OnDrawRectangle(1, 1, mapSize.x, mapSize.y);
            DivideTreeNode(rootNode, 0);
        }

        private void DivideTreeNode(TreeNode treeNode, int n)
        {
            Debug.Log("트리생성");
            bool isGenerate = GenerateRoom(treeNode, treeNode.startTile, treeNode.direction);
            if (isGenerate == false)
            {
                return;
            }
            if(n != maxNode)
            {
                int[] array = { 0, 1, 2, 3 };
                List<int> dir = new List<int>();
                dir.AddRange(array);
                dir.Remove(treeNode.direction);
                int leftDir = dir[Random.Range(0, dir.Count)]; 
                dir.Remove(leftDir);
                int rightDir = dir[Random.Range(0, dir.Count)];
                treeNode.leftTree = new TreeNode(treeNode.startTile, leftDir);
                treeNode.rightTree = new TreeNode(treeNode.startTile, rightDir);

                SetEndPosition(treeNode);


                DivideTreeNode(treeNode.leftTree, n + 1);
                DivideTreeNode(treeNode.rightTree, n + 1);
                treeNode.leftTree.parentTree = treeNode;
                treeNode.rightTree.parentTree = treeNode;
            }
            OnDrawRoom(treeNode);
        }

        private bool GenerateRoom(TreeNode treeNode, Vector3Int startTile, int dir)
        {
            int x = startTile.x;
            int y = startTile.y;
            treeNode.roomRect.width = Random.Range(minRoomSize, maxRoomSize);
            treeNode.roomRect.height = Random.Range(minRoomSize, maxRoomSize);

            treeNode.roomList.Clear();
            treeNode.wallList.Clear();

            if (dir == up)
            {
                int temp = Random.Range(1, treeNode.roomRect.width);
                treeNode.roomRect.x = x - temp;
                treeNode.roomRect.y = y;

                for (int i = x - temp; i <= x + treeNode.roomRect.width - temp; i++)
                {
                    for (int j = y; j <= y + treeNode.roomRect.height; j++)
                    {
                        if (FloorPosition.ContainsKey(new Vector3Int(i, j, 0)) == false)
                        {
                            return false;
                        }
                        else if (FloorPosition[new Vector3Int(i, j, 0)] == true)
                        {
                            return false;
                        }
                    }
                }

                for (int i = x - temp; i <= x + treeNode.roomRect.width - temp; i++)
                {
                    for (int j = y; j <= y + treeNode.roomRect.height; j++)
                    {
                        FloorPosition[new Vector3Int(i, j, 0)] = true;
                        treeNode.wallList[new Vector2Int(i, j)] = false;
                        treeNode.roomList[new Vector2Int(i, j)] = Floor;
                        if (((i == x - temp) || (i == x + treeNode.roomRect.width - temp)) || ((j == y) || (j == y + treeNode.roomRect.height)))
                        {
                            treeNode.wallList[new Vector2Int(i, j)] = true;
                        }
                    }
                }
            }
            else if (dir == right)
            {
                int temp = Random.Range(1, treeNode.roomRect.height);
                treeNode.roomRect.x = x;
                treeNode.roomRect.y = y - temp;
                for (int j = y - temp; j <= y + treeNode.roomRect.height - temp; j++)
                {
                    for (int i = x; i <= x + treeNode.roomRect.width; i++)
                    {
                        if(FloorPosition.ContainsKey(new Vector3Int(i, j, 0)) == false)
                        {
                            return false;
                        }
                        else if (FloorPosition[new Vector3Int(i, j, 0)] == true)
                        {
                            return false;
                        }
                    }
                }

                for (int j = y - temp; j <= y + treeNode.roomRect.height - temp; j++)
                {
                    for(int i = x; i <= x + treeNode.roomRect.width; i++)
                    {
                        treeNode.roomList[new Vector2Int(i, j)] = Floor;
                        treeNode.wallList[new Vector2Int(i, j)] = false;

                        if (((i == x) || (i == x + treeNode.roomRect.width)) || ((j == y - temp) || (j == y + treeNode.roomRect.height - temp)))
                        {
                            treeNode.wallList[new Vector2Int(i, j)] = true;
                        }
                    }
                }
            }
            else if (dir == down)
            {
                int temp = Random.Range(1, treeNode.roomRect.width);
                treeNode.roomRect.x = x - temp;
                treeNode.roomRect.y = y - treeNode.roomRect.height;

                for (int i = x - temp; i <= x + treeNode.roomRect.width - temp; i++)
                {
                    for (int j = y; j >= y - treeNode.roomRect.height; j--)
                    {
                        if (FloorPosition.ContainsKey(new Vector3Int(i, j, 0)) == false)
                        {
                            return false;

                        }
                        else if (FloorPosition[new Vector3Int(i, j, 0)] == true)
                        {
                            return false;
                        }
                    }
                }

                for (int i = x - temp; i <= x + treeNode.roomRect.width - temp; i++)
                {
                    for (int j = y; j >= y - treeNode.roomRect.height; j--)
                    {
                        treeNode.roomList[new Vector2Int(i, j)] = Floor;
                        treeNode.wallList[new Vector2Int(i, j)] = false;

                        if ((((i == x - temp) || (i == x + treeNode.roomRect.width - temp)) || ((j == y) || (j == y - treeNode.roomRect.height)))&&((i != x) && (j != y)))
                        {
                            treeNode.wallList[new Vector2Int(i, j)] = true;
                        }
                    }
                }
            }
            else if (dir == left)
            {
                int temp = Random.Range(1, treeNode.roomRect.height);
                treeNode.roomRect.x = x - treeNode.roomRect.width;
                treeNode.roomRect.y = y - temp;

                for (int j = y - temp; j <= y + treeNode.roomRect.height - temp; j++)
                {
                    for (int i = x; i >= x - treeNode.roomRect.width; i--)
                    {
                        if (FloorPosition.ContainsKey(new Vector3Int(i, j, 0)) == false)
                        {
                            return false;

                        }
                        else if (FloorPosition[new Vector3Int(i, j, 0)] == true)
                        {
                            return false;
                        }
                    }
                }
                for (int j = y - temp; j <= y + treeNode.roomRect.height - temp; j++)
                {
                    for (int i = x; i >= x - treeNode.roomRect.width; i--)
                    {
                        treeNode.roomList[new Vector2Int(i, j)] = Floor;
                        treeNode.wallList[new Vector2Int(i, j)] = false;

                        if (((i == x) || (i == x - treeNode.roomRect.width)) || ((j == y - temp) || (j == y + treeNode.roomRect.height - temp)))
                        {
                            treeNode.wallList[new Vector2Int(i, j)] = true;
                        }
                    }
                }
            }
            return true;
        }

        private void SetEndPosition(TreeNode treeNode)
        {
            int x = 0;
            int y = 0;
            int Dir = treeNode.leftTree.direction;
            if (Dir == up)
            {
                y = treeNode.roomRect.yMax;
                x = Random.Range(treeNode.roomRect.xMin, treeNode.roomRect.xMax);
                treeNode.leftTree.startTile = new Vector3Int(x, y, 0) + new Vector3Int(0, 1, 0);
            }
            else if (Dir == down)
            {
                y = treeNode.roomRect.yMin;
                x = Random.Range(treeNode.roomRect.xMin, treeNode.roomRect.xMax);
                treeNode.leftTree.startTile = new Vector3Int(x, y, 0) + new Vector3Int(0, -1, 0);
            }
            else if (Dir == left)
            {
                x = treeNode.roomRect.xMin;
                y = Random.Range(treeNode.roomRect.yMin, treeNode.roomRect.yMax);
                treeNode.leftTree.startTile = new Vector3Int(x, y, 0) + new Vector3Int(-1, 0, 0);
            }
            else if (Dir == right)
            {
                x = treeNode.roomRect.xMax;
                y = Random.Range(treeNode.roomRect.yMin, treeNode.roomRect.yMax);
                treeNode.leftTree.startTile = new Vector3Int(x, y, 0) + new Vector3Int(1, 0, 0);
            }
            treeNode.wallList[new Vector2Int(x, y)] = false;
            Dir = treeNode.rightTree.direction;
            if (Dir == up)
            {   
                y = treeNode.roomRect.yMax;
                x = Random.Range(treeNode.roomRect.xMin, treeNode.roomRect.xMax);
                treeNode.rightTree.startTile = new Vector3Int(x, y, 0) + new Vector3Int(0, 1, 0);
            }
            else if (Dir == down)
            {
                y = treeNode.roomRect.yMin;
                x = Random.Range(treeNode.roomRect.xMin, treeNode.roomRect.xMax);
                treeNode.rightTree.startTile = new Vector3Int(x, y, 0) + new Vector3Int(0, -1, 0);
            }
            else if (Dir == left)
            {
                x = treeNode.roomRect.xMin;
                y = Random.Range(treeNode.roomRect.yMin, treeNode.roomRect.yMax);
                treeNode.rightTree.startTile = new Vector3Int(x, y, 0) + new Vector3Int(-1, 0, 0);
            }
            else if (Dir == right)
            {
                x = treeNode.roomRect.xMax;
                y = Random.Range(treeNode.roomRect.yMin, treeNode.roomRect.yMax);
                treeNode.rightTree.startTile = new Vector3Int(x, y, 0) + new Vector3Int(1, 0, 0);
            }
            treeNode.wallList[new Vector2Int(x, y)] = false;
        }

        private void OnDrawFloor(TreeNode treeNode)
        {
            int x = treeNode.roomRect.x;
            int y = treeNode.roomRect.y;
            int width = treeNode.roomRect.width;
            int height = treeNode.roomRect.height;

            GameObject toInstantiate = VoidTile[0];

            for (int i = x; i <= x + width; i++)
            {
                for(int j = y; j <= y + height; j++)
                {
                    if(treeNode.roomList[new Vector2Int(i, j)] == Floor)
                    {
                        toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                        GameObject instance = Instantiate(toInstantiate,
                            new Vector3Int(i, j, 0)
                                , Quaternion.identity) as GameObject;
    
                    }

                }
            }
        }

        private void OnDrawWall(TreeNode treeNode)
        {
            int x = treeNode.roomRect.x;
            int y = treeNode.roomRect.y;
            int width = treeNode.roomRect.width;
            int height = treeNode.roomRect.height;

            GameObject toInstantiate = VoidTile[0];

            for (int i = x; i <= x + width; i++)
            {
                for (int j = y; j <= y + height; j++)
                {
                    if (treeNode.wallList[new Vector2Int(i, j)] == true)
                    {
                        toInstantiate = wallTiles[Random.Range(0, wallTiles.Length)];
                        GameObject instance = Instantiate(toInstantiate,
                            new Vector3Int(i, j, 0)
                                , Quaternion.identity) as GameObject;
                    }

                }
            }
        }

        private void OnDrawRoom(TreeNode treeNode)
        {
            OnDrawFloor(treeNode);
            OnDrawWall(treeNode);
        }
        private void OnDrawRectangle(int x, int y, int width, int height) //가상의 사각형 선 생성(맵 최대 크기)
        {
            //LineRenderer lineRenderer = Instantiate(rectangle, lineHolder).GetComponent<LineRenderer>();
            //lineRenderer.positionCount = 5;
            //lineRenderer.SetPosition(0, new Vector2(x, y));
            //lineRenderer.SetPosition(1, new Vector2(x + width, y));
            //lineRenderer.SetPosition(2, new Vector2(x + width, y + height));
            //lineRenderer.SetPosition(3, new Vector2(x, y + height));
            //lineRenderer.SetPosition(4, new Vector2(x, y));

            for (int i = x - (width / 2); i <= x + (width / 2); i++)
            {
                for (int j = y - (height / 2); j <= y + (height / 2); j++)
                {
                    if ((i == x - (width / 2) || i == x + (width / 2) || (j == y - (height / 2) || j == y + (height / 2))))
                    {
                        FloorPosition[new Vector3Int(i, j, 0)] = true;
                    }
                    else
                    {
                        FloorPosition[new Vector3Int(i, j, 0)] = false;
                    }
                }
            }
        }
    }
}
        