// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp.InitializeParameter;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.CodeRefactorings;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.InitializeParameter
{
    public partial class InitializeMemberFromParameterTests : AbstractCSharpCodeActionTest
    {
        protected override CodeRefactoringProvider CreateCodeRefactoringProvider(Workspace workspace, TestParameters parameters)
            => new CSharpInitializeMemberFromParameterCodeRefactoringProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInitializeFieldWithSameName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    private string s;

    public C([||]string s)
    {
    }
}",
@"
class C
{
    private string s;

    public C(string s)
    {
        this.s = s;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEndOfParameter1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    private string s;

    public C(string s[||])
    {
    }
}",
@"
class C
{
    private string s;

    public C(string s)
    {
        this.s = s;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEndOfParameter2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    private string s;

    public C(string s[||], string t)
    {
    }
}",
@"
class C
{
    private string s;

    public C(string s, string t)
    {
        this.s = s;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInitializeFieldWithUnderscoreName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    private string _s;

    public C([||]string s)
    {
    }
}",
@"
class C
{
    private string _s;

    public C(string s)
    {
        _s = s;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInitializeWritableProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    private string S { get; }

    public C([||]string s)
    {
    }
}",
@"
class C
{
    private string S { get; }

    public C(string s)
    {
        S = s;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInitializeFieldWithDifferentName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    private string t;

    public C([||]string s)
    {
    }
}",
@"
class C
{
    private string t;

    public C(string s)
    {
        S = s;
    }

    public string S { get; }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInitializeNonWritableProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    private string S => null;

    public C([||]string s)
    {
    }
}",
@"
class C
{
    private string S => null;

    public string S1 { get; }

    public C(string s)
    {
        S1 = s;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInitializeDoesNotUsePropertyWithUnrelatedName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    private string T { get; }

    public C([||]string s)
    {
    }
}",
@"
class C
{
    private string T { get; }
    public string S { get; }

    public C(string s)
    {
        S = s;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInitializeFieldWithWrongType1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    private int s;

    public C([||]string s)
    {
    }
}",
@"
class C
{
    private int s;

    public C(string s)
    {
        S = s;
    }

    public string S { get; }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInitializeFieldWithWrongType2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    private int s;

    public C([||]string s)
    {
    }
}",
@"
class C
{
    private readonly string s1;
    private int s;

    public C(string s)
    {
        s1 = s;
    }
}", index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInitializeFieldWithConvertibleType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    private object s;

    public C([||]string s)
    {
    }
}",
@"
class C
{
    private object s;

    public C(string s)
    {
        this.s = s;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWhenAlreadyInitialized1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
class C
{
    private int s;
    private int x;

    public C([||]string s)
    {
        x = s;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWhenAlreadyInitialized2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
class C
{
    private int s;
    private int x;

    public C([||]string s)
    {
        x = s ?? throw new Exception();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWhenAlreadyInitialized3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    private int s;

    public C([||]string s)
    {
        s = 0;
    }
}",

@"
class C
{
    private int s;

    public C([||]string s)
    {
        s = 0;
        S = s;
    }

    public string S { get; }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsertionLocation1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    private string s;
    private string t;

    public C([||]string s, string t)
    {
        this.t = t;   
    }
}",
@"
class C
{
    private string s;
    private string t;

    public C(string s, string t)
    {
        this.s = s;
        this.t = t;   
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsertionLocation2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    private string s;
    private string t;

    public C(string s, [||]string t)
    {
        this.s = s;   
    }
}",
@"
class C
{
    private string s;
    private string t;

    public C(string s, string t)
    {
        this.s = s;
        this.t = t;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsertionLocation3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    private string s;

    public C([||]string s)
    {
        if (true) { } 
    }
}",
@"
class C
{
    private string s;

    public C(string s)
    {
        if (true) { }

        this.s = s;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
class C
{
    private string s;

    public void M([||]string s)
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsertionLocation4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    private string s;
    private string t;

    public C(string s, [||]string t)
        => this.s = s;   
}",
@"
class C
{
    private string s;
    private string t;

    public C(string s, string t)
    {
        this.s = s;
        this.t = t;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsertionLocation5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    private string s;
    private string t;

    public C([||]string s, string t)
        => this.t = t;   
}",
@"
class C
{
    private string s;
    private string t;

    public C(string s, string t)
    {
        this.s = s;
        this.t = t;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsertionLocation6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    public C(string s, [||]string t)
    {
        S = s;   
    }

    public string S { get; }
}",
@"
class C
{
    public C(string s, string t)
    {
        S = s;
        T = t;
    }

    public string S { get; }
    public string T { get; }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsertionLocation7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    public C([||]string s, string t)
    {
        T = t;   
    }

    public string T { get; }
}",
@"
class C
{
    public C(string s, string t)
    {
        S = s;
        T = t;   
    }

    public string S { get; }
    public string T { get; }
}");
        }

        [WorkItem(19956, "https://github.com/dotnet/roslyn/issues/19956")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInitializeParameter)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNoBlock()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    private string s;

    public C(string s[||])
}",
@"
class C
{
    private string s;

    public C(string s)
    {
        this.s = s;
    }
}");
        }
    }
}
