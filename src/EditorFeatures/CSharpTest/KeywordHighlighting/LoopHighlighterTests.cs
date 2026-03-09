// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Editor.CSharp.KeywordHighlighting.KeywordHighlighters;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.KeywordHighlighting
{
    public class LoopHighlighterTests : AbstractCSharpKeywordHighlighterTests
    {
        internal override IHighlighter CreateHighlighter()
        {
            return new LoopHighlighter();
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
        {|Cursor:[|while|]|} (true)
        {
            if (x)
            {
                [|break|];
            }
            else
            {
                [|continue|];
            }
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
        [|while|] (true)
        {
            if (x)
            {
                {|Cursor:[|break|]|};
            }
            else
            {
                [|continue|];
            }
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
        [|while|] (true)
        {
            if (x)
            {
                [|break|];
            }
            else
            {
                {|Cursor:[|continue|]|};
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
        {|Cursor:[|do|]|}
        {
            if (x)
            {
                [|break|];
            }
            else
            {
                [|continue|];
            }
        }
        [|while|] (true);
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
        [|do|]
        {
            if (x)
            {
                {|Cursor:[|break|]|};
            }
            else
            {
                [|continue|];
            }
        }
        [|while|] (true);
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
        [|do|]
        {
            if (x)
            {
                [|break|];
            }
            else
            {
                {|Cursor:[|continue|]|};
            }
        }
        [|while|] (true);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample2_4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        [|do|]
        {
            if (x)
            {
                [|break|];
            }
            else
            {
                [|continue|];
            }
        }
        {|Cursor:[|while|]|} (true);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample2_5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        do
        {
            if (x)
            {
                break;
            }
            else
            {
                continue;
            }
        }
        while {|Cursor:(true)|};
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample2_6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        [|do|]
        {
            if (x)
            {
                [|break|];
            }
            else
            {
                [|continue|];
            }
        }
        [|while|] (true);{|Cursor:|}
    }
}");
        }

        private const string SpecExample3 = @"for (int i = 0; i < 10; i++) {
    if (x) {
        break;
    }
    else {
        continue;
    }
}";

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample3_1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        {|Cursor:[|for|]|} (int i = 0; i < 10; i++)
        {
            if (x)
            {
                [|break|];
            }
            else
            {
                [|continue|];
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample3_2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        [|for|] (int i = 0; i < 10; i++)
        {
            if (x)
            {
                {|Cursor:[|break|];|}
            }
            else
            {
                [|continue|];
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample3_3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        [|for|] (int i = 0; i < 10; i++)
        {
            if (x)
            {
                [|break|];
            }
            else
            {
                {|Cursor:[|continue|];|}
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample4_1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        {|Cursor:[|foreach|]|} (var a in x)
        {
            if (x)
            {
                [|break|];
            }
            else
            {
                [|continue|];
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample4_2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        [|foreach|] (var a in x)
        {
            if (x)
            {
                {|Cursor:[|break|];|}
            }
            else
            {
                [|continue|];
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExample4_3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        [|foreach|] (var a in x)
        {
            if (x)
            {
                [|break|];
            }
            else
            {
                {|Cursor:[|continue|];|}
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedExample1_1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        {|Cursor:[|foreach|]|} (var a in x)
        {
            if (a)
            {
                [|break|];
            }
            else
            {
                switch (b)
                {
                    case 0:
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
        public async Task TestNestedExample1_2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        [|foreach|] (var a in x)
        {
            if (a)
            {
                {|Cursor:[|break|];|}
            }
            else
            {
                switch (b)
                {
                    case 0:
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
        public async Task TestNestedExample1_3()
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
                    case 0:
                        while (true)
                        {
                            {|Cursor:[|do|]|}
                            {
                                [|break|];
                            }
                            [|while|] (false);
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
        public async Task TestNestedExample1_4()
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
                    case 0:
                        while (true)
                        {
                            [|do|]
                            {
                                {|Cursor:[|break|];|}
                            }
                            [|while|] (false);
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
        public async Task TestNestedExample1_5()
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
                    case 0:
                        while (true)
                        {
                            [|do|]
                            {
                                [|break|];
                            }
                            {|Cursor:[|while|]|} (false);
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
        public async Task TestNestedExample1_6()
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
                    case 0:
                        while (true)
                        {
                            [|do|]
                            {
                                [|break|];
                            }
                            [|while|] (false);{|Cursor:|}
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
        public async Task TestNestedExample1_7()
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
                    case 0:
                        {|Cursor:[|while|]|} (true)
                        {
                            do
                            {
                                break;
                            }
                            while (false);
                            [|break|];
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
        public async Task TestNestedExample1_8()
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
                    case 0:
                        [|while|] (true)
                        {
                            do
                            {
                                break;
                            }
                            while (false);
                            {|Cursor:[|break|];|}
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

        // TestNestedExample1 9-13 are in SwitchStatementHighlighterTests.cs

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedExample1_14()
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
                    case 0:
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

            {|Cursor:[|for|]|} (int i = 0; i < 10; i++)
            {
                [|break|];
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedExample1_15()
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
                    case 0:
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

            [|for|] (int i = 0; i < 10; i++)
            {
                {|Cursor:[|break|];|}
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedExample2_1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        {|Cursor:[|foreach|]|} (var a in x)
        {
            if (a)
            {
                [|continue|];
            }
            else
            {
                while (true)
                {
                    do
                    {
                        continue;
                    }
                    while (false);
                    continue;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                continue;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedExample2_2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        [|foreach|] (var a in x)
        {
            if (a)
            {
                {|Cursor:[|continue|];|}
            }
            else
            {
                while (true)
                {
                    do
                    {
                        continue;
                    }
                    while (false);
                    continue;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                continue;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedExample2_3()
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
                continue;
            }
            else
            {
                while (true)
                {
                    {|Cursor:[|do|]|}
                    {
                        [|continue|];
                    }
                    [|while|] (false);
                    continue;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                continue;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedExample2_4()
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
                continue;
            }
            else
            {
                while (true)
                {
                    [|do|]
                    {
                        {|Cursor:[|continue|];|}
                    }
                    [|while|] (false);
                    continue;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                continue;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedExample2_5()
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
                continue;
            }
            else
            {
                while (true)
                {
                    [|do|]
                    {
                        [|continue|];
                    }
                    {|Cursor:[|while|]|} (false);
                    continue;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                continue;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedExample2_6()
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
                continue;
            }
            else
            {
                while (true)
                {
                    do
                    {
                        continue;
                    }
                    while {|Cursor:(false)|};
                    continue;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                continue;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedExample2_7()
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
                continue;
            }
            else
            {
                while (true)
                {
                    [|do|]
                    {
                        [|continue|];
                    }
                    [|while|] (false);{|Cursor:|}
                    continue;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                continue;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedExample2_8()
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
                continue;
            }
            else
            {
                {|Cursor:[|while|]|} (true)
                {
                    do
                    {
                        continue;
                    }
                    while (false);
                    [|continue|];
                }
            }

            for (int i = 0; i < 10; i++)
            {
                continue;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedExample2_9()
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
                continue;
            }
            else
            {
                [|while|] (true)
                {
                    do
                    {
                        continue;
                    }
                    while (false);
                    {|Cursor:[|continue|];|}
                }
            }

            for (int i = 0; i < 10; i++)
            {
                continue;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedExample2_10()
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
                continue;
            }
            else
            {
                while (true)
                {
                    do
                    {
                        continue;
                    }
                    while (false);
                    continue;
                }
            }

            {|Cursor:[|for|]|} (int i = 0; i < 10; i++)
            {
                [|continue|];
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedExample2_11()
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
                continue;
            }
            else
            {
                while (true)
                {
                    do
                    {
                        continue;
                    }
                    while (false);
                    continue;
                }
            }

            [|for|] (int i = 0; i < 10; i++)
            {
                {|Cursor:[|continue|];|}
            }
        }
    }
}");
        }
    }
}
