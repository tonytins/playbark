// I hereby waive this project under the public domain - see UNLICENSE for details.

/// <summary>
/// Retrieves configuration settings from a TOML file if it exists; otherwise, returns a default configuration.
/// </summary>
/// <param name="file">The name of the configuration file (defaults to "config.toml").</param>
/// <returns>A Config object populated with values from the file, or a default Config instance if the file is not found.</returns>
static Config ReadConfig(string file)
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

void Init(int screenWidth, int screenHeight, int fps)
{
    var pos = new Vector3(0.2f, 0.4f, 0.2f);
    var target = new Vector3(0.0f, 0.0f, 0.0f);
    var up = new Vector3(0.0f, 1.0f, 0.0f);

    InitWindow(screenWidth, screenHeight, $"PlayBark");
    World3D.InitCamera(pos, target, up, CameraProjection.Perspective);
    SetTargetFPS(fps);
}

int GameLoop()
{
    var config = ReadConfig("config.toml");
    Init(config.Width, config.Height, 60);

    while (!WindowShouldClose())
    {
        BeginDrawing();
        ClearBackground(Color.White);

        DrawFPS(10, 10);

        EndDrawing();
    }

    CloseWindow();

    return 0;
}

GameLoop();
