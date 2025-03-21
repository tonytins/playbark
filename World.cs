// I hereby waive this project under the public domain - see UNLICENSE for details.
namespace PlayBark;

internal static class World
{
    public static Camera3D Camera(Vector3 pos, Vector3 target, Vector3 up, CameraProjection projection)
    {
        Camera3D camera = new();
        camera.Position = pos;
        camera.Target = target;
        camera.Up = up;
        camera.FovY = 45.0f;
        camera.Projection = projection;

        return camera;
    }
}