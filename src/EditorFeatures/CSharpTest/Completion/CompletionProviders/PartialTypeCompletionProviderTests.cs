// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp.Completion.Providers;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Completion.CompletionProviders
{
    public class PartialTypeCompletionProviderTests : AbstractCSharpCompletionProviderTests
    {
        public PartialTypeCompletionProviderTests(CSharpTestWorkspaceFixture workspaceFixture) : base(workspaceFixture)
        {
        }

        internal override CompletionProvider CreateCompletionProvider()
        {
            return new PartialTypeCompletionProvider();
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestRecommendTypesWithoutPartial()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class C { }

partial class $$";

            await VerifyItemIsAbsentAsync(text, "C");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPartialClass1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
partial class C { }

partial class $$";

            await VerifyItemExistsAsync(text, "C");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPartialGenericClass1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Bar { }

partial class C<Bar> { }

partial class $$";

            await VerifyItemExistsAsync(text, "C<Bar>");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPartialGenericClassCommitOnParen()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Bar { }

partial class C<Bar> { }

partial class $$";

            var expected = @"
class Bar { }

partial class C<Bar> { }

partial class C<";

            await VerifyProviderCommitAsync(text, "C<Bar>", expected, '<', "");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPartialGenericClassCommitOnTab()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Bar { }

partial class C<Bar> { }

partial class $$";

            var expected = @"
class Bar { }

partial class C<Bar> { }

partial class C<Bar>";

            await VerifyProviderCommitAsync(text, "C<Bar>", expected, null, "");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPartialGenericClassCommitOnSpace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
partial class C<T> { }

partial class $$";

            var expected = @"
partial class C<T> { }

partial class C<T> ";

            await VerifyProviderCommitAsync(text, "C<T>", expected, ' ', "");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPartialClassWithModifiers()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
partial class C { }

internal partial class $$";

            await VerifyItemExistsAsync(text, "C");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPartialStruct()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
partial struct S { }

partial struct $$";

            await VerifyItemExistsAsync(text, "S");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPartialInterface()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
partial interface I { }

partial interface $$";

            await VerifyItemExistsAsync(text, "I");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTypeKindMatches1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
partial struct S { }

partial class $$";

            await VerifyNoItemsExistAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTypeKindMatches2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
partial class C { }

partial struct $$";

            await VerifyNoItemsExistAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPartialClassesInSameNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
namespace N
{
    partial class Goo { }
}

namespace N
{
    partial class $$
}";

            await VerifyItemExistsAsync(text, "Goo");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotPartialClassesAcrossDifferentNamespaces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
namespace N
{
    partial class Goo { }
}

partial class $$";

            await VerifyNoItemsExistAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotPartialClassesInOuterNamespaces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
partial class C { }

namespace N
{
    partial class $$
}
";

            await VerifyNoItemsExistAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotPartialClassesInOuterClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
partial class C
{
    partial class $$
}
";

            await VerifyNoItemsExistAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestClassWithConstraint()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
partial class C1<T> where T : System.Exception { }

partial class $$";

            var expected = @"
partial class C1<T> where T : System.Exception { }

partial class C1<T>";

            await VerifyProviderCommitAsync(text, "C1<T>", expected, null, "");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDoNotSuggestCurrentMember()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"partial class F$$";

            await VerifyNoItemsExistAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
partial class C1 { }

partial class //$$";

            await VerifyNoItemsExistAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPartialClassWithReservedName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
partial class @class { }

partial class $$";

            var expected = @"
partial class @class { }

partial class @class";

            await VerifyProviderCommitAsync(text, "@class", expected, null, "");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPartialGenericClassWithReservedName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
partial class @class<T> { }

partial class $$";

            var expected = @"
partial class @class<T> { }

partial class @class<T>";

            await VerifyProviderCommitAsync(text, "@class<T>", expected, null, "");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPartialGenericInterfaceWithVariance()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
partial interface I<out T> { }

partial interface $$";

            var expected = @"
partial interface I<out T> { }

partial interface I<out T>";

            await VerifyProviderCommitAsync(text, "I<out T>", expected, null, "");
        }
    }
}
