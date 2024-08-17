using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Runtime.CompilerServices;
using System.Threading;
namespace DungeonGenerator
{
       public class TreeNode
    {
        public TreeNode leftTree;
        public TreeNode rightTree;
        public TreeNode parentTree;
        public RectInt treeSize;
        public RectInt dungeonSize;
        public bool isSpawnExit = false; //제일 왼쪽 노드에서만 하나 생성
        public bool isSpawnPlayer = false; //제일 오른쪽 노드에서만 하나 생성

        

        public TreeNode(int x, int y, int width, int height)
        {
            treeSize.x = x;
            treeSize.y = y;
            treeSize.width = width;
            treeSize.height = height;
        }
    }

    public class DungeonGenerator : MonoBehaviour
    {
        public static DungeonGenerator dungeonInstance = null;

        //[SerializeField] private GameObject[] enemyTiles;
        [SerializeField] private GameObject[] floorTiles;
        [SerializeField] private GameObject[] wallTiles;
        [SerializeField] private GameObject[] CeilingTiles;
        [SerializeField] private GameObject[] VoidTile;
        [SerializeField] private GameObject[] ExitTile;
        [SerializeField] private GameObject[] Player;
        [SerializeField] private GameObject[] Monster;

        [SerializeField] private Vector2Int mapSize;

        [SerializeField] private int maxNode;
        [SerializeField] private float minDivideSize;
        [SerializeField] private float maxDivideSize;
        [SerializeField] private int minRoomSize;

        [SerializeField] private GameObject line;
        [SerializeField] private Transform lineHolder;
        [SerializeField] private GameObject rectangle;

        [SerializeField] private GameObject _gameManager;

        [SerializeField] private int monsterCounter;

        private BoardManager _boardManager;

        public Dictionary<Vector2Int, int> mapList = new Dictionary<Vector2Int, int>(); //0 : 빈 공간, 1 : 바닥 타일, 2 : 벽 타일, 3 : 천장 타일, 4 : 벽 + 천장
        public Dictionary<Vector2Int, bool> wallList = new Dictionary<Vector2Int, bool>();
        public Dictionary<Vector2Int, int> ceilingList = new Dictionary<Vector2Int, int>();
        
        public List<Vector3Int> RoomPosition = new List<Vector3Int>(); //방 바닥타일 좌표만 저장
        public List<Vector3Int> FloorPosition = new List<Vector3Int>(); //전체 바닥타일 좌표만 저장


        private enum Tile_num //0 : 빈 공간, 1 : 바닥 타일, 2 : 벽 타일, 3 : 천장 타일, 4 : 벽 + 천장
        {
            None,
            Floor,
            Wall,
        }
        private int None = (int)Tile_num.None;
        private int Floor = (int)Tile_num.Floor;
        private int Wall = (int)Tile_num.Wall;
        

        void ReferenceComponent()
        {
            _boardManager = GetComponent<BoardManager>();
        }
        private void Start()
        {
            _boardManager = _gameManager.GetComponent<BoardManager>();
            if (dungeonInstance == null)
            {
                dungeonInstance = this;
            }
            else if (dungeonInstance != this)
            {
                Destroy(gameObject);
            }

            GameManager.instance.monsters.Clear();
            Debug.Log("스테이지" + GameManager.instance.stage);

            ReferenceComponent();
            FloorPosition.Clear();
            mapList.Clear();
            OnDrawRectangle(0, 0, mapSize.x, mapSize.y); //맵의 총 크기 그림 + 딕셔너리에 정보 저장
            TreeNode rootNode = new TreeNode(0, 0, mapSize.x, mapSize.y); //첫 뿌리 노드 생성
            rootNode.isSpawnExit = true;
            rootNode.isSpawnPlayer = true;
            DivideTree(rootNode, 0); //트리노드 정해준 수만큼 만듦
            GenerateDungeon(rootNode, 0); //방 생성
            GenerateRoad(rootNode, 0); //길 생성
            GenerateWall(0, 0, mapSize.x, mapSize.y);
            GenerateCeiling(0, 0);
            OnDrawCeiling(0, 0);
            OnDrawWall();

        }

