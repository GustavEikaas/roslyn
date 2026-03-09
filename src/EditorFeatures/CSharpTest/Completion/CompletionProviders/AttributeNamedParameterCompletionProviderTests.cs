// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp.Completion.Providers;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Completion.CompletionProviders;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Completion.CompletionSetSources
{
    public class AttributeNamedParameterCompletionProviderTests : AbstractCSharpCompletionProviderTests
    {
        public AttributeNamedParameterCompletionProviderTests(CSharpTestWorkspaceFixture workspaceFixture) : base(workspaceFixture)
        {
        }

        internal override CompletionProvider CreateCompletionProvider()
        {
            return new AttributeNamedParameterCompletionProvider();
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SendEnterThroughToEditorTest()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string markup = @"
using System;
class class1
{
    [Test($$
    public void Goo()
    {
    }
}
 
public class TestAttribute : Attribute
{
    public ConsoleColor Color { get; set; }
}";

            await VerifySendEnterThroughToEnterAsync(markup, "Color =", sendThroughEnterOption: EnterKeyRule.Never, expected: false);
            await VerifySendEnterThroughToEnterAsync(markup, "Color =", sendThroughEnterOption: EnterKeyRule.AfterFullyTypedWord, expected: true);
            await VerifySendEnterThroughToEnterAsync(markup, "Color =", sendThroughEnterOption: EnterKeyRule.Always, expected: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CommitCharacterTest()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string markup = @"
using System;
class class1
{
    [Test($$
    public void Goo()
    {
    }
}
 
public class TestAttribute : Attribute
{
    public ConsoleColor Color { get; set; }
}";

            await VerifyCommonCommitCharactersAsync(markup, textTypedSoFar: "");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SimpleAttributeUsage()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System;
class class1
{
    [Test($$
    public void Goo()
    {
    }
}
 
public class TestAttribute : Attribute
{
    public ConsoleColor Color { get; set; }
}";

            await VerifyItemExistsAsync(markup, "Color =");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterComma()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System;
class class1
{
    [Test(Color = ConsoleColor.Black, $$
    public void Goo()
    {
    }
}

public class TestAttribute : Attribute
{
    public ConsoleColor Color { get; set; }
    public string Text { get; set; }
}";

            await VerifyItemExistsAsync(markup, "Text =");
        }

        [WorkItem(544345, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/544345")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExistingItemsAreFiltered()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System;
class class1
{
    [Test(Color = ConsoleColor.Black, $$
    public void Goo()
    {
    }
}

public class TestAttribute : Attribute
{
    public ConsoleColor Color { get; set; }
    public string Text { get; set; }
}";

            await VerifyItemExistsAsync(markup, "Text =");
            await VerifyItemIsAbsentAsync(markup, "Color =");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AttributeConstructor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System;
class TestAttribute : Attribute
{
    public TestAttribute(int a = 42)
    { }
}
 
[Test($$
class Goo
{ }
";

            await VerifyItemExistsAsync(markup, "a:");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AttributeConstructorAfterComma()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System;
class TestAttribute : Attribute
{
    public TestAttribute(int a = 42, string s = """")
    { }
}

[Test(s:"""", $$
class Goo
{ }
";

            await VerifyItemExistsAsync(markup, "a:");
        }

        [WorkItem(545426, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/545426")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPropertiesInScript()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System;

class TestAttribute : Attribute
{
    public string Text { get; set; }
    public TestAttribute(int number = 42)
    {
    }
}
 
[Test($$
class Goo
{
}";

            await VerifyItemExistsAsync(markup, "Text =");
        }

        [WorkItem(1075278, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1075278")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System;
class class1
{
    [Test( //$$
    public void Goo()
    {
    }
}
 
public class TestAttribute : Attribute
{
    public ConsoleColor Color { get; set; }
}";

            await VerifyNoItemsExistAsync(markup);
        }
    }
}
