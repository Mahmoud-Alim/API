using DotNetEnv;

namespace API;

public static class EnvFileLoader
{
    public static void Load()
    {
        var envFilePath = FindEnvFile();
        if (envFilePath is not null)
        {
            Env.Load(envFilePath);
        }
        else
        {
            Env.Load();
        }
    }

    private static string? FindEnvFile()
    {
        var startingPoints = new[]
        {
            Directory.GetCurrentDirectory(),
            AppContext.BaseDirectory
        };

        foreach (var start in startingPoints.Distinct(StringComparer.Ordinal))
        {
            var directory = new DirectoryInfo(start);
            while (directory is not null)
            {
                var candidate = Path.Combine(directory.FullName, ".env");
                if (File.Exists(candidate))
                {
                    return candidate;
                }

                directory = directory.Parent;
            }
        }

        return null;
    }
}
