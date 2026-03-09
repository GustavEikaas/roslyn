// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Editor.CSharp.KeywordHighlighting.KeywordHighlighters;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.KeywordHighlighting
{
    public class ReturnStatementHighlighterTests : AbstractCSharpKeywordHighlighterTests
    {
        internal override IHighlighter CreateHighlighter()
        {
            return new ReturnStatementHighlighter();
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"static double CalculateArea(double radius)
{
    Func<double, double> f = r => {
        if (Double.IsNan(r))
        {
            {|Cursor:[|return|]|} Double.NaN;
        }
        else
        {
            [|return|] r * r * Math.PI;
        }
    };
    return calcArea(radius);
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInLambda_NotOnReturnValue()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    static double CalculateArea(double radius)
    {
        Func<double, double> f = r => {
            if (Double.IsNan(r))
            {
                return {|Cursor:Double.NaN|};
            }
            else
            {
                return r * r * Math.PI;
            }
        };
        return calcArea(radius);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInLambda_OnSemicolon()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    static double CalculateArea(double radius)
    {
        Func<double, double> f = r => {
            if (Double.IsNan(r))
            {
                [|return|] Double.NaN;{|Cursor:|}
            }
            else
            {
                [|return|] r * r * Math.PI;
            }
        };
        return calcArea(radius);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInLambda_SecondOccurence()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    static double CalculateArea(double radius)
    {
        Func<double, double> f = r => {
            if (Double.IsNan(r))
            {
                [|return|] Double.NaN;
            }
            else
            {
                {|Cursor:[|return|]|} r * r * Math.PI;
            }
        };
        return calcArea(radius);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInLambda_SecondOccurence_NotOnReturnValue()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    static double CalculateArea(double radius)
    {
        Func<double, double> f = r => {
            if (Double.IsNan(r))
            {
                return Double.NaN;
            }
            else
            {
                return {|Cursor:r * r * Math.PI|};
            }
        };
        return calcArea(radius);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInLambda_SecondOccurence_OnSemicolon()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    static double CalculateArea(double radius)
    {
        Func<double, double> f = r => {
            if (Double.IsNan(r))
            {
                [|return|] Double.NaN;
            }
            else
            {
                [|return|] r * r * Math.PI;{|Cursor:|}
            }
        };
        return calcArea(radius);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInMethodWithLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    static double CalculateArea(double radius)
    {
        Func<double, double> f = r => {
            if (Double.IsNan(r))
            {
                return Double.NaN;
            }
            else
            {
                return r * r * Math.PI;
            }
        };
        {|Cursor:[|return|]|} calcArea(radius);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInMethodWithLambda_NotOnReturnValue()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    static double CalculateArea(double radius)
    {
        Func<double, double> f = r => {
            if (Double.IsNan(r))
            {
                return Double.NaN;
            }
            else
            {
                return r * r * Math.PI;
            }
        };
        return {|Cursor:calcArea(radius)|};
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInMethodWithLambda_OnSemicolon()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    static double CalculateArea(double radius)
    {
        Func<double, double> f = r => {
            if (Double.IsNan(r))
            {
                return Double.NaN;
            }
            else
            {
                return r * r * Math.PI;
            }
        };
        [|return|] calcArea(radius);{|Cursor:|}
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInConstructor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    C()
    {
        {|Cursor:[|return|]|};
        [|return|];
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInDestructor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    ~C()
    {
        {|Cursor:[|return|]|};
        [|return|];
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInOperator()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    public static string operator +(C a)
    {
        {|Cursor:[|return|]|} null;
        [|return|] null;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInConversionOperator()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    public static explicit operator string(C a)
    {
        {|Cursor:[|return|]|} null;
        [|return|] null;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInGetter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    int P
    {
        get
        {
            {|Cursor:[|return|]|} 0;
            [|return|] 0;
        }
        set
        {
            return;
            return;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInSetter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    int P
    {
        get
        {
            return 0;
            return 0;
        }
        set
        {
            {|Cursor:[|return|]|};
            [|return|];
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInAdder()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    event EventHandler E
    {
        add
        {
            {|Cursor:[|return|]|};
            [|return|];
        }
        remove
        {
            return;
            return;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInRemover()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    event EventHandler E
    {
        add
        {
            return;
            return;
        }
        remove
        {
            {|Cursor:[|return|]|};
            [|return|];
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInLocalFunction()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        void F()
        {
            {|Cursor:[|return|]|};
            [|return|];
        }

        return;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInSimpleLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        Action<string> f = s =>
        {
            {|Cursor:[|return|]|};
            [|return|];
        };

        return;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInParenthesizedLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        Action<string> f = (s) =>
        {
            {|Cursor:[|return|]|};
            [|return|];
        };

        return;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordHighlighting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInAnonymousMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        Action<string> f = delegate
        {
            {|Cursor:[|return|]|};
            [|return|];
        };

        return;
    }
}");
        }
    }
}
