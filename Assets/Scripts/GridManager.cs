using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class GridManager : MonoBehaviour
{
    public int gridSize = 10;
    public int mineCount = 10;

    public GameObject cellPrefab;
    public Transform gridParent;

    [Header("Sprites")]
    public Sprite closedSprite;
    public Sprite emptySprite;
    public Sprite flagSprite;
    public Sprite mineSprite;
    public Sprite explodedMineSprite;
    public Sprite wrongFlagSprite;
    public Sprite[] numberSprites; 

    private Cell[,] grid;

    public int flagsLeft;
    public bool isGameOver;

    [Header("UI")]
    public TextMeshProUGUI flagsText;
    public TextMeshProUGUI timerText;

    private int timeElapsed = 0;
    private bool timerRunning = false;
    IEnumerator Start()
    {
        yield return null;

        SetupGridSize();
        GenerateGrid();
        UpdateFlagsUI();
        UpdateTimerUI();
    }

    void GenerateGrid()
    {
        flagsLeft = mineCount;
        grid = new Cell[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                GameObject obj = Instantiate(cellPrefab, gridParent);
                Cell cell = obj.GetComponent<Cell>();

                cell.x = x;
                cell.y = y;
                cell.Init(this);

                grid[x, y] = cell;
            }
        }

        PlaceMines();
        CalculateNumbers();
    }

    void PlaceMines()
    {
        int placed = 0;

        while (placed < mineCount)
        {
            int x = Random.Range(0, gridSize);
            int y = Random.Range(0, gridSize);

            if (!grid[x, y].isMine)
            {
                grid[x, y].isMine = true;
                placed++;
            }
        }
    }

    void CalculateNumbers()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (grid[x, y].isMine) continue;

                int count = 0;

                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        int nx = x + dx;
                        int ny = y + dy;

                        if (IsInside(nx, ny) && grid[nx, ny].isMine)
                            count++;
                    }
                }

                grid[x, y].neighborMines = count;
            }
        }
    }

    public void OpenNeighbors(int x, int y)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int nx = x + dx;
                int ny = y + dy;

                if (IsInside(nx, ny))
                {
                    if (!grid[nx, ny].isOpen)
                        grid[nx, ny].Open();
                }
            }
        }
    }

    bool IsInside(int x, int y)
    {
        return x >= 0 && y >= 0 && x < gridSize && y < gridSize;
    }

    public void GameOver(Cell explodedCell)
    {
        isGameOver = true;
        timerRunning = false;
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Cell cell = grid[x, y];

                if (cell.isMine && !cell.isFlagged)
                {
                    cell.SetSprite(mineSprite);
                }

                if (cell == explodedCell)
                {
                    cell.SetSprite(explodedMineSprite);
                }

                if (cell.isFlagged && !cell.isMine)
                {
                    cell.SetSprite(wrongFlagSprite);
                }
            }
        }
    }

    public void CheckWin()
    {
        int correctFlags = 0;

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Cell cell = grid[x, y];

                if (cell.isMine && cell.isFlagged)
                    correctFlags++;
            }
        }

        if (correctFlags == mineCount)
        {
            Win();
        }
    }

    void Win()
    {
        isGameOver = true;
        timerRunning = false;
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Cell cell = grid[x, y];

                if (!cell.isMine)
                {
                    cell.SetSprite(emptySprite);
                }
            }
        }
    }
    void SetupGridSize()
    {
        GridLayoutGroup layout = gridParent.GetComponent<GridLayoutGroup>();
        RectTransform rect = gridParent.GetComponent<RectTransform>();

        float size = rect.rect.width;

        float spacingFactor = 0.025f;

        float totalUnits = gridSize + spacingFactor * (gridSize - 1);

        float cellSize = size / totalUnits;

        float spacing = cellSize * spacingFactor;

        layout.cellSize = new Vector2(cellSize, cellSize);
        layout.spacing = new Vector2(spacing, spacing);
        layout.constraintCount = gridSize;
    }
    public bool IsTimerRunning()
    {
        return timerRunning;
    }
    public void UpdateFlagsUI()
    {
        flagsText.text = flagsLeft.ToString("000");
    }
    void UpdateTimerUI()
    {
        timerText.text = timeElapsed.ToString("000");
    }
    IEnumerator Timer()
    {
        timerRunning = true;

        while (timerRunning)
        {
            yield return new WaitForSeconds(1f);
            timeElapsed++;
            UpdateTimerUI();
        }
    }
    public void StartTimer()
    {
        StartCoroutine(Timer());
    }
}