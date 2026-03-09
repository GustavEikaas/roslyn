// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.UseCoalesceExpression;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics;
using Microsoft.CodeAnalysis.Test.Utilities;
using Microsoft.CodeAnalysis.UseCoalesceExpression;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.UseCoalesceExpression
{
    public class UseCoalesceExpressionTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace)
            => (new CSharpUseCoalesceExpressionDiagnosticAnalyzer(), new UseCoalesceExpressionCodeFixProvider());

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnLeft_Equals()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    void M(string x, string y)
    {
        var z = [||]x == null ? y : x;
    }
}",
@"using System;

class C
{
    void M(string x, string y)
    {
        var z = x ?? y;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnLeft_NotEquals()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    void M(string x, string y)
    {
        var z = [||]x != null ? x : y;
    }
}",
@"using System;

class C
{
    void M(string x, string y)
    {
        var z = x ?? y;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnRight_Equals()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    void M(string x, string y)
    {
        var z = [||]null == x ? y : x;
    }
}",
@"using System;

class C
{
    void M(string x, string y)
    {
        var z = x ?? y;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnRight_NotEquals()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    void M(string x, string y)
    {
        var z = [||]null != x ? x : y;
    }
}",
@"using System;

class C
{
    void M(string x, string y)
    {
        var z = x ?? y;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestComplexExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    void M(string x, string y)
    {
        var z = [||]x.ToString() == null ? y : x.ToString();
    }
}",
@"using System;

class C
{
    void M(string x, string y)
    {
        var z = x.ToString() ?? y;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestParens1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    void M(string x, string y)
    {
        var z = [||](x == null) ? y : x;
    }
}",
@"using System;

class C
{
    void M(string x, string y)
    {
        var z = x ?? y;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestParens2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    void M(string x, string y)
    {
        var z = [||](x) == null ? y : x;
    }
}",
@"using System;

class C
{
    void M(string x, string y)
    {
        var z = x ?? y;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestParens3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    void M(string x, string y)
    {
        var z = [||]x == null ? y : (x);
    }
}",
@"using System;

class C
{
    void M(string x, string y)
    {
        var z = x ?? y;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestParens4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    void M(string x, string y)
    {
        var z = [||]x == null ? (y) : x;
    }
}",
@"using System;

class C
{
    void M(string x, string y)
    {
        var z = x ?? y;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAll1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    void M(string x, string y)
    {
        var z1 = {|FixAllInDocument:x|} == null ? y : x;
        var z2 = x != null ? x : y;
    }
}",
@"using System;

class C
{
    void M(string x, string y)
    {
        var z1 = x ?? y;
        var z2 = x ?? y;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAll2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    void M(string x, string y, string z)
    {
        var w = {|FixAllInDocument:x|} != null ? x : y.ToString(z != null ? z : y);
    }
}",
@"using System;

class C
{
    void M(string x, string y, string z)
    {
        var w = x ?? y.ToString(z ?? y);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAll3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    void M(string x, string y, string z)
    {
        var w = {|FixAllInDocument:x|} != null ? x : y != null ? y : z;
    }
}",
@"using System;

class C
{
    void M(string x, string y, string z)
    {
        var w = x ?? y ?? z;
    }
}");
        }

        [WorkItem(16025, "https://github.com/dotnet/roslyn/issues/16025")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTrivia1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class Program
{
    public Program()
    {
        string x = "";

        string y = [|x|] == null ? string.Empty : x;
    }
}",
@"using System;

class Program
{
    public Program()
    {
        string x = "";

        string y = x ?? string.Empty;
    }
}");
        }

        [WorkItem(17028, "https://github.com/dotnet/roslyn/issues/17028")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInExpressionOfT()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
using System.Linq.Expressions;

class C
{
    void Main(string s, string y)
    {
        Expression<Func<string>> e = () => [||]s != null ? s : y;
    }
}",
@"using System;
using System.Linq.Expressions;

class C
{
    void Main(string s, string y)
    {
        Expression<Func<string>> e = () => {|Warning:s ?? y|};
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnconstrainedTypeParameter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
class C<T>
{
    void Main(T t)
    {
        var v = [||]t == null ? throw new Exception() : t;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestStructConstrainedTypeParameter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
class C<T> where T : struct
{
    void Main(T t)
    {
        var v = [||]t == null ? throw new Exception() : t;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestClassConstrainedTypeParameter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C<T> where T : class
{
    void Main(T t)
    {
        var v = [||]t == null ? throw new Exception() : t;
    }
}",
@"
class C<T> where T : class
{
    void Main(T t)
    {
        var v = t ?? throw new Exception();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnNullable()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
class C
{
    void Main(int? t)
    {
        var v = [||]t == null ? throw new Exception() : t;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnArray()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void Main(int[] t)
    {
        var v = [||]t == null ? throw new Exception() : t;
    }
}",
@"
class C
{
    void Main(int[] t)
    {
        var v = t ?? throw new Exception();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnInterface()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void Main(System.ICloneable t)
    {
        var v = [||]t == null ? throw new Exception() : t;
    }
}",
@"
class C
{
    void Main(System.ICloneable t)
    {
        var v = t ?? throw new Exception();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnDynamic()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void Main(dynamic t)
    {
        var v = [||]t == null ? throw new Exception() : t;
    }
}",
@"
class C
{
    void Main(dynamic t)
    {
        var v = t ?? throw new Exception();
    }
}");
        }
    }
}
