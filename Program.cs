// I hereby waive this project under the public domain - see UNLICENSE for details.

// Initialization
//--------------------------------------------------------------------------------------
const int screenWidth = 600;
const int screenHeight = 450;
Raylib.InitWindow(screenWidth, screenHeight, "PlayBark");

// Based on WavingCube example:
// https://github.com/raylib-cs/raylib-cs/blob/master/Examples/Models/WavingCubes.cs

// Initialize the camera
var camera = new Camera3D();
camera.Position = new Vector3(30.0f, 20.0f, 30.0f);
camera.Target = new Vector3(0.0f, 0.0f, 0.0f);
camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
camera.FovY = 70.0f;
camera.Projection = CameraProjection.Perspective;

const int numBlocks = 15;

Raylib.SetTargetFPS(60);

// Main game loop
while (!Raylib.WindowShouldClose())
{
    var time = Raylib.GetTime();
    var scale = (2.0f + (float)Math.Sin(time)) * 0.7f;

    var cameraTime = time * 0.3;

    camera.Position.X = (float)Math.Cos(cameraTime) * 40.0f;
    camera.Position.Z = (float)Math.Cos(cameraTime) * 40.0f;

    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.RayWhite);

    Raylib.BeginMode3D(camera);

    Raylib.DrawGrid(10, 5.0f);

    for (int x = 0; x < numBlocks; x++)
    {
        for (int y = 0; y < numBlocks; y++)
        {
            for (int z = 0; z < numBlocks; z++)
            {
                var blockScale = (x + y + z) / 30.0f;
                var scatter = (float)Math.Sin(blockScale * 20.0f + (float)(time * 4.0f));

                var cubePos = new Vector3(
                       (float)(x - numBlocks / 2) * (scale * 3.0f) + scatter,
                        (float)(x - numBlocks / 2) * (scale * 2.0f) + scatter,
                        (float)(x - numBlocks / 2) * (scale * 3.0f) + scatter
                );

                var cubeColor = Raylib.ColorFromHSV((float)(((x + y + z) * 18) % 360), 0.75f, 0.9f);

                var cubeSize = (2.4f - scale) * blockScale;

                Raylib.DrawCube(cubePos, cubeSize, cubeSize, cubeSize, cubeColor);
            }
        }
    }

    Raylib.EndMode3D();

    Raylib.DrawFPS(10, 10);

    Raylib.EndDrawing();
}

Raylib.CloseWindow();
