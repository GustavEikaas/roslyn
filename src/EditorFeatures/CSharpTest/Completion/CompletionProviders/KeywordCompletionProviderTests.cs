// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp.Completion.Providers;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Completion.CompletionProviders;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.IntelliSense.CompletionSetSources
{
    public class KeywordCompletionProviderTests : AbstractCSharpCompletionProviderTests
    {
        public KeywordCompletionProviderTests(CSharpTestWorkspaceFixture workspaceFixture) : base(workspaceFixture)
        {
        }

        internal override CompletionProvider CreateCompletionProvider()
        {
            return new KeywordCompletionProvider();
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IsCommitCharacterTest()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyCommonCommitCharactersAsync("$$", textTypedSoFar: "");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
        public void IsTextualTriggerCharacterTest()
        {
            TestCommonIsTextualTriggerCharacter();
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SendEnterThroughToEditorTest()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifySendEnterThroughToEnterAsync("$$", "class", sendThroughEnterOption: EnterKeyRule.Never, expected: false);
            await VerifySendEnterThroughToEnterAsync("$$", "class", sendThroughEnterOption: EnterKeyRule.AfterFullyTypedWord, expected: true);
            await VerifySendEnterThroughToEnterAsync("$$", "class", sendThroughEnterOption: EnterKeyRule.Always, expected: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InEmptyFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = "$$";

            await VerifyAnyItemExistsAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInInactiveCode()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"class C
{
    void M()
    {
#if false
$$
";
            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInCharLiteral()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"class C
{
    void M()
    {
        var c = '$$';
";

            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInUnterminatedCharLiteral()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"class C
{
    void M()
    {
        var c = '$$   ";

            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInUnterminatedCharLiteralAtEndOfFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"class C
{
    void M()
    {
        var c = '$$";

            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInString()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"class C
{
    void M()
    {
        var s = ""$$"";
";

            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInStringInDirective()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = "#r \"$$\"";

            await VerifyNoItemsExistAsync(markup, SourceCodeKind.Script);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInUnterminatedString()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"class C
{
    void M()
    {
        var s = ""$$   ";

            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInUnterminatedStringInDirective()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = "#r \"$$\"";

            await VerifyNoItemsExistAsync(markup, SourceCodeKind.Script);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInUnterminatedStringAtEndOfFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"class C
{
    void M()
    {
        var s = ""$$";

            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInVerbatimString()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"class C
{
    void M()
    {
        var s = @""
$$
"";
";

            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInUnterminatedVerbatimString()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"class C
{
    void M()
    {
        var s = @""
$$
";

            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInUnterminatedVerbatimStringAtEndOfFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"class C
{
    void M()
    {
        var s = @""$$";

            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInSingleLineComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"class C
{
    void M()
    {
        // $$
";

            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInSingleLineCommentAtEndOfFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"namespace A
{
}// $$";

            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInMutliLineComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"class C
{
    void M()
    {
/*
    $$
*/
";

            await VerifyNoItemsExistAsync(markup);
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
            await VerifyItemInLinkedFilesAsync(markup, "public", null);
            await VerifyItemInLinkedFilesAsync(markup, "for", null);
        }

        [WorkItem(7768, "https://github.com/dotnet/roslyn/issues/7768")]
        [WorkItem(8228, "https://github.com/dotnet/roslyn/issues/8228")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FormattingAfterCompletionCommit_AfterGetAccessorInSingleLineIncompleteProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markupBeforeCommit = @"class Program
{
    int P {g$$
    void Main() { }
}";

            var expectedCodeAfterCommit = @"class Program
{
    int P {get;
    void Main() { }
}";
            await VerifyProviderCommitAsync(markupBeforeCommit, "get", expectedCodeAfterCommit, commitChar: ';', textTypedSoFar: "g");
        }

        [WorkItem(7768, "https://github.com/dotnet/roslyn/issues/7768")]
        [WorkItem(8228, "https://github.com/dotnet/roslyn/issues/8228")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FormattingAfterCompletionCommit_AfterBothAccessorsInSingleLineIncompleteProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markupBeforeCommit = @"class Program
{
    int P {get;set$$
    void Main() { }
}";

            var expectedCodeAfterCommit = @"class Program
{
    int P {get;set;
    void Main() { }
}";
            await VerifyProviderCommitAsync(markupBeforeCommit, "set", expectedCodeAfterCommit, commitChar: ';', textTypedSoFar: "set");
        }

        [WorkItem(7768, "https://github.com/dotnet/roslyn/issues/7768")]
        [WorkItem(8228, "https://github.com/dotnet/roslyn/issues/8228")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FormattingAfterCompletionCommit_InSingleLineMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markupBeforeCommit = @"class Program
{
    public static void Test() { return$$
    void Main() { }
}";

            var expectedCodeAfterCommit = @"class Program
{
    public static void Test() { return;
    void Main() { }
}";
            await VerifyProviderCommitAsync(markupBeforeCommit, "return", expectedCodeAfterCommit, commitChar: ';', textTypedSoFar: "return");
        }

        [WorkItem(14218, "https://github.com/dotnet/roslyn/issues/14218")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PredefinedTypeKeywordsShouldBeRecommendedAfterCaseInASwitch()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Program
{
    public static void Test()
    {
        object o = null;
        switch (o)
        {
            case $$
        }
    }
}";

            await VerifyItemExistsAsync(text, "int");
            await VerifyItemExistsAsync(text, "string");
            await VerifyItemExistsAsync(text, "byte");
            await VerifyItemExistsAsync(text, "char");
        }
    
        [WpfFact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PrivateOrProtectedModifiers()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class C
{
$$
}";

            await VerifyItemExistsAsync(text, "private");
            await VerifyItemExistsAsync(text, "protected");
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PrivateProtectedModifier()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class C
{
    private $$
}";

            await VerifyItemExistsAsync(text, "protected");
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ProtectedPrivateModifier()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class C
{
    protected $$
}";

            await VerifyItemExistsAsync(text, "private");
        }
    }
}
