using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AssemblyLab
{
    public class AssemblyLoader
    {
        private string _assemblyPath;

        public AssemblyLoader(string assemblyPath)
        {
            _assemblyPath = assemblyPath;
        }

        private Type[] LoadAssembly()
        {
            if(!File.Exists(_assemblyPath))
            {
                throw new FileNotFoundException($"Assembly cannot be located: {_assemblyPath}");
            }

            Assembly assembly = Assembly.LoadFile(_assemblyPath);

            return assembly.GetTypes();
        }

        public MemberInfo[] GetPublicMembers()
        {
            Type[] assemblyTypes = LoadAssembly();

            if (assemblyTypes == null)
            {
                return null;
            }
            else
            {
                return assemblyTypes
                    .SelectMany(type => type.GetMembers())
                    .OrderBy(type => type.DeclaringType.Namespace)
                    .ThenBy(type => type.DeclaringType.Name)
                    .ThenBy(type => type.Name)
                    .ToArray();
            }
        }

    }
}
