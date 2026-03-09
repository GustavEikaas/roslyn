// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.MakeMethodSynchronous;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.MakeMethodSynchronous;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics.MakeMethodSynchronous
{
    public class MakeMethodSynchronousTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace)
            => (null, new CSharpMakeMethodSynchronousCodeFixProvider());

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodSynchronous)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTaskReturnType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System.Threading.Tasks;

class C
{
    async Task [|Goo|]()
    {
    }
}",
@"
using System.Threading.Tasks;

class C
{
    void Goo()
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodSynchronous)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTaskOfTReturnType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System.Threading.Tasks;

class C
{
    async Task<int> [|Goo|]()
    {
    }
}",
@"
using System.Threading.Tasks;

class C
{
    int Goo()
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodSynchronous)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSecondModifier()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System.Threading.Tasks;

class C
{
    public async Task [|Goo|]()
    {
    }
}",
@"
using System.Threading.Tasks;

class C
{
    public void Goo()
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodSynchronous)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFirstModifier()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System.Threading.Tasks;

class C
{
    async public Task [|Goo|]()
    {
    }
}",
@"
using System.Threading.Tasks;

class C
{
    public void Goo()
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodSynchronous)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTrailingTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System.Threading.Tasks;

class C
{
    async // comment
    Task [|Goo|]()
    {
    }
}",
@"
using System.Threading.Tasks;

class C
{
    void Goo()
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodSynchronous)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestRenameMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System.Threading.Tasks;

class C
{
    async Task [|GooAsync|]()
    {
    }
}",
@"
using System.Threading.Tasks;

class C
{
    void Goo()
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodSynchronous)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestRenameMethod1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System.Threading.Tasks;

class C
{
    async Task [|GooAsync|]()
    {
    }

    void Bar()
    {
        GooAsync();
    }
}",
@"
using System.Threading.Tasks;

class C
{
    void Goo()
    {
    }

    void Bar()
    {
        Goo();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodSynchronous)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestParenthesizedLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System.Threading.Tasks;

class C
{
    void Goo()
    {
        Func<Task> f =
            async () [|=>|] { };
    }
}",
@"
using System.Threading.Tasks;

class C
{
    void Goo()
    {
        Func<Task> f =
            () => { };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodSynchronous)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSimpleLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System.Threading.Tasks;

class C
{
    void Goo()
    {
        Func<string, Task> f =
            async a [|=>|] { };
    }
}",
@"
using System.Threading.Tasks;

class C
{
    void Goo()
    {
        Func<string, Task> f =
            a => { };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodSynchronous)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLambdaWithExpressionBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System.Threading.Tasks;

class C
{
    void Goo()
    {
        Func<string, Task> f =
            async a [|=>|] 1;
    }
}",
@"
using System.Threading.Tasks;

class C
{
    void Goo()
    {
        Func<string, Task> f =
            a => 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodSynchronous)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAnonymousMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System.Threading.Tasks;

class C
{
    void Goo()
    {
        Func<Task> f =
            async [|delegate|] { };
    }
}",
@"
using System.Threading.Tasks;

class C
{
    void Goo()
    {
        Func<Task> f =
            delegate { };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodSynchronous)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAll()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Threading.Tasks;

public class Class1
{
    {|FixAllInDocument:async Task GooAsync()
    {
        BarAsync();
    }

    async Task<int> BarAsync()
    {
        GooAsync();
    }|}
}",
@"using System.Threading.Tasks;

public class Class1
{
    void Goo()
    {
        Bar();
    }

