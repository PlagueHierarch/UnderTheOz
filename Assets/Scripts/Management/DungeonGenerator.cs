using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Runtime.CompilerServices;

namespace DungeonGenerator
{
    public class TreeNode
    {
        public TreeNode leftTree;
        public TreeNode rightTree;
        public TreeNode parentTree;
        public RectInt treeSize;
        public RectInt dungeonSize;

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
        //[SerializeField] private GameObject[] enemyTiles;
        [SerializeField] private GameObject[] floorTiles;
        [SerializeField] private GameObject[] wallTiles;
        [SerializeField] private GameObject[] CeilingTiles;
        [SerializeField] private GameObject VoidTile;

        [SerializeField] private Vector2Int mapSize;

        [SerializeField] private int maxNode;
        [SerializeField] private float minDivideSize;
        [SerializeField] private float maxDivideSize;
        [SerializeField] private int minRoomSize;

        [SerializeField] private GameObject line;
        [SerializeField] private Transform lineHolder;
        [SerializeField] private GameObject rectangle;

        private Transform boardHolder;
        private void Awake()
        {

            OnDrawRectangle(0, 0, mapSize.x, mapSize.y); //���� ����� �°� ���� �׸�
            TreeNode rootNode = new TreeNode(0, 0, mapSize.x, mapSize.y); //��Ʈ�� �� Ʈ�� ����
            DivideTree(rootNode, 0); //Ʈ�� ����
            GenerateDungeon(rootNode, 0); //�� ����
            GenerateRoad(rootNode, 0); //�� ����
        }

        private void DivideTree(TreeNode treeNode, int n) //��� �Լ�
        {
            if (n < maxNode) //0 ���� �����ؼ� ����� �ִ񰪿� �̸� �� ���� �ݺ�
            {
                RectInt size = treeNode.treeSize; //���� Ʈ���� ���� �� ����, �簢���� ������ ��� ���� Rect ���
                int length = size.width >= size.height ? size.width : size.height; //�簢���� ���ο� ���� �� ���̰� �� ����, Ʈ���� ������ ������ ���ؼ����� ���
                int split = Mathf.RoundToInt(Random.Range(length * minDivideSize, length * maxDivideSize)); //���ؼ� ������ �ּ� ������ �ִ� ���� ������ ���� �������� ����
                if (size.width >= size.height) //����
                {
                    treeNode.leftTree = new TreeNode(size.x, size.y, split, size.height); //���ؼ��� ������ ���� ���� split�� ���� ���̷�, ���� Ʈ���� height���� ���� ���̷� ���
                    treeNode.rightTree = new TreeNode(size.x + split, size.y, size.width - split, size.height); //x���� split���� ���� ��ǥ ����, ���� Ʈ���� width���� split���� �� ���� ���� ����
                    OnDrawLine(new Vector2(size.x + split, size.y), new Vector2(size.x + split, size.y + size.height)); //���ؼ� ������
                }
                else //����
                {
                    treeNode.leftTree = new TreeNode(size.x, size.y, size.width, split);
                    treeNode.rightTree = new TreeNode(size.x, size.y + split, size.width, size.height - split);
                    OnDrawLine(new Vector2(size.x, size.y + split), new Vector2(size.x + size.width, size.y + split));
                }
                treeNode.leftTree.parentTree = treeNode; //������ Ʈ���� �θ� Ʈ���� �Ű������� ���� Ʈ���� �Ҵ�
                treeNode.rightTree.parentTree = treeNode;
                DivideTree(treeNode.leftTree, n + 1); //��� �Լ�, �ڽ� Ʈ���� �Ű������� �ѱ�� ��� �� 1 ���� ��Ŵ
                DivideTree(treeNode.rightTree, n + 1);
            }
        }

