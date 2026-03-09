// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Recommendations
{
    public class AsyncKeywordRecommenderTests : KeywordRecommenderTests
    {
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodDeclaration1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(@"class C
{
    $$
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodDeclaration2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(@"class C
{
    public $$
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodDeclaration3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(@"class C
{
    $$ public void goo() { }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodDeclarationInGlobalStatement1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string text = @"$$";
            await VerifyKeywordAsync(SourceCodeKind.Script, text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodDeclarationInGlobalStatement2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string text = @"public $$";
            await VerifyKeywordAsync(SourceCodeKind.Script, text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExpressionContext()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(@"class C
{
    void goo()
    {
        goo($$
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInParameter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"class C
{
    void goo($$)
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestBeforeLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(@"
class Program
{
    static void Main(string[] args)
    {
        var z =  $$ () => 2;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotIfAlreadyAsync2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"
class Program
{
    static void Main(string[] args)
    {
        var z = async $$ () => 2;
    }
}");
        }

        [WorkItem(578061, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/578061")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"
namespace Goo
{
    $$
}");
        }

        [WorkItem(578069, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/578069")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterPartialInNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"
namespace Goo
{
    partial $$
}");
        }

        [WorkItem(578750, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/578750")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterPartialInClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"
class Goo
{
    partial $$
}");
        }

        [Fact]
        [WorkItem(8616, "https://github.com/dotnet/roslyn/issues/8616")]
        [Test.Utilities.CompilerTrait(Test.Utilities.CompilerFeature.LocalFunctions)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLocalFunction()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(@"
class Goo
{
    public void M()
    {
        $$
    }
}");
        }

        [Fact]
        [WorkItem(14525, "https://github.com/dotnet/roslyn/issues/14525")]
        [Test.Utilities.CompilerTrait(Test.Utilities.CompilerFeature.LocalFunctions)]
        [Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLocalFunction2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(@"
class Goo
{
    public void M()
    {
        unsafe $$
    }
}");
        }

        [Fact]
        [WorkItem(14525, "https://github.com/dotnet/roslyn/issues/14525")]
        [Test.Utilities.CompilerTrait(Test.Utilities.CompilerFeature.LocalFunctions)]
        [Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLocalFunction3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(@"
class Goo
{
    public void M()
    {
        unsafe $$ void L() { }
    }
}");
        }

        [Fact]
        [WorkItem(8616, "https://github.com/dotnet/roslyn/issues/8616")]
        [Test.Utilities.CompilerTrait(Test.Utilities.CompilerFeature.LocalFunctions)]
        [Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLocalFunction4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(@"
class Goo
{
    public void M()
    {
        $$ void L() { }
    }
}");
        }

        [Fact]
        [WorkItem(8616, "https://github.com/dotnet/roslyn/issues/8616")]
        [Test.Utilities.CompilerTrait(Test.Utilities.CompilerFeature.LocalFunctions)]
        [Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLocalFunction5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(@"
class Goo
{
    public void M(Action<int> a)
    {
        M(async () =>
        {
            $$
        });
    }
}");
        }

        [Fact]
        [WorkItem(8616, "https://github.com/dotnet/roslyn/issues/8616")]
        [Test.Utilities.CompilerTrait(Test.Utilities.CompilerFeature.LocalFunctions)]
        [Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLocalFunction6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"
class Goo
{
    public void M()
    {
        int $$
    }
}");
        }

        [Fact]
        [WorkItem(8616, "https://github.com/dotnet/roslyn/issues/8616")]
        [Test.Utilities.CompilerTrait(Test.Utilities.CompilerFeature.LocalFunctions)]
        [Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLocalFunction7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"
class Goo
{
    public void M()
    {
        static $$
    }
}");
        }
    }
}
