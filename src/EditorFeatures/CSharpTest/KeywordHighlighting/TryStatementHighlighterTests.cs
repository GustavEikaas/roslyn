// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Editor.CSharp.KeywordHighlighting.KeywordHighlighters;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.KeywordHighlighting
{
    public class TryStatementHighlighterTests : AbstractCSharpKeywordHighlighterTests
    {
        internal override IHighlighter CreateHighlighter()
        {
            return new TryStatementHighlighter();
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample1_1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        {|Cursor:[|try|]|}
        {
            try
            {
            }
            catch (Exception e)
            {
            }
        }
        [|finally|]
        {
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
    void M()
    {
        try
        {
            {|Cursor:[|try|]|}
            {
            }
            [|catch|] (Exception e)
            {
            }
        }
        finally
        {
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
    void M()
    {
        try
        {
            [|try|]
            {
            }
            {|Cursor:[|catch|]|} (Exception e)
            {
            }
        }
        finally
        {
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
    void M()
    {
        [|try|]
        {
            try
            {
            }
            catch (Exception e)
            {
            }
        }
        {|Cursor:[|finally|]|}
        {
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExceptionFilter1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        try
        {
            {|Cursor:[|try|]|}
            {
            }
            [|catch|] (Exception e) [|when|] (e != null)
            {
            }
        }
        finally
        {
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExceptionFilter2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        try
        {
            [|try|]
            {
            }
            {|Cursor:[|catch|]|} (Exception e) [|when|] (e != null)
            {
            }
        }
        finally
        {
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExceptionFilter3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        try
        {
            [|try|]
            {
            }
            [|catch|] (Exception e) {|Cursor:[|when|]|} (e != null)
            {
            }
        }
        finally
        {
        }
    }
}");
        }
    }
}
