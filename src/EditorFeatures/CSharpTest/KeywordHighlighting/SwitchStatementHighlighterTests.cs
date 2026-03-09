// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Editor.CSharp.KeywordHighlighting.KeywordHighlighters;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.KeywordHighlighting
{
    public class SwitchStatementHighlighterTests : AbstractCSharpKeywordHighlighterTests
    {
        internal override IHighlighter CreateHighlighter()
        {
            return new SwitchStatementHighlighter();
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample1_OnSwitchKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        {|Cursor:[|switch|]|} (i)
        {
            [|case|] 0:
                CaseZero();
                [|break|];
            [|case|] 1:
                CaseOne();
                [|break|];
            [|default|]:
                CaseOthers();
                [|break|];
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample1_OnCaseKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        [|switch|] (i)
        {
            {|Cursor:[|case|]|} 0:
                CaseZero();
                [|break|];
            [|case|] 1:
                CaseOne();
                [|break|];
            [|default|]:
                CaseOthers();
                [|break|];
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample1_AfterCaseColon()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        [|switch|] (i)
        {
            [|case|] 0:{|Cursor:|}
                CaseZero();
                [|break|];
            [|case|] 1:
                CaseOne();
                [|break|];
            [|default|]:
                CaseOthers();
                [|break|];
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample1_NotOnCaseValue()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        switch (i)
        {
            case {|Cursor:0|}:
                CaseZero();
                break;
            case 1:
                CaseOne();
                break;
            default:
                CaseOthers();
                break;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample1_OnBreakStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        [|switch|] (i)
        {
            [|case|] 0:
                CaseZero();
                {|Cursor:[|break|];|}
            [|case|] 1:
                CaseOne();
                [|break|];
            [|default|]:
                CaseOthers();
                [|break|];
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample1_OnDefaultLabel()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        [|switch|] (i)
        {
            [|case|] 0:
                CaseZero();
                [|break|];
            [|case|] 1:
                CaseOne();
                [|break|];
            {|Cursor:[|default|]:|}
                CaseOthers();
                [|break|];
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample2_OnGotoCaseKeywords()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        [|switch|] (i)
        {
            [|case|] 0:
                CaseZero();
                {|Cursor:[|goto case|]|} 1;
            [|case|] 1:
                CaseOne();
                [|goto default|];
            [|default|]:
                CaseOthers();
                [|break|];
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample2_AfterGotoCaseSemicolon()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        [|switch|] (i)
        {
            [|case|] 0:
                CaseZero();
                [|goto case|] 1;{|Cursor:|}
            [|case|] 1:
                CaseOne();
                [|goto default|];
            [|default|]:
                CaseOthers();
                [|break|];
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample2_NotOnGotoCaseValue()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        switch (i)
        {
            case 0:
                CaseZero();
                goto case {|Cursor:1|};
            case 1:
                CaseOne();
                goto default;
            default:
                CaseOthers();
                break;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample2_OnGotoDefaultStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        [|switch|] (i)
        {
            [|case|] 0:
                CaseZero();
                [|goto case|] 1;
            [|case|] 1:
                CaseOne();
                {|Cursor:[|goto default|];|}
            [|default|]:
                CaseOthers();
                [|break|];
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedExample1_OnSwitchKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        foreach (var a in x)
        {
            if (a)
            {
                break;
            }
            else
            {
                {|Cursor:[|switch|]|} (b)
                {
                    [|case|] 0:
                        while (true)
                        {
                            do
                            {
                                break;
                            }
                            while (false);
                            break;
                        }

                        [|break|];
                }
            }

            for (int i = 0; i < 10; i++)
            {
                break;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedExample1_OnCaseKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        foreach (var a in x)
        {
            if (a)
            {
                break;
            }
            else
            {
                [|switch|] (b)
                {
                    {|Cursor:[|case|]|} 0:
                        while (true)
                        {
                            do
                            {
                                break;
                            }
                            while (false);
                            break;
                        }

                        [|break|];
                }
            }

            for (int i = 0; i < 10; i++)
            {
                break;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedExample1_NotBeforeCaseValue()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        foreach (var a in x)
        {
            if (a)
            {
                break;
            }
            else
            {
                switch (b)
                {
                    case {|Cursor:|}0:
                        while (true)
                        {
                            do
                            {
                                break;
                            }
                            while (false);
                            break;
                        }

                        break;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                break;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedExample1_AfterCaseColon()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        foreach (var a in x)
        {
            if (a)
            {
                break;
            }
            else
            {
                [|switch|] (b)
                {
                    [|case|] 0:{|Cursor:|}
                        while (true)
                        {
                            do
                            {
                                break;
                            }
                            while (false);
                            break;
                        }

                        [|break|];
                }
            }

            for (int i = 0; i < 10; i++)
            {
                break;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedExample1_OnBreakStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        foreach (var a in x)
        {
            if (a)
            {
                break;
            }
            else
            {
                [|switch|] (b)
                {
                    [|case|] 0:
                        while (true)
                        {
                            do
                            {
                                break;
                            }
                            while (false);
                            break;
                        }

                        {|Cursor:[|break|];|}
                }
            }

            for (int i = 0; i < 10; i++)
            {
                break;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Bug3483()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    static void M()
    {
        [|switch|] (2)
        {
            [|case|] 1:
                {|Cursor:[|goto|]|}
        }
    }
}");
        }

        [WorkItem(25039, "https://github.com/dotnet/roslyn/issues/25039")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithUnrelatedGotoStatement_OnGotoCaseGotoKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        label:
        [|switch|] (i)
        {
            [|case|] 0:
                CaseZero();
                [|{|Cursor:goto|} case|] 1;
            [|case|] 1:
                CaseOne();
                [|goto default|];
            [|default|]:
                CaseOthers();
                goto label;
        }
    }
}");
        }

        [WorkItem(25039, "https://github.com/dotnet/roslyn/issues/25039")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithUnrelatedGotoStatement_OnGotoDefaultGotoKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        label:
        [|switch|] (i)
        {
            [|case|] 0:
                CaseZero();
                [|goto case|] 1;
            [|case|] 1:
                CaseOne();
                [|{|Cursor:goto|} default|];
            [|default|]:
                CaseOthers();
                goto label;
        }
    }
}");
        }

        [WorkItem(25039, "https://github.com/dotnet/roslyn/issues/25039")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithUnrelatedGotoStatement_NotOnGotoLabelGotoKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        label:
        switch (i)
        {
            case 0:
                CaseZero();
                goto case 1;
            case 1:
                CaseOne();
                goto default;
            default:
                CaseOthers();
                {|Cursor:goto|} label;
        }
    }
}");
        }

        [WorkItem(25039, "https://github.com/dotnet/roslyn/issues/25039")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithNestedStatements_OnSwitchKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        {|Cursor:[|switch|]|} (i)
        {
            [|case|] 0:
            {
                CaseZero();
                [|goto case|] 1;
            }
            [|case|] 1:
            {
                CaseOne();
                [|goto default|];
            }
            [|default|]:
            {
                CaseOthers();
                [|break|];
            }
        }
    }
}");
        }

        [WorkItem(25039, "https://github.com/dotnet/roslyn/issues/25039")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithNestedStatements_OnBreakKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        [|switch|] (i)
        {
            [|case|] 0:
            {
                CaseZero();
                [|goto case|] 1;
            }
            [|case|] 1:
            {
                CaseOne();
                [|goto default|];
            }
            [|default|]:
            {
                CaseOthers();
                {|Cursor:[|break|]|};
            }
        }
    }
}");
        }

        [WorkItem(25039, "https://github.com/dotnet/roslyn/issues/25039")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithGotoCaseAndBreakInsideLoop_OnSwitchKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        {|Cursor:[|switch|]|} (true)
        {
            [|case|] true:
                while (true)
                {
                    [|goto case|] true;
                    break;

                    switch (true)
                    {
                        case true:
                            goto case true;
                            break;
                    }
                }
        }
    }
}");
        }

        [WorkItem(25039, "https://github.com/dotnet/roslyn/issues/25039")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithGotoCaseAndBreakInsideLoop_OnGotoCaseGotoKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        [|switch|] (true)
        {
            [|case|] true:
                while (true)
                {
                    [|{|Cursor:goto|} case|] true;
                    break;

                    switch (true)
                    {
                        case true:
                            goto case true;
                            break;
                    }
                }
        }
    }
}");
        }

        [WorkItem(25039, "https://github.com/dotnet/roslyn/issues/25039")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithGotoCaseAndBreakInsideLoop_NotOnLoopBreakKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        switch (true)
        {
            case true:
                while (true)
                {
                    goto case true;
                    {|Cursor:break|};

                    switch (true)
                    {
                        case true:
                            goto case true;
                            break;
                    }
                }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithWhenClauseAndPattern_OnSwitchKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        {|Cursor:[|switch|]|} (true)
        {
            [|case|] true when true:
                [|break|];
            [|case|] bool b:
                [|break|];
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithWhenClauseAndPattern_NotOnWhenKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        switch (true)
        {
            case true {|Cursor:when|} true:
                break;
            case bool b:
                break;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithWhenClauseAndPattern_AfterWhenCaseColon()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        [|switch|] (true)
        {
            [|case|] true when true:{|Cursor:|}
                [|break|];
            [|case|] bool b:
                [|break|];
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithWhenClauseAndPattern_AfterPatternCaseColon()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        [|switch|] (true)
        {
            [|case|] true when true:
                [|break|];
            [|case|] bool b:{|Cursor:|}
                [|break|];
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithWhenClauseAndPattern_NotOnWhenValue()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        switch (true)
        {
            case true when {|Cursor:true|}:
                break;
            case bool b:
                break;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithWhenClauseAndPattern_NotOnPattern()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        switch (true)
        {
            case true when true:
                break;
            case {|Cursor:bool b|}:
                break;
        }
    }
}");
        }
    }
}
