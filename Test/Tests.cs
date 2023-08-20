using System;
using System.Numerics;
using NUnit.Framework;
using Snake;


namespace Test
{
    public class Tests
    {
        /// <summary>
        /// Проверяем, что метод Update генерирует новую еду после подбора еды змейкой.
        /// </summary>
        [Test]
        public void Update_WithPickUpFoodSituation_ShouldGenerateNewFood()
        {
            var snake = new Snake.Snake(new Vector2(2, 2), new Vector2(1, 0));
            var gameBoard = new GameBoard(25, 10);
            var foodController = new FoodController(gameBoard, snake) {Food = {X = 2, Y = 2}};
            var gameOverController = new GameOverController(gameBoard, snake);
            var inputController = new InputController(snake);
            var gameController = new GameController(foodController, gameOverController, inputController, snake);
            
            gameController.Update(0);
            
            Assert.AreNotEqual(foodController.Food, snake.GetPoint(0),
                "Необходимо генерировать новую еду после подбора ее змейкой.");
        }
        
        /// <summary>
        /// Проверяем, что метод Update увеличивает размер змейки после подбора еды.
        /// </summary>
        [Test]
        public void Update_WithPickUpFoodSituation_ShouldIncreaseSnakeSize()
        {
            var snake = new Snake.Snake(new Vector2(2, 2), new Vector2(1, 0));
            var gameBoard = new GameBoard(25, 10);
            var foodController = new FoodController(gameBoard, snake) {Food = {X = 2, Y = 2}};
            var gameOverController = new GameOverController(gameBoard, snake);
            var inputController = new InputController(snake);
            var gameController = new GameController(foodController, gameOverController, inputController, snake);
            
            gameController.Update(0);
            
            Assert.AreEqual(2, snake.GetSize(),
                "Необходимо увеличивать размер змейки после подбора еды.");
        }

        /// <summary>
        /// Проверяем, что метод Update возвращает True, если змейка находится за пределами игрового поля.
        /// </summary>
        [TestCase(9, 5)]
        [TestCase(0, 5)]
        [TestCase(5, 0)]
        [TestCase(5, 9)]
        public void Update_WithSnakeWentOutside_ShouldReturnTrue(int snakeX, int snakeY)
        {
            var snake = new Snake.Snake(new Vector2(snakeX, snakeY), new Vector2(1, 0));
            var gameBoard = new GameBoard(10, 10);
            var foodController = new FoodController(gameBoard, snake) {Food = {X = 2, Y = 2}};
            var gameOverController = new GameOverController(gameBoard, snake);
            var inputController = new InputController(snake);
            var gameController = new GameController(foodController, gameOverController, inputController, snake);
            
            var actualResult = gameController.Update(0);
            
            Assert.IsTrue(actualResult, "Змейка находится за пределами игрового поля." +
                                        $"\nМетод Update вернул значение: {actualResult}. " +
                                        "Ожидаемое значение: True");
        }

        /// <summary>
        /// Проверяем, что метод Update возвращает False, если змейка находится внутри игрового поля.
        /// </summary>
        [TestCase(1, 5)]
        [TestCase(4, 5)]
        [TestCase(5, 7)]
        [TestCase(3, 2)]
        public void Update_WithSnakeInside_ShouldReturnFalse(int snakeX, int snakeY)
        {
            var snake = new Snake.Snake(new Vector2(snakeX, snakeY), new Vector2(1, 0));
            var gameBoard = new GameBoard(10, 10);
            var foodController = new FoodController(gameBoard, snake) {Food = {X = 2, Y = 2}};
            var gameOverController = new GameOverController(gameBoard, snake);
            var inputController = new InputController(snake);
            var gameController = new GameController(foodController, gameOverController, inputController, snake);
            
            var actualResult = gameController.Update(0);
            
            Assert.IsFalse(actualResult, "Змейка находится внутри игрового поля." +
                                         $"\nМетод Update вернул значение: {actualResult}. " +
                                         "Ожидаемое значение: False");
        }

        /// <summary>
        /// Проверяем, что метод Update возвращает False, если змейка не врезается в себя.
        /// </summary>
        [Test]
        public void Update_WithSnakeDidNotCrashHerself_ShouldReturnFalse()
        {
            var snake = new Snake.Snake(new Vector2(5, 5), new Vector2(1, 0));
            var gameBoard = new GameBoard(10, 10);
            var foodController = new FoodController(gameBoard, snake) {Food = {X = 2, Y = 2}};
            var gameOverController = new GameOverController(gameBoard, snake);
            var inputController = new InputController(snake);
            var gameController = new GameController(foodController, gameOverController, inputController, snake);
            
            var actualResult = gameController.Update(0);
 
            Assert.IsFalse(actualResult, "Змейка НЕ врезалась в себя." +
                                         $"\nМетод Update вернул значение: {actualResult}. " +
                                         "Ожидаемое значение: False");
        }

        /// <summary>
        /// Проверяем, что метод Update возвращает True, если змейка врезается в себя.
        /// </summary>
        [Test]
        public void Update_GameOverSituationSnakeHasCollisionWithOtherParts_ReturnTrue()
        {
            var snake = new Snake.Snake(new Vector2(5, 5), new Vector2(1, 0));
            var gameBoard = new GameBoard(10, 10);
            var foodController = new FoodController(gameBoard, snake) {Food = {X = 2, Y = 2}};
            var gameOverController = new GameOverController(gameBoard, snake);
            var inputController = new InputController(snake);
            var gameController = new GameController(foodController, gameOverController, inputController, snake);
            
            for (var i = 0; i < 7; i++)
            {
                snake.IncreaseSize();
                snake.MoveForward();
            }

            snake.SetMoveDirectionToDown();
            snake.MoveForward();
            snake.SetMoveDirectionToLeft();
            snake.MoveForward();
            snake.SetMoveDirectionToUp();
            snake.MoveForward();
            var actualResult = gameController.Update(0);
            
            Assert.IsTrue(actualResult, "Змейка врезалась в себя." +
                                        $"\nМетод Update вернул значение: {actualResult}. " +
                                        "Ожидаемое значение: True");
        }

        /// <summary>
        /// Проверяем, что метод Update верно перемещает змейку.
        /// </summary>
        [TestCase(ConsoleKey.UpArrow, 0, -1)]
        [TestCase(ConsoleKey.DownArrow, 0, 1)]
        [TestCase(ConsoleKey.LeftArrow, -1, 0)]
        [TestCase(ConsoleKey.RightArrow, 1, 0)]
        public void Update_EmulateButtonPress_SnakeMove(ConsoleKey key, int directionX, int directionY)
        {
            var direction = new Vector2(directionX, directionY);
            var snake = new Snake.Snake(new Vector2(5, 5), new Vector2(0, 0));
            var gameBoard = new GameBoard(10, 10);
            var foodController = new FoodController(gameBoard, snake) {Food = {X = 2, Y = 2}};
            var gameOverController = new GameOverController(gameBoard, snake);
            var inputController = new InputController(snake);
            var gameController = new GameController(foodController, gameOverController, inputController, snake);
            var expectedResult = snake.GetPoint(0) + direction;
            
            gameController.Update(key);
            var actualResult = snake.GetPoint(0);
           
            Assert.AreEqual(expectedResult, actualResult,
                "Змейка движется в неправильном направлении." +
                $"\nПолученные координаты головы змейки: [{actualResult.X}, {actualResult.Y}]. " +
                $"Ожидаемые координаты: [{expectedResult.X}, {expectedResult.Y}].");
        }
    }
}