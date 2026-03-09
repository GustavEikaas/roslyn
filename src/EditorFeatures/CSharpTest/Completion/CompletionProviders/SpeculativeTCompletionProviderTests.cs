// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp.Completion.Providers;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Completion.CompletionProviders
{
    public class SpeculativeTCompletionProviderTests : AbstractCSharpCompletionProviderTests
    {
        public SpeculativeTCompletionProviderTests(CSharpTestWorkspaceFixture workspaceFixture) : base(workspaceFixture)
        {
        }

        internal override CompletionProvider CreateCompletionProvider()
        {
            return new SpeculativeTCompletionProvider();
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IsCommitCharacterTest()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string markup = @"
class C
{
    $$
}";

            await VerifyCommonCommitCharactersAsync(markup, textTypedSoFar: "");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
        public void IsTextualTriggerCharacterTest()
        {
            TestCommonIsTextualTriggerCharacter();
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SendEnterThroughToEditorTest()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string markup = @"
class C
{
    $$
}";

            await VerifySendEnterThroughToEnterAsync(markup, "T", sendThroughEnterOption: EnterKeyRule.Never, expected: false);
            await VerifySendEnterThroughToEnterAsync(markup, "T", sendThroughEnterOption: EnterKeyRule.AfterFullyTypedWord, expected: true);
            await VerifySendEnterThroughToEnterAsync(markup, "T", sendThroughEnterOption: EnterKeyRule.Always, expected: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    $$
}";

            await VerifyItemExistsAsync(markup, "T");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InInterface()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
interface I
{
    $$
}";

            await VerifyItemExistsAsync(markup, "T");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InStruct()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
struct S
{
    $$
}";

            await VerifyItemExistsAsync(markup, "T");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
namespace N
{
    $$
}";

            await VerifyItemIsAbsentAsync(markup, "T");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInEnum()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
enum E
{
    $$
}";

            await VerifyItemIsAbsentAsync(markup, "T");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterDelegate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    delegate $$
}";

            await VerifyItemExistsAsync(markup, "T");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotAfterVoid()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    void $$
}";

            await VerifyItemIsAbsentAsync(markup, "T");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotAfterInt()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    int $$
}";

            await VerifyItemIsAbsentAsync(markup, "T");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InGeneric()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System;
class C
{
    Func<$$
}";

            await VerifyItemExistsAsync(markup, "T");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InNestedGeneric1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System;
class C
{
    Func<Func<$$
}";

            await VerifyItemExistsAsync(markup, "T");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InNestedGeneric2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System;
class C
{
    Func<Func<int,$$
}";

            await VerifyItemExistsAsync(markup, "T");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InScript()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"$$";

            await VerifyItemExistsAsync(markup, "T", expectedDescriptionOrNull: null, sourceCodeKind: SourceCodeKind.Script);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotAfterVoidInScript()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"void $$";

            await VerifyItemIsAbsentAsync(markup, "T", expectedDescriptionOrNull: null, sourceCodeKind: SourceCodeKind.Script);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotAfterIntInScript()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"int $$";

            await VerifyItemIsAbsentAsync(markup, "T", expectedDescriptionOrNull: null, sourceCodeKind: SourceCodeKind.Script);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InGenericInScript()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System;
Func<$$
";

            await VerifyItemExistsAsync(markup, "T", expectedDescriptionOrNull: null, sourceCodeKind: SourceCodeKind.Script);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InNestedGenericInScript1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System;
Func<Func<$$
";

            await VerifyItemExistsAsync(markup, "T", expectedDescriptionOrNull: null, sourceCodeKind: SourceCodeKind.Script);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InNestedGenericInScript2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System;
Func<Func<int,$$
";

            await VerifyItemExistsAsync(markup, "T", expectedDescriptionOrNull: null, sourceCodeKind: SourceCodeKind.Script);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    // $$
}";

            await VerifyItemIsAbsentAsync(markup, "T");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInXmlDocComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    /// <summary>
    /// $$
    /// </summary>
    void Goo() { }
}";

            await VerifyItemIsAbsentAsync(markup, "T");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterAsyncTask()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading.Tasks;
class Program
{
    async Task<$$
}";

            await VerifyItemExistsAsync(markup, "T");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
        public async Task NotAfterAsync()
        {
            var markup = @"
using System.Threading.Tasks;
class Program
{
    async $$
}";

            await VerifyItemIsAbsentAsync(markup, "T");
        }

        [WorkItem(968256, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/968256")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task UnionOfItemsFromBothContexts()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"<Workspace>
    <Project Language=""C#"" CommonReferences=""true"" AssemblyName=""Proj1"" PreprocessorSymbols=""GOO"">
        <Document FilePath=""CurrentDocument.cs""><![CDATA[
class C
{
#if GOO
    void goo() {
#endif

$$

#if GOO
    }
#endif
}
]]>
        </Document>
    </Project>
    <Project Language=""C#"" CommonReferences=""true"" AssemblyName=""Proj2"">
        <Document IsLinkFile=""true"" LinkAssemblyName=""Proj1"" LinkFilePath=""CurrentDocument.cs""/>
    </Project>
</Workspace>";
            await VerifyItemInLinkedFilesAsync(markup, "T", null);
        }

        [WorkItem(1020654, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1020654")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterAsyncTaskWithBraceCompletion()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading.Tasks;
class Program
{
    async Task<$$>
}";

            await VerifyItemExistsAsync(markup, "T");
        }

        [WorkItem(13480, "https://github.com/dotnet/roslyn/issues/13480")]
        [Fact]
        [Test.Utilities.CompilerTrait(Test.Utilities.CompilerFeature.LocalFunctions)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task LocalFunctionReturnType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    public void M()
    {
        $$
    }
}";
            await VerifyItemExistsAsync(markup, "T");
        }

        [Fact(Skip = "https://github.com/dotnet/roslyn/issues/14525")]
        [Test.Utilities.CompilerTrait(Test.Utilities.CompilerFeature.LocalFunctions)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task LocalFunctionAfterAyncTask()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    public void M()
    {
        async Task<$$>
    }
}";
            await VerifyItemExistsAsync(markup, "T");
        }

        [Fact(Skip = "https://github.com/dotnet/roslyn/issues/14525")]
        [Test.Utilities.CompilerTrait(Test.Utilities.CompilerFeature.LocalFunctions)]
        public async Task LocalFunctionAfterAsync()
        {
            var markup = @"
class C
{
    public void M()
    {
        async $$
    }
}";
            await VerifyItemIsAbsentAsync(markup, "T");
        }
    }
}
