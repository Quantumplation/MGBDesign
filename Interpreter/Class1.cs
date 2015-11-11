using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Interpreter
{
    public class Interpreter
    {

        private Assembly _compiledAssembly;

        public Interpreter(string account, string repo, string project, string commitHash = null)
        {
            var wc = new WebClient();
            var baseUriStr = $"https://cdn.rawgit.com/{account}/{repo}/{commitHash}/{project}/";
            var baseUri = new Uri(baseUriStr);
            // Fetch csProj file
            var csProj = new Uri(baseUri, $"{project}.csproj");
            var projContents = wc.DownloadString(csProj);
            var includes = Regex.Matches(projContents, "<Compile Include=\"((?!Properties).*\\.cs)\".*/>");
            var files = new List<string>();
            foreach (var match in includes.OfType<Match>())
            {
                var fileUri = new Uri(baseUri, $"{match.Groups[1].Captures[0]}");
                files.Add(wc.DownloadString(fileUri));
            }
            var trees = files.Select(x => SyntaxFactory.ParseSyntaxTree(x));
            var compilation = CSharpCompilation.Create(
                assemblyName: $"{Guid.NewGuid().ToString("N")}.dll",
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                syntaxTrees: trees,
                references: new[]
                {
                    MetadataReference.CreateFromFile(typeof (object).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof (Beacon).Assembly.Location)
                });
            using (var stream = new MemoryStream())
            {
                var compileResult = compilation.Emit(stream);
                _compiledAssembly = Assembly.Load(stream.GetBuffer());
            }

            //public T Get<T>(context)
            //{
            //    // Get the file
            //    // TODO: network
            //    var temp = Path.GetTempFileName();

            //    try
            //    {
            //        File.Copy(uri, temp, true);
            //        var file = File.ReadAllText(temp);
            //        var tree = SyntaxFactory.ParseSyntaxTree(file);
            //        var compilation = CSharpCompilation.Create(
            //            assemblyName: "a.dll",
            //            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
            //            syntaxTrees: new[] {tree},
            //            references:
            //                new[]
            //                {
            //                    MetadataReference.CreateFromFile(typeof (object).Assembly.Location),
            //                    MetadataReference.CreateFromFile(typeof (Beacon).Assembly.Location)
            //                });
            //        Assembly compiledAssembly;
            //        using (var stream = new MemoryStream())
            //        {
            //            var compileResult = compilation.Emit(stream);
            //            compiledAssembly = Assembly.Load(stream.GetBuffer());
            //        }
            //    }
            //    finally
            //    {
            //        File.Delete(temp);
            //    }

            //}
        }

        public void Populate<T>(T contextA)
            where T : IContext
        {
            var contextType = contextA.GetType();
            Type baseType = contextType;
            while (baseType != null && (!baseType.IsConstructedGenericType || baseType.GetGenericTypeDefinition() != typeof (BaseContext<,>)))
            {
                baseType = contextType.BaseType;
            }
            var constantsType = baseType.GetGenericArguments()[0];
            var type = _compiledAssembly.GetTypes().SingleOrDefault(x => constantsType.IsAssignableFrom(x));
            if (type == null) throw new NullReferenceException();
            var constants = Activator.CreateInstance(type, contextA);
            contextA.SetConstant(constants);
        } 
    }
}
