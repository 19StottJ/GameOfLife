using Raylib_cs;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

const int WIDTH = 180;
const int HEIGHT = 90;

const int SCALE = 9;

bool[] cellsA = new bool[WIDTH * HEIGHT];
bool[] cellsB = new bool[WIDTH * HEIGHT];

bool currentBuffer = false;

Raylib.InitWindow(WIDTH * SCALE, HEIGHT * SCALE, "Game of Life");

bool GetAt(int x, int y)
{
    if (x < 0 || x >= WIDTH)
    {
        return false;
    }
    if (y < 0 || y >= HEIGHT)
    {
        return false;
    }
    if (currentBuffer)
    {
        return cellsA[y * WIDTH + x];
    } else
    {
        return cellsB[y * WIDTH + x];
    }
}

void SetAt(int x, int y, bool state)
{
    if (currentBuffer)
    {
        cellsA[y * WIDTH + x] = state;
    }
    else
    {
        cellsB[y * WIDTH + x] = state;
    }
}

void SimulateBoard()
{
    for (int x = 0; x < WIDTH; x++)
    {
        for (int y = 0; y < HEIGHT; y++)
        {
            bool alive = GetAt(x, y);
            int neighbours = CountNeighbours(x, y);
            bool nextAlive = false;
            if (alive)
            {
                if (neighbours == 2 || neighbours == 3)
                {
                    nextAlive = true;
                }
            } else if (neighbours == 3) {
                nextAlive = true;
            }

            if (currentBuffer)
            {
                cellsB[y * WIDTH + x] = nextAlive;
            } else
            {
                cellsA[y * WIDTH + x] = nextAlive;
            }

        }
    }
    currentBuffer = !currentBuffer;
}

int CountNeighbours(int x, int y)
{
    int total = 0;
    if (GetAt(x+1, y))
    {
        total += 1;
    }
    if (GetAt(x-1, y))
    {
        total += 1;
    }
    if (GetAt(x, y-1))
    {
        total += 1;
    }
    if (GetAt(x, y+1))
    {
        total += 1;
    }
    if (GetAt(x-1, y-1))
    {
        total += 1;
    }
    if (GetAt(x-1, y+1))
    {
        total += 1;
    }
    if (GetAt(x+1, y-1))
    {
        total += 1;
    }
    if (GetAt(x+1, y+1))
    {
        total += 1;
    }
    return total;
}

bool isSimulating = false;

Raylib.SetTargetFPS(30);

while (true)
{

    if (Raylib.WindowShouldClose())
    {
        break;
    }
    Raylib.BeginDrawing();
    for (int x = 0; x < WIDTH; x++)
    {
        for (int y = 0; y < HEIGHT; y++)
        {
            Color color = GetAt(x, y) ? Color.White : Color.Black;
            Color backColor = GetAt(x, y) ? Color.Black : Color.White;
            Raylib.DrawRectangle(x*SCALE, y*SCALE, SCALE, SCALE, color);
            Raylib.DrawRectangleLinesEx(new Rectangle(x * SCALE, y * SCALE, SCALE, SCALE), 0.5F, backColor);
        }
    }
    Raylib.EndDrawing();
    if (Raylib.IsMouseButtonPressed(MouseButton.Left))
    {
        int x = Raylib.GetMouseX() / SCALE;
        int y = Raylib.GetMouseY() / SCALE;
        bool alive = GetAt(x, y);
        SetAt(x, y, !alive);
    }
    if (Raylib.IsKeyPressed(KeyboardKey.Space))
    {
        isSimulating = !isSimulating;
    }

    if (isSimulating)
    {
        SimulateBoard();
    } else if (Raylib.IsKeyPressed(KeyboardKey.Enter))
    {
        SimulateBoard();
    }
}