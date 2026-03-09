// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editor.CSharp.LineSeparator;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.LineSeparators
{
    [UseExportProvider]
    public class LineSeparatorTests
    {
        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEmptyFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await AssertTagsOnBracesOrSemicolonsAsync(contents: string.Empty);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEmptyClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"class C
{
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestClassWithOneMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"class C
{
    void M()
    {
    }
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestClassWithTwoMethods()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"class C
{
    void M()
    {
    }

    void N()
    {
    }
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 0, 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestClassWithTwoNonEmptyMethods()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"class C
{
    void M()
    {
        N();
    }

    void N()
    {
        M();
    }
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 1, 4);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestClassWithMethodAndField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"class C
{
    void M()
    {
    }

    int field;
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 0, 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEmptyNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"namespace N
{
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNamespaceAndClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"namespace N
{
    class C
    {
    }
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNamespaceAndTwoClasses()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"namespace N
{
    class C
    {
    }

    class D
    {
    }
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 0, 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNamespaceAndTwoClassesAndDelegate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"namespace N
{
    class C
    {
    }

    class D
    {
    }

    delegate void Del();
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 0, 1, 3);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"class C
{
    class N
    {
    }
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTwoNestedClasses()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"class C
{
    class N
    {
    }

    class N2
    {
    }
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 0, 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestStruct()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"struct S
{
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterface()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"interface I
{
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEnum()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"enum E
{
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"class C
{
    int Prop
    {
        get
        {
            return 0;
        }
        set
        {
        }
    }
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 4);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPropertyAndField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"class C
{
    int Prop
    {
        get
        {
            return 0;
        }
        set
        {
        }
    }

    int field;
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 3, 5);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestClassWithFieldAndMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"class C
{
    int field;

    void M()
    {
    }
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 0, 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task UsingDirective()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"using System;

class C
{
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 0, 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task UsingDirectiveInNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"namespace N
{
    using System;

    class C
    {
    }
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 0, 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PropertyStyleEventDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"class C
{
    event EventHandler E
    {
        add { }
        remove { }
    }

    int i;
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 2, 4);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IndexerDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"class C
{
    int this[int i]
    {
        get { return i; }
        set { }
    }

    int i;
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 3, 5);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Constructor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"class C
{
    C()
    {
    }

    int i;
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 0, 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Destructor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"class C
{
    ~C()
    {
    }

    int i;
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 0, 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Operator()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"class C
{
    static C operator +(C lhs, C rhs)
    {
    }

    int i;
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 0, 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ConversionOperator()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"class C
{
    static implicit operator C(int i)
    {
    }

    int i;
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 0, 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Bug930292()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"class Program
{
void A() { }
void B() { }
void C() { }
void D() { }
}
";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 4);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Bug930289()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"namespace Roslyn.Compilers.CSharp
{
internal struct ArrayElement<T>
{
internal T Value;
internal ArrayElement(T value) { this.Value = value; }
public static implicit operator ArrayElement<T>(T value) { return new ArrayElement<T>(value); }
}
}
";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 6);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConsoleApp()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file = @"using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
    }
}";
            await AssertTagsOnBracesOrSemicolonsAsync(file, 2, 4);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
        [WorkItem(1297, "https://github.com/dotnet/roslyn/issues/1297")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExpressionBodiedProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await AssertTagsOnBracesOrSemicolonsAsync(@"class C
{
    int Prop => 3;

    void M()
    {
    }
}", 0, 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
        [WorkItem(1297, "https://github.com/dotnet/roslyn/issues/1297")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExpressionBodiedIndexer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await AssertTagsOnBracesOrSemicolonsAsync(@"class C
{
    int this[int i] => 3;

    void M()
    {
    }
}", 0, 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
        [WorkItem(1297, "https://github.com/dotnet/roslyn/issues/1297")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExpressionBodiedEvent()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            // This is not valid code, and parses all wrong, but just in case a user writes it.  Note
            // the 3 is because there is a skipped } in the event declaration.
            await AssertTagsOnBracesOrSemicolonsAsync(@"class C
{
    event EventHandler MyEvent => 3;

    void M()
    {
    }
}", 3);
        }

        #region Negative (incomplete) tests

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IncompleteClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await AssertTagsOnBracesOrSemicolonsAsync(@"class C");
            await AssertTagsOnBracesOrSemicolonsAsync(@"class C {");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IncompleteEnum()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await AssertTagsOnBracesOrSemicolonsAsync(@"enum E");
            await AssertTagsOnBracesOrSemicolonsAsync(@"enum E {");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IncompleteMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await AssertTagsOnBracesOrSemicolonsAsync(@"void goo() {");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IncompleteProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await AssertTagsOnBracesOrSemicolonsAsync(@"class C { int P { get; set; void");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IncompleteEvent()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await AssertTagsOnBracesOrSemicolonsAsync(@"public event EventHandler");
            await AssertTagsOnBracesOrSemicolonsAsync(@"public event EventHandler {");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IncompleteIndexer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await AssertTagsOnBracesOrSemicolonsAsync(@"int this[int i]");
            await AssertTagsOnBracesOrSemicolonsAsync(@"int this[int i] {");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IncompleteOperator()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            // top level operators not supported in script code
            await AssertTagsOnBracesOrSemicolonsTokensAsync(@"C operator +(C lhs, C rhs) {", Array.Empty<int>(), Options.Regular);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IncompleteConversionOperator()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await AssertTagsOnBracesOrSemicolonsAsync(@"implicit operator C(int i) {");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.LineSeparators)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IncompleteMember()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await AssertTagsOnBracesOrSemicolonsAsync(@"class C { private !C(");
        }

        #endregion

        private async Task AssertTagsOnBracesOrSemicolonsAsync(string contents, params int[] tokenIndices)
        {
            await AssertTagsOnBracesOrSemicolonsTokensAsync(contents, tokenIndices);
            await AssertTagsOnBracesOrSemicolonsTokensAsync(contents, tokenIndices, Options.Script);
        }

        private async Task AssertTagsOnBracesOrSemicolonsTokensAsync(string contents, int[] tokenIndices, CSharpParseOptions options = null)
        {
            using (var workspace = TestWorkspace.CreateCSharp(contents, options))
            {
                var document = workspace.CurrentSolution.GetDocument(workspace.Documents.First().Id);
                var spans = await new CSharpLineSeparatorService().GetLineSeparatorsAsync(document, (await document.GetSyntaxRootAsync()).FullSpan, CancellationToken.None);
                var tokens = (await document.GetSyntaxRootAsync(CancellationToken.None)).DescendantTokens().Where(t => t.Kind() == SyntaxKind.CloseBraceToken || t.Kind() == SyntaxKind.SemicolonToken);

                Assert.Equal(tokenIndices.Length, spans.Count());

                int i = 0;
                foreach (var span in spans.OrderBy(t => t.Start))
                {
                    var expectedToken = tokens.ElementAt(tokenIndices[i]);

                    var expectedSpan = expectedToken.Span;

                    var message = string.Format("Expected to match curly {0} at span {1}.  Actual span {2}",
                                                tokenIndices[i],
                                                expectedSpan,
                                                span);
                    Assert.True(expectedSpan == span, message);
                    ++i;
                }
            }
        }

        private static SyntaxToken GetOpenBrace(SyntaxTree syntaxTree, SyntaxToken token)
        {
            return token.Parent.ChildTokens().Where(n => n.Kind() == SyntaxKind.OpenBraceToken).Single();
        }
    }
}