        private void DivideTree(TreeNode treeNode, int n) //트리노드 정해준 수만큼 만듦 (재귀 함수)
        {

            if (n < maxNode) //최대 노드보다 작을때만 실행
            {
                RectInt size = treeNode.treeSize; //부모로부터 사각형 크기 받아옴
                int length = size.width >= size.height ? size.width : size.height; //가로 세로 중 더 긴 곳을 저장
                int split = Mathf.RoundToInt(Random.Range(length * minDivideSize, length * maxDivideSize)); //정해준 비율 사이에서 랜덤으로 사각형을 나눔
                if (size.width >= size.height) //가로가 더 길때
                {
                    treeNode.leftTree = new TreeNode(size.x, size.y, split, size.height); //x축 기준으로 왼쪽 노드 생성 (세로는 부모노드의 세로 길이 받아옴)
                    treeNode.rightTree = new TreeNode(size.x + split, size.y, size.width - split, size.height); //x축 기준으로 오른쪽 노드 생성
                    OnDrawLine(new Vector2(size.x + split, size.y), new Vector2(size.x + split, size.y + size.height)); //가상의 선 생성
                }
                else //세로가 더 길때
                {
                    treeNode.leftTree = new TreeNode(size.x, size.y, size.width, split);
                    treeNode.rightTree = new TreeNode(size.x, size.y + split, size.width, size.height - split);
                    OnDrawLine(new Vector2(size.x, size.y + split), new Vector2(size.x + size.width, size.y + split));
                }

                if(n == 0)
                {
                    if (treeNode.isSpawnExit == true)
                    {
                        treeNode.leftTree.isSpawnExit = true;
                        treeNode.rightTree.isSpawnExit = false;
                    }
                }
                else
                {
                    if (treeNode.isSpawnExit == true)
                    {
                        treeNode.leftTree.isSpawnExit = false;
                        treeNode.rightTree.isSpawnExit = true;
                    }
                }

                if (treeNode.isSpawnPlayer == true)
                {
                    treeNode.leftTree.isSpawnPlayer = false;
                    treeNode.rightTree.isSpawnPlayer = true;
                }

                if((treeNode.isSpawnExit == false) && (treeNode.isSpawnPlayer == false))
                {
                    treeNode.leftTree.isSpawnExit = false;
                    treeNode.rightTree.isSpawnExit = false;
                    treeNode.leftTree.isSpawnPlayer = false;
                    treeNode.rightTree.isSpawnPlayer = false;
                }

                treeNode.leftTree.parentTree = treeNode; //부모트리 연결
                treeNode.rightTree.parentTree = treeNode;
                DivideTree(treeNode.leftTree, n + 1); //재귀함수, 다시 한번 실행
                DivideTree(treeNode.rightTree, n + 1);
            }
        }

        private RectInt GenerateDungeon(TreeNode treeNode, int n) //방 생성
        {
            if (n == maxNode) //제일 하위 노드일때만 실행
            {
                RectInt size = treeNode.treeSize;
                int width = Mathf.Max(Random.Range(size.width / 2, size.width - 1)); //노드의 크기안에서 랜덤으로 방의 크기를 정함
                int height = Mathf.Max(Random.Range(size.height / 2, size.height - 1));
                int x = treeNode.treeSize.x + Random.Range(1, size.width - width); //방이 노드 크기를 넘지 않도록 함
                int y = treeNode.treeSize.y + Random.Range(1, size.height - height);
                OnDrawDungeon(x, y, width, height, treeNode.isSpawnExit, treeNode.isSpawnPlayer); //던전(타일) 생성
                return new RectInt(x, y, width, height); //던전(방)의 크기 리턴
            }
            treeNode.leftTree.dungeonSize = GenerateDungeon(treeNode.leftTree, n + 1); //재귀함수로 노트 나눔
            treeNode.rightTree.dungeonSize = GenerateDungeon(treeNode.rightTree, n + 1);
            return treeNode.leftTree.dungeonSize; //자식트리중 아무던전의 사이즈 가져옴
        }

