using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace Sample.Infra.Data.Interceptors
{
    /// <summary>
    /// A interceptor to add 'NoLock' to querys on Entity Framework
    /// </summary>
    public class DbNoLockInterceptor : DbCommandInterceptor
    {
        private static readonly Regex _tableAliasRegex =
         new Regex(@"(?<tableAlias>FROM +(\[.*\]\.)?(\[.*\]) AS (\[.*\])(?! WITH \(NOLOCK\)))",
             RegexOptions.Multiline |
             RegexOptions.IgnoreCase |
             RegexOptions.Compiled);

        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            if (!command.CommandText.Contains("WITH (NOLOCK)"))
            {
                command.CommandText = _tableAliasRegex.Replace(command.CommandText, "${tableAlias} WITH (NOLOCK)");
            }

            return base.ReaderExecuting(command, eventData, result);
        }
    }
}