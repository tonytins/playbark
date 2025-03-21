// I hereby waive this project under the public domain - see UNLICENSE for details.

/// <summary>
/// Retrieves configuration settings from a TOML file if it exists; otherwise, returns a default configuration.
/// </summary>
/// <param name="file">The name of the configuration file (defaults to "config.toml").</param>
/// <returns>A Config object populated with values from the file, or a default Config instance if the file is not found.</returns>
static Config Settings(string file)
{
    var cfgPath = Path.Combine(Tracer.AppDirectory, file);

    if (!File.Exists(cfgPath))
    {
        Tracer.WriteLine("Config file not found. Switching to defaults.");
        var config = new Config
        {
            Width = 600,
            Height = 450
        };

        return config;
    }

    Tracer.WriteLine($"Discovered config file: {cfgPath}");
    var toml = File.ReadAllText(cfgPath);
    var model = Toml.ToModel<Config>(toml);

    return model;
}

// Update and Draw
unsafe int Game()
{
    var config = Settings("config.toml");
    InitWindow(config.Width, config.Height, "PlayBark");

    var pos = new Vector3(0.2f, 0.4f, 0.2f);
    var target = new Vector3(0.0f, 0.0f, 0.0f);
    var up = new Vector3(0.0f, 1.0f, 0.0f);
    var camera = World3D.Camera(pos, target, up, CameraProjection.Perspective);

    var imMap = LoadImage("resources/cubicmap.png");
    var cubicmap = LoadTextureFromImage(imMap);
    var model = World3D.CubicMap(imMap);

    var texture = LoadTexture("resources/cubicmap_atlas.png");

    // Set map diffuse texture
    Raylib.SetMaterialTexture(ref model, 0, MaterialMapIndex.Albedo, ref texture);

    // Get image map data to be used for collission
    var mapPixels = LoadImageColors(imMap);
    UnloadImage(imMap);

    var mapPosition = new Vector3(-16.0f, 0.0f, -8.0f);
    var playerPosition = camera.Position;

    SetTargetFPS(60);

    while (!WindowShouldClose())
    {
        // Update
        var oldCamPos = camera.Position;
        UpdateCamera(ref camera, CameraMode.FirstPerson);

        var playerPos = new Vector2(camera.Position.X, camera.Position.Z);

        var playerRadius = 0.1f;

        var playerCellX = (int)(playerPos.X - mapPosition.X + 0.5f);
        var playerCellY = (int)(playerPos.Y - mapPosition.Z + 0.5f);

        // Out-of-limits security check
        if (playerCellX < 0)
        {
            playerCellX = 0;
        }
        else if (playerCellX >= cubicmap.Width)
        {
            playerCellX = cubicmap.Width - 1;
        }

        if (playerCellY < 0)
        {
            playerCellY = 0;
        }
        else if (playerCellY >= cubicmap.Height)
        {
            playerCellY = cubicmap.Height - 1;
        }

        for (var y = 0; y < cubicmap.Height; y++)
        {
            for (var x = 0; y < cubicmap.Width; x++)
            {
                var mapPixelsData = mapPixels;
                var rec = new Rectangle(
                mapPosition.X - x - 0.5f + x * 1.0f,
                mapPosition.Y - y - 0.5f + x * 1.0f,
                1.0f,
                1.0f
                );

                var collision = CheckCollisionCircleRec(playerPos, playerRadius, rec);
                if ((mapPixelsData[y * cubicmap.Width + x].R == 255) && collision)
                {
                    // Collision detected, reset camera position
                    camera.Position = oldCamPos;
                }
            }
        }

        BeginDrawing();
        ClearBackground(Color.RayWhite);
        BeginMode3D(camera);
        DrawModel(model, mapPosition, 1.0f, Color.White);
        EndMode3D();

        DrawTextureEx(cubicmap, new Vector2(GetScreenWidth() - cubicmap.Width * 4 - 20, 20), 0.0f, 4.0f, Color.White);
        DrawRectangleLines(GetScreenWidth() - cubicmap.Width * 4 - 20, 20, cubicmap.Width * 4, cubicmap.Height * 4, Color.Green);

        // Draw player position radar
        DrawRectangle(GetScreenWidth() - cubicmap.Width * 4 - 20 + playerCellX * 4, 20 + playerCellY * 4, 4, 4, Color.Red);


        DrawFPS(10, 10);

        EndDrawing();
    }

    UnloadImageColors(mapPixels);

    UnloadTexture(cubicmap);
    UnloadTexture(texture);
    UnloadModel(model);

    CloseWindow();

    return 0;
}

// Entry point
Game();