        private void GenerateRoad(TreeNode treeNode, int n) //길 생성(재귀 함수)
        {
            Transform parent = GameObject.Find("Floor").transform;
            if (n == maxNode)
            {
                return;
            }//제일 최하위 노드는 길 생성 못함
            int x1 = GetCenterX(treeNode.leftTree.dungeonSize); //중간 좌표 구함
            int x2 = GetCenterX(treeNode.rightTree.dungeonSize);
            int y1 = GetCenterY(treeNode.leftTree.dungeonSize);
            int y2 = GetCenterY(treeNode.rightTree.dungeonSize);
            GameObject toInstantiate = VoidTile[0];
            for (int x = Mathf.Min(x1, x2); x <= Mathf.Max(x1, x2); x++) //둘 중 값이 더 작은 곳부터 큰 곳까지 길 생성
            {
                toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                GameObject instance = Instantiate(toInstantiate,
                    new Vector3Int(x - mapSize.x / 2, y1 - mapSize.y / 2, 0)
                        , Quaternion.identity) as GameObject;
                mapList[new Vector2Int(x - mapSize.x / 2, y1 - mapSize.y / 2)] = Floor;
                FloorPosition.Add(new Vector3Int(x -mapSize.x / 2, y1 - mapSize.y / 2));
                instance.transform.parent = parent;
            }
            for (int y = Mathf.Min(y1, y2); y <= Mathf.Max(y1, y2); y++)
            {
                toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                GameObject instance = Instantiate(toInstantiate,
                    new Vector3Int(x2 - mapSize.x / 2, y - mapSize.y / 2, 0)
                        , Quaternion.identity) as GameObject;
                mapList[new Vector2Int(x2 - mapSize.x / 2, y - mapSize.y / 2)] = Floor;
                FloorPosition.Add(new Vector3Int(x2 - mapSize.x / 2, y - mapSize.y / 2));

                instance.transform.parent = parent;
            }
            GenerateRoad(treeNode.leftTree, n + 1);
            GenerateRoad(treeNode.rightTree, n + 1);
        }

        private void OnDrawLine(Vector2 from, Vector2 to) 
        {
            //LineRenderer lineRenderer = Instantiate(line, lineHolder).GetComponent<LineRenderer>();
            //lineRenderer.SetPosition(0, from - mapSize / 2);
            //lineRenderer.SetPosition(1, to - mapSize / 2);
        }

        private void OnDrawDungeon(int x, int y, int width, int height, bool isLeft, bool isRight) //방 생성
        {
            RoomPosition.Clear();
            GameObject toInstantiate = VoidTile[0];
            for (int i = x; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                    Transform parent = GameObject.Find("Floor").transform;
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                    GameObject instance = Instantiate(toInstantiate,
                        new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0)
                            , Quaternion.identity) as GameObject;

                    mapList[new Vector2Int(i - mapSize.x / 2, j - mapSize.y / 2)] = Floor;
                    RoomPosition.Add(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0));
                    FloorPosition.Add(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0));

