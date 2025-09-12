using System;
namespace Flashback.Core.Net.SmokeTests
{
    public class SmokeRunner
    {
        public static int Main(string[] args)
        {
            string dbPath = args.Length > 0 ? args[0] : System.IO.Path.Combine(System.IO.Path.GetTempPath(), "flashback-smoke.db");
            return RepositorySmokeTest.Run(dbPath);
        }
    }
}
