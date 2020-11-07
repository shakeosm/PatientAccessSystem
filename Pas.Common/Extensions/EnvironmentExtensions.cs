using System;
using Microsoft.Extensions.Hosting;

namespace Pas.Common.Extensions
{
    /// <summary>
    ///     QoL class to ease detection of environments and
    ///     help make code readable and understandable.
    ///     Environments are set using the ASPNETCORE:Environment environment variable
    /// </summary>
    /// <remarks>
    ///     https://blogs.msdn.microsoft.com/mvpawardprogram/2017/04/25/custom-environ-asp-net-core/
    /// </remarks>
    public static class EnvironmentExtensions
    {
        public static bool IsEnvironment(string Environment)
        {
          bool isEnvironment = Environment.Equals(System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), StringComparison.CurrentCultureIgnoreCase);

          return isEnvironment;


        }

        public static bool IsInternal()
        {
            bool isInternal =  IsEnvironment(Environments.Local) || IsEnvironment(Environments.Development) ||
                   IsEnvironment(Environments.Test) || IsEnvironment(Environments.Integration);

            return isInternal;
        }

        public static bool IsExternal()
        {
            return !IsInternal();
        }


        public static bool IsLocal(this IHostEnvironment env)
        {
            return env.IsEnvironment(Environments.Local);
        }


        public static bool IsIntegration(this IHostEnvironment env)
        {
            return env.IsEnvironment(Environments.Integration);
        }


        public static bool IsUat(this IHostEnvironment env)
        {
            return env.IsEnvironment(Environments.Uat);
        }


        public static bool IsTest(this IHostEnvironment env)
        {
            return env.IsEnvironment(Environments.Test);
        }


        public static bool IsDemo(this IHostEnvironment env)
        {
            return env.IsEnvironment(Environments.Demo);
        }

        public static bool IsTraining(this IHostEnvironment env)
        {
            return env.IsEnvironment(Environments.Training);
        }

        /// <summary>
        ///     Whether the environment is an internal (i.e. non production)
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public static bool IsInternal(this IHostEnvironment env)
        {
            return IsTest(env) || env.IsDevelopment() || IsIntegration(env) || IsLocal(env);
        }

        public static bool IsExternal(this IHostEnvironment env)
        {
            return !IsInternal(env);
        }


        internal static class Environments
        {
            public const string Local = "Local";
            public const string Development = "Development";
            public const string Integration = "Integration";
            public const string Uat = "UAT";
            public const string Test = "Test";
            public const string Demo = "Demo";
            public const string Training = "Training";
        }
    }
}