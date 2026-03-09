// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp.Completion.Providers;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Completion.CompletionProviders
{
    public class ExplicitInterfaceTypeCompletionProviderTests : AbstractCSharpCompletionProviderTests
    {
        public ExplicitInterfaceTypeCompletionProviderTests(CSharpTestWorkspaceFixture workspaceFixture)
            : base(workspaceFixture)
        {
        }

        internal override CompletionProvider CreateCompletionProvider()
            => new ExplicitInterfaceTypeCompletionProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAtStartOfClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections;

class C : IList
{
    int $$
}
";
            await VerifyAnyItemExistsAsync(markup, hasSuggestionModeItem: true);
            await VerifyItemExistsAsync(markup, "IEnumerable");
            await VerifyItemExistsAsync(markup, "ICollection");
            await VerifyItemExistsAsync(markup, "IList");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
        [WorkItem(459044, "https://devdiv.visualstudio.com/DefaultCollection/DevDiv/_workitems?id=459044")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInMisplacedUsing()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    using ($$)
}
";
            await VerifyNoItemsExistAsync(markup); // no crash
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAtStartOfStruct()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections;

struct C : IList
{
    int $$
}
";

            await VerifyAnyItemExistsAsync(markup, hasSuggestionModeItem: true);
            await VerifyItemExistsAsync(markup, "IEnumerable");
            await VerifyItemExistsAsync(markup, "ICollection");
            await VerifyItemExistsAsync(markup, "IList");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections;

class C : IList
{
    int i;
    int $$
}
";

            await VerifyAnyItemExistsAsync(markup, hasSuggestionModeItem: true);
            await VerifyItemExistsAsync(markup, "IEnumerable");
            await VerifyItemExistsAsync(markup, "ICollection");
            await VerifyItemExistsAsync(markup, "IList");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections;

class C : IList
{
    void Goo() { }
    int $$
}
";

            await VerifyAnyItemExistsAsync(markup, hasSuggestionModeItem: true);
            await VerifyItemExistsAsync(markup, "IEnumerable");
            await VerifyItemExistsAsync(markup, "ICollection");
            await VerifyItemExistsAsync(markup, "IList");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterExpressionBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections;

class C : IList
{
    int Goo() => 0;
    int $$
}
";

            await VerifyAnyItemExistsAsync(markup, hasSuggestionModeItem: true);
            await VerifyItemExistsAsync(markup, "IEnumerable");
            await VerifyItemExistsAsync(markup, "ICollection");
            await VerifyItemExistsAsync(markup, "IList");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithAttributeFollowing()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections;

class C : IList
{
    int Goo() => 0;
    int $$

    [Attr]
    int Bar();
}
";

            await VerifyAnyItemExistsAsync(markup, hasSuggestionModeItem: true);
            await VerifyItemExistsAsync(markup, "IEnumerable");
            await VerifyItemExistsAsync(markup, "ICollection");
            await VerifyItemExistsAsync(markup, "IList");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithModifierFollowing()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections;

class C : IList
{
    int Goo() => 0;
    int $$

    public int Bar();
}
";

            await VerifyAnyItemExistsAsync(markup, hasSuggestionModeItem: true);
            await VerifyItemExistsAsync(markup, "IEnumerable");
            await VerifyItemExistsAsync(markup, "ICollection");
            await VerifyItemExistsAsync(markup, "IList");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithTypeFollowing()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections;

class C : IList
{
    int Goo() => 0;
    int $$

    int Bar();
}
";

            await VerifyAnyItemExistsAsync(markup, hasSuggestionModeItem: true);
            await VerifyItemExistsAsync(markup, "IEnumerable");
            await VerifyItemExistsAsync(markup, "ICollection");
            await VerifyItemExistsAsync(markup, "IList");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithTypeFollowing2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections;

class C : IList
{
    int Goo() => 0;
    int $$

    X Bar();
}
";

            await VerifyAnyItemExistsAsync(markup, hasSuggestionModeItem: true);
            await VerifyItemExistsAsync(markup, "IEnumerable");
            await VerifyItemExistsAsync(markup, "ICollection");
            await VerifyItemExistsAsync(markup, "IList");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInMember()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections;

class C : IList
{
    void Goo()
    {
        int $$
    }
}
";

            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotWithAccessibility()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections;

class C : IList
{
    public int $$
}
";

            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInInterface()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections;

interface I : IList
{
    int $$
}
";

            await VerifyNoItemsExistAsync(markup);
        }
    }
}
