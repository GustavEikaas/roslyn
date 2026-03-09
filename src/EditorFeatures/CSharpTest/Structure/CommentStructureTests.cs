// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Structure;
using Microsoft.CodeAnalysis.Editor.UnitTests.Structure;
using Microsoft.CodeAnalysis.Structure;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Roslyn.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Structure
{
    public class CommentTests : AbstractSyntaxStructureProviderTests
    {
        protected override string LanguageName => LanguageNames.CSharp;

        internal override async Task<ImmutableArray<BlockSpan>> GetBlockSpansWorkerAsync(Document document, int position)
        {
            var root = await document.GetSyntaxRootAsync();
            var trivia = root.FindTrivia(position, findInsideTrivia: true);

            var token = trivia.Token;

            if (token.LeadingTrivia.Contains(trivia))
            {
                return CSharpStructureHelpers.CreateCommentBlockSpan(token.LeadingTrivia);
            }
            else if (token.TrailingTrivia.Contains(trivia))
            {
                return CSharpStructureHelpers.CreateCommentBlockSpan(token.TrailingTrivia);
            }
            else
            {
                return Contract.FailWithReturn<ImmutableArray<BlockSpan>>();
            }
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSimpleComment1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|span:// Hello
// $$C#|}
class C
{
}
";

            await VerifyBlockSpansAsync(code,
                Region("span", "// Hello ...", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSimpleComment2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|span:// Hello
//
// $$C#!|}
class C
{
}
";

            await VerifyBlockSpansAsync(code,
                Region("span", "// Hello ...", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSimpleComment3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|span:// Hello

// $$C#!|}
class C
{
}
";

            await VerifyBlockSpansAsync(code,
                Region("span", "// Hello ...", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLineCommentGroupFollowedByDocumentationComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|span:// Hello

// $$C#!|}
/// <summary></summary>
class C
{
}
";

            await VerifyBlockSpansAsync(code,
                Region("span", "// Hello ...", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMultilineComment1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|span:/* Hello
$$C# */|}
class C
{
}
";

            await VerifyBlockSpansAsync(code,
                Region("span", "/* Hello ...", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMultilineCommentOnOneLine()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|span:/* Hello $$C# */|}
class C
{
}
";

            await VerifyBlockSpansAsync(code,
                Region("span", "/* Hello C# ...", autoCollapse: true));
        }

        [WorkItem(791, "https://github.com/dotnet/roslyn/issues/791")]
        [WorkItem(1108049, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1108049")]
        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIncompleteMultilineCommentZeroSpace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|span:$$/*|}";

            await VerifyBlockSpansAsync(code,
                Region("span", "/*  ...", autoCollapse: true));
        }

        [WorkItem(791, "https://github.com/dotnet/roslyn/issues/791")]
        [WorkItem(1108049, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1108049")]
        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIncompleteMultilineCommentSingleSpace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|span:$$/* |}";

            await VerifyBlockSpansAsync(code,
                Region("span", "/*  ...", autoCollapse: true));
        }
    }
}
