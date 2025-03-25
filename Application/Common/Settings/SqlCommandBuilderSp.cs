using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Settings
{
    public static class SqlCommandBuilderSp
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
