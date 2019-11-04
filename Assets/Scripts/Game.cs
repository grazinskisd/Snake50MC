using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Grid grid;
    public float snakeMoveTime;
    public Text scoreText;
    public GameObject endPanel;
    public Text finalScoreText;
    public Button restartButton;

    private bool _isRunning = true;
    private List<Point> _snake;
    private Direction _movementDirection = Direction.RIGHT;
    private float _timeSinceLastMove;
    private int _score = 0;

    private void Awake()
    {
        _snake = new List<Point>();
        _snake.Add(new Point(5, 3));
        _snake.Add(new Point(4, 3));
        _snake.Add(new Point(3, 3));
    }

    // Start is called before the first frame update
    void Start()
    {
        endPanel.SetActive(false);
        restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
        DrawSnake();
        SpawnFood();
    }

    private void SpawnFood()
    {
        List<Point> emptyCells = grid.GetEmptyCells();
        int randomCell = UnityEngine.Random.Range(0, emptyCells.Count);
        grid.IncCell(emptyCells[randomCell], 1);
    }

    private void DrawSnake()
    {
        for (int i = 0; i < _snake.Count; i++)
        {
            grid.IncCell(_snake[i], 1);
        }
    }

    private void EraseSnake()
    {
        for (int i = 0; i < _snake.Count; i++)
        {
            grid.IncCell(_snake[i], -1);
        }
    }

    private void UpdateSnake()
    {
        Point nextPosition = new Point(_snake[0].x + _movementDirection.x, _snake[0].y + _movementDirection.y);
        if (!grid.IsInsideBounts(nextPosition) || IsInsideSnake(nextPosition))
        {
            _isRunning = false;
            endPanel.SetActive(true);
            finalScoreText.text = string.Format("Score: {0}", _score);
        }
        else
        {
            if(grid.IsOccupied(nextPosition))
            {
                SpawnFood();
                _score++;
                scoreText.text = _score.ToString();
            }
            EraseSnake();
            UpdateSnakePosition(nextPosition);
            DrawSnake();
        }
    }

    private bool IsInsideSnake(Point p)
    {
        for (int i = 0; i < _snake.Count; i++)
        {
            if(_snake[i].x == p.x && _snake[i].y == p.y)
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateSnakePosition(Point nextPosition)
    {
        for (int i = 0; i < _snake.Count; i++)
        {
            Point tmp = _snake[i];
            _snake[i] = nextPosition;
            nextPosition = tmp;
        }

        if (grid.IsOccupied(nextPosition))
        {
            _snake.Add(nextPosition);
            grid.IncCell(nextPosition, -1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isRunning)
        {
            BindKeyToDirection(KeyCode.W, Direction.UP);
            BindKeyToDirection(KeyCode.S, Direction.DOWN);
            BindKeyToDirection(KeyCode.A, Direction.LEFT);
            BindKeyToDirection(KeyCode.D, Direction.RIGHT);

            _timeSinceLastMove += Time.deltaTime;
            if (_timeSinceLastMove >= snakeMoveTime)
            {
                _timeSinceLastMove = 0;
                UpdateSnake();
            }
        }
    }

    public void BindKeyToDirection(KeyCode code, Direction dir)
    {
        if (Input.GetKeyDown(code))
        {
            _movementDirection = dir;
            UpdateSnake();
            _timeSinceLastMove = 0;
        }
    }
}

public class Direction: Point
{
    public static Direction UP = new Direction(0, -1);
    public static Direction DOWN = new Direction(0, 1);
    public static Direction LEFT = new Direction(-1, 0);
    public static Direction RIGHT = new Direction(1, 0);

    public Direction(int x, int y): base(x, y)
    {
    }
}
