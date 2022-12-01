namespace Application.Helpers;

/// <summary>
///     Helper class for configuration values set during compilation.
/// </summary>
public static class Configuration
{
    /// <summary>
    ///     Constant boolean flag for whether the application is running in development mode.
    /// </summary>
    /// <remarks>
    ///     This flag is set during compilation, and depends on the csproj file.
    /// </remarks>
    /// <author>
    ///     <name>Mads Mandahl-Barth</name>
    /// </author>
    public static bool IsDebug {
        get {
            #if DEBUG
                return true;
            #else
                return false;
            #endif
        }
    }

    /// <summary>
    ///     Constant boolean flag for whether the application is running in production mode.
    /// </summary>
    /// <remarks>
    ///     This flag is set during compilation, and depends on the csproj file.
    /// </remarks>
    /// <author>
    ///     <name>Mads Mandahl-Barth</name>
    /// </author>
    public static bool IsRelease {
        get {
            #if RELEASE
                return true;
            #else
                return false;
            #endif
        }
    }
}