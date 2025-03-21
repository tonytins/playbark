// I hereby waive this project under the public domain - see UNLICENSE for details.
namespace PlayBark;

/// <summary>
/// Provides utilities for creating and managing 3D world elements, including camera setup and map generation.
/// </summary>
internal static class World3D
{
    /// <summary>
    /// Creates and returns a configured Camera3D instance.
    /// </summary>
    /// <param name="pos">The position of the camera in 3D space.</param>
    /// <param name="target">The point the camera is looking at.</param>
    /// <param name="up">The up direction for the camera.</param>
    /// <param name="projection">The projection type of the camera.</param>
    /// <returns>A configured Camera3D instance.</returns>
    public static Camera3D Camera(Vector3 pos, Vector3 target, Vector3 up, CameraProjection projection)
    {
        var camera = new Camera3D();
        camera.Position = pos;
        camera.Target = target;
        camera.Up = up;
        camera.FovY = 45.0f;
        camera.Projection = projection;

        return camera;
    }

    /// <summary>
    /// Generates a 3D cubic map model from a given image map.
    /// </summary>
    /// <param name="imMap">The image used to generate the cubic map.</param>
    /// <returns>A Model representing the cubic map.</returns>
    public static Model CubicMap(Image imMap)
    {
        var mesh = GenMeshCubicmap(imMap, new Vector3(1.0f, 1.0f, 1.0f));
        var model = LoadModelFromMesh(mesh);

        return model;
    }
}