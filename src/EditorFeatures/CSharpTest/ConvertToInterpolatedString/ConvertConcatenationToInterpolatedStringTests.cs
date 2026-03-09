// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp.ConvertToInterpolatedString;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.CodeRefactorings;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.ConvertToInterpolatedString
{
    public class ConvertConcatenationToInterpolatedStringTests : AbstractCSharpCodeActionTest
    {
        protected override CodeRefactoringProvider CreateCodeRefactoringProvider(Workspace workspace, TestParameters parameters)
            => new CSharpConvertConcatenationToInterpolatedStringRefactoringProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingOnSimpleString()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"public class C
{
    void M()
    {
        var v = [||]""string"";
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingOnConcatenatedStrings1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"public class C
{
    void M()
    {
        var v = [||]""string"" + ""string"";
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingOnConcatenatedStrings2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"public class C
{
    void M()
    {
        var v = ""string"" + [||]""string"";
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithStringOnLeft()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"public class C
{
    void M()
    {
        var v = [||]""string"" + 1;
    }
}",
@"public class C
{
    void M()
    {
        var v = $""string{1}"";
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestRightSideOfString()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"public class C
{
    void M()
    {
        var v = ""string""[||] + 1;
    }
}",
@"public class C
{
    void M()
    {
        var v = $""string{1}"";
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithStringOnRight()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"public class C
{
    void M()
    {
        var v = 1 + [||]""string"";
    }
}",
@"public class C
{
    void M()
    {
        var v = $""{1}string"";
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithComplexExpressionOnLeft()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"public class C
{
    void M()
    {
        var v = 1 + 2 + [||]""string"";
    }
}",
@"public class C
{
    void M()
    {
        var v = $""{1 + 2}string"";
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithTrivia1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
public class C
{
    void M()
    {
        var v =
            // Leading trivia
            1 + 2 + [||]""string"" /* trailing trivia */;
    }
}",
@"
public class C
{
    void M()
    {
        var v =
            // Leading trivia
            $""{1 + 2}string"" /* trailing trivia */;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithComplexExpressions()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"public class C
{
    void M()
    {
        var v = 1 + 2 + [||]""string"" + 3 + 4;
    }
}",
@"public class C
{
    void M()
    {
        var v = $""{1 + 2}string{3}{4}"";
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithEscapes1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
public class C
{
    void M()
    {
        var v = ""\r"" + 2 + [||]""string"" + 3 + ""\n"";
    }
}",
@"
public class C
{
    void M()
    {
        var v = $""\r{2}string{3}\n"";
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithEscapes2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
public class C
{
    void M()
    {
        var v = ""\\r"" + 2 + [||]""string"" + 3 + ""\\n"";
    }
}",
@"
public class C
{
    void M()
    {
        var v = $""\\r{2}string{3}\\n"";
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithVerbatimString1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"public class C
{
    void M()
    {
        var v = 1 + [||]@""string"";
    }
}",
@"public class C
{
    void M()
    {
        var v = $@""{1}string"";
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingWithMixedStringTypes1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"public class C
{
    void M()
    {
        var v = 1 + [||]@""string"" + 2 + ""string"";
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingWithMixedStringTypes2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"public class C
{
    void M()
    {
        var v = 1 + @""string"" + 2 + [||]""string"";
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithOverloadedOperator()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"public class D
{
    public static bool operator +(D d, string s) => false;
    public static bool operator +(string s, D d) => false;
}

public class C
{
    void M()
    {
        D d = null;
        var v = 1 + [||]""string"" + d;
    }
}",
@"public class D
{
    public static bool operator +(D d, string s) => false;
    public static bool operator +(string s, D d) => false;
}

public class C
{
    void M()
    {
        D d = null;
        var v = $""{1}string"" + d;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithOverloadedOperator2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"public class D
{
    public static bool operator +(D d, string s) => false;
    public static bool operator +(string s, D d) => false;
}

public class C
{
    void M()
    {
        D d = null;
        var v = d + [||]""string"" + 1;
    }
}");
        }

        [WorkItem(16820, "https://github.com/dotnet/roslyn/issues/16820")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithMultipleStringConcatinations()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"public class C
{
    void M()
    {
        var v = ""A"" + 1 + [||]""B"" + ""C"";
    }
}",
@"public class C
{
    void M()
    {
        var v = $""A{1}BC"";
    }
}");
        }

        [WorkItem(16820, "https://github.com/dotnet/roslyn/issues/16820")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithMultipleStringConcatinations2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"public class C
{
    void M()
    {
        var v = ""A"" + [||]""B"" + ""C"" + 1;
    }
}",
@"public class C
{
    void M()
    {
        var v = $""ABC{1}"";
    }
}");
        }

        [WorkItem(16820, "https://github.com/dotnet/roslyn/issues/16820")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithMultipleStringConcatinations3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"public class C
{
    void M()
    {
        var v = ""A"" + 1 + [||]""B"" + ""C"" + 2 +""D""+ ""E""+ ""F"" + 3;
    }
}",
@"public class C
{
    void M()
    {
        var v = $""A{1}BC{2}DEF{3}"";
    }
}");
        }

        [WorkItem(16820, "https://github.com/dotnet/roslyn/issues/16820")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithMultipleStringConcatinations4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"public class C
{
    void M()
    {
        var v = ""A"" + 1 + [||]""B"" + @""C"";
    }
}");
        }

        [WorkItem(20943, "https://github.com/dotnet/roslyn/issues/20943")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingWithDynamic1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        dynamic a = ""b"";
        string c = [||]""d"" + a + ""e"";
    }
}");
        }

        [WorkItem(20943, "https://github.com/dotnet/roslyn/issues/20943")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingWithDynamic2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        dynamic dynamic = null;
        var x = dynamic.someVal + [||]"" $"";
    }
}");
        }

        [WorkItem(23536, "https://github.com/dotnet/roslyn/issues/23536")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithStringLiteralWithBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            {
                await TestInRegularAndScriptAsync(
    @"public class C
{
    void M()
    {
        var v = 1 + [||]""{string}"";
    }
}",
    @"public class C
{
    void M()
    {
        var v = $""{1}{{string}}"";
    }
}");
            }
        }

        [WorkItem(23536, "https://github.com/dotnet/roslyn/issues/23536")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithStringLiteralWithDoubleBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            {
                await TestInRegularAndScriptAsync(
    @"public class C
{
    void M()
    {
        var v = 1 + [||]""{{string}}"";
    }
}",
    @"public class C
{
    void M()
    {
        var v = $""{1}{{{{string}}}}"";
    }
}");
            }
        }

        [WorkItem(23536, "https://github.com/dotnet/roslyn/issues/23536")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithMultipleStringLiteralsWithBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            {
                await TestInRegularAndScriptAsync(
    @"public class C
{
    void M()
    {
        var v = ""{"" + 1 + [||]""}"";
    }
}",
    @"public class C
{
    void M()
    {
        var v = $""{{{1}}}"";
    }
}");
            }
        }

        [WorkItem(23536, "https://github.com/dotnet/roslyn/issues/23536")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithVerbatimStringWithBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"public class C
{
    void M()
    {
        var v = 1 + [||]@""{string}"";
    }
}",
@"public class C
{
    void M()
    {
        var v = $@""{1}{{string}}"";
    }
}");
        }

        [WorkItem(23536, "https://github.com/dotnet/roslyn/issues/23536")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertToInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithMultipleVerbatimStringsWithBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"public class C
{
    void M()
    {
        var v = @""{"" + 1 + [||]@""}"";
    }
}",
@"public class C
{
    void M()
    {
        var v = $@""{{{1}}}"";
    }
}");
        }
    }
}
