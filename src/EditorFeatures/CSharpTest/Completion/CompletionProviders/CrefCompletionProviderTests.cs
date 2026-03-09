// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Completion.Providers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.Shared.Extensions;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Completion.CompletionProviders
{
    public class CrefCompletionProviderTests : AbstractCSharpCompletionProviderTests
    {
        public CrefCompletionProviderTests(CSharpTestWorkspaceFixture workspaceFixture) : base(workspaceFixture)
        {
        }

        internal override CompletionProvider CreateCompletionProvider()
        {
            return new CrefCompletionProvider();
        }

        protected override async Task VerifyWorkerAsync(
            string code, int position,
            string expectedItemOrNull, string expectedDescriptionOrNull,
            SourceCodeKind sourceCodeKind, bool usePreviousCharAsTrigger, bool checkForAbsence,
            int? glyph, int? matchPriority, bool? hasSuggestionItem)
        {
            await VerifyAtPositionAsync(code, position, usePreviousCharAsTrigger, expectedItemOrNull, expectedDescriptionOrNull, sourceCodeKind, checkForAbsence, glyph, matchPriority, hasSuggestionItem);
            await VerifyAtEndOfFileAsync(code, position, usePreviousCharAsTrigger, expectedItemOrNull, expectedDescriptionOrNull, sourceCodeKind, checkForAbsence, glyph, matchPriority, hasSuggestionItem);

            // Items cannot be partially written if we're checking for their absence,
            // or if we're verifying that the list will show up (without specifying an actual item)
            if (!checkForAbsence && expectedItemOrNull != null)
            {
                await VerifyAtPosition_ItemPartiallyWrittenAsync(code, position, usePreviousCharAsTrigger, expectedItemOrNull, expectedDescriptionOrNull, sourceCodeKind, checkForAbsence, glyph, matchPriority, hasSuggestionItem);
                await VerifyAtEndOfFile_ItemPartiallyWrittenAsync(code, position, usePreviousCharAsTrigger, expectedItemOrNull, expectedDescriptionOrNull, sourceCodeKind, checkForAbsence, glyph, matchPriority, hasSuggestionItem);
            }
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NameCref()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
namespace Goo
{
    /// <see cref=""$$""/> 
    class Program
    {
    }
}";
            await VerifyItemExistsAsync(text, "AccessViolationException");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task QualifiedCref()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
namespace Goo
{

    class Program
    {
        /// <see cref=""Program.$$""/> 
        void goo() { }
    }
}";
            await VerifyItemExistsAsync(text, "goo");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CrefArgumentList()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
namespace Goo
{

    class Program
    {
        /// <see cref=""Program.goo($$""/> 
        void goo(int i) { }
    }
}";
            await VerifyItemIsAbsentAsync(text, "goo(int)");
            await VerifyItemExistsAsync(text, "int");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CrefTypeParameterInArgumentList()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
namespace Goo
{

    class Program<T>
    {
        /// <see cref=""Program{Q}.goo($$""/> 
        void goo(T i) { }
    }
}";
            await VerifyItemExistsAsync(text, "Q");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion), WorkItem(530887, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530887")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PrivateMember()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
namespace Goo
{
    /// <see cref=""C.$$""/> 
    class Program<T>
    {
    }

    class C
    {
        private int Private;
        public int Public;
    }
}";
            await VerifyItemExistsAsync(text, "Private");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterSingleQuote()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
namespace Goo
{
    /// <see cref='$$'/> 
    class Program
    {
    }
}";
            await VerifyItemExistsAsync(text, "Exception");
        }

        [WorkItem(531315, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/531315")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EscapePredefinedTypeName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
/// <see cref=""@vo$$""/>
class @void { }
";
            await VerifyItemExistsAsync(text, "@void");
        }

        [WorkItem(531345, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/531345")]
        [WorkItem(598159, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/598159")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ShowParameterNames()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"/// <see cref=""C.$$""/>
class C
{
    void M(int x) { }
    void M(ref long x) { }
    void M<T>(T x) { }
}

";
            await VerifyItemExistsAsync(text, "M(int)");
            await VerifyItemExistsAsync(text, "M(ref long)");
            await VerifyItemExistsAsync(text, "M{T}(T)");
        }

        [WorkItem(531345, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/531345")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ShowTypeParameterNames()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"/// <see cref=""C$$""/>
class C<TGoo>
{
    void M(int x) { }
    void M(long x) { }
    void M(string x) { }
}

";
            await VerifyItemExistsAsync(text, "C{TGoo}");
        }

        [WorkItem(531156, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/531156")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ShowConstructors()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;

/// <see cref=""C.$$""/>
class C<T>
{
    public C(int x) { }

    public C() { }

    public C(T x) { }
}

";
            await VerifyItemExistsAsync(text, "C");
            await VerifyItemExistsAsync(text, "C(T)");
            await VerifyItemExistsAsync(text, "C(int)");
        }

        [WorkItem(598679, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/598679")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoParamsModifier()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"/// <summary>
/// <see cref=""C.$$""/>
/// </summary>
class C
        {
            void M(int x) { }
            void M(params long[] x) { }
        }


";
            await VerifyItemExistsAsync(text, "M(long[])");
        }

        [WorkItem(607773, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/607773")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task UnqualifiedTypes()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
using System.Collections.Generic;
/// <see cref=""List{T}.$$""/>
class C { }
";
            await VerifyItemExistsAsync(text, "Enumerator");
        }

        [WorkItem(607773, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/607773")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CommitUnqualifiedTypes()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
using System.Collections.Generic;
/// <see cref=""List{T}.$$""/>
class C { }
";

            var expected = @"
using System.Collections.Generic;
/// <see cref=""List{T}.Enumerator ""/>
class C { }
";
            await VerifyProviderCommitAsync(text, "Enumerator", expected, ' ', "Enum");
        }

        [WorkItem(642285, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/642285")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestOperators()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    /// <see cref=""$$""/>
    public static Test operator !(Test t)
    {
        return new Test();
    }
    public static int operator +(Test t1, Test t2) // Invoke FAR here on operator
    {
        return 1;
    }
    public static bool operator true(Test t)
    {
        return true;
    }
    public static bool operator false(Test t)
    {
        return false;
    }
}
";
            await VerifyItemExistsAsync(text, "operator !(Test)");
            await VerifyItemExistsAsync(text, "operator +(Test, Test)");
            await VerifyItemExistsAsync(text, "operator true(Test)");
            await VerifyItemExistsAsync(text, "operator false(Test)");
        }

        [WorkItem(641096, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/641096")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestIndexers()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
/// <see cref=""thi$$""/>
class Program
{
    int[] arr;

    public int this[int i]
    {
        get { return arr[i]; }
    }
}
";
            await VerifyItemExistsAsync(text, "this[int]");
        }

        [WorkItem(531315, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/531315")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CommitEscapedPredefinedTypeName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
/// <see cref=""@vo$$""/>
class @void { }
";

            var expected = @"using System;
/// <see cref=""@void ""/>
class @void { }
";
            await VerifyProviderCommitAsync(text, "@void", expected, ' ', "@vo");
        }

        [WorkItem(598159, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/598159")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task RefOutModifiers()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"/// <summary>
/// <see cref=""C.$$""/>
/// </summary>
class C
{
    void M(ref int x) { }
    void M(out long x) { }
}

";
            await VerifyItemExistsAsync(text, "M(ref int)");
            await VerifyItemExistsAsync(text, "M(out long)");
        }

        [WorkItem(673587, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/673587")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NestedNamespaces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"namespace N
{
    class C
    {
        void sub() { }
    }
    namespace N
    {
        class C
        { }
    }
}
class Program
{
    /// <summary>
    /// <see cref=""N.$$""/> // type N. here
    /// </summary>
    static void Main(string[] args)
    {

    }
}";
            await VerifyItemExistsAsync(text, "N");
            await VerifyItemExistsAsync(text, "C");
        }

        [WorkItem(730338, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/730338")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PermitTypingTypeParameters()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
using System.Collections.Generic;
/// <see cref=""List$$""/>
class C { }
";

            var expected = @"
using System.Collections.Generic;
/// <see cref=""List{""/>
class C { }
";
            await VerifyProviderCommitAsync(text, "List{T}", expected, '{', "List");
        }

        [WorkItem(730338, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/730338")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PermitTypingParameterTypes()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
using System.Collections.Generic;
/// <see cref=""goo$$""/>
class C 
{ 
    public void goo(int x) { }
}
";

            var expected = @"
using System.Collections.Generic;
/// <see cref=""goo(""/>
class C 
{ 
    public void goo(int x) { }
}
";
            await VerifyProviderCommitAsync(text, "goo(int)", expected, '(', "goo");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CrefCompletionSpeculatesOutsideTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
/// <see cref=""$$
class C
{
}";
            using (var workspace = TestWorkspace.Create(LanguageNames.CSharp, new CSharpCompilationOptions(OutputKind.ConsoleApplication), new CSharpParseOptions(), new[] { text }))
            {
                var called = false;
                var provider = new CrefCompletionProvider(testSpeculativeNodeCallbackOpt: n =>
                {
                    // asserts that we aren't be asked speculate on nodes inside documentation trivia.
                    // This verifies that the provider is asking for a speculative SemanticModel
                    // by walking to the node the documentation is attached to. 

                    called = true;
                    var parent = n.GetAncestor<DocumentationCommentTriviaSyntax>();
                    Assert.Null(parent);
                });

                var hostDocument = workspace.DocumentWithCursor;
                var document = workspace.CurrentSolution.GetDocument(hostDocument.Id);
                var service = CreateCompletionService(workspace,
                    ImmutableArray.Create<CompletionProvider>(provider));
                var completionList = await GetCompletionListAsync(service, document, hostDocument.CursorPosition.Value, CompletionTrigger.Invoke);

                Assert.True(called);
            }
        }

        [WorkItem(16060, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/16060")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SpecialTypeNames()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
using System;
/// <see cref=""$$""/>
class C 
{ 
    public void goo(int x) { }
}
";

            await VerifyItemExistsAsync(text, "uint");
            await VerifyItemExistsAsync(text, "UInt32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoSuggestionAfterEmptyCref()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
using System;
/// <see cref="""" $$
class C 
{ 
    public void goo(int x) { }
}
";

            await VerifyNoItemsExistAsync(text);
        }
        
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
        [WorkItem(23957, "https://github.com/dotnet/roslyn/issues/23957")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CRef_InParameter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
using System;
class C 
{ 
    /// <see cref=""C.My$$
    public void MyMethod(in int x) { }
}
";

            await VerifyItemExistsAsync(text, "MyMethod(in int)");
        }
    }
}
