using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board : MonoBehaviour
{
    public GameObject TilePrefab;

    public Sprite[] sprites;

    Tile[] tiles;

    public Vector2 TilesOffset = Vector2.one;

    public int width = 6;
    public int heigth = 4;

    public bool canMove = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        CreateTiles();
        ShuffleTiles();
        PlaceTiles();

        canMove = false;
        yield return new WaitForSeconds(2f);
        canMove = true;

        HideTiles();
    }

    void CreateTiles()
    {
        var length = width * heigth;
        tiles = new Tile[length];

        for(int i = 0; i < length; i++)
        {
            var sprite = sprites[i/2];

            tiles[i] = CreateTile(sprite);
        }
    }

    Tile CreateTile(Sprite faceSprite)
    {
        var gameobject = Instantiate(TilePrefab);
        gameobject.transform.parent = transform;

        var tile = gameobject.GetComponent<Tile>();
        tile.uncovered = true;
        tile.frontFace = faceSprite;

        return tile;
    }

    void ShuffleTiles()
    {
        for(int i = 0; i < 1000; i++)
        {
            int indexOne = Random.Range(0, tiles.Length);
            int indexTwo = Random.Range(0, tiles.Length);

            var tileOne = tiles[indexOne];
            var tileTwo = tiles[indexTwo];

            tiles[indexOne] = tileTwo;
            tiles[indexTwo] = tileOne;
        }
    }

    void PlaceTiles()
    {
        for (int i = 0; i < width * heigth; i++)
        {
            int x = i % width;
            int y = i / width;

            tiles[i].transform.localPosition = new Vector3(x * TilesOffset.x, y * TilesOffset.y, 0);
        }
    }

    void HideTiles()
    {
        tiles.ToList().ForEach(tile => tile.uncovered = false);
    }

    bool CheckIfEnd()
    {
        return tiles.All(tile => !tile.active);
    }

    public void CheckPair()
    {
        StartCoroutine(CheckPairCoroutine());
    }

    IEnumerator CheckPairCoroutine()
    {
        var tilesUncovered = tiles
            .Where(tile => tile.active)
            .Where(tile => tile.uncovered)
            .ToArray();

        if (tilesUncovered.Length != 2)
            yield break;

        var tileOne = tilesUncovered[0];
        var tileTwo = tilesUncovered[1];

        canMove = false;
        yield return new WaitForSeconds(0.5f);
        canMove = true;

        if (tileOne.frontFace == tileTwo.frontFace)
        {
            tileOne.active = false;
            tileTwo.active = false;
        }
        else
        {
            tileOne.uncovered = false;
            tileTwo.uncovered = false;
        }

        if(CheckIfEnd())
        {
            canMove = true;
            Debug.Log("Koniec Gry");
            yield return new WaitForSeconds(5f);

            Application.Quit();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
