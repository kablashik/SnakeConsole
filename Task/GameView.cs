using System;
using System.Numerics;

namespace Snake;

/// <summary>
/// Класс отвечает за отрисовку игры.
/// </summary>
public class GameView
{
    // Настройки отрисовки игры.
    private GameViewSettings _gameViewSettings;

    // Змейка.
    private Snake _snake;

    // Позиция хвоста змейки.
    private Vector2 _snakeTail;

    // Игровое поле.
    private GameBoard _gameBoard;
            
    // Контроллер еды.
    private FoodController _foodController; 

    public GameView(GameViewSettings gameViewSettings, GameBoard gameBoard, Snake snake, FoodController foodController)
    {
        _gameViewSettings = gameViewSettings;
        _snake = snake;
        _foodController = foodController;
        _gameBoard = gameBoard;

        var snakeTailIndex = _snake.GetSize() - 1;
        _snakeTail = _snake.GetPoint(snakeTailIndex);
    }

    /// <summary>
    /// Метод рисует все игровые объекты.
    /// </summary>
    public void DrawGameObjects()
    {
        DrawFood();
        DrawSnake();
    }

    /// <summary>
    /// Метод рисует карту по созданному массиву символов.
    /// </summary>
    public void DrawMap()
    {
        // Создаем массив карты.
        var map = CreateMap();

        // Создаем цикл, в котором проходим по массиву map и выводим его на экран.
        for (var i = 0; i < map.GetLength(0); i++)
        {
            for (var j = 0; j < map.GetLength(1); j++)
            {
                Console.Write(map[i, j]);
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Метод создает массив символов на основании символьных полей класса GameViewSettings,
    /// по которому в методе DrawMap будет нарисована карта.
    /// </summary>
    private char[,] CreateMap()
    {
        // Создаем массив с картой - конвертируем значения в int, так как координаты вектора имеют тип float.
        var map = new char[(int)_gameBoard.Size.Y, (int)_gameBoard.Size.X];

        // Создаем цикл, в котором заполняем массив игрового поля символами из полей _gameViewSettings. 
        for (var i = 0; i < map.GetLength(0); i++)
        {
            for (var j = 0; j < map.GetLength(1); j++)
            {
                // // Если элемент массива на горизонтальных границах игрового поля.
                if (i == 0 || i == map.GetLength(0) - 1)
                {
                    // Если элемент массива при этом на углу игрового поля.
                    if (j == 0 || j == map.GetLength(1) - 1)
                    {
                        map[i, j] = _gameViewSettings.MapCorner;
                    }
                    else
                    {
                        // Если элемент массива на горизонтальной стороне игрового поля.
                        map[i, j] = _gameViewSettings.MapHorizontal;
                    }
                }
                // Если элемент массива на вертикальной стороне игрового поля.
                else if (j == 0 || j == map.GetLength(1) - 1)
                {
                    map[i, j] = _gameViewSettings.MapVertical;
                }
                else
                {
                    // Иначе - внутреннее поле.
                    map[i, j] = _gameViewSettings.FieldPart;
                }
            }
        }

        // Возвращаем из метода массив с картой.
        return map;
    }

    /// <summary>
    /// Метод рисует еду.
    /// </summary>
    private void DrawFood()
    {
        Console.SetCursorPosition((int) _foodController.Food.X, (int) _foodController.Food.Y);
        Console.Write(_gameViewSettings.Food);
    }

    /// <summary>
    /// Метод рисует змейку.
    /// </summary>
    private void DrawSnake()
    {
        var snakeSize = _snake.GetSize();
        // Цикл для отрисовки змейки.
        for (var i = 0; i < snakeSize; i++)
        {
            // Сетим курсор на текущий сегмент змейки.
            Console.SetCursorPosition((int) _snake.GetPoint(i).X, (int) _snake.GetPoint(i).Y);

            // Если индекс равен нулю - значит это голова змейки.
            if (i == 0)
            {
                // Рисуем голову змейки.
                Console.Write(_gameViewSettings.SnakeHead);
            }
            else
            {
                // Иначе, рисуем тело змейки.
                Console.Write(_gameViewSettings.SnakeBody);
            }
        }

        // Если сохраненная позиция хвоста не равна текущей позиции хвоста змейки (змейка передвинулась).
        if (_snakeTail != _snake.GetPoint(snakeSize - 1))
        {
            // Сетим курсор в прошлую позицию хвоста змейки.
            Console.SetCursorPosition((int) _snakeTail.X, (int) _snakeTail.Y);
            
            // Рисуем пустой участок игрового поля на месте прошлой позиции хвоста.
            Console.Write(_gameViewSettings.FieldPart);

            // Полю с позицией хвоста змейки присваиваем новую позицию хвоста змейки.
            _snakeTail = _snake.GetPoint(snakeSize - 1);
        }
    }
}