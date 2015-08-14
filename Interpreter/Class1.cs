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

        public Interpreter(Uri repo, string token, string commitHash = null)
        {
            var wc = new WebClient();
            // Fetch csProj file
            var csProj = new Uri(repo, $"{commitHash}/DesignValues/DesignValues.csproj?token={token}");
            var projContents = wc.DownloadString(csProj);
            var includes = Regex.Matches(projContents, "<Compile Include=\"((?!Properties).*\\.cs)\"/>");
            var files = new List<string>();
            foreach (var match in includes.OfType<Match>())
            {
                var fileUri = new Uri(repo, $"{commitHash}/DesignValues/{match.Captures[0]}?token={token}");
                files.Add(wc.DownloadString(fileUri));
            }
            var trees = files.Select(x => SyntaxFactory.ParseSyntaxTree(x));
            var compilation = CSharpCompilation.Create(
                assemblyName: "a.dll",
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
            //        var type = compiledAssembly.GetTypes().SingleOrDefault(x => typeof (T).IsAssignableFrom(x));
            //        if (type == null) throw new NullReferenceException();
            //        return (T) Activator.CreateInstance(type, contexts);
            //    }
            //    finally
            //    {
            //        File.Delete(temp);
            //    }

            //}
        }
    }
}
