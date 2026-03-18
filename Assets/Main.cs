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
        CreateGrid();
        PlaceMines();
        Calculator();
        SpawnVisual();
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

    void PlaceMines()
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

    void Calculator()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y].isMine) continue;

                int count = 0;

                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        int nx = x + dx;
                        int ny = y + dy;

                        if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                        {
                            if (grid[nx, ny].isMine)
                                count++;
                        }
                    }
                }

                grid[x, y].neighborMines = count;
            }
        }
    }

    void SpawnVisual()
    {
        cellObjects = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject obj = Instantiate(cellPrefab, new Vector3(x, y, 0), Quaternion.identity);
                obj.name = $"Cell {x} {y}";

                CellView view = obj.GetComponent<CellView>();
                view.Init(x, y, this);

                cellObjects[x, y] = obj;
            }
        }
    }

    public void RevealCell(int x, int y)
    {
        Cell cell = grid[x, y];

        if (cell.isRevealed)
            return;

        cell.isRevealed = true;

        if (cell.isMine)
        {
            Debug.Log("GAME OVER");
            ShowAllMines();
            return;
        }

        UpdateCellVisual(x, y);

        if (cell.neighborMines == 0)
        {
            RevealNeighbors(x, y);
        }
    }
    void RevealNeighbors(int x, int y)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    if (!grid[nx, ny].isRevealed)
                    {
                        RevealCell(nx, ny);
                    }
                }
            }
        }
    }

    void UpdateCellVisual(int x, int y)
    {
        Cell cell = grid[x, y];
        CellView view = cellObjects[x, y].GetComponent<CellView>();

        if (cell.isMine)
        {
            view.ShowMine();
        }
        else
        {
            view.ShowNumber(cell.neighborMines);
        }
    }

    void ShowAllMines()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y].isMine)
                {
                    cellObjects[x, y].GetComponent<CellView>().ShowMine();
                }
            }
        }
    }

}
