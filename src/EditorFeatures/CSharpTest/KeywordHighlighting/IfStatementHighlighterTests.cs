// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Editor.CSharp.KeywordHighlighting;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.KeywordHighlighting
{
    public class IfStatementHighlighterTests : AbstractCSharpKeywordHighlighterTests
    {
        internal override IHighlighter CreateHighlighter()
        {
            return new IfStatementHighlighter();
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfStatementWithIfAndSingleElse1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        {|Cursor:[|if|]|} (a < 5)
        {
            // blah
        }
        [|else|]
        {
            // blah
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfStatementWithIfAndSingleElse2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        [|if|] (a < 5)
        {
            // blah
        }
        {|Cursor:[|else|]|}
        {
            // blah
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfStatementWithIfAndElseIfAndElse1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        {|Cursor:[|if|]|} (a < 5)
        {
            // blah
        }
        [|else if|] (a == 10)
        {
            // blah
        }
        [|else|]
        {
            // blah
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfStatementWithIfAndElseIfAndElse2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        [|if|] (a < 5)
        {
            // blah
        }
        {|Cursor:[|else if|]|} (a == 10)
        {
            // blah
        }
        [|else|]
        {
            // blah
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfStatementWithIfAndElseIfAndElse3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        [|if|] (a < 5)
        {
            // blah
        }
        [|else if|] (a == 10)
        {
            // blah
        }
        {|Cursor:[|else|]|}
        {
            // blah
        }
    }
}");
        }

        private const string Code3 = @"
public class C
{
    public void Goo()
    {
        int a = 10;
        if (a < 5)
        {
            // blah
        }
        else 
        if (a == 10)
        {
            // blah
        }
        else
        {
            // blah
        }
    }
}";

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfStatementWithElseIfOnDifferentLines1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        {|Cursor:[|if|]|} (a < 5)
        {
            // blah
        }
        [|else|]
        [|if|] (a == 10)
        {
            // blah
        }
        [|else|]
        {
            // blah
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfStatementWithElseIfOnDifferentLines2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        [|if|] (a < 5)
        {
            // blah
        }
        {|Cursor:[|else|]|}
        [|if|] (a == 10)
        {
            // blah
        }
        [|else|]
        {
            // blah
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfStatementWithElseIfOnDifferentLines3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        [|if|] (a < 5)
        {
            // blah
        }
        [|else|]
        {|Cursor:[|if|]|} (a == 10)
        {
            // blah
        }
        [|else|]
        {
            // blah
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfStatementWithElseIfOnDifferentLines4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        [|if|] (a < 5)
        {
            // blah
        }
        [|else|]
        [|if|] (a == 10)
        {
            // blah
        }
        {|Cursor:[|else|]|}
        {
            // blah
        }
    }
}");
        }

        private const string Code4 = @"
public class C
{
    public void Goo()
    {
        int a = 10;
        if(a < 5) {
            // blah
        }
        else if(a == 10) {
            // blah
        }
        else{
            // blah
        }
    }
}";

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfStatementWithIfAndElseIfAndElseTouching1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        {|Cursor:[|if|]|}(a < 5)
        {
            // blah
        }
        [|else if|](a == 10)
        {
            // blah
        }
        [|else|]{
            // blah
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfStatementWithIfAndElseIfAndElseTouching2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        [|if|](a < 5)
        {
            // blah
        }
        {|Cursor:[|else if|]|}(a == 10)
        {
            // blah
        }
        [|else|]{
            // blah
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfStatementWithIfAndElseIfAndElseTouching3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        [|if|](a < 5)
        {
            // blah
        }
        [|else if|](a == 10)
        {
            // blah
        }
        {|Cursor:[|else|]|}{
            // blah
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExtraSpacesBetweenElseAndIf1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        {|Cursor:[|if|]|} (a < 5)
        {
            // blah
        }
        [|else if|] (a == 10)
        {
            // blah
        }
        [|else|]
        {
            // blah
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExtraSpacesBetweenElseAndIf2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        [|if|] (a < 5)
        {
            // blah
        }
        {|Cursor:[|else if|]|} (a == 10)
        {
            // blah
        }
        [|else|]
        {
            // blah
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExtraSpacesBetweenElseAndIf3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        [|if|] (a < 5)
        {
            // blah
        }
        [|else if|] (a == 10)
        {
            // blah
        }
        {|Cursor:[|else|]|}
        {
            // blah
        }
    }
}");
        }

        private const string Code6 = @"
public class C
{
    public void Goo()
    {
        int a = 10;
        if (a < 5)
        {
            // blah
        }
        else /* test */ if (a == 10)
        {
            // blah
        }
        else
        {
            // blah
        }
    }
}";

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCommentBetweenElseIf1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        {|Cursor:[|if|]|} (a < 5)
        {
            // blah
        }
        [|else|] /* test */ [|if|] (a == 10)
        {
            // blah
        }
        [|else|]
        {
            // blah
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCommentBetweenElseIf2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        [|if|] (a < 5)
        {
            // blah
        }
        {|Cursor:[|else|]|} /* test */ [|if|] (a == 10)
        {
            // blah
        }
        [|else|]
        {
            // blah
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCommentBetweenElseIf3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        [|if|] (a < 5)
        {
            // blah
        }
        [|else|] /* test */ {|Cursor:[|if|]|} (a == 10)
        {
            // blah
        }
        [|else|]
        {
            // blah
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCommentBetweenElseIf4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        [|if|] (a < 5)
        {
            // blah
        }
        [|else|] /* test */ [|if|] (a == 10)
        {
            // blah
        }
        {|Cursor:[|else|]|}
        {
            // blah
        }
    }
}");
        }

        private const string Code7 = @"
public class C
{
    public void Goo()
    {
        int a = 10;
        int b = 15;
        if (a < 5) {
            // blah
            if (b < 15)
                b = 15;
            else
                b = 14;
        }
        else if (a == 10) {
            // blah
        }
        else {
            // blah
        }
    }
}";

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedIfDoesNotHighlight1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        int b = 15;
        {|Cursor:[|if|]|} (a < 5)
        {
            // blah
            if (b < 15)
                b = 15;
            else
                b = 14;
        }
        [|else if|] (a == 10)
        {
            // blah
        }
        [|else|]
        {
            // blah
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedIfDoesNotHighlight2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        int b = 15;
        [|if|] (a < 5)
        {
            // blah
            if (b < 15)
                b = 15;
            else
                b = 14;
        }
        {|Cursor:[|else if|]|} (a == 10)
        {
            // blah
        }
        [|else|]
        {
            // blah
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedIfDoesNotHighlight3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class C
{
    public void Goo()
    {
        int a = 10;
        int b = 15;
        [|if|] (a < 5)
        {
            // blah
            if (b < 15)
                b = 15;
            else
                b = 14;
        }
        [|else if|] (a == 10)
        {
            // blah
        }
        {|Cursor:[|else|]|}
        {
            // blah
        }
    }
}");
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
        {|Cursor:[|if|]|} (x)
        {
            if (y)
            {
                F();
            }
            else if (z)
            {
                G();
            }
            else
            {
                H();
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample2_1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        if (x)
        {
            {|Cursor:[|if|]|} (y)
            {
                F();
            }
            [|else if|] (z)
            {
                G();
            }
            [|else|]
            {
                H();
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample2_2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        if (x)
        {
            [|if|] (y)
            {
                F();
            }
            {|Cursor:[|else if|]|} (z)
            {
                G();
            }
            [|else|]
            {
                H();
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample2_3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        if (x)
        {
            [|if|] (y)
            {
                F();
            }
            [|else if|] (z)
            {
                G();
            }
            {|Cursor:[|else|]|}
            {
                H();
            }
        }
    }
}");
        }
    }
}
