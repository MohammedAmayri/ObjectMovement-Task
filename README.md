# Object Movement Game

This is a comand line-based program that simultes the movement of an object on a table.

## How to Run

1. **Clone the Repository:**


2. **Navigate to the Project Directory:**


3. **Compile and Run the Program:**


4. **Follow the On-Screen Instructions:**

    - Enter the table size and object's starting position (width, height, x, y).
    - Enter the movement instructions (0 = quit, 1 = move forward, 2 = move backward, 3 = rotate clockwise, 4 = rotate counterclockwise).

    Note: Use Ctrl+C to exit the game at any time.


## Additional Notes

- The program validates input to ensure correct and meaningful values.
- The table can have different shapes by implementing new classes that implement the `IShape` interface.
- Additional commands can be added easily by extending the command handling logic in the `ExecuteInstructions` method.

Feel free to explore and modify the code to experiment with different scenarios!
