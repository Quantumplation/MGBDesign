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
    public class Interpreter : IRuleProvider
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
        }

        public TConstants Get<TConstants, TContext>(TContext contextA)
            where TConstants : BaseConstants<TContext, TConstants>
            where TContext : BaseContext<TConstants, TContext>
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
            return (TConstants)constants;
        } 
    }
}
