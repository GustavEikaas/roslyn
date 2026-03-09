// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp.Completion.Providers;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Completion.CompletionProviders;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Completion.CompletionSetSources
{
    public class TupleNameCompletionProviderTests : AbstractCSharpCompletionProviderTests
    {
        public TupleNameCompletionProviderTests(CSharpTestWorkspaceFixture workspaceFixture) : base(workspaceFixture)
        {
        }

        internal override CompletionProvider CreateCompletionProvider() => new TupleNameCompletionProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterOpenParen()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemExistsAsync(@"
class Program
{
    static void Main(string[] args)
    {
        (int word, int zword) t = ($$
    }
}", "word:");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterOpenParenWithBraceCompletion()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemExistsAsync(@"
class Program
{
    static void Main(string[] args)
    {
        (int word, int zword) t = ($$)
    }
}", "word:");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterOpenParenInTupleExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemExistsAsync(@"
class Program
{
    static void Main(string[] args)
    {
        (int word, int zword) t = ($$, zword: 2
    }
}", "word:");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterOpenParenInTupleExpressionWithBraceCompletion()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemExistsAsync(@"
class Program
{
    static void Main(string[] args)
    {
        (int word, int zword) t = ($$, zword: 2
    }
}", "word:");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterComma()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemExistsAsync(@"
class Program
{
    static void Main(string[] args)
    {
        (int word, int zword) t = (1, $$
    }
}", "zword:");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterCommaWithBraceCompletion()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemExistsAsync(@"
class Program
{
    static void Main(string[] args)
    {
        (int word, int zword) t = (1, $$)
    }
}", "zword:");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InTupleAsArgument()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemExistsAsync(@"
class Program
{
    static void Main((int word, int zword) args)
    {
         Main(($$))
    }
}", "word:");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MultiplePossibleTuples()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class Program
{
    static void Main((int number, int znumber) args) { }
    static void Main((string word, int zword) args) {
        Main(($$
    }
}";
            await VerifyItemExistsAsync(markup, "word:");
            await VerifyItemExistsAsync(markup, "number:");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MultiplePossibleTuplesAfterComma()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class Program
{
    static void Main((int number, int znumber) args) { }
    static void Main((string word, int zword) args) {
        Main((1, $$
    }
}";
            await VerifyItemExistsAsync(markup, "zword:");
            await VerifyItemExistsAsync(markup, "znumber:");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AtIndexGreaterThanNumberOfTupleElements()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class Program
{
    static void Main(string[] args)
    {
        (int word, int zword) t = (1, 2, 3, 4, $$ 
    }
}";
            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ConvertCastToTupleExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    void goo()
    {
        (int goat, int moat) x = (g$$)1;
    }
}";
            await VerifyItemExistsAsync(markup, "goat:");
        }
    }
}
