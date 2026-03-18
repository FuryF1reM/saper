using UnityEngine;

public class Main : MonoBehaviour
{
    [Header("SetkaNastroyki")]
    public int width = 10;
    public int height = 10;
    public int mineCount = 10;

    [Header("Referensi")]
    public GameObject cellPrefab;
    private Cell[,] grid;
    private GameObject[,] cellObjects;

    void Start()
    {
        
    }

    public class Cell
    {
        public bool isMine;
        public bool isRevealed;
        public int neighborMines;
    }

    void CreateGrid()
    {
        grid = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new Cell();
            }
        }
    }

    void PlaceCell()
    {
        int placed = 0;

        while (placed < mineCount)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            if (!grid[x, y].isMine)
            {
                grid[x, y].isMine = true;
                placed++;
            }
        }
    }

    

}
