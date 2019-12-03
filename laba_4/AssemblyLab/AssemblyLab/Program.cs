using System;
using System.IO;
using System.Reflection;

namespace AssemblyLab
{
    class Program
    {
        private const string AssemblyPath = @"C:\Users\Alexander\Desktop\лабы\5 сем\СПП\labz\laba_4\TestLab\TestLab\bin\Debug\TestLab.exe";

        static int Main(string[] args)
        {
            try
            {
                var assemblyLoader = new AssemblyLoader(AssemblyPath);
                var memberInfos = assemblyLoader.GetPublicMembers();

                PrintAssemblyMembersInfo(memberInfos);
                Console.ReadLine();
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        private static void PrintAssemblyMembersInfo(MemberInfo[] memberInfos)
        {
            if (memberInfos == null)
            {
                Console.WriteLine("No public methods found.");
            }
            else
            {
                foreach (MemberInfo memberInfo in memberInfos)
                {
                    Console.WriteLine($"{memberInfo.DeclaringType.FullName}.{memberInfo.Name}");
                }
            }
        }
    }
}
