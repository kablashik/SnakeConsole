using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace Snake
{
    public class Task
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false; // Команда для отключения курсора.
                
            // Создаем контроллеры и все игровые объекты.
            CreateGameViewAndControllers(out var gameController, out var gameView);
            
            gameView.DrawMap(); // Нарисовали карту.

            while (true)
            {
                // Отрисоваем все игровые объекты.
                gameView.DrawGameObjects(); 
                
                Thread.Sleep(200); // Задержка в 200мс.

                var pressedKey = GetPressedKey(); // Получаем нажатую клавишу.
                
                // Проверяем значение, возвращенное из метода Update.
                if (gameController.Update(pressedKey)) 
                {
                    Console.Clear(); // При проигрыше стираем все игровое поле.
                    Console.WriteLine("You lose"); // Выводим сообщение о проигрыше.
                    break;
                }
            }
        }

        /// <summary>
        /// Метод создает контроллеры и все игровые объекты.
        /// </summary>
        private static void CreateGameViewAndControllers(out GameController gameController, out GameView gameView)
        {
            var gameViewSettings = new GameViewSettings();
            var snake = new Snake(new Vector2(14, 3), new Vector2(1, 0));
            var gameBoard = new GameBoard(30, 8);
            var gameOverController = new GameOverController(gameBoard, snake);
            var inputController = new InputController(snake);
            var foodController = new FoodController(gameBoard, snake);
            
            foodController.GenerateNewFood(); // Создаем первую еду.

            gameView = new GameView(gameViewSettings, gameBoard, snake, foodController);
            gameController = new GameController(foodController, gameOverController, inputController, snake);
        }

        /// <summary>
        /// Метод получает нажатую клавишу.
        /// </summary>
        private static ConsoleKey GetPressedKey()
        {
            if (Console.KeyAvailable)
            {
                return Console.ReadKey(true).Key;
            }

            return ConsoleKey.NoName;
        }
    }

    /// <summary>
    /// Класс, отвечающий за процесс игры.
    /// </summary>
    public class GameController
    {
        private FoodController _foodController;
        private GameOverController _gameOverController;
        private InputController _inputController;
        private Snake _snake;
        
        /// <summary>
        /// Конструктор объекта GameController, который сохраняет ссылки на объекты классов:
        /// FoodController, GameOverController, InputController, Snake.
        /// </summary>
        public GameController(FoodController foodController, GameOverController gameOverController,
            InputController inputController, Snake snake)
        {
            // Конструктор сохраняет игровые контроллеры в поля класса.
            _foodController = foodController;
            _gameOverController = gameOverController;
            _inputController = inputController;
            _snake = snake;
        }

        /// <summary>
        /// Метод содержит в себе всю игровую механику:
        /// 1)Проверяет, проиграл ли игрок
        /// 2)Проверяет, подобрала змейку еду, если подобрала генерирует новую.
        /// 3)Меняет направление змейки при нажатой кнопке, иначе двигает змейку в том же направлении.
        /// Возвращает True если игрок проиграл, иначе False
        /// </summary>
        public bool Update(ConsoleKey consoleKey)
        {
            //TODO: Допишите реализацию данного метода.
            // Если игрок проиграл - возвращаем true.
            if (_gameOverController.CheckGameOver())
            {
                return true;
            }
            // Если змейка подобрала еду - увеличиваем размер змейки и генерируем новую еду.
            if (_foodController.IsFoodPickedUp())
            {
                _snake.IncreaseSize();
                _foodController.GenerateNewFood();
            }

            // Если нажата кнопка на клавиатуре (нажатая клавиша не равна ConsoleKey.NoName) - меняем направление змейке.
            if (consoleKey != ConsoleKey.NoName)
            {
                _inputController.ProcessInput(consoleKey);
            }
            else
            {
                // Иначе, двигаем змейку в прежнем направлении.
                _inputController.MoveSnake();
            }

            // Возвращаем false. До этой строки дойдем в случае, если игрок не проиграл.
            return false;
        }
    }
}