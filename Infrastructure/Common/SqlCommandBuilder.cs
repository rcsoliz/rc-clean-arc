
namespace Infrastructure.Common
{
    public class SqlCommandBuilder
    {
        public static string Exec(string storedProcedure) =>
            $"EXEC {storedProcedure}";

        public static string BuildExecWithParams(string storedProcedureName, params string[] parameters)
        {
            var paramString = string.Join(", ", parameters);
            return $"EXEC {storedProcedureName} {paramString}";
        }

    }
}