    int Bar()
    {
        Goo();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodSynchronous)]
        [WorkItem(13961, "https://github.com/dotnet/roslyn/issues/13961")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestRemoveAwaitFromCaller1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Threading.Tasks;

public class Class1
{
    async Task [|GooAsync|]()
    {
    }

    async void BarAsync()
    {
        await GooAsync();
    }
}",
@"using System.Threading.Tasks;

public class Class1
{
    void Goo()
    {
    }

    async void BarAsync()
    {
        Goo();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodSynchronous)]
        [WorkItem(13961, "https://github.com/dotnet/roslyn/issues/13961")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestRemoveAwaitFromCaller2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Threading.Tasks;

public class Class1
{
    async Task [|GooAsync|]()
    {
    }

    async void BarAsync()
    {
        await GooAsync().ConfigureAwait(false);
    }
}",
@"using System.Threading.Tasks;

public class Class1
{
    void Goo()
    {
    }

    async void BarAsync()
    {
        Goo();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodSynchronous)]
        [WorkItem(13961, "https://github.com/dotnet/roslyn/issues/13961")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestRemoveAwaitFromCaller3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Threading.Tasks;

public class Class1
{
    async Task [|GooAsync|]()
    {
    }

    async void BarAsync()
    {
        await this.GooAsync();
    }
}",
@"using System.Threading.Tasks;

public class Class1
{
    void Goo()
    {
    }

    async void BarAsync()
    {
        this.Goo();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodSynchronous)]
        [WorkItem(13961, "https://github.com/dotnet/roslyn/issues/13961")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestRemoveAwaitFromCaller4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Threading.Tasks;

public class Class1
{
    async Task [|GooAsync|]()
    {
    }

    async void BarAsync()
    {
        await this.GooAsync().ConfigureAwait(false);
    }
}",
@"using System.Threading.Tasks;

public class Class1
{
    void Goo()
    {
    }

    async void BarAsync()
    {
        this.Goo();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodSynchronous)]
        [WorkItem(13961, "https://github.com/dotnet/roslyn/issues/13961")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestRemoveAwaitFromCallerNested1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Threading.Tasks;

public class Class1
{
    async Task<int> [|GooAsync|](int i)
    {
    }

    async void BarAsync()
    {
        await this.GooAsync(await this.GooAsync(0));
    }
}",
@"using System.Threading.Tasks;

public class Class1
{
    int Goo(int i)
    {
    }

    async void BarAsync()
    {
        this.Goo(this.Goo(0));
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodSynchronous)]
        [WorkItem(13961, "https://github.com/dotnet/roslyn/issues/13961")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestRemoveAwaitFromCallerNested()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Threading.Tasks;

public class Class1
{
    async Task<int> [|GooAsync|](int i)
    {
    }

    async void BarAsync()
    {
        await this.GooAsync(await this.GooAsync(0).ConfigureAwait(false)).ConfigureAwait(false);
    }
}",
@"using System.Threading.Tasks;

public class Class1
{
    int Goo(int i)
    {
    }

    async void BarAsync()
    {
        this.Goo(this.Goo(0));
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodAsynchronous)]
        [WorkItem(14133, "https://github.com/dotnet/roslyn/issues/14133")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task RemoveAsyncInLocalFunction()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Threading.Tasks;

class C
{
    public void M1()
    {
        async Task [|M2Async|]()
        {
        }
    }
}",
@"using System.Threading.Tasks;

class C
{
    public void M1()
    {
        void M2()
        {
        }
    }
}");
        }

        [Theory]
        [InlineData("Task<C>", "C")]
        [InlineData("Task<int>", "int")]
        [InlineData("Task", "void")]
        [InlineData("void", "void")]
        [Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodAsynchronous)]
        [WorkItem(18307, "https://github.com/dotnet/roslyn/issues/18307")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task RemoveAsyncInLocalFunctionKeepsTrivia(string asyncReturn, string expectedReturn)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
$@"using System;
using System.Threading.Tasks;

class C
{{
    public void M1()
    {{
        // Leading trivia
        /*1*/ async {asyncReturn} /*2*/ [|M2Async|]/*3*/() /*4*/
        {{
            throw new NotImplementedException();
        }}
    }}
}}",
$@"using System;
using System.Threading.Tasks;

class C
{{
    public void M1()
    {{
        // Leading trivia
        /*1*/
        {expectedReturn} /*2*/ M2/*3*/() /*4*/
        {{
            throw new NotImplementedException();
        }}
    }}
}}");
        }

        [Theory]
        [InlineData("", "Task<C>", "\r\n    C")]
        [InlineData("", "Task<int>", "\r\n    int")]
        [InlineData("", "Task", "\r\n    void")]
        [InlineData("", "void", "\r\n    void")]
        [InlineData("public", "Task<C>", " C")]
        [InlineData("public", "Task<int>", " int")]
        [InlineData("public", "Task", " void")]
        [InlineData("public", "void", " void")]
        [Trait(Traits.Feature, Traits.Features.CodeActionsMakeMethodAsynchronous)]
        [WorkItem(18307, "https://github.com/dotnet/roslyn/issues/18307")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task RemoveAsyncKeepsTrivia(string modifiers, string asyncReturn, string expectedReturn)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
$@"using System;
using System.Threading.Tasks;

class C
{{
    // Leading trivia
    {modifiers}/*1*/ async {asyncReturn} /*2*/ [|M2Async|]/*3*/() /*4*/
    {{
        throw new NotImplementedException();
    }}
}}",
$@"using System;
using System.Threading.Tasks;

class C
{{
    // Leading trivia
    {modifiers}/*1*/{expectedReturn} /*2*/ M2/*3*/() /*4*/
    {{
        throw new NotImplementedException();
    }}
}}");
        }
    }
}
