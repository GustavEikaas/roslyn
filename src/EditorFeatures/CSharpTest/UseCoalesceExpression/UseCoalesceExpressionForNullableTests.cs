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
    public class UseCoalesceExpressionForNullableTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace)
            => (new CSharpUseCoalesceExpressionForNullableDiagnosticAnalyzer(),
                new UseCoalesceExpressionForNullableCodeFixProvider());

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCoalesceExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnLeft_Equals()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    void M(int? x, int? y)
    {
        var z = [||]!x.HasValue ? y : x.Value;
    }
}",
@"using System;

class C
{
    void M(int? x, int? y)
    {
        var z = x ?? y ;
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
    void M(int? x, int? y)
    {
        var z = [||]x.HasValue ? x.Value : y;
    }
}",
@"using System;

class C
{
    void M(int? x, int? y)
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
    void M(int? x, int? y)
    {
        var z = [||]!(x + y).HasValue ? y : (x + y).Value;
    }
}",
@"using System;

class C
{
    void M(int? x, int? y)
    {
        var z = (x + y) ?? y ;
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
    void M(int? x, int? y)
    {
        var z = [||](x.HasValue) ? x.Value : y;
    }
}",
@"using System;

class C
{
    void M(int? x, int? y)
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
    void M(int? x, int? y)
    {
        var z1 = {|FixAllInDocument:x|}.HasValue ? x.Value : y;
        var z2 = !x.HasValue ? y : x.Value;
    }
}",
@"using System;

class C
{
    void M(int? x, int? y)
    {
        var z1 = x ?? y;
        var z2 = x ?? y ;
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
    void M(int? x, int? y, int? z)
    {
        var w = {|FixAllInDocument:x|}.HasValue ? x.Value : y.ToString(z.HasValue ? z.Value : y);
    }
}",
@"using System;

class C
{
    void M(int? x, int? y, int? z)
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
    void M(int? x, int? y, int? z)
    {
        var w = {|FixAllInDocument:x|}.HasValue ? x.Value : y.HasValue ? y.Value : z;
    }
}",
@"using System;

class C
{
    void M(int? x, int? y, int? z)
    {
        var w = x ?? y ?? z;
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
    void M(int? x, int? y)
    {
        Expression<Func<int>> e = () => [||]!x.HasValue ? y : x.Value;
    }
}",
@"using System;
using System.Linq.Expressions;

class C
{
    void M(int? x, int? y)
    {
        Expression<Func<int>> e = () => {|Warning:x ?? y|} ;
    }
}");
        }
    }
}