        private RectInt GenerateDungeon(TreeNode treeNode, int n) //�� ����
        {
            if (n == maxNode) //��尡 �������� ���� ���ǹ� ����
            {
                RectInt size = treeNode.treeSize;
                int width = Mathf.Max(Random.Range(size.width / 2, size.width - 1)); //Ʈ�� ���� ������ ������ ũ�� ����, �ּ� ũ�� : width / 2
                int height = Mathf.Max(Random.Range(size.height / 2, size.height - 1));
                int x = treeNode.treeSize.x + Random.Range(1, size.width - width); //�ִ� ũ�� : width / 2
                int y = treeNode.treeSize.y + Random.Range(1, size.height - height);
                OnDrawDungeon(x, y, width, height); //���� ������
                return new RectInt(x, y, width, height); //���� ���� ������ ũ��� ���� ������ �� ũ�� ������ Ȱ��
            }
            treeNode.leftTree.dungeonSize = GenerateDungeon(treeNode.leftTree, n + 1); //���� �� = ���� ũ��
            treeNode.rightTree.dungeonSize = GenerateDungeon(treeNode.rightTree, n + 1);
            return treeNode.leftTree.dungeonSize; //�θ� Ʈ���� ���� ũ��� �ڽ� Ʈ���� ���� ũ�� �״�� ���
        }

        private void GenerateRoad(TreeNode treeNode, int n) //�� ����
        {
            if (n == maxNode)
            {
                return;
            }//��尡 �������� ���� ���� �������� ����
            int x1 = GetCenterX(treeNode.leftTree.dungeonSize); //�ڽ� Ʈ���� ���� �߾� ��ġ�� ������
            int x2 = GetCenterX(treeNode.rightTree.dungeonSize);
            int y1 = GetCenterY(treeNode.leftTree.dungeonSize);
            int y2 = GetCenterY(treeNode.rightTree.dungeonSize);
            GameObject toInstantiate = VoidTile;
            for (int x = Mathf.Min(x1, x2); x <= Mathf.Max(x1, x2); x++) //x1�� x2�� ���� ���� ������ ���� ū ������ Ÿ�� ����
            {
                toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                GameObject instance = Instantiate(toInstantiate,
                    new Vector3Int(x - mapSize.x / 2, y1 - mapSize.y / 2, 0)
                        , Quaternion.identity) as GameObject;

            }
            for (int y = Mathf.Min(y1, y2); y <= Mathf.Max(y1, y2); y++)
            {
                toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                GameObject instance = Instantiate(toInstantiate,
                    new Vector3Int(x2 - mapSize.x / 2, y - mapSize.y / 2, 0)
                        , Quaternion.identity) as GameObject;
            }
            GenerateRoad(treeNode.leftTree, n + 1);
            GenerateRoad(treeNode.rightTree, n + 1);
        }

        private void OnDrawLine(Vector2 from, Vector2 to) //���� �������� �̿��� ������ �׸��� �޼ҵ�
        {
            LineRenderer lineRenderer = Instantiate(line, lineHolder).GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, from - mapSize / 2);
            lineRenderer.SetPosition(1, to - mapSize / 2);
        }

        private void OnDrawDungeon(int x, int y, int width, int height) //ũ�⿡ ���� Ÿ���� �����ϴ� �޼ҵ�
        {
            GameObject toInstantiate = VoidTile;
            for (int i = x; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                    GameObject instance = Instantiate(toInstantiate, 
                        new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0)
                            , Quaternion.identity) as GameObject;
                }

            }
        }

        private void OnDrawRectangle(int x, int y, int width, int height) //���� �������� �̿��� �簢���� �׸��� �޼ҵ�
        {
            LineRenderer lineRenderer = Instantiate(rectangle, lineHolder).GetComponent<LineRenderer>();
            lineRenderer.positionCount = 5;
            lineRenderer.SetPosition(0, new Vector2(x, y) - mapSize / 2); //��ġ�� ȭ�� �߾ӿ� ����
            lineRenderer.SetPosition(1, new Vector2(x + width, y) - mapSize / 2);
            lineRenderer.SetPosition(2, new Vector2(x + width, y + height) - mapSize / 2);
            lineRenderer.SetPosition(3, new Vector2(x, y + height) - mapSize / 2);
            lineRenderer.SetPosition(4, new Vector2(x, y) - mapSize / 2);

        }

        private int GetCenterX(RectInt size)
        {
            return size.x + size.width / 2;
        }

        private int GetCenterY(RectInt size)
        {
            return size.y + size.height / 2;
        }
    }
}