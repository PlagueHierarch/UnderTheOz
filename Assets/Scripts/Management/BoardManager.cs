using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Runtime.CompilerServices;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count (int  min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public GameObject[] floorTiles;
    //public GameObject[] enemyTiles;
    public GameObject[] wallTiles;
    public GameObject[] CeilingTiles;
    public GameObject VoidTile;

    private Transform boardHolder;
    private List <Vector3> gridPositions = new List<Vector3> ();

    void InitialiseList()
    {
        gridPositions.Clear ();

        for (int x = 1; x < columns - 1; x++)
        {
            for(int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3 (x, y, 0f));
            }
        }
    }
    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
        GameObject toInstantiate = VoidTile;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f)
                    , Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }

        toInstantiate = VoidTile;
        for (int x = -1; x < columns + 1; x++)
        {
            for(int y = -1; y < rows + 1; y++)
            {
                if((x == -1 || x == columns) || (y == -1 || y == rows))
                {
                    toInstantiate = wallTiles[Random.Range(0, wallTiles.Length)];
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f)
                        , Quaternion.identity) as GameObject;

                    instance.transform.SetParent(boardHolder);
                }
            }
        }
        for (int x = -1; x < columns + 1; x++)
        {
            for(int y = 0; y < rows + 2; y++)
            {
                toInstantiate = VoidTile;

                if ((x == -1 || x == columns) && (y < rows + 1 && y > 0))
                {
                    toInstantiate = CeilingTiles[2];
                }
                else if ((x == -1) && (y == 0))
                {
                    toInstantiate = CeilingTiles[5];
                }
                else if ((x == columns) && (y == 0))
                {
                    toInstantiate = CeilingTiles[6];
                }
                else if ((x == -1) && (y == rows + 1))
                {
                    toInstantiate = CeilingTiles[1];
                }
                else if ((x == columns) && (y == rows + 1))
                {
                    toInstantiate = CeilingTiles[0];
                }
                else if (((x > -1) && (x < columns)) && ((y == 0) || (y == rows + 1)))
                {
                    toInstantiate = CeilingTiles[7];
                }
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f)
                        , Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    //타일 테스트용 함수

    //void BoardSetup()
    //{
    //    boardHolder = new GameObject ("Board").transform;
    //    GameObject toInstantiate = VoidTile;

    //    for (int x = -1; x < columns + 2; x++)
    //    {
    //        for(int y = -1; y < rows + 2; y++)
    //        {
    //            {
    //            toInstantiate = VoidTile;
    //            if ((x < columns && x > -1)&&(y < rows && y > -1))
    //            {
    //                toInstantiate = floorTiles[Random.Range (0, floorTiles.Length)];
    //            }
    //            if((x != columns + 1) && ( y != rows + 1))
    //            {
    //                if((x == -1 || x == columns) || (y == -1 || y == rows))
    //                {
    //                    toInstantiate = wallTiles[Random.Range (0, wallTiles.Length)];
    //                }
    //            }

    //            GameObject instance = Instantiate (toInstantiate, new Vector3(x,y,0f)
    //                , Quaternion.identity) as GameObject;

    //                instance.transform.SetParent(boardHolder);
    //            }
    //            {
    //                toInstantiate = VoidTile;
    //                if ((x == -1 || x == columns)&&(y != -1 && y < rows + 1))
    //                {
    //                    toInstantiate = CeilingTiles[2];
    //                }
    //                {
    //                    if ((x == -1) && (y == 0))
    //                    {
    //                        toInstantiate = CeilingTiles[5];
    //                    }
    //                    if ((x == columns) && (y == 0))
    //                    {
    //                        toInstantiate = CeilingTiles[6];
    //                    }
    //                    if ((x == -1) && (y == rows + 1))
    //                    {
    //                        toInstantiate = CeilingTiles[1];
    //                    }
    //                    if ((x == columns) && (y == rows + 1))
    //                    {
    //                        toInstantiate = CeilingTiles[0];
    //                    }
    //                }
    //                if(((x >= 0)&&(x < columns))&&((y == 0)||(y == rows + 1)))
    //                {
    //                    toInstantiate = CeilingTiles[7];
    //                }

    //                if(toInstantiate != VoidTile)
    //                {
    //                    GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f)
    //                        , Quaternion.identity) as GameObject;

    //                    instance.transform.SetParent(boardHolder);
    //                }

    //            }
    //        }
    //    }
    //}

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range (0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range (minimum, maximum + 1);

        for(int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosisiton = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
            Instantiate(tileChoice, randomPosisiton, Quaternion.identity);
        }
    }

    public void SetupScence(int level)
    {
        //BoardSetup();
        //InitialiseList();
    }
}
