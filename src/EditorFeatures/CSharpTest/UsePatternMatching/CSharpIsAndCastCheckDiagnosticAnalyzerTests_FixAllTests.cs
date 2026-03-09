// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.UsePatternMatching
{
    public partial class CSharpIsAndCastCheckDiagnosticAnalyzerTests
    {
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FixAllInDocument1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        if (x is string)
        {
            {|FixAllInDocument:var|} v1 = (string)x;
        }

        if (x is bool)
        {
            var v2 = (bool)x;
        }
    }
}",
@"class C
{
    void M()
    {
        if (x is string v1)
        {
        }

        if (x is bool v2)
        {
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FixAllInDocument2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        if (x is string)
        {
            var v1 = (string)x;
        }

        if (x is bool)
        {
            {|FixAllInDocument:var|} v2 = (bool)x;
        }
    }
}",
@"class C
{
    void M()
    {
        if (x is string v1)
        {
        }

        if (x is bool v2)
        {
        }
    }
}");
        }
    }
}
