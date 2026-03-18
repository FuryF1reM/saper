using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    public int x;
    public int y;

    public bool isMine;
    public bool isOpen;
    public bool isFlagged;

    public int neighborMines;

    private Image image;
    private GridManager grid;

    public void Init(GridManager gridManager)
    {
        grid = gridManager;
        image = GetComponent<Image>();
        SetSprite(grid.closedSprite);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (grid.isGameOver) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isFlagged)
            {
                ToggleFlag();
                return;
            }

            if (!isOpen)
                Open();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            ToggleFlag();
        }
    }

    void ToggleFlag()
    {
        if (isOpen) return;
        
        if (!isFlagged && grid.flagsLeft <= 0) return;

        isFlagged = !isFlagged;

        if (isFlagged)
        {
            grid.flagsLeft--;
            SetSprite(grid.flagSprite);
        }
        else
        {
            grid.flagsLeft++;
            SetSprite(grid.closedSprite);
        }

        grid.CheckWin();
    }

    public void Open()
    {
        if (isOpen || isFlagged) return;

        isOpen = true;

        if (isMine)
        {
            grid.GameOver(this);
            return;
        }

        if (neighborMines > 0)
        {
            SetSprite(grid.numberSprites[neighborMines]);
        }
        else
        {
            SetSprite(grid.emptySprite);
            grid.OpenNeighbors(x, y);
        }
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
