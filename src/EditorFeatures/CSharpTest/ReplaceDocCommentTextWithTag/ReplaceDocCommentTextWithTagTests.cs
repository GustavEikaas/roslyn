// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp.ReplaceDocCommentTextWithTag;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.CodeRefactorings;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.ReplaceDocCommentTextWithTag
{
    public class ReplaceDocCommentTextWithTagTests : AbstractCSharpCodeActionTest
    {
        protected override CodeRefactoringProvider CreateCodeRefactoringProvider(Workspace workspace, TestParameters parameters)
            => new CSharpReplaceDocCommentTextWithTagCodeRefactoringProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestStartOfKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
/// Testing keyword [||]null.
class C<TKey>
{
}",

@"
/// Testing keyword <see langword=""null""/>.
class C<TKey>
{
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEndOfKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
/// Testing keyword abstract[||].
class C<TKey>
{
}",

@"
/// Testing keyword <see langword=""abstract""/>.
class C<TKey>
{
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEndOfKeyword_NewLineFollowing()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
/// Testing keyword static[||]
class C<TKey>
{
}",

@"
/// Testing keyword <see langword=""static""/>
class C<TKey>
{
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSelectedKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
/// Testing keyword [|abstract|].
class C<TKey>
{
}",

@"
/// Testing keyword <see langword=""abstract""/>.
class C<TKey>
{
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
/// Testing keyword asy[||]nc.
class C<TKey>
{
}",

@"
/// Testing keyword <see langword=""async""/>.
class C<TKey>
{
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInsideKeywordIfNonEmptySpan()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"
/// TKey must implement the System.IDisposable int[|erf|]ace
class C<TKey>
{
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestStartOfFullyQualifiedTypeName_Start()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
/// TKey must implement the [||]System.IDisposable interface.
class C<TKey>
{
}",

@"
/// TKey must implement the <see cref=""System.IDisposable""/> interface.
class C<TKey>
{
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestStartOfFullyQualifiedTypeName_Mid1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
/// TKey must implement the System[||].IDisposable interface.
class C<TKey>
{
}",

@"
/// TKey must implement the <see cref=""System.IDisposable""/> interface.
class C<TKey>
{
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestStartOfFullyQualifiedTypeName_Mid2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
/// TKey must implement the System.[||]IDisposable interface.
class C<TKey>
{
}",

@"
/// TKey must implement the <see cref=""System.IDisposable""/> interface.
class C<TKey>
{
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestStartOfFullyQualifiedTypeName_End()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
/// TKey must implement the System.IDisposable[||] interface.
class C<TKey>
{
}",

@"
/// TKey must implement the <see cref=""System.IDisposable""/> interface.
class C<TKey>
{
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestStartOfFullyQualifiedTypeName_Selected()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
/// TKey must implement the [|System.IDisposable|] interface.
class C<TKey>
{
}",

@"
/// TKey must implement the <see cref=""System.IDisposable""/> interface.
class C<TKey>
{
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTypeParameterReference()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
/// [||]TKey must implement the System.IDisposable interface.
class C<TKey>
{
}",

@"
/// <typeparamref name=""TKey""/> must implement the System.IDisposable interface.
class C<TKey>
{
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTypeParameterReference_EmptyClassBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
/// [||]TKey must implement the System.IDisposable interface.
class C<TKey>{}",

@"
/// <typeparamref name=""TKey""/> must implement the System.IDisposable interface.
class C<TKey>{}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCanSeeInnerMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
/// Use WriteLine[||] as a Console.WriteLine replacement
class C
{
    void WriteLine<TKey>(TKey value) { }
}",

@"
/// Use <see cref=""WriteLine""/> as a Console.WriteLine replacement
class C
{
    void WriteLine<TKey>(TKey value) { }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnMispelledName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"
/// Use WriteLine1[||] as a Console.WriteLine replacement
class C
{
    void WriteLine<TKey>(TKey value) { }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodTypeParameterSymbol()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    /// value has type TKey[||] so we don't box primitives.
    void WriteLine<TKey>(TKey value) { }
}",

@"
class C
{
    /// value has type <typeparamref name=""TKey""/> so we don't box primitives.
    void WriteLine<TKey>(TKey value) { }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodTypeParameterSymbol_EmptyBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    /// value has type TKey[||] so we don't box primitives.
    void WriteLine<TKey>(TKey value){}
}",

@"
class C
{
    /// value has type <typeparamref name=""TKey""/> so we don't box primitives.
    void WriteLine<TKey>(TKey value){}
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodTypeParameterSymbol_ExpressionBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    /// value has type TKey[||] so we don't box primitives.
    object WriteLine<TKey>(TKey value) => null;
}",

@"
class C
{
    /// value has type <typeparamref name=""TKey""/> so we don't box primitives.
    object WriteLine<TKey>(TKey value) => null;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodTypeParameter_SemicolonBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    /// value has type TKey[||] so we don't box primitives.
    void WriteLine<TKey>(TKey value);
}",

@"
class C
{
    /// value has type <typeparamref name=""TKey""/> so we don't box primitives.
    void WriteLine<TKey>(TKey value);
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodParameterSymbol()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    /// value[||] has type TKey so we don't box primitives.
    void WriteLine<TKey>(TKey value) { }
}",

@"
class C
{
    /// <paramref name=""value""/> has type TKey so we don't box primitives.
    void WriteLine<TKey>(TKey value) { }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodParameterSymbol_EmptyBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    /// value[||] has type TKey so we don't box primitives.
    void WriteLine<TKey>(TKey value){}
}",

@"
class C
{
    /// <paramref name=""value""/> has type TKey so we don't box primitives.
    void WriteLine<TKey>(TKey value){}
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodParameterSymbol_ExpressionBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    /// value[||] has type TKey so we don't box primitives.
    object WriteLine<TKey>(TKey value) => null;
}",

@"
class C
{
    /// <paramref name=""value""/> has type TKey so we don't box primitives.
    object WriteLine<TKey>(TKey value) => null;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodParameterSymbol_SemicolonBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    /// value[||] has type TKey so we don't box primitives.
    void WriteLine<TKey>(TKey value);
}",

@"
class C
{
    /// <paramref name=""value""/> has type TKey so we don't box primitives.
    void WriteLine<TKey>(TKey value);
}");
        }

        [WorkItem(22278, "https://github.com/dotnet/roslyn/issues/22278")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNonApplicableKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"
/// Testing keyword interfa[||]ce.
class C<TKey>
{
}");
        }

        [WorkItem(22278, "https://github.com/dotnet/roslyn/issues/22278")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInXMLAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"
/// Testing keyword inside <see langword =""nu[||]ll""/>
class C
{
    void WriteLine<TKey>(TKey value) { }
}");
        }

        [WorkItem(22278, "https://github.com/dotnet/roslyn/issues/22278")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceDocCommentTextWithTag)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInXMLAttribute2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"
/// Testing keyword inside <see langword =""nu[||]ll""
class C
{
    void WriteLine<TKey>(TKey value) { }
}");
        }
    }
}