                    instance.transform.parent = parent;
                }
            }
            if(isLeft == true)
            {
                GenerateExit(ExitTile, RoomPosition);
            }
            if(isRight == true)
            {
                GeneratePlayer(Player, RoomPosition);
            }
            GenerateMonster(Monster, RoomPosition);
        }

        private void OnDrawRectangle(int x, int y, int width, int height) //가상의 사각형 선 생성(맵 최대 크기)
        {
            //LineRenderer lineRenderer = Instantiate(rectangle, lineHolder).GetComponent<LineRenderer>();
            //lineRenderer.positionCount = 5;
            //lineRenderer.SetPosition(0, new Vector2(x, y) - mapSize / 2);
            //lineRenderer.SetPosition(1, new Vector2(x + width, y) - mapSize / 2);
            //lineRenderer.SetPosition(2, new Vector2(x + width, y + height) - mapSize / 2);
            //lineRenderer.SetPosition(3, new Vector2(x, y + height) - mapSize / 2);
            //lineRenderer.SetPosition(4, new Vector2(x, y) - mapSize / 2);

            for (int i = x - (mapSize.x / 2) - 1; i <= (x + width) - (mapSize.x / 2) + 1; i++)
            {
                for (int j = y - (mapSize.y / 2) - 1; j <= (y + height) - (mapSize.y / 2) + 1; j++)
                {
                    mapList.Add(new Vector2Int(i, j), None);
                    ceilingList.Add(new Vector2Int(i, j), -1);
                    wallList.Add(new Vector2Int(i, j), false);
                }
            }
        }


        private void GenerateWall(int x, int y, int width, int height)
        {
            for (int i = x - (mapSize.x / 2) - 1; i <= (x + width) - (mapSize.x / 2) + 1; i++)
            {
                for (int j = y - (mapSize.y / 2) - 1; j <= (y + height) - (mapSize.y / 2) + 1; j++)
                {
                    if (mapList[new Vector2Int(i, j)] == Floor)
                    {
                        if (mapList[new Vector2Int(i + 1, j)] == None)
                        {
                            wallList[new Vector2Int(i + 1, j)] = true;
                            mapList[new Vector2Int(i + 1, j)] = Wall;
                        }
                        if (mapList[new Vector2Int(i - 1, j)] == None)
                        {
                            wallList[new Vector2Int(i - 1, j)] = true;
                            mapList[new Vector2Int(i - 1, j)] = Wall;
                        }
                        if (mapList[new Vector2Int(i, j + 1)] == None)
                        {
                            wallList[new Vector2Int(i, j + 1)] = true;
                            mapList[new Vector2Int(i, j + 1)] = Wall;
                        }
                        if (mapList[new Vector2Int(i, j - 1)] == None)
                        {
                            wallList[new Vector2Int(i, j - 1)] = true;
                            mapList[new Vector2Int(i, j - 1)] = Wall;
                        }
                        if (mapList[new Vector2Int(i + 1, j + 1)] == None)
                        {
                            wallList[new Vector2Int(i + 1, j + 1)] = true;
                            mapList[new Vector2Int(i + 1, j + 1)] = Wall;
                        }
                        if (mapList[new Vector2Int(i + 1, j - 1)] == None)
                        {
                            wallList[new Vector2Int(i + 1, j - 1)] = true;
                            mapList[new Vector2Int(i + 1, j - 1)] = Wall;
                        }
                        if (mapList[new Vector2Int(i - 1, j - 1)] == None)
                        {
                            wallList[new Vector2Int(i - 1, j - 1)] = true;
                            mapList[new Vector2Int(i - 1, j - 1)] = Wall;
                        }
                        if (mapList[new Vector2Int(i - 1, j + 1)] == None)
                        {
                            wallList[new Vector2Int(i - 1, j + 1)] = true;
                            mapList[new Vector2Int(i - 1, j + 1)] = Wall;
                        }
                    }
                }
            }
        }

        private void GenerateCeiling(int x, int y)
        {
            for (int i = x - (mapSize.x / 2); i <= x + (mapSize.x / 2); i++)
            {
                for (int j = y + (mapSize.y / 2); j >= y - (mapSize.y / 2); j--)
                {
                    if (mapList[new Vector2Int(i, j)] == Wall)
                    {
                        if (ceilingList[new Vector2Int(i, j)] == -1)
                        {
                            //특별 예외
                            if ((mapList[new Vector2Int(i + 1, j)] == Wall) && (mapList[new Vector2Int(i - 1, j)] == Wall) && (mapList[new Vector2Int(i, j + 1)] == Wall) && (mapList[new Vector2Int(i, j - 1)] != Wall)
                                && (mapList[new Vector2Int(i + 1, j + 1)] == Wall) && (mapList[new Vector2Int(i - 1, j + 1)] != Wall) && (mapList[new Vector2Int(i + 1, j - 1)] == Wall))
                            {
                                ceilingList[new Vector2Int(i, j + 1)] = 13;
                                continue;
                            }
                            if ((mapList[new Vector2Int(i + 1, j)] == Wall) && (mapList[new Vector2Int(i - 1, j)] == Wall) && (mapList[new Vector2Int(i, j + 1)] != Wall) && (mapList[new Vector2Int(i, j - 1)] == Wall)
                                && (mapList[new Vector2Int(i + 1, j + 1)] == Wall) && (mapList[new Vector2Int(i - 1, j + 1)] != Wall) && (mapList[new Vector2Int(i + 1, j - 1)] == Wall) && (mapList[new Vector2Int(i - 1, j - 1)] != Wall))
                            {
                                ceilingList[new Vector2Int(i, j + 1)] = 5;
                                ceilingList[new Vector2Int(i, j)] = 14;
                                continue;
                            }
                            if ((mapList[new Vector2Int(i + 1, j)] != Wall) && (mapList[new Vector2Int(i - 1, j)] == Wall) && (mapList[new Vector2Int(i, j + 1)] == Wall) && (mapList[new Vector2Int(i, j - 1)] == Wall)
                                && (mapList[new Vector2Int(i + 1, j + 1)] != Wall) && (mapList[new Vector2Int(i - 1, j + 1)] == Wall) && (mapList[new Vector2Int(i + 1, j - 1)] != Wall) && (mapList[new Vector2Int(i - 1, j - 1)] != Wall))
                            {
                                ceilingList[new Vector2Int(i, j + 1)] = 6;
                                ceilingList[new Vector2Int(i, j)] = 14;
                                continue;
                            }
                            if ((mapList[new Vector2Int(i + 1, j)] == Wall) && (mapList[new Vector2Int(i - 1, j)] == Wall) && ((mapList[new Vector2Int(i, j + 1)] != Wall) || (mapList[new Vector2Int(i, j - 1)] != Wall)))
                            {
                                ceilingList[new Vector2Int(i, j + 1)] = 13;
                                continue;
                            }
                            if ((mapList[new Vector2Int(i + 1, j)] != Wall) && (mapList[new Vector2Int(i - 1, j)] == Wall) && (mapList[new Vector2Int(i, j + 1)] == Wall) && (mapList[new Vector2Int(i, j - 1)] == Wall)
                                && (mapList[new Vector2Int(i + 1, j + 1)] != Wall) && (mapList[new Vector2Int(i - 1, j + 1)] == Wall) && (mapList[new Vector2Int(i + 1, j - 1)] != Wall)
                                && (mapList[new Vector2Int(i - 1, j - 1)] == Wall) && (mapList[new Vector2Int(i, j - 2)] != Wall))
                            {
                                ceilingList[new Vector2Int(i, j)] = 14;
                                continue;
                            }
                            {   //기본 경우의 수
                                if ((mapList[new Vector2Int(i, j - 1)] == Wall) && ((mapList[new Vector2Int(i - 1, j)] != Wall) || (mapList[new Vector2Int(i + 1, j)] != Wall)))
                                {
                                    ceilingList[new Vector2Int(i, j)] = 14;
                                }
                                if ((mapList[new Vector2Int(i + 1, j)] == Wall) && (mapList[new Vector2Int(i, j - 1)] == Wall) && (mapList[new Vector2Int(i, j + 1)] != Wall))
                                {
                                    ceilingList[new Vector2Int(i, j + 1)] = 5;
                                }
                                if ((mapList[new Vector2Int(i - 1, j)] == Wall) && (mapList[new Vector2Int(i, j - 1)] == Wall) && (mapList[new Vector2Int(i, j + 1)] != Wall))
                                {
                                    ceilingList[new Vector2Int(i, j + 1)] = 6;
                                }
                                if ((mapList[new Vector2Int(i + 1, j)] == Wall) && (mapList[new Vector2Int(i, j + 1)] == Wall) && (mapList[new Vector2Int(i, j - 1)] != Wall))
                                {
                                    ceilingList[new Vector2Int(i, j + 1)] = 7;
                                    continue;
                                }
                                if ((mapList[new Vector2Int(i - 1, j)] == Wall) && (mapList[new Vector2Int(i, j + 1)] == Wall) && (mapList[new Vector2Int(i, j - 1)] != Wall))
                                {
                                    ceilingList[new Vector2Int(i, j + 1)] = 4;
                                    continue;
                                }
                                if ((mapList[new Vector2Int(i - 1, j)] == Wall) && (mapList[new Vector2Int(i, j + 1)] != Wall) && (mapList[new Vector2Int(i, j - 1)] != Wall) && (mapList[new Vector2Int(i + 1, j)] != Wall))
                                {
                                    ceilingList[new Vector2Int(i, j + 1)] = 2;
                                }
                                if ((mapList[new Vector2Int(i + 1, j)] == Wall) && (mapList[new Vector2Int(i, j + 1)] != Wall) && (mapList[new Vector2Int(i, j - 1)] != Wall) && (mapList[new Vector2Int(i - 1, j)] != Wall))
                                {
                                    ceilingList[new Vector2Int(i, j + 1)] = 0;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void OnDrawWall()
        {
            for (int i = -(mapSize.x / 2) - 1; i <= (mapSize.x / 2) + 1; i++)
            {
                for (int j = -(mapSize.y / 2) - 1; j <= (mapSize.y / 2) + 1; j++)
                {
                    if (wallList[new Vector2Int(i, j)] == true)
                    {
                        InstanceRandomTile(i, j, wallTiles, Wall);
                    }
                }
            }
        }

        private void OnDrawCeiling(int x, int y)
        {
            for (int i = x - (mapSize.x / 2); i <= x + (mapSize.x / 2); i++)
            {
                for (int j = y - (mapSize.y / 2); j <= y + (mapSize.y / 2); j++)
                {
                    if (ceilingList[new Vector2Int(i, j)] != -1)
                    {
                        InstanceCeilingTile(i, j, CeilingTiles, ceilingList[new Vector2Int(i, j)]);
                    }
                }
            }
        }

        private void GenerateExit(GameObject[] exitTile, List<Vector3Int> gridPosition)
        {
            Debug.Log("탈출구");
            _boardManager.LayoutObjectAtRandom(exitTile, 1, 1, gridPosition);
        }
        private void GeneratePlayer(GameObject[] player, List<Vector3Int> gridPosition)
        {
            _boardManager.LayoutObjectAtRandom(player, 1, 1, gridPosition);
        }

        private void GenerateMonster(GameObject[] monster, List<Vector3Int> gridPosition)
        {
            _boardManager.LayoutObjectAtRandom(monster, 1, 2, gridPosition);
        }
        private int GetCenterX(RectInt size)
        {
            return size.x + size.width / 2;
        }

        private int GetCenterY(RectInt size)
        {
            return size.y + size.height / 2;
        }

        private void InstanceRandomTile(int x, int y, GameObject[] Tiles, int tile)
        {
            Transform parent = GameObject.Find("Walls").transform;
            GameObject toInstantiate = VoidTile[0];
            toInstantiate = Tiles[Random.Range(0, Tiles.Length)];
            GameObject instance = Instantiate(toInstantiate,
                new Vector3Int(x, y, 0)
                    , Quaternion.identity) as GameObject;

            mapList[new Vector2Int(x, y)] = tile;
            instance.transform.parent = parent;
        }

        private void InstanceCeilingTile(int x, int y, GameObject[] Tiles, int n)
        {
            Transform parent = GameObject.Find("Ceiling").transform;
            GameObject toInstantiate = VoidTile[0];
            toInstantiate = Tiles[n];
            GameObject instance = Instantiate(toInstantiate,
                new Vector3Int(x, y, 0)
                    , Quaternion.identity) as GameObject;
            instance.transform.parent = parent;
        }
    }
}