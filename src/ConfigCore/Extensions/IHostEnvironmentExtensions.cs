
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigCore.Extensions
{
    public static class IHostEnvironmentExtensions
    {
        public const string QAEnvironment = "QA";
        public const string TestEnvironment = "Test";
        public const string LocalEnvironment = "Local";

        public static bool IsQA(this IHostEnvironment hostingEnvironment)
        {
            return hostingEnvironment.IsEnvironment(QAEnvironment);
        }
        public static bool IsTest(this IHostEnvironment hostingEnvironment)
        {
            return hostingEnvironment.IsEnvironment(TestEnvironment);
        }
        public static bool IsLocal(this IHostEnvironment hostingEnvironment)
        {
            return hostingEnvironment.IsEnvironment(LocalEnvironment);
        }
    }
}
