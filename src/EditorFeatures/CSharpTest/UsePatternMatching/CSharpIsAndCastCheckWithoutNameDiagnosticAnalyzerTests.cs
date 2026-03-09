// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.UsePatternMatching;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.UsePatternMatching
{
    public partial class CSharpIsAndCastCheckWithoutNameDiagnosticAnalyzerTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace)
            => (new CSharpIsAndCastCheckWithoutNameDiagnosticAnalyzer(), new CSharpIsAndCastCheckWithoutNameCodeFixProvider());

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestBinaryExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class TestFile
{
    int i;
    bool M(object obj)
    {
        return [||]obj is TestFile && ((TestFile)obj).i > 0;
    }
}",
@"class TestFile
{
    int i;
    bool M(object obj)
    {
        return obj is TestFile {|Rename:file|} && file.i > 0;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInCSharp6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class TestFile
{
    int i;
    bool M(object obj)
    {
        return [||]obj is TestFile && ((TestFile)obj).i > 0;
    }
}", parameters: new TestParameters(parseOptions: CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp6)));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExpressionBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class TestFile
{
    int i;
    bool M(object obj)
        => [||]obj is TestFile && ((TestFile)obj).i > 0;
}",

@"class TestFile
{
    int i;
    bool M(object obj)
        => obj is TestFile {|Rename:file|} && file.i > 0;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class TestFile
{
    int i;
    static object obj;

    bool M = [||]obj is TestFile && ((TestFile)obj).i > 0;
}",

@"class TestFile
{
    int i;
    static object obj;

    bool M = obj is TestFile {|Rename:file|} && file.i > 0;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLambdaBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
using System;

class TestFile
{
    int i;

    void Goo(Func<bool> f) { }

    bool M(object obj)
        => Goo(() => [||]obj is TestFile && ((TestFile)obj).i > 0, () => obj is TestFile && ((TestFile)obj).i > 0);
}",
@"
using System;

class TestFile
{
    int i;

    void Goo(Func<bool> f) { }

    bool M(object obj)
        => Goo(() => obj is TestFile {|Rename:file|} && file.i > 0, () => obj is TestFile && ((TestFile)obj).i > 0);
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDefiniteAssignment1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class TestFile
{
    int i;
    bool M(object obj)
    {
        if ([||]obj is TestFile)
        {
            M(((TestFile)obj).i);
            M(((TestFile)obj).i);
        }
        else
        {
            M(((TestFile)obj).i);
            M(((TestFile)obj).i);
        }
    }
}",
@"class TestFile
{
    int i;
    bool M(object obj)
    {
        if (obj is TestFile {|Rename:file|})
        {
            M(file.i);
            M(file.i);
        }
        else
        {
            M(((TestFile)obj).i);
            M(((TestFile)obj).i);
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDefiniteAssignment2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class TestFile
{
    int i;
    bool M(object obj)
    {
        if (!([||]obj is TestFile))
        {
            M(((TestFile)obj).i);
            M(((TestFile)obj).i);
        }
        else
        {
            M(((TestFile)obj).i);
            M(((TestFile)obj).i);
        }
    }
}",
@"class TestFile
{
    int i;
    bool M(object obj)
    {
        if (!(obj is TestFile {|Rename:file|}))
        {
            M(((TestFile)obj).i);
            M(((TestFile)obj).i);
        }
        else
        {
            M(file.i);
            M(file.i);
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnAnalyzerMatch()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class TestFile
{
    bool M(object obj)
    {
        if ([||]obj is TestFile)
        {
            var file = (TestFile)obj;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnNullable()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"struct TestFile
{
    bool M(object obj)
    {
        if ([||]obj is TestFile?)
        {
            var i = ((TestFile?)obj).Value;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestComplexMatch()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class TestFile
{
    int i;
    bool M(object obj)
    {
        return [||]M(null) is TestFile && ((TestFile)M(null)).i > 0;
    }
}",

@"class TestFile
{
    int i;
    bool M(object obj)
    {
        return M(null) is TestFile {|Rename:file|} && file.i > 0;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class TestFile
{
    int i;
    bool M(object obj)
    {
        return [||]obj is TestFile && /*before*/ ((TestFile)obj) /*after*/.i > 0;
    }
}",

@"class TestFile
{
    int i;
    bool M(object obj)
    {
        return obj is TestFile {|Rename:file|} && /*before*/ file /*after*/.i > 0;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixOnlyAfterIsCheck()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class TestFile
{
    int i;
    bool M(object obj)
    {
        return ((TestFile)obj).i > 0 && [||]obj is TestFile && ((TestFile)obj).i > 0;
    }
}",

@"class TestFile
{
    int i;
    bool M(object obj)
    {
        return ((TestFile)obj).i > 0 && obj is TestFile {|Rename:file|} && file.i > 0;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestArrayNaming()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class TestFile
{
    int i;
    bool M(object obj)
    {
        return [||]obj is int[] && ((int[])obj) > 0;
    }
}",

@"class TestFile
{
    int i;
    bool M(object obj)
    {
        return obj is int[] {|Rename:v|} && v > 0;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNamingConflict1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class TestFile
{
    int i;
    bool M(object obj)
    {
        TestFile file = null;
        return [||]obj is TestFile && ((TestFile)obj).i > 0;
    }
}",

@"class TestFile
{
    int i;
    bool M(object obj)
    {
        TestFile file = null;
        return obj is TestFile {|Rename:file1|} && file1.i > 0;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNamingConflict2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class TestFile
{
    int i;
    bool M(object obj)
    {
        if ([||]obj is TestFile)
        {
            TestFile file = null;
            M(((TestFile)obj).i);
        }
    }
}",
@"class TestFile
{
    int i;
    bool M(object obj)
    {
        if (obj is TestFile {|Rename:file1|})
        {
            TestFile file = null;
            M(file1.i);
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNamingNoConflict1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class TestFile
{
    int i;
    bool M(object obj)
    {
        if ([||]obj is TestFile)
        {
            var v = new { file = 0 };
            M(((TestFile)obj).i);
        }
    }
}",
@"class TestFile
{
    int i;
    bool M(object obj)
    {
        if (obj is TestFile {|Rename:file|})
        {
            var v = new { file = 0 };
            M(file.i);
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNamingNoConflict2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class TestFile
{
    int i;
    bool M(object obj)
    {
        if ([||]obj is TestFile)
        {
            var v = (file: 0, x: 1);
            M(((TestFile)obj).i);
        }
    }
}",
@"class TestFile
{
    int i;
    bool M(object obj)
    {
        if (obj is TestFile {|Rename:file|})
        {
            var v = (file: 0, x: 1);
            M(file.i);
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNamingNoConflict3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class X { public int file; }

class TestFile
{
    int i;
    bool M(object obj, X x)
    {
        if ([||]obj is TestFile)
        {
            var v = new { x.file };
            M(((TestFile)obj).i);
        }
    }
}",
@"
class X { public int file; }

class TestFile
{
    int i;
    bool M(object obj, X x)
    {
        if (obj is TestFile {|Rename:file|})
        {
            var v = new { x.file };
            M(file.i);
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInlineTypeCheck)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNamingNoConflict4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class X { public int file; }

class TestFile
{
    int i;
    bool M(object obj, X x)
    {
        if ([||]obj is TestFile)
        {
            var v = (x.file, 0);
            M(((TestFile)obj).i);
        }
    }
}",
@"
class X { public int file; }

class TestFile
{
    int i;
    bool M(object obj, X x)
    {
        if (obj is TestFile {|Rename:file|})
        {
            var v = (x.file, 0);
            M(file.i);
        }
    }
}");
        }
    }
}
