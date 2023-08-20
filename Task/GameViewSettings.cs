namespace Snake;

/// <summary>
/// Класс с настройками отрисовки игры (из каких элементов будет отрисовываться наша игра).
/// </summary>
public class GameViewSettings
{
    // Пустой символ - внутренняя часть игрового поля.
    public char FieldPart = ' ';
        
    // Символ горизонтальных линий игрового поля (верхняя и нижняя границы).
    public char MapHorizontal = '-';
        
    // Символ вертикальных линий игрового поля (левая и правая границы).
    public char MapVertical = '|';
        
    // Символ углов игрового поля (4 угла).
    public char MapCorner = '+';
        
    // Поле - голова змейки.
    public char SnakeHead = '@';
    
    // Поле - тело змейки.
    public char SnakeBody = '#';
    
    // Поле - еда.
    public char Food = 'X';
}