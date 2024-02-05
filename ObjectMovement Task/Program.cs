using System;

namespace ObjectMovement_Task
{

    class Program
    {
        static void Main(string[] args)
        {
            ObjectMovementGame objectMovementGame = new ObjectMovementGame();
            Console.CancelKeyPress += (sender, eventArgs) => {
                Console.WriteLine("Exiting the game.");
                Environment.Exit(0);
            };
            while (true)
            {
                objectMovementGame.StartGame();
            }
        }
    }

    class ObjectMovementGame
    {
        public void StartGame()
        {
            Console.WriteLine("Welcome to this game for object moving (Yey!). Please enter table size and object's starting position (width, height, x, y). To exit the game at anytime (Ctrl+C)");
            var (width, height, x, y) = GetTableDimensionsAndObjectPosition();
            Rectangle rectangle = new Rectangle();
            Board board = new Board(width, height, rectangle);
            Object object1 = new Object(x, y);
            Console.WriteLine($"Current position: {object1.GetPosition()}");
            Console.WriteLine("Now please enter the instructions");
            int[] instructions = GetInstructions();

            ExecuteInstructions(object1, board, instructions);
        }
        private (int, int, int, int) GetTableDimensionsAndObjectPosition()
        {
            (int, int, int, int) result;
            do
            {
                Console.WriteLine("Enter table size and object's starting position (width, height, x, y):");
                string input = Console.ReadLine();
                result = ValidateAndConvertinitialInput(input);
            } while (result == (int.MinValue, int.MinValue, int.MinValue, int.MinValue));

            return result;
        }
       
        private (int, int, int, int) ValidateAndConvertinitialInput(string input)
        {
            string[] inputValues = input.Split(',');

            if (inputValues.Length != 4)
            {
                Console.WriteLine("Invalid input. Please enter four values separated by commas.");
                return (int.MinValue, int.MinValue, int.MinValue, int.MinValue);
            }

            int[] result;
            if (!Array.TrueForAll(inputValues, val => int.TryParse(val, out _)))
            {
                Console.WriteLine("Invalid input. Please enter valid integer values.");
                return (int.MinValue, int.MinValue, int.MinValue, int.MinValue);
            }

            result = Array.ConvertAll(inputValues, int.Parse);

            // Validate table dimensions
            if (result[0] <= 0 || result[1] <= 0)
            {
                Console.WriteLine("Invalid table dimensions. Width and height must be positive integers.");
                return (int.MinValue, int.MinValue, int.MinValue, int.MinValue);
            }

            // Validate initial position
            if (result[2] < 0 || result[3] < 0 || result[2] >= result[0] || result[3] >= result[1])
            {
                Console.WriteLine("Invalid initial position. Coordinates must be non-negative and within the table bounds.");
                return (int.MinValue, int.MinValue, int.MinValue, int.MinValue);
            }

            return (result[0], result[1], result[2], result[3]);
        }


        private int[] GetInstructions()
        {
            int[] instructions;
            do
            {
                Console.WriteLine("Enter commands (0 = quit, 1 = move forward, 2 = move backward, 3 = rotate clockwise, 4 = rotate counterclockwise). Entering any other instruction will be ignored by the game");
                string input = Console.ReadLine();
                instructions = ValidateAndConvertInput(input);
            } while (instructions == null);

            return instructions;
        }

        private int[] ValidateAndConvertInput(string input)
        {
            string[] inputValues = input.Split(',');

            if (inputValues.Length == 0)
            {
                Console.WriteLine("Invalid input. Please enter at least one value.");
                return null;
            }

            int[] result;
            if (!Array.TrueForAll(inputValues, val => int.TryParse(val, out _)))
            {
                Console.WriteLine("Invalid input. Please enter valid integer values");
                return null;
            }

            result = Array.ConvertAll(inputValues, int.Parse);

            return result;
        }


        private void ExecuteInstructions(Object object1, Board board, int[] instructions)
        {
            foreach (int instruction in instructions)
            {
               
                if (instruction == 1 || instruction == 2)
                {
                    object1.MoveObject(instruction,object1.Direction);
                    if (board.IsOutOfBounds(object1.XPosition,object1.YPosition))
                    {
                        Console.WriteLine("[-1, -1]");
                        break;
                    }
                }
                else if (instruction == 3 || instruction == 4)
                {
                    object1.UpdateDirectionIndex(instruction);
                }
                else if (instruction == 0)
                {
                    FinishGame(object1, board);
                }
            }
        }

        private void FinishGame(Object object1, Board board)
        {
            if (board.IsOutOfBounds(object1.XPosition, object1.YPosition))
            {
                Console.WriteLine("[-1, -1]");
            }
            else
            {
                Console.WriteLine($"The object's final position as two integers: {object1.GetPosition()}");
            }
        }
    }

    public interface IShape
    {
        bool IsOutOfBounds(int x, int y, int width, int length);
    }

    public class Rectangle : IShape
    {
        public bool IsOutOfBounds(int x, int y, int width, int length)
        {

            return x < 0 || y < 0 || x >= width || y >= length;
        }
    }

    class Board
    {
        private int width;
        private int length;
        private IShape shape;
        private int rotation; 

        public Board(int width, int length, IShape shape)
        {
            this.width = width;
            this.length = length;
            this.shape = shape;
            this.rotation = 0;
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Length
        {
            get { return length; }
            set { length = value; }
        }
        public int Rotation
        {
            get { return rotation; }
            set { rotation = value % 360; }
        }

        public bool IsOutOfBounds(int x, int y)
        {
            return shape.IsOutOfBounds(x, y, width, length);
        }

    }




    class Object
    {
        int xPosition;
        int yPosition;
        char[] directions = { 'N', 'E', 'S', 'W' };
        int currentDirectionIndex = 0;
        char direction;
        public Object(int xPostion,int yPosition)
        {
            this.xPosition = xPostion;
            this.yPosition = yPosition;
            direction = directions[0];
        }

        public int XPosition
        {
            get { return xPosition; }
        }

        public int YPosition
        {
            get { return yPosition; }
        }

        public char Direction
        {
            get { return direction; }
        }

        

        public string GetPosition()
        {
            return $"{xPosition}, {yPosition}";
        }

        public void UpdateDirectionIndex(int rotationDirection)
        {
            if(rotationDirection == 3)
            {
                rotationDirection = 1;
            }
            else
            {
                rotationDirection = -1;
            }
            
            currentDirectionIndex = (currentDirectionIndex + rotationDirection + directions.Length) % directions.Length;
            direction = directions[currentDirectionIndex];
        }

      

        public void MoveObject(int movement,char direction)
        {

            switch (direction)
            {
                case 'N':
                    if (movement == 1)
                        yPosition--;
                    else if (movement == 2)
                        yPosition++;
                    break;

                case 'E':
                    if (movement == 1)
                        xPosition++;
                    else if (movement == 2)
                        xPosition--;
                    break;

                case 'S':
                    if (movement == 1)
                        yPosition--;
                    else if (movement == 2)
                        yPosition++;
                    break;

                case 'W':
                    if (movement == 1)
                        xPosition--;
                    else if (movement == 2)
                        xPosition++;
                    break;
            }
        }

    }
}
