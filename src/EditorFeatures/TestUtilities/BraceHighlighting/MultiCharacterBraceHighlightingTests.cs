// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.Test.Utilities;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.UnitTests.BraceHighlighting
{
    public class MultiCharacterBraceHighlightingTests : AbstractBraceHighlightingTests
    {
        protected override TestWorkspace CreateWorkspace(string markup, ParseOptions options)
            => TestWorkspace.Create(
                NoCompilationConstants.LanguageName, compilationOptions: null, parseOptions: options, content: markup);

        internal override IBraceMatchingService GetBraceMatchingService(TestWorkspace workspace)
            => new TestBraceMatchingService();

        private ImmutableArray<TextSpan> GetSpans(BraceMatchingResult? result)
            => result.HasValue
                ? ImmutableArray.Create(result.Value.LeftSpan, result.Value.RightSpan)
                : ImmutableArray<TextSpan>.Empty;

        private class TestBraceMatchingService : IBraceMatchingService
        {
            public async Task<BraceMatchingResult?> GetMatchingBracesAsync(
                Document document, int position, CancellationToken cancellationToken = default(CancellationToken))
            {
                var text = (await document.GetTextAsync(cancellationToken)).ToString();
                var braces = GetMatchingBraces(text, position);
                if (braces.HasValue)
                {
                    Debug.Assert(text.Substring(braces.Value.LeftSpan.Start, braces.Value.LeftSpan.Length) == "<@");
                    Debug.Assert(text.Substring(braces.Value.RightSpan.Start, braces.Value.RightSpan.Length) == "@>");
                }

                return braces;
            }

            public BraceMatchingResult? GetMatchingBraces(
                string text, int position)
            {
                if (position < text.Length)
                {
                    var ch = text[position];

                    // Look for   <@   @>  depending on where the caret is.

                    //      ^<@     @>
                    if (ch == '<')
                    {
                        Debug.Assert(text[position + 1] == '@');
                        var secondAt = text.IndexOf('@', position + 2);
                        return new BraceMatchingResult(new TextSpan(position, 2), new TextSpan(secondAt, 2));
                    }

                    //  <^@    @>     or   <@    ^@>
                    if (ch == '@')
                    {
                        if (text[position - 1] == '<')
                        {
                            var secondAt = text.IndexOf('@', position + 1);
                            return new BraceMatchingResult(new TextSpan(position - 1, 2), new TextSpan(secondAt, 2));
                        }
                        else
                        {
                            Debug.Assert(text[position + 1] == '>');
                            var lessThan = text.LastIndexOf('<', position);
                            return new BraceMatchingResult(new TextSpan(lessThan, 2), new TextSpan(position, 2));
                        }
                    }

                    // <@    @^>
                    if (ch == '>')
                    {
                        Debug.Assert(text[position - 1] == '@');
                        var lessThan = text.LastIndexOf('<', position);
                        return new BraceMatchingResult(new TextSpan(lessThan, 2), new TextSpan(position - 1, 2));
                    }
                }

                return null;
            }
        }

        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnBrace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"$$ <@    @>");
        }

        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnLeftOfStartBrace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"$$[|<@|]    [|@>|]");
        }

        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideStartBrace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"[|<$$@|]    [|@>|]");
        }

        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnRightOfStartBrace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"<@$$    @>");
        }

        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnLeftOfCloseBrace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"<@    $$@>");
        }

        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideCloseBrace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"[|<@|]    [|@$$>|]");
        }

        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnRightOfCloseBrace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"[|<@|]    [|@>$$|]");
        }

        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterBrace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"<@    @> $$");
        }



        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnBrace2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"$$ <@    @><@    @>");
        }

        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnLeftOfStartBrace2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"$$[|<@|]    [|@>|]<@    @>");
        }

        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideStartBrace2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"[|<$$@|]    [|@>|]<@    @>");
        }

        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnRightOfStartBrace2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"<@$$    @><@    @>");
        }

        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnLeftOfCloseBrace2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"<@    $$@><@    @>");
        }

        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideCloseBrace3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"[|<@|]    [|@$$>|]<@    @>");
        }

        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnRightOfCloseBrace2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"[|<@|]    [|@>|]$$[|<@|]    [|@>|]");
        }

        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInSecondBracePair()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"<@    @>[|<$$@|]    [|@>|]");
        }

        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterSecondBracePairStart()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"<@    @><@$$    @>");
        }

        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotBeforeSecondBracePairEnd()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"<@    @><@    $$@>");
        }

        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInSecondBracePairEnd()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"<@    @>[|<@|]    [|@$$>|]");
        }

        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAtSecondBracePairEnd()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"<@    @>[|<@|]    [|@>|]$$");
        }

        [WorkItem(18050, "https://github.com/dotnet/roslyn/issues/18050")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterSecondBracePairEnd()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestBraceHighlightingAsync(
"<@    @><@    @>  $$");
        }
    }
}
