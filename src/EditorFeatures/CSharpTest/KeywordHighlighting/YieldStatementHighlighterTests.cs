// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Editor.CSharp.KeywordHighlighting.KeywordHighlighters;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.KeywordHighlighting
{
    public class YieldStatementHighlighterTests : AbstractCSharpKeywordHighlighterTests
    {
        internal override IHighlighter CreateHighlighter()
        {
            return new YieldStatementHighlighter();
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample1_1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    IEnumerable<int> Range(int min, int max)
    {
        while (true)
        {
            if (min >= max)
            {
                {|Cursor:[|yield break|];|}
            }

            [|yield return|] min++;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample1_2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    IEnumerable<int> Range(int min, int max)
    {
        while (true)
        {
            if (min >= max)
            {
                [|yield break|];
            }

            {|Cursor:[|yield return|]|} min++;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample1_3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    IEnumerable<int> Range(int min, int max)
    {
        while (true)
        {
            if (min >= max)
            {
                yield break;
            }

            yield return {|Cursor:min++|};
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample1_4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    IEnumerable<int> Range(int min, int max)
    {
        while (true)
        {
            if (min >= max)
            {
                [|yield break|];
            }

            [|yield return|] min++;{|Cursor:|}
        }
    }
}");
        }
    }
}
