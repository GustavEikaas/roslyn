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
    public class XmlDocumentationCommentCompletionProviderTests : AbstractCSharpCompletionProviderTests
    {
        public XmlDocumentationCommentCompletionProviderTests(CSharpTestWorkspaceFixture workspaceFixture) : base(workspaceFixture)
        {
        }

        internal override CompletionProvider CreateCompletionProvider()
        {
            return new XmlDocCommentCompletionProvider();
        }

        private async Task VerifyItemsExistAsync(string markup, params string[] items)
        {
            foreach (var item in items)
            {
                await VerifyItemExistsAsync(markup, item);
            }
        }

        private async Task VerifyItemsAbsentAsync(string markup, params string[] items)
        {
            foreach (var item in items)
            {
                await VerifyItemIsAbsentAsync(markup, item);
            }
        }

        protected override async Task VerifyWorkerAsync(
            string code, int position, string expectedItemOrNull, string expectedDescriptionOrNull,
            SourceCodeKind sourceCodeKind, bool usePreviousCharAsTrigger, bool checkForAbsence,
            int? glyph, int? matchPriority, bool? hasSuggestionItem)
        {
            // We don't need to try writing comments in from of items in doc comments.
            await VerifyAtPositionAsync(code, position, usePreviousCharAsTrigger, expectedItemOrNull, expectedDescriptionOrNull, sourceCodeKind, checkForAbsence, glyph, matchPriority, hasSuggestionItem);
            await VerifyAtEndOfFileAsync(code, position, usePreviousCharAsTrigger, expectedItemOrNull, expectedDescriptionOrNull, sourceCodeKind, checkForAbsence, glyph, matchPriority, hasSuggestionItem);

            // Items cannot be partially written if we're checking for their absence,
            // or if we're verifying that the list will show up (without specifying an actual item)
            if (!checkForAbsence && expectedItemOrNull != null)
            {
                await VerifyAtPosition_ItemPartiallyWrittenAsync(code, position, usePreviousCharAsTrigger, expectedItemOrNull, expectedDescriptionOrNull, sourceCodeKind, checkForAbsence, glyph, matchPriority, hasSuggestionItem);
                await VerifyAtEndOfFile_ItemPartiallyWrittenAsync(code, position, usePreviousCharAsTrigger, expectedItemOrNull, expectedDescriptionOrNull, sourceCodeKind, checkForAbsence, glyph, matchPriority, hasSuggestionItem);
            }
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AlwaysVisibleAtAnyLevelItems1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsExistAsync(@"
public class goo
{
    /// $$
    public void bar() { }
}", "see", "seealso", "![CDATA[", "!--");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AlwaysVisibleAtAnyLevelItems2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsExistAsync(@"
public class goo
{
    /// <summary> $$ </summary>
    public void bar() { }
}", "see", "seealso", "![CDATA[", "!--");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AlwaysVisibleNotTopLevelItems1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsExistAsync(@"
public class goo
{
    /// <summary> $$ </summary>
    public void bar() { }
}", "c", "code", "list", "para");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AlwaysVisibleNotTopLevelItems2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsAbsentAsync(@"
public class goo
{
    /// $$ 
    public void bar() { }
}", "c", "code", "list", "para", "paramref", "typeparamref");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AlwaysVisibleTopLevelOnlyItems1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsExistAsync(@"
public class goo
{
    /// $$ 
    public void bar() { }
}", "exception", "include", "permission");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AlwaysVisibleTopLevelOnlyItems2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsAbsentAsync(@"
public class goo
{
    /// <summary> $$ </summary>
    public void bar() { }
}", "exception", "include", "permission");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TopLevelSingleUseItems1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsExistAsync(@"
public class goo
{
    ///  $$
    public void bar() { }
}", "example", "remarks", "summary");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TopLevelSingleUseItems2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsAbsentAsync(@"
public class goo
{
    ///  <summary> $$ </summary>
    public void bar() { }
}", "example", "remarks", "summary");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TopLevelSingleUseItems3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsAbsentAsync(@"
public class goo
{
    ///  <summary> $$ </summary>
    /// <example></example>
    /// <remarks></remarks>
    
    public void bar() { }
}", "example", "remarks", "summary");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OnlyInListItems()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsAbsentAsync(@"
public class goo
{
    ///  <summary> $$ </summary>
    /// <example></example>
    /// <remarks></remarks>
    
    public void bar() { }
}", "listheader", "item", "term", "description");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OnlyInListItems2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsAbsentAsync(@"
public class goo
{
    ///   $$ 
    
    public void bar() { }
}", "listheader", "item", "term", "description");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OnlyInListItems3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsExistAsync(@"
public class goo
{
    ///   <list>$$</list>
    
    public void bar() { }
}", "listheader", "item", "term", "description");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OnlyInListItems4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsExistAsync(@"
public class goo
{
    ///   <list><$$</list>
    
    public void bar() { }
}", "listheader", "item", "term", "description");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ListHeaderItems()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsExistAsync(@"
public class goo
{
    ///  <summary>
    ///  <list><listheader> $$ </listheader></list>
    ///  </summary>
    /// <example></example>
    /// <remarks></remarks>
    
    public void bar() { }
}", "term", "description");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task VoidMethodDeclarationItems()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemIsAbsentAsync(@"
public class goo
{
    
    /// $$
    public void bar() { }
}", "returns");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MethodReturns()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemExistsAsync(@"
public class goo
{
    
    /// $$
    public int bar() { }
}", "returns");
        }

        [WorkItem(8627, "https://github.com/dotnet/roslyn/issues/8627")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ReadWritePropertyNoReturns()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemIsAbsentAsync(@"
public class goo
{
    
    /// $$
    public int bar { get; set; }
}", "returns");
        }

        [WorkItem(8627, "https://github.com/dotnet/roslyn/issues/8627")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ReadWritePropertyValue()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemExistsAsync(@"
public class goo
{
    
    /// $$
    public int bar { get; set; }
}", "value");
        }

        [WorkItem(8627, "https://github.com/dotnet/roslyn/issues/8627")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ReadOnlyPropertyNoReturns()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemIsAbsentAsync(@"
public class goo
{
    
    /// $$
    public int bar { get; }
}", "returns");
        }

        [WorkItem(8627, "https://github.com/dotnet/roslyn/issues/8627")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ReadOnlyPropertyValue()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemExistsAsync(@"
public class goo
{
    
    /// $$
    public int bar { get; }
}", "value");
        }

        [WorkItem(8627, "https://github.com/dotnet/roslyn/issues/8627")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WriteOnlyPropertyNoReturns()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemIsAbsentAsync(@"
public class goo
{
    
    /// $$
    public int bar { set; }
}", "returns");
        }

        [WorkItem(8627, "https://github.com/dotnet/roslyn/issues/8627")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WriteOnlyPropertyValue()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemExistsAsync(@"
public class goo
{
    
    /// $$
    public int bar { set; }
}", "value");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MethodParamTypeParam()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
public class goo<TGoo>
{
    
    /// $$
    public int bar<TBar>(TBar green) { }
}";

            await VerifyItemsExistAsync(text, "typeparam name=\"TBar\"", "param name=\"green\"");
            await VerifyItemsAbsentAsync(text, "typeparam name=\"TGoo\"");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IndexerParamTypeParam()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsExistAsync(@"
public class goo<T>
{

    /// $$
    public int this[T green] { get { } set { } }
}", "param name=\"green\"");
        }

        [WorkItem(17872, "https://github.com/dotnet/roslyn/issues/17872")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MethodParamRefName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
public class Outer<TOuter>
{
    public class Inner<TInner>
    {
        /// <summary>
        /// $$
        /// </summary>
        public int Method<TMethod>(T green) { }
    }
}";
            await VerifyItemsExistAsync(
                text,
                "typeparamref name=\"TOuter\"",
                "typeparamref name=\"TInner\"",
                "typeparamref name=\"TMethod\"",
                "paramref name=\"green\"");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ClassTypeParamRefName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsExistAsync(@"
/// <summary>
/// $$
/// </summary>
public class goo<T>
{
    public int bar<T>(T green) { }
}", "typeparamref name=\"T\"");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ClassTypeParam()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsExistAsync(@"
/// $$
public class goo<T>
{
    public int bar<T>(T green) { }
}", "typeparam name=\"T\"");
        }

        [WorkItem(638802, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/638802")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TagsAfterSameLineClosedTag()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"/// <summary>
/// <goo></goo>$$
/// 
/// </summary>
";

            await VerifyItemsExistAsync(text, "!--", "![CDATA[", "c", "code", "list", "para", "seealso", "see");
        }

        [WorkItem(734825, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/734825")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EnumMember()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"public enum z
{
    /// <summary>
    /// 
    /// </summary>
    /// <$$
    a
}
";

            await VerifyItemsExistAsync(text);
        }

        [WorkItem(954679, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/954679")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CompletionList()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemExistsAsync(@"
/// $$
public class goo
{
}", "completionlist");
        }

        [WorkItem(775091, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/775091")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ParamRefNames()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemExistsAsync(@"
/// <summary>
/// <paramref name=""$$""/>
/// </summary>
static void Main(string[] args)
{
}
", "args");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ParamNamesInEmptyAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemExistsAsync(@"
/// <param name=""$$""/>
static void Goo(string str)
{
}
", "str");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
        [WorkItem(26713, "https://github.com/dotnet/roslyn/issues/26713")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DelegateParams()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemExistsAsync(@"
/// $$
delegate void D(object o);
", "param name=\"o\"");
        }

        [WorkItem(17872, "https://github.com/dotnet/roslyn/issues/17872")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TypeParamRefNamesInEmptyAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
public class Outer<TOuter>
{
    public class Inner<TInner>
    {
        /// <summary>
        /// <typeparamref name=""$$""/>
        /// </summary>
        public int Method<TMethod>(T green) { }
    }
}";

            await VerifyItemsExistAsync(text, "TOuter", "TInner", "TMethod");
        }

        [WorkItem(17872, "https://github.com/dotnet/roslyn/issues/17872")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TypeParamRefNamesPartiallyTyped()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
public class Outer<TOuter>
{
    public class Inner<TInner>
    {
        /// <summary>
        /// <typeparamref name=""T$$""/>
        /// </summary>
        public int Method<TMethod>(T green) { }
    }
}";

            await VerifyItemsExistAsync(text, "TOuter", "TInner", "TMethod");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TypeParamNamesInEmptyAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
public class Outer<TOuter>
{
    public class Inner<TInner>
    {
        /// <typeparam name=""$$""/>
        public int Method<TMethod>(T green) { }
    }
}";

            await VerifyItemsExistAsync(text, "TMethod");
            await VerifyItemsAbsentAsync(text, "TOuter", "TInner");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TypeParamNamesInWrongScope()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
public class Outer<TOuter>
{
    public class Inner<TInner>
    {
        /// <summary>
        /// <typeparam name=""$$""/>
        /// </summary>
        public int Method<TMethod>(T green) { }
    }
}";

            await VerifyItemsExistAsync(text, "TMethod");
            await VerifyItemsAbsentAsync(text, "TOuter", "TInner");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TypeParamNamesPartiallyTyped()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
public class Outer<TOuter>
{
    public class Inner<TInner>
    {
        /// <typeparam name=""T$$""/>
        public int Method<TMethod>(T green) { }
    }
}";

            await VerifyItemsExistAsync(text, "TMethod");
            await VerifyItemsAbsentAsync(text, "TOuter", "TInner");
        }

        [WorkItem(8322, "https://github.com/dotnet/roslyn/issues/8322")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PartialTagCompletion()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsExistAsync(@"
public class goo
{
    /// <r$$
    public void bar() { }
}", "!--", "![CDATA[", "completionlist", "example", "exception", "include", "permission", "remarks", "see", "seealso", "summary");
        }

        [WorkItem(8322, "https://github.com/dotnet/roslyn/issues/8322")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PartialTagCompletionNestedTags()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsExistAsync(@"
public class goo
{
    /// <summary>
    /// <r$$
    /// </summary>
    public void bar() { }
}", "!--", "![CDATA[", "c", "code", "list", "para", "see", "seealso");
        }

        [WorkItem(11487, "https://github.com/dotnet/roslyn/issues/11487")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TypeParamAtTopLevelOnly()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsAbsentAsync(@"
/// <summary>
/// $$
/// </summary>
public class Goo<T>
{
}", "typeparam name=\"T\"");
        }

        [WorkItem(11487, "https://github.com/dotnet/roslyn/issues/11487")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ParamAtTopLevelOnly()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsAbsentAsync(@"
/// <summary>
/// $$
/// </summary>
static void Goo(string str)
{
}", "param name=\"str\"");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ListAttributeNames()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsExistAsync(@"
class C
{
    /// <summary>
    /// <list $$></list>
    /// </summary>
    static void Goo()
    {
    }
}", "type");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ListTypeAttributeValue()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsExistAsync(@"
class C
{
    /// <summary>
    /// <list type=""$$""></list>
    /// </summary>
    static void Goo()
    {
    }
}", "bullet", "number", "table");
        }

        [WorkItem(11489, "https://github.com/dotnet/roslyn/issues/11490")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SeeAttributeNames()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsExistAsync(@"
class C
{
    /// <summary>
    /// <see $$/>
    /// </summary>
    static void Goo()
    {
    }
}", "cref", "langword");
        }

        [WorkItem(22789, "https://github.com/dotnet/roslyn/issues/22789")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task LangwordCompletionInPlainText()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsExistAsync(@"
class C
{
    /// <summary>
    /// Some text $$
    /// </summary>
    static void Goo()
    {
    }
}", "null", "sealed", "true", "false", "await");
        }

        [WorkItem(22789, "https://github.com/dotnet/roslyn/issues/22789")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task LangwordCompletionAfterAngleBracket1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsAbsentAsync(@"
class C
{
    /// <summary>
    /// Some text <$$
    /// </summary>
    static void Goo()
    {
    }
}", "null", "sealed", "true", "false", "await");
        }

        [WorkItem(22789, "https://github.com/dotnet/roslyn/issues/22789")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task LangwordCompletionAfterAngleBracket2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsAbsentAsync(@"
class C
{
    /// <summary>
    /// Some text <s$$
    /// </summary>
    static void Goo()
    {
    }
}", "null", "sealed", "true", "false", "await");
        }

        [WorkItem(22789, "https://github.com/dotnet/roslyn/issues/22789")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task LangwordCompletionAfterAngleBracket3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsExistAsync(@"
class C
{
    /// <summary>
    /// Some text < $$
    /// </summary>
    static void Goo()
    {
    }
}", "null", "sealed", "true", "false", "await");
        }

        [WorkItem(11490, "https://github.com/dotnet/roslyn/issues/11490")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SeeLangwordAttributeValue()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyItemsExistAsync(@"
class C
{
    /// <summary>
    /// <see langword=""$$""/>
    /// </summary>
    static void Goo()
    {
    }
}", "null", "true", "false", "await");
        }

        [WorkItem(11489, "https://github.com/dotnet/roslyn/issues/11489")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AttributeNameAfterTagNameInIncompleteTag()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class C
{
    /// <exception $$
    static void Goo()
    {
    }
}";
            await VerifyItemExistsAsync(text, "cref", usePreviousCharAsTrigger: true);
        }

        [WorkItem(11489, "https://github.com/dotnet/roslyn/issues/11489")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AttributeNameAfterTagNameInElementStartTag()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class C
{
    /// <exception $$>
    void Goo() { }
}
";
            await VerifyItemExistsAsync(text, "cref");
        }

        [WorkItem(11489, "https://github.com/dotnet/roslyn/issues/11489")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AttributeNameAfterTagNameInEmptyElement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class C
{
    /// <see $$/>
    void Goo() { }
}
";
            await VerifyItemExistsAsync(text, "cref");
        }

        [WorkItem(11489, "https://github.com/dotnet/roslyn/issues/11489")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AttributeNameAfterTagNamePartiallyTyped()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class C
{
    /// <exception c$$
    void Goo() { }
}
";
            await VerifyItemExistsAsync(text, "cref");
        }

        [WorkItem(11489, "https://github.com/dotnet/roslyn/issues/11489")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AttributeNameAfterSpecialCrefAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class C
{
    /// <summary>
    /// <list cref=""String"" $$
    /// </summary>
    void Goo() { }
}
";
            await VerifyItemExistsAsync(text, "type");
        }

        [WorkItem(11489, "https://github.com/dotnet/roslyn/issues/11489")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AttributeNameAfterSpecialNameAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class C
{
    /// <summary>
    /// <list name=""goo"" $$
    /// </summary>
    void Goo() { }
}
";
            await VerifyItemExistsAsync(text, "type");
        }

        [WorkItem(11489, "https://github.com/dotnet/roslyn/issues/11489")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AttributeNameAfterTextAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class C
{
    /// <summary>
    /// <list goo="""" $$
    /// </summary>
    void Goo() { }
}
";
            await VerifyItemExistsAsync(text, "type");
        }

        [WorkItem(11489, "https://github.com/dotnet/roslyn/issues/11489")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AttributeNameInWrongTagTypeEmptyElement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class C
{
    /// <summary>
    /// <list $$/>
    /// </summary>
    void Goo() { }
}
";
            await VerifyItemExistsAsync(text, "type");
        }

        [WorkItem(11489, "https://github.com/dotnet/roslyn/issues/11489")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AttributeNameInWrongTagTypeElementStartTag()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class C
{
    /// <summary>
    /// <see $$>
    /// </summary>
    void Goo() { }
}
";
            await VerifyItemExistsAsync(text, "langword");
        }

        [WorkItem(11489, "https://github.com/dotnet/roslyn/issues/11489")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AttributeValueOnQuote()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class C
{
    /// <summary>
    /// <see langword=""$$
    /// </summary>
    static void Goo()
    {
    }
}";
            await VerifyItemExistsAsync(text, "await", usePreviousCharAsTrigger: true);
        }

        [WorkItem(757, "https://github.com/dotnet/roslyn/issues/757")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TermAndDescriptionInsideItem()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class C
{
    /// <summary>
    ///     <list type=""table"">
    ///         <item>
    ///             $$
    ///         </item>
    ///     </list>
    /// </summary>
    static void Goo()
    {
    }
}";
            await VerifyItemExistsAsync(text, "term");
            await VerifyItemExistsAsync(text, "description");
        }
    }
}
