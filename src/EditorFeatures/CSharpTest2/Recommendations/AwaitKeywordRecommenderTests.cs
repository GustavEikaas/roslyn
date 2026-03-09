// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Recommendations
{
    public class AwaitKeywordRecommenderTests : KeywordRecommenderTests
    {
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInTypeContext()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"
class Program
{
    $$
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInSynchronousMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(@"
class Program
{
    void goo()
    {
        $$
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestStatementInAsyncMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(@"
class Program
{
    async void goo()
    {
        $$
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExpressionInAsyncMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(@"
class Program
{
    async void goo()
    {
        var z = $$
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInQuery()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"
class Program
{
    async void goo()
    {
        var z = from a in ""char""
                select $$
    }
}");
        }

        [WorkItem(907052, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/907052")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInFinally()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(@"
class Program
{
    async void goo()
    {
        try { }
        finally { $$ } 
    }
}");
        }

        [WorkItem(907052, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/907052")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInCatch()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(@"
class Program
{
    async void goo()
    {
        try { }
        catch { $$ } 
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInLock()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"
class Program
{
    async void goo()
    {
       lock(this) { $$ } 
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInAsyncLambdaInCatch()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(@"
class Program
{
    async void goo()
    {
        try { }
        catch { var z = async () => $$ } 
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAwaitInLock()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(@"
class Program
{
    async void goo()
    {
        lock($$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInGlobalStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string text = @"$$";
            await VerifyKeywordAsync(SourceCodeKind.Script, text);
        }
    }
}
