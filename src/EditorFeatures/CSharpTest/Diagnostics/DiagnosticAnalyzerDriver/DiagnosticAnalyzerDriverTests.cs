// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Test.Utilities;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.Test.Utilities;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.UnitTests.Diagnostics;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics.UserDiagnosticProviderEngine
{
    [UseExportProvider]
    public class DiagnosticAnalyzerDriverTests
    {
        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DiagnosticAnalyzerDriverAllInOne()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var source = TestResource.AllInOneCSharpCode;

            // AllInOneCSharpCode has no properties with initializers or named types with primary constructors.
            var symbolKindsWithNoCodeBlocks = new HashSet<SymbolKind>();
            symbolKindsWithNoCodeBlocks.Add(SymbolKind.Property);
            symbolKindsWithNoCodeBlocks.Add(SymbolKind.NamedType);


            var analyzer = new CSharpTrackingDiagnosticAnalyzer();
            using (var workspace = TestWorkspace.CreateCSharp(source, TestOptions.Regular))
            {
                var document = workspace.CurrentSolution.Projects.Single().Documents.Single();
                AccessSupportedDiagnostics(analyzer);
#pragma warning disable VSTHRD103 // Call async methods when in an async method
                await DiagnosticProviderTestUtilities.GetAllDiagnosticsAsync(analyzer, document, new Text.TextSpan(0, document.GetTextAsync().Result.Length));
#pragma warning restore VSTHRD103 // Call async methods when in an async method
                analyzer.VerifyAllAnalyzerMembersWereCalled();
                analyzer.VerifyAnalyzeSymbolCalledForAllSymbolKinds();
                analyzer.VerifyAnalyzeNodeCalledForAllSyntaxKinds(new HashSet<SyntaxKind>());
                analyzer.VerifyOnCodeBlockCalledForAllSymbolAndMethodKinds(symbolKindsWithNoCodeBlocks, true);
            }
        }

        [Fact, WorkItem(908658, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/908658")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DiagnosticAnalyzerDriverVsAnalyzerDriverOnCodeBlock()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var methodNames = new string[] { "Initialize", "AnalyzeCodeBlock" };
            var source = @"
[System.Obsolete]
class C
{
    int P { get; set; }
    delegate void A();
    delegate string F();
}
";

            var ideEngineAnalyzer = new CSharpTrackingDiagnosticAnalyzer();
            using (var ideEngineWorkspace = TestWorkspace.CreateCSharp(source))
            {
                var ideEngineDocument = ideEngineWorkspace.CurrentSolution.Projects.Single().Documents.Single();
#pragma warning disable VSTHRD103 // Call async methods when in an async method
                await DiagnosticProviderTestUtilities.GetAllDiagnosticsAsync(ideEngineAnalyzer, ideEngineDocument, new Text.TextSpan(0, ideEngineDocument.GetTextAsync().Result.Length));
#pragma warning restore VSTHRD103 // Call async methods when in an async method
                foreach (var method in methodNames)
                {
                    Assert.False(ideEngineAnalyzer.CallLog.Any(e => e.CallerName == method && e.MethodKind == MethodKind.DelegateInvoke && e.ReturnsVoid));
                    Assert.False(ideEngineAnalyzer.CallLog.Any(e => e.CallerName == method && e.MethodKind == MethodKind.DelegateInvoke && !e.ReturnsVoid));
                    Assert.True(ideEngineAnalyzer.CallLog.Any(e => e.CallerName == method && e.SymbolKind == SymbolKind.NamedType));
                    Assert.False(ideEngineAnalyzer.CallLog.Any(e => e.CallerName == method && e.SymbolKind == SymbolKind.Property));
                }
            }

            var compilerEngineAnalyzer = new CSharpTrackingDiagnosticAnalyzer();
            using (var compilerEngineWorkspace = TestWorkspace.CreateCSharp(source))
            {
#pragma warning disable VSTHRD103 // Call async methods when in an async method
                var compilerEngineCompilation = (CSharpCompilation)compilerEngineWorkspace.CurrentSolution.Projects.Single().GetCompilationAsync().Result;
#pragma warning restore VSTHRD103 // Call async methods when in an async method
                compilerEngineCompilation.GetAnalyzerDiagnostics(new[] { compilerEngineAnalyzer });
                foreach (var method in methodNames)
                {
                    Assert.False(compilerEngineAnalyzer.CallLog.Any(e => e.CallerName == method && e.MethodKind == MethodKind.DelegateInvoke && e.ReturnsVoid));
                    Assert.False(compilerEngineAnalyzer.CallLog.Any(e => e.CallerName == method && e.MethodKind == MethodKind.DelegateInvoke && !e.ReturnsVoid));
                    Assert.True(compilerEngineAnalyzer.CallLog.Any(e => e.CallerName == method && e.SymbolKind == SymbolKind.NamedType));
                    Assert.False(compilerEngineAnalyzer.CallLog.Any(e => e.CallerName == method && e.SymbolKind == SymbolKind.Property));
                }
            }
        }

        [Fact]
        [WorkItem(759, "https://github.com/dotnet/roslyn/issues/759")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DiagnosticAnalyzerDriverIsSafeAgainstAnalyzerExceptions()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var source = TestResource.AllInOneCSharpCode;
            using (var workspace = TestWorkspace.CreateCSharp(source, TestOptions.Regular))
            {
                var document = workspace.CurrentSolution.Projects.Single().Documents.Single();
                await ThrowingDiagnosticAnalyzer<SyntaxKind>.VerifyAnalyzerEngineIsSafeAgainstExceptionsAsync(async analyzer =>
#pragma warning disable VSTHRD103 // Call async methods when in an async method
                    await DiagnosticProviderTestUtilities.GetAllDiagnosticsAsync(analyzer, document, new Text.TextSpan(0, document.GetTextAsync().Result.Length), logAnalyzerExceptionAsDiagnostics: true));
#pragma warning restore VSTHRD103 // Call async methods when in an async method
            }
        }

        [WorkItem(908621, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/908621")]
        [Fact]
        public void DiagnosticServiceIsSafeAgainstAnalyzerExceptions_1()
        {
            var analyzer = new ThrowingDiagnosticAnalyzer<SyntaxKind>();
            analyzer.ThrowOn(typeof(DiagnosticAnalyzer).GetProperties().Single().Name);
            AccessSupportedDiagnostics(analyzer);
        }

        [WorkItem(908621, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/908621")]
        [Fact]
        public void DiagnosticServiceIsSafeAgainstAnalyzerExceptions_2()
        {
            var analyzer = new ThrowingDoNotCatchDiagnosticAnalyzer<SyntaxKind>();
            analyzer.ThrowOn(typeof(DiagnosticAnalyzer).GetProperties().Single().Name);
            var exceptions = new List<Exception>();
            try
            {
                AccessSupportedDiagnostics(analyzer);
            }
            catch (Exception e)
            {
                exceptions.Add(e);
            }

            Assert.True(exceptions.Count == 0);
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AnalyzerOptionsArePassedToAllAnalyzers()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            using (var workspace = TestWorkspace.CreateCSharp(TestResource.AllInOneCSharpCode, TestOptions.Regular))
            {
                var currentProject = workspace.CurrentSolution.Projects.Single();

                var additionalDocId = DocumentId.CreateNewId(currentProject.Id);
                var newSln = workspace.CurrentSolution.AddAdditionalDocument(additionalDocId, "add.config", SourceText.From("random text"));
                currentProject = newSln.Projects.Single();
                var additionalDocument = currentProject.GetAdditionalDocument(additionalDocId);
                AdditionalText additionalStream = new AdditionalTextDocument(additionalDocument.State);
                AnalyzerOptions options = new AnalyzerOptions(ImmutableArray.Create(additionalStream));
                var analyzer = new OptionsDiagnosticAnalyzer<SyntaxKind>(expectedOptions: options);

                var sourceDocument = currentProject.Documents.Single();
#pragma warning disable VSTHRD103 // Call async methods when in an async method
                await DiagnosticProviderTestUtilities.GetAllDiagnosticsAsync(analyzer, sourceDocument, new Text.TextSpan(0, sourceDocument.GetTextAsync().Result.Length));
#pragma warning restore VSTHRD103 // Call async methods when in an async method
                analyzer.VerifyAnalyzerOptions();
            }
        }

        private void AccessSupportedDiagnostics(DiagnosticAnalyzer analyzer)
        {
            var diagnosticService = new TestDiagnosticAnalyzerService(LanguageNames.CSharp, analyzer);
            diagnosticService.GetDiagnosticDescriptors(projectOpt: null);
        }

        private class ThrowingDoNotCatchDiagnosticAnalyzer<TLanguageKindEnum> : ThrowingDiagnosticAnalyzer<TLanguageKindEnum>, IBuiltInAnalyzer where TLanguageKindEnum : struct
        {
            public bool OpenFileOnly(Workspace workspace) => false;

            public DiagnosticAnalyzerCategory GetAnalyzerCategory()
            {
                return DiagnosticAnalyzerCategory.SyntaxAnalysis | DiagnosticAnalyzerCategory.SemanticDocumentAnalysis | DiagnosticAnalyzerCategory.ProjectAnalysis;
            }
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AnalyzerCreatedAtCompilationLevelNeedNotBeCompilationAnalyzer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var source = @"x";

            var analyzer = new CompilationAnalyzerWithSyntaxTreeAnalyzer();
            using (var ideEngineWorkspace = TestWorkspace.CreateCSharp(source))
            {
                var ideEngineDocument = ideEngineWorkspace.CurrentSolution.Projects.Single().Documents.Single();
#pragma warning disable VSTHRD103 // Call async methods when in an async method
                var diagnostics = await DiagnosticProviderTestUtilities.GetAllDiagnosticsAsync(analyzer, ideEngineDocument, new Text.TextSpan(0, ideEngineDocument.GetTextAsync().Result.Length));
#pragma warning restore VSTHRD103 // Call async methods when in an async method

                var diagnosticsFromAnalyzer = diagnostics.Where(d => d.Id == "SyntaxDiagnostic");

                Assert.Equal(1, diagnosticsFromAnalyzer.Count());
            }
        }

        private class CompilationAnalyzerWithSyntaxTreeAnalyzer : DiagnosticAnalyzer
        {
            private const string ID = "SyntaxDiagnostic";

            private static readonly DiagnosticDescriptor s_syntaxDiagnosticDescriptor =
                new DiagnosticDescriptor(ID, title: "Syntax", messageFormat: "Syntax", category: "Test", defaultSeverity: DiagnosticSeverity.Warning, isEnabledByDefault: true);

            public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            {
                get
                {
                    return ImmutableArray.Create(s_syntaxDiagnosticDescriptor);
                }
            }

            public override void Initialize(AnalysisContext context)
            {
                context.RegisterCompilationStartAction(CreateAnalyzerWithinCompilation);
            }

            public void CreateAnalyzerWithinCompilation(CompilationStartAnalysisContext context)
            {
                context.RegisterSyntaxTreeAction(new SyntaxTreeAnalyzer().AnalyzeSyntaxTree);
            }

            private class SyntaxTreeAnalyzer
            {
                public void AnalyzeSyntaxTree(SyntaxTreeAnalysisContext context)
                {
                    context.ReportDiagnostic(Diagnostic.Create(s_syntaxDiagnosticDescriptor, context.Tree.GetRoot().GetFirstToken().GetLocation()));
                }
            }
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CodeBlockAnalyzersOnlyAnalyzeExecutableCode()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var source = @"
using System;
class C
{
    void F(int x = 0)
    {
        Console.WriteLine(0);
    }
}
";

            var analyzer = new CodeBlockAnalyzerFactory();
            using (var ideEngineWorkspace = TestWorkspace.CreateCSharp(source))
            {
                var ideEngineDocument = ideEngineWorkspace.CurrentSolution.Projects.Single().Documents.Single();
#pragma warning disable VSTHRD103 // Call async methods when in an async method
                var diagnostics = await DiagnosticProviderTestUtilities.GetAllDiagnosticsAsync(analyzer, ideEngineDocument, new Text.TextSpan(0, ideEngineDocument.GetTextAsync().Result.Length));
#pragma warning restore VSTHRD103 // Call async methods when in an async method
                var diagnosticsFromAnalyzer = diagnostics.Where(d => d.Id == CodeBlockAnalyzerFactory.Descriptor.Id);
                Assert.Equal(2, diagnosticsFromAnalyzer.Count());
            }

            source = @"
using System;
class C
{
    void F(int x = 0, int y = 1, int z = 2)
    {
        Console.WriteLine(0);
    }
}
";

            using (var compilerEngineWorkspace = TestWorkspace.CreateCSharp(source))
            {
#pragma warning disable VSTHRD103 // Call async methods when in an async method
                var compilerEngineCompilation = (CSharpCompilation)compilerEngineWorkspace.CurrentSolution.Projects.Single().GetCompilationAsync().Result;
#pragma warning restore VSTHRD103 // Call async methods when in an async method
                var diagnostics = compilerEngineCompilation.GetAnalyzerDiagnostics(new[] { analyzer });
                var diagnosticsFromAnalyzer = diagnostics.Where(d => d.Id == CodeBlockAnalyzerFactory.Descriptor.Id);
                Assert.Equal(4, diagnosticsFromAnalyzer.Count());
            }
        }

        private class CodeBlockAnalyzerFactory : DiagnosticAnalyzer
        {
            public static DiagnosticDescriptor Descriptor = DescriptorFactory.CreateSimpleDescriptor("DummyDiagnostic");

            public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            {
                get
                {
                    return ImmutableArray.Create(Descriptor);
                }
            }

            public override void Initialize(AnalysisContext context)
            {
                context.RegisterCodeBlockStartAction<SyntaxKind>(CreateAnalyzerWithinCodeBlock);
            }

            public void CreateAnalyzerWithinCodeBlock(CodeBlockStartAnalysisContext<SyntaxKind> context)
            {
                var blockAnalyzer = new CodeBlockAnalyzer();
                context.RegisterCodeBlockEndAction(blockAnalyzer.AnalyzeCodeBlock);
                context.RegisterSyntaxNodeAction(blockAnalyzer.AnalyzeNode, blockAnalyzer.SyntaxKindsOfInterest.ToArray());
            }

            private class CodeBlockAnalyzer
            {
                public ImmutableArray<SyntaxKind> SyntaxKindsOfInterest
                {
                    get
                    {
                        return ImmutableArray.Create(SyntaxKind.MethodDeclaration, SyntaxKind.ExpressionStatement, SyntaxKind.EqualsValueClause);
                    }
                }

                public void AnalyzeCodeBlock(CodeBlockAnalysisContext context)
                {
                }

                public void AnalyzeNode(SyntaxNodeAnalysisContext context)
                {
                    // Ensure only executable nodes are analyzed.
                    Assert.NotEqual(SyntaxKind.MethodDeclaration, context.Node.Kind());
                    context.ReportDiagnostic(Diagnostic.Create(Descriptor, context.Node.GetLocation()));
                }
            }
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDiagnosticSpan()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var source = @"// empty code";

            var analyzer = new InvalidSpanAnalyzer();
            using (var compilerEngineWorkspace = TestWorkspace.CreateCSharp(source))
            {
                var compilerEngineCompilation = (CSharpCompilation)(await compilerEngineWorkspace.CurrentSolution.Projects.Single().GetCompilationAsync());

                var diagnostics = compilerEngineCompilation.GetAnalyzerDiagnostics(new[] { analyzer });
                AssertEx.Any(diagnostics, d => d.Id == AnalyzerHelper.AnalyzerExceptionDiagnosticId);
            }
        }

        private class InvalidSpanAnalyzer : DiagnosticAnalyzer
        {
            public static DiagnosticDescriptor Descriptor = DescriptorFactory.CreateSimpleDescriptor("DummyDiagnostic");

            public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
                => ImmutableArray.Create(Descriptor);

            public override void Initialize(AnalysisContext context)
                => context.RegisterSyntaxTreeAction(Analyze);

            private void Analyze(SyntaxTreeAnalysisContext context)
                => context.ReportDiagnostic(Diagnostic.Create(Descriptor, Location.Create(context.Tree, TextSpan.FromBounds(1000, 2000))));
        }
    }
}
