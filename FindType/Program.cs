using System;
using System.Linq;
using System.Reflection;

class Program
{
    static void Main()
    {
        var path = @"C:\Users\adria\.nuget\packages\microsoft.openapi\2.7.5\lib\netstandard2.0\Microsoft.OpenApi.dll";
        var asm = Assembly.LoadFrom(path);
        var t = asm.GetTypes().FirstOrDefault(x => x.Name == "OpenApiSecuritySchemeReference");
        var ctors = t.GetConstructors();
        foreach(var c in ctors) {
            Console.WriteLine(c.ToString());
        }
    }
}
