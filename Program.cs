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
    InitWindow(config.Width, config.Height, $"PlayBark");

    var pos = new Vector3(0.2f, 0.4f, 0.2f);
    var target = new Vector3(0.0f, 0.0f, 0.0f);
    var up = new Vector3(0.0f, 1.0f, 0.0f);
    var camera = World3D.Camera(pos, target, up, CameraProjection.Perspective);

    var imMap = LoadImage("resources/cubicmap.png");
    var cubicMap = LoadTextureFromImage(imMap);
    var mesh = GenMeshCubicmap(imMap, new Vector3(1.0f, 1.0f, 1.0f));
    var model = LoadModelFromMesh(mesh);

    var texture = LoadTexture("resources/cubicmap_atlas.png");

    // Set map diffuse texture
    SetMaterialTexture(ref model, 0, MaterialMapIndex.Albedo, ref texture);

    // Get image map data to be used for collission
    var mapPixels = LoadImageColors(imMap);
    UnloadImage(imMap);

    SetTargetFPS(60);
    while (!WindowShouldClose())
    {
        BeginDrawing();
        ClearBackground(Color.White);

        var oldCamPos = camera.Position;
        UpdateCamera(ref camera, CameraMode.FirstPerson);

        DrawFPS(10, 10);

        EndDrawing();
    }

    UnloadImageColors(mapPixels);

    UnloadTexture(cubicMap);
    UnloadTexture(texture);
    UnloadModel(model);

    CloseWindow();

    return 0;
}

// Entry point
Game();
