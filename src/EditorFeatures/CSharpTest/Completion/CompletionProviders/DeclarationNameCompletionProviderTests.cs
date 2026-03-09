// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Completion.Providers;
using Microsoft.CodeAnalysis.Diagnostics.Analyzers.NamingStyles;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Completion.CompletionProviders;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.NamingStyles;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Simplification;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;
using static Microsoft.CodeAnalysis.Diagnostics.Analyzers.NamingStyles.SymbolSpecification;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Completion.CompletionSetSources
{
    public class DeclarationNameCompletionProviderTests : AbstractCSharpCompletionProviderTests
    {
        public DeclarationNameCompletionProviderTests(CSharpTestWorkspaceFixture workspaceFixture) : base(workspaceFixture)
        {
        }

        internal override CompletionProvider CreateCompletionProvider()
        {
            return new DeclarationNameCompletionProvider();
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NameWithOnlyType1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
public class MyClass
{
    MyClass $$
}
";
            await VerifyItemExistsAsync(markup, "myClass", glyph: (int)Glyph.FieldPublic);
            await VerifyItemExistsAsync(markup, "MyClass", glyph: (int)Glyph.PropertyPublic);
            await VerifyItemExistsAsync(markup, "GetMyClass", glyph: (int)Glyph.MethodPublic);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AsyncTaskOfT()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading.Tasks;
public class C
{
    async Task<C> $$
}
";
            await VerifyItemExistsAsync(markup, "GetCAsync");
        }

        [Fact(Skip = "not yet implemented"), Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NonAsyncTaskOfT()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
public class C
{
    Task<C> $$
}
";
            await VerifyItemExistsAsync(markup, "GetCAsync");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MethodDeclaration1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
public class C
{
    virtual C $$
}
";
            await VerifyItemExistsAsync(markup, "GetC");
            await VerifyItemIsAbsentAsync(markup, "C");
            await VerifyItemIsAbsentAsync(markup, "c");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WordBreaking1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;
public class C
{
    CancellationToken $$
}
";
            await VerifyItemExistsAsync(markup, "cancellationToken");
            await VerifyItemExistsAsync(markup, "cancellation");
            await VerifyItemExistsAsync(markup, "token");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WordBreaking2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
interface I {}
public class C
{
    I $$
}
";
            await VerifyItemExistsAsync(markup, "GetI");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WordBreaking3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
interface II {}
public class C
{
    II $$
}
";
            await VerifyItemExistsAsync(markup, "GetI");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WordBreaking4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
interface IGoo {}
public class C
{
    IGoo $$
}
";
            await VerifyItemExistsAsync(markup, "Goo");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WordBreaking5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class SomeWonderfullyLongClassName {}
public class C
{
    SomeWonderfullyLongClassName $$
}
";
            await VerifyItemExistsAsync(markup, "Some");
            await VerifyItemExistsAsync(markup, "SomeWonderfully");
            await VerifyItemExistsAsync(markup, "SomeWonderfullyLong");
            await VerifyItemExistsAsync(markup, "SomeWonderfullyLongClass");
            await VerifyItemExistsAsync(markup, "Name");
            await VerifyItemExistsAsync(markup, "ClassName");
            await VerifyItemExistsAsync(markup, "LongClassName");
            await VerifyItemExistsAsync(markup, "WonderfullyLongClassName");
            await VerifyItemExistsAsync(markup, "SomeWonderfullyLongClassName");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Parameter1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;
public class C
{
    void Goo(CancellationToken $$
}
";
            await VerifyItemExistsAsync(markup, "cancellationToken", glyph: (int)Glyph.Parameter);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Parameter2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;
public class C
{
    void Goo(int x, CancellationToken c$$
}
";
            await VerifyItemExistsAsync(markup, "cancellationToken", glyph: (int)Glyph.Parameter);
        }


        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Parameter3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;
public class C
{
    void Goo(CancellationToken c$$) {}
}
";
            await VerifyItemExistsAsync(markup, "cancellationToken", glyph: (int)Glyph.Parameter);
        }

        [WorkItem(19260, "https://github.com/dotnet/roslyn/issues/19260")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EscapeKeywords1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Text;
public class C
{
    void Goo(StringBuilder $$) {}
}
";
            await VerifyItemExistsAsync(markup, "stringBuilder", glyph: (int)Glyph.Parameter);
            await VerifyItemExistsAsync(markup, "@string", glyph: (int)Glyph.Parameter);
            await VerifyItemExistsAsync(markup, "builder", glyph: (int)Glyph.Parameter);
        }

        [WorkItem(19260, "https://github.com/dotnet/roslyn/issues/19260")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EscapeKeywords2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class For { }
public class C
{
    void Goo(For $$) {}
}
";
            await VerifyItemExistsAsync(markup, "@for", glyph: (int)Glyph.Parameter);
        }

        [WorkItem(19260, "https://github.com/dotnet/roslyn/issues/19260")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EscapeKeywords3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class For { }
public class C
{
    void goo()
    {
        For $$
    }
}
";
            await VerifyItemExistsAsync(markup, "@for");
        }

        [WorkItem(19260, "https://github.com/dotnet/roslyn/issues/19260")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EscapeKeywords4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Text;
public class C
{
    void goo()
    {
        StringBuilder $$
    }
}
";
            await VerifyItemExistsAsync(markup, "stringBuilder");
            await VerifyItemExistsAsync(markup, "@string");
            await VerifyItemExistsAsync(markup, "builder");
        }

        [WorkItem(25214, "https://github.com/dotnet/roslyn/issues/25214")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TypeImplementsLazyOfType1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System;
using System.Collections.Generic;

internal class Example
{
    public Lazy<Item> $$
}

public class Item { }
";
            await VerifyItemExistsAsync(markup, "item");
            await VerifyItemExistsAsync(markup, "Item");
            await VerifyItemExistsAsync(markup, "GetItem");
        }

        [WorkItem(25214, "https://github.com/dotnet/roslyn/issues/25214")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TypeImplementsLazyOfType2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System;
using System.Collections.Generic;

internal class Example
{
    public List<Lazy<Item>> $$
}

public class Item { }
";
            await VerifyItemExistsAsync(markup, "items");
            await VerifyItemExistsAsync(markup, "Items");
            await VerifyItemExistsAsync(markup, "GetItems");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoSuggestionsForInt()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;
public class C
{
    int $$
}
";
            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoSuggestionsForLong()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;
public class C
{
    long $$
}
";
            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoSuggestionsForDouble()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;
public class C
{
    double $$
}
";
            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoSuggestionsForFloat()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;
public class C
{
    float $$
}
";
            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoSuggestionsForSbyte()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;
public class C
{
    sbyte $$
}
";
            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoSuggestionsForShort()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;
public class C
{
    short $$
}
";
            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoSuggestionsForUint()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;
public class C
{
    uint $$
}
";
            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoSuggestionsForUlong()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;
public class C
{
    ulong $$
}
";
            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestionsForUShort()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;
public class C
{
    ushort $$
}
";
            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoSuggestionsForBool()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;
public class C
{
    bool $$
}
";
            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoSuggestionsForByte()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;
public class C
{
    byte $$
}
";
            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoSuggestionsForChar()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;
public class C
{
    char $$
}
";
            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoSuggestionsForString()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
public class C
{
    string $$
}
";
            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoSingleLetterClassNameSuggested()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
public class C
{
    C $$
}
";
            await VerifyItemIsAbsentAsync(markup, "C");
            await VerifyItemIsAbsentAsync(markup, "c");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ArrayElementTypeSuggested()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;
public class MyClass
{
    MyClass[] $$
}
";
            await VerifyItemExistsAsync(markup, "MyClasses");
            await VerifyItemIsAbsentAsync(markup, "Array");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotTriggeredByVar()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
public class C
{
    var $$
}
";
            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotAfterVoid()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
public class C
{
    void $$
}
";
            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterGeneric()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
public class C
{
    System.Collections.Generic.IEnumerable<C> $$
}
";
            await VerifyItemExistsAsync(markup, "GetCs");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NothingAfterVar()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
public class C
{
    void goo()
    {
        var $$
    }
}
";
            await VerifyNoItemsExistAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCorrectOrder()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
public class MyClass
{
    MyClass $$
}
";
            var items = await GetCompletionItemsAsync(markup, SourceCodeKind.Regular);
            Assert.Equal(
                new[] { "myClass", "my", "@class", "MyClass", "My", "Class", "GetMyClass", "GetMy", "GetClass" },
                items.Select(item => item.DisplayText));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDescriptionInsideClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
public class MyClass
{
    MyClass $$
}
";
            await VerifyItemExistsAsync(markup, "myClass", glyph: (int)Glyph.FieldPublic, expectedDescriptionOrNull: CSharpFeaturesResources.Suggested_name);
            await VerifyItemExistsAsync(markup, "MyClass", glyph: (int)Glyph.PropertyPublic, expectedDescriptionOrNull: CSharpFeaturesResources.Suggested_name);
            await VerifyItemExistsAsync(markup, "GetMyClass", glyph: (int)Glyph.MethodPublic, expectedDescriptionOrNull: CSharpFeaturesResources.Suggested_name);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDescriptionInsideMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
public class MyClass
{
    void M()
    {
        MyClass $$
    }
}
";
            await VerifyItemExistsAsync(markup, "myClass", glyph: (int)Glyph.Local, expectedDescriptionOrNull: CSharpFeaturesResources.Suggested_name);
            await VerifyItemIsAbsentAsync(markup, "MyClass");
            await VerifyItemIsAbsentAsync(markup, "GetMyClass");
        }

        [WorkItem(20273, "https://github.com/dotnet/roslyn/issues/20273")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Alias1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using MyType = System.String;
public class C
{
    MyType $$
}
";
            await VerifyItemExistsAsync(markup, "my");
            await VerifyItemExistsAsync(markup, "type");
            await VerifyItemExistsAsync(markup, "myType");
        }
        [WorkItem(20273, "https://github.com/dotnet/roslyn/issues/20273")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AliasWithInterfacePattern()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using IMyType = System.String;
public class C
{
    MyType $$
}
";
            await VerifyItemExistsAsync(markup, "my");
            await VerifyItemExistsAsync(markup, "type");
            await VerifyItemExistsAsync(markup, "myType");
        }

        [WorkItem(20016, "https://github.com/dotnet/roslyn/issues/20016")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotAfterExistingName1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using IMyType = System.String;
public class C
{
    MyType myType $$
}
";
            await VerifyNoItemsExistAsync(markup);
        }

        [WorkItem(20016, "https://github.com/dotnet/roslyn/issues/20016")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotAfterExistingName2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using IMyType = System.String;
public class C
{
    MyType myType, MyType $$
}
";
            await VerifyNoItemsExistAsync(markup);
        }

        [WorkItem(19409, "https://github.com/dotnet/roslyn/issues/19409")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OutVarArgument()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class Test
{
    void Do(out Test goo)
    {
        Do(out var $$
    }
}
";
            await VerifyItemExistsAsync(markup, "test");
        }

        [WorkItem(19409, "https://github.com/dotnet/roslyn/issues/19409")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OutArgument()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class Test
{
    void Do(out Test goo)
    {
        Do(out Test $$
    }
}
";
            await VerifyItemExistsAsync(markup, "test");
        }

        [WorkItem(19409, "https://github.com/dotnet/roslyn/issues/19409")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OutGenericArgument()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class Test
{
    void Do<T>(out T goo)
    {
        Do(out Test $$
    }
}
";
            await VerifyItemExistsAsync(markup, "test");
        }


        [WorkItem(22342, "https://github.com/dotnet/roslyn/issues/22342")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleExpressionDeclaration1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class Test
{
    void Do()
    {
        (System.Array array, System.Action $$ 
    }
}
";
            await VerifyItemExistsAsync(markup, "action");
        }

        [WorkItem(22342, "https://github.com/dotnet/roslyn/issues/22342")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleExpressionDeclaration2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class Test
{
    void Do()
    {
        (array, action $$
    }
}
";
            await VerifyItemIsAbsentAsync(markup, "action");
        }

        [WorkItem(22342, "https://github.com/dotnet/roslyn/issues/22342")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleExpressionDeclaration_NestedTuples()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class Test
{
    void Do()
    {
        ((int i1, int i2), (System.Array array, System.Action $$
    }
}
";
            await VerifyItemExistsAsync(markup, "action");
        }

        [WorkItem(22342, "https://github.com/dotnet/roslyn/issues/22342")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleExpressionDeclaration_NestedTuples_CompletionInTheMiddle()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class Test
{
    void Do()
    {
        ((System.Array array, System.Action $$), (int i1, int i2))
    }
}
";
            await VerifyItemExistsAsync(markup, "action");
        }

        [WorkItem(22342, "https://github.com/dotnet/roslyn/issues/22342")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleElementDefinition1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class Test
{
    void Do()
    {
        (System.Array $$
    }
}
";
            await VerifyItemExistsAsync(markup, "array");
        }

        [WorkItem(22342, "https://github.com/dotnet/roslyn/issues/22342")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleElementDefinition2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class Test
{
    (System.Array $$) Test() => default;
}
";
            await VerifyItemExistsAsync(markup, "array");
        }

        [WorkItem(22342, "https://github.com/dotnet/roslyn/issues/22342")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleElementDefinition3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class Test
{
    (System.Array array, System.Action $$) Test() => default;
}
";
            await VerifyItemExistsAsync(markup, "action");
        }

        [WorkItem(22342, "https://github.com/dotnet/roslyn/issues/22342")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleElementDefinition4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class Test
{
    (System.Array $$
}
";
            await VerifyItemExistsAsync(markup, "array");
        }

        [WorkItem(22342, "https://github.com/dotnet/roslyn/issues/22342")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleElementDefinition5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class Test
{
    void M((System.Array $$
}
";
            await VerifyItemExistsAsync(markup, "array");
        }

        [WorkItem(22342, "https://github.com/dotnet/roslyn/issues/22342")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleElementDefinition_NestedTuples()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class Test
{
    void M(((int, int), (int, System.Array $$
}
";
            await VerifyItemExistsAsync(markup, "array");
        }

        [WorkItem(22342, "https://github.com/dotnet/roslyn/issues/22342")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleElementDefinition_InMiddleOfTuple()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class Test
{
    void M((int, System.Array $$),int)
}
";
            await VerifyItemExistsAsync(markup, "array");
        }

        [WorkItem(22342, "https://github.com/dotnet/roslyn/issues/22342")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleElementTypeInference()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class Test
{
    void Do()
    {
        (var accessViolationException, var $$) = (new AccessViolationException(), new Action(() => { }));
    }
}
";
            // Currently not supported:
            await VerifyItemIsAbsentAsync(markup, "action");
            // see https://github.com/dotnet/roslyn/issues/27138
            // after the issue ist fixed we expect this to work:
            // await VerifyItemExistsAsync(markup, "action");
        }

        [WorkItem(22342, "https://github.com/dotnet/roslyn/issues/22342")]
        [Fact(Skip = "Not yet supported"), Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleElementInGenericTypeArgument()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class Test
{
    void Do()
    {
        System.Func<(System.Action $$
    }
}
";
            await VerifyItemExistsAsync(markup, "action");
        }

        [WorkItem(22342, "https://github.com/dotnet/roslyn/issues/22342")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleElementInvocationInsideTuple()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class Test
{
    void Do()
    {
            int M(int i1, int i2) => i1;
            var t=(e1: 1, e2: M(1, $$));
    }
}
";
            await VerifyNoItemsExistAsync(markup);
        }

        [WorkItem(17987, "https://github.com/dotnet/roslyn/issues/17987")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Pluralize1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections.Generic;
class Index
{
    IEnumerable<Index> $$
}
";
            await VerifyItemExistsAsync(markup, "Indices");
        }

        [WorkItem(17987, "https://github.com/dotnet/roslyn/issues/17987")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Pluralize2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections.Generic;
class Test
{
    IEnumerable<IEnumerable<Test>> $$
}
";
            await VerifyItemExistsAsync(markup, "tests");
        }

        [WorkItem(17987, "https://github.com/dotnet/roslyn/issues/17987")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Pluralize3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections.Generic;
using System.Threading;
class Test
{
    IEnumerable<CancellationToken> $$
}
";
            await VerifyItemExistsAsync(markup, "cancellationTokens");
            await VerifyItemExistsAsync(markup, "cancellations");
            await VerifyItemExistsAsync(markup, "tokens");
        }

        [WorkItem(17987, "https://github.com/dotnet/roslyn/issues/17987")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PluralizeList()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections.Generic;
using System.Threading;
class Test
{
    List<CancellationToken> $$
}
";
            await VerifyItemExistsAsync(markup, "cancellationTokens");
            await VerifyItemExistsAsync(markup, "cancellations");
            await VerifyItemExistsAsync(markup, "tokens");
        }

        [WorkItem(17987, "https://github.com/dotnet/roslyn/issues/17987")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PluralizeArray()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections.Generic;
using System.Threading;
class Test
{
    CancellationToken[] $$
}
";
            await VerifyItemExistsAsync(markup, "cancellationTokens");
            await VerifyItemExistsAsync(markup, "cancellations");
            await VerifyItemExistsAsync(markup, "tokens");
        }

        [WorkItem(23497, "https://github.com/dotnet/roslyn/issues/23497")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InPatternMatching1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;

public class C
{
    public static void Main()
    {
        object obj = null;
        if (obj is CancellationToken $$) { }
    }
}
";
            await VerifyItemExistsAsync(markup, "cancellationToken");
            await VerifyItemExistsAsync(markup, "cancellation");
            await VerifyItemExistsAsync(markup, "token");
        }

        [WorkItem(23497, "https://github.com/dotnet/roslyn/issues/23497")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InPatternMatching2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;

public class C
{
    public static bool Foo()
    {
        object obj = null;
        return obj is CancellationToken $$
    }
}
";
            await VerifyItemExistsAsync(markup, "cancellationToken");
            await VerifyItemExistsAsync(markup, "cancellation");
            await VerifyItemExistsAsync(markup, "token");
        }

        [WorkItem(23497, "https://github.com/dotnet/roslyn/issues/23497")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InPatternMatching3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;

public class C
{
    public static void Main()
    {
        object obj = null;
        switch(obj)
        {
            case CancellationToken $$
        }
    }
}
";
            await VerifyItemExistsAsync(markup, "cancellationToken");
            await VerifyItemExistsAsync(markup, "cancellation");
            await VerifyItemExistsAsync(markup, "token");
        }

        [WorkItem(23497, "https://github.com/dotnet/roslyn/issues/23497")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InPatternMatching4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;

public class C
{
    public static void Main()
    {
        object obj = null;
        if (obj is CancellationToken ca$$) { }
    }
}
";
            await VerifyItemExistsAsync(markup, "cancellationToken");
            await VerifyItemExistsAsync(markup, "cancellation");
        }

        [WorkItem(23497, "https://github.com/dotnet/roslyn/issues/23497")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InPatternMatching5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;

public class C
{
    public static bool Foo()
    {
        object obj = null;
        return obj is CancellationToken to$$
    }
}
";
            await VerifyItemExistsAsync(markup, "cancellationToken");
            await VerifyItemExistsAsync(markup, "token");
        }

        [WorkItem(23497, "https://github.com/dotnet/roslyn/issues/23497")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InPatternMatching6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading;

public class C
{
    public static void Main()
    {
        object obj = null;
        switch(obj)
        {
            case CancellationToken to$$
        }
    }
}
";
            await VerifyItemExistsAsync(markup, "cancellationToken");
            await VerifyItemExistsAsync(markup, "token");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InUsingStatement1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.IO;

class C
{
    void M()
    {
        using (StreamReader s$$
    }
}
";
            await VerifyItemExistsAsync(markup, "streamReader");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InUsingStatement2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.IO;

class C
{
    void M()
    {
        using (StreamReader s1, $$
    }
}
";
            await VerifyItemExistsAsync(markup, "streamReader");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InUsingStatement_Var()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.IO;

class C
{
    void M()
    {
        using (var m$$ = new MemoryStream())
    }
}
";
            await VerifyItemExistsAsync(markup, "memoryStream");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InForStatement1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.IO;

class C
{
    void M()
    {
        for (StreamReader s$$
    }
}
";
            await VerifyItemExistsAsync(markup, "streamReader");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InForStatement2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.IO;

class C
{
    void M()
    {
        for (StreamReader s1, $$
    }
}
";
            await VerifyItemExistsAsync(markup, "streamReader");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InForStatement_Var()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.IO;

class C
{
    void M()
    {
        for (var m$$ = new MemoryStream();
    }
}
";
            await VerifyItemExistsAsync(markup, "memoryStream");
        }

        [WorkItem(26021, "https://github.com/dotnet/roslyn/issues/26021")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InForEachStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.IO;

class C
{
    void M()
    {
        foreach (StreamReader $$
    }
}
";
            await VerifyItemExistsAsync(markup, "streamReader");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InForEachStatement_Var()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.IO;

class C
{
    void M()
    {
        foreach (var m$$ in new[] { new MemoryStream() })
    }
}
";
            await VerifyItemExistsAsync(markup, "memoryStream");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DisabledByOption()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var workspace = WorkspaceFixture.GetWorkspace();
            var originalOptions = WorkspaceFixture.GetWorkspace().Options;
            try
            {
                workspace.Options = originalOptions.
                    WithChangedOption(CompletionOptions.ShowNameSuggestions, LanguageNames.CSharp, false);

                var markup = @"
class Test
{
    Test $$
}
";
                await VerifyNoItemsExistAsync(markup);
            }
            finally
            {
                workspace.Options = originalOptions;
            }
        }

        [WorkItem(23590, "https://github.com/dotnet/roslyn/issues/23590")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TypeImplementsIEnumerableOfType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections.Generic;

public class Class1
{
  public void Method()
  {
    Container $$
  }
}

public class Container : ContainerBase { }
public class ContainerBase : IEnumerable<ContainerBase> { }
";
            await VerifyItemExistsAsync(markup, "container");
        }

        [WorkItem(23590, "https://github.com/dotnet/roslyn/issues/23590")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TypeImplementsIEnumerableOfType2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections.Generic;

public class Class1
{
  public void Method()
  {
     Container $$
  }
}

public class ContainerBase : IEnumerable<Container> { }
public class Container : ContainerBase { }
";
            await VerifyItemExistsAsync(markup, "container");
        }

        [WorkItem(23590, "https://github.com/dotnet/roslyn/issues/23590")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TypeImplementsIEnumerableOfType3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections.Generic;

public class Class1
{
  public void Method()
  {
     Container $$
  }
}

public class Container : IEnumerable<Container> { }
";
            await VerifyItemExistsAsync(markup, "container");
        }

        [WorkItem(23590, "https://github.com/dotnet/roslyn/issues/23590")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TypeImplementsIEnumerableOfType4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections.Generic;
using System.Threading.Tasks;

public class Class1
{
  public void Method()
  {
     TaskType $$
  }
}

public class ContainerBase : IEnumerable<Container> { }
public class Container : ContainerBase { }
public class TaskType : Task<Container> { }
";
            await VerifyItemExistsAsync(markup, "taskType");
        }

        [WorkItem(23590, "https://github.com/dotnet/roslyn/issues/23590")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TypeImplementsTaskOfType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading.Tasks;

public class Class1
{
  public void Method()
  {
    Container $$
  }
}

public class Container : ContainerBase { }
public class ContainerBase : Task<ContainerBase> { }
";
            await VerifyItemExistsAsync(markup, "container");
        }

        [WorkItem(23590, "https://github.com/dotnet/roslyn/issues/23590")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TypeImplementsTaskOfType2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Threading.Tasks;

public class Class1
{
  public void Method()
  {
     Container $$
  }
}

public class Container : Task<ContainerBase> { }
public class ContainerBase : Container { }
";
            await VerifyItemExistsAsync(markup, "container");
        }

        [WorkItem(23590, "https://github.com/dotnet/roslyn/issues/23590")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TypeImplementsTaskOfType3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections.Generic;
using System.Threading.Tasks;

public class Class1
{
  public void Method()
  {
    EnumerableType $$
  }
}

public class TaskType : TaskTypeBase { }
public class TaskTypeBase : Task<TaskTypeBase> { }
public class EnumerableType : IEnumerable<TaskType> { }
";
            await VerifyItemExistsAsync(markup, "taskTypes");
        }

        [WorkItem(23590, "https://github.com/dotnet/roslyn/issues/23590")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TypeIsNullableOfNullable()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections.Generic;

public class Class1
{
  public void Method()
  {
      // This code isn't legal, but we want to ensure we don't crash in this broken code scenario
      IEnumerable<Nullable<int?>> $$
  }
}
";
            await VerifyItemExistsAsync(markup, "nullables");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CustomNamingStyleInsideClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var workspace = WorkspaceFixture.GetWorkspace();
            var originalOptions = workspace.Options;

            try
            {
                workspace.Options = workspace.Options.WithChangedOption(
                    new OptionKey(SimplificationOptions.NamingPreferences, LanguageNames.CSharp),
                    NamesEndWithSuffixPreferences());

                var markup = @"
class Configuration
{
    Configuration $$
}
";
                await VerifyItemExistsAsync(markup, "ConfigurationField", glyph: (int)Glyph.FieldPublic,
                    expectedDescriptionOrNull: CSharpFeaturesResources.Suggested_name);
                await VerifyItemExistsAsync(markup, "ConfigurationProperty", glyph: (int)Glyph.PropertyPublic,
                    expectedDescriptionOrNull: CSharpFeaturesResources.Suggested_name);
                await VerifyItemExistsAsync(markup, "ConfigurationMethod", glyph: (int)Glyph.MethodPublic,
                    expectedDescriptionOrNull: CSharpFeaturesResources.Suggested_name);
                await VerifyItemIsAbsentAsync(markup, "ConfigurationLocal");
                await VerifyItemIsAbsentAsync(markup, "ConfigurationLocalFunction");
            }
            finally
            {
                workspace.Options = originalOptions;
            }
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CustomNamingStyleInsideMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var workspace = WorkspaceFixture.GetWorkspace();
            var originalOptions = workspace.Options;

            try
            {
                workspace.Options = workspace.Options.WithChangedOption(
                    new OptionKey(SimplificationOptions.NamingPreferences, LanguageNames.CSharp),
                    NamesEndWithSuffixPreferences());

                var markup = @"
class Configuration
{
    void M()
    {
        Configuration $$
    }
}
";
                await VerifyItemExistsAsync(markup, "ConfigurationLocal", glyph: (int)Glyph.Local,
                    expectedDescriptionOrNull: CSharpFeaturesResources.Suggested_name);
                await VerifyItemExistsAsync(markup, "ConfigurationLocalFunction", glyph: (int)Glyph.MethodPublic,
                    expectedDescriptionOrNull: CSharpFeaturesResources.Suggested_name);
                await VerifyItemIsAbsentAsync(markup, "ConfigurationField");
                await VerifyItemIsAbsentAsync(markup, "ConfigurationMethod");
                await VerifyItemIsAbsentAsync(markup, "ConfigurationProperty");
            }
            finally
            {
                workspace.Options = originalOptions;
            }
        }

        private static NamingStylePreferences NamesEndWithSuffixPreferences()
        {
            var specificationStyles = new[]
            {
                SpecificationStyle(new SymbolKindOrTypeKind(SymbolKind.Field), "Field"),
                SpecificationStyle(new SymbolKindOrTypeKind(SymbolKind.Property), "Property"),
                SpecificationStyle(new SymbolKindOrTypeKind(MethodKind.Ordinary), "Method"),
                SpecificationStyle(new SymbolKindOrTypeKind(SymbolKind.Local), "Local"),
                SpecificationStyle(new SymbolKindOrTypeKind(MethodKind.LocalFunction), "LocalFunction"),
            };

            return new NamingStylePreferences(
                specificationStyles.Select(t => t.specification).ToImmutableArray(),
                specificationStyles.Select(t => t.style).ToImmutableArray(),
                specificationStyles.Select(t => CreateRule(t.specification, t.style)).ToImmutableArray());

            // Local functions

            (SymbolSpecification specification, NamingStyle style) SpecificationStyle(SymbolKindOrTypeKind kind, string suffix)
            {
                var symbolSpecification = new SymbolSpecification(
                    id: null,
                    symbolSpecName: suffix,
                    ImmutableArray.Create(kind),
                    accessibilityList: default,
                    modifiers: default);

                var namingStyle = new NamingStyle(
                    Guid.NewGuid(),
                    name: suffix,
                    capitalizationScheme: Capitalization.PascalCase,
                    prefix: "",
                    suffix: suffix,
                    wordSeparator: "");

                return (symbolSpecification, namingStyle);
            }

            SerializableNamingRule CreateRule(SymbolSpecification specification, NamingStyle style)
            {
                return new SerializableNamingRule()
                {
                    SymbolSpecificationID = specification.ID,
                    NamingStyleID = style.ID,
                    EnforcementLevel = ReportDiagnostic.Error
                };
            }
        }
    }
}
