// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp.CodeStyle;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.CodeRefactorings;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.ReplaceMethodWithProperty;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.CodeActions.ReplaceMethodWithProperty
{
    public class ReplaceMethodWithPropertyTests : AbstractCSharpCodeActionTest
    {
        protected override CodeRefactoringProvider CreateCodeRefactoringProvider(Workspace workspace, TestParameters parameters)
            => new ReplaceMethodWithPropertyCodeRefactoringProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodWithGetName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    int [||]GetGoo()
    {
    }
}",
@"class C
{
    int Goo
    {
        get
        {
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodWithoutGetName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    int [||]Goo()
    {
    }
}",
@"class C
{
    int Goo
    {
        get
        {
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
        [WorkItem(6034, "https://github.com/dotnet/roslyn/issues/6034")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodWithArrowBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    int [||]GetGoo() => 0;
}",
@"class C
{
    int Goo
    {
        get
        {
            return 0;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodWithoutBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    int [||]GetGoo();
}",
@"class C
{
    int Goo { get; }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodWithModifiers()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    public static int [||]GetGoo()
    {
    }
}",
@"class C
{
    public static int Goo
    {
        get
        {
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodWithAttributes()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    [A]
    int [||]GetGoo()
    {
    }
}",
@"class C
{
    [A]
    int Goo
    {
        get
        {
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodWithTrivia_1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    // Goo
    int [||]GetGoo()
    {
    }
}",
@"class C
{
    // Goo
    int Goo
    {
        get
        {
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIndentation()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    int [||]GetGoo()
    {
        int count;
        foreach (var x in y)
        {
            count += bar;
        }
        return count;
    }
}",
@"class C
{
    int Goo
    {
        get
        {
            int count;
            foreach (var x in y)
            {
                count += bar;
            }
            return count;
        }
    }
}");
        }

        [WorkItem(21460, "https://github.com/dotnet/roslyn/issues/21460")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfDefMethod1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
#if true
    int [||]GetGoo()
    {
    }
#endif
}",
@"class C
{
#if true
    int Goo
    {
        get
        {
        }
    }
#endif
}");
        }

        [WorkItem(21460, "https://github.com/dotnet/roslyn/issues/21460")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfDefMethod2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
#if true
    int [||]GetGoo()
    {
    }

    void SetGoo(int val)
    {
    }
#endif
}",
@"class C
{
#if true
    int Goo
    {
        get
        {
        }
    }

    void SetGoo(int val)
    {
    }
#endif
}");
        }

        [WorkItem(21460, "https://github.com/dotnet/roslyn/issues/21460")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfDefMethod3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
#if true
    int [||]GetGoo()
    {
    }

    void SetGoo(int val)
    {
    }
#endif
}",
@"class C
{
#if true
    int Goo
    {
        get
        {
        }

        set
        {
        }
    }
#endif
}", index: 1);
        }

        [WorkItem(21460, "https://github.com/dotnet/roslyn/issues/21460")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfDefMethod4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
#if true
    void SetGoo(int val)
    {
    }

    int [||]GetGoo()
    {
    }
#endif
}",
@"class C
{
#if true
    void SetGoo(int val)
    {
    }

    int Goo
    {
        get
        {
        }
    }
#endif
}");
        }

        [WorkItem(21460, "https://github.com/dotnet/roslyn/issues/21460")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfDefMethod5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
#if true
    void SetGoo(int val)
    {
    }

    int [||]GetGoo()
    {
    }
#endif
}",
@"class C
{

#if true

    int Goo
    {
        get
        {
        }

        set
        {
        }
    }
#endif
}", index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodWithTrivia_2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    // Goo
    int [||]GetGoo()
    {
    }
    // SetGoo
    void SetGoo(int i)
    {
    }
}",
@"class C
{
    // Goo
    // SetGoo
    int Goo
    {
        get
        {
        }

        set
        {
        }
    }
}",
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExplicitInterfaceMethod_1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    int [||]I.GetGoo()
    {
    }
}",
@"class C
{
    int I.Goo
    {
        get
        {
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExplicitInterfaceMethod_2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"interface I
{
    int GetGoo();
}

class C : I
{
    int [||]I.GetGoo()
    {
    }
}",
@"interface I
{
    int Goo { get; }
}

class C : I
{
    int I.Goo
    {
        get
        {
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExplicitInterfaceMethod_3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"interface I
{
    int [||]GetGoo();
}

class C : I
{
    int I.GetGoo()
    {
    }
}",
@"interface I
{
    int Goo { get; }
}

class C : I
{
    int I.Goo
    {
        get
        {
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    [At[||]tr]
    int GetGoo()
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    int GetGoo()
    {
[||]
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestVoidMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    void [||]GetGoo()
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAsyncMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    async Task [||]GetGoo()
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenericMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    int [||]GetGoo<T>()
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExtensionMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"static class C
{
    int [||]GetGoo(this int i)
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodWithParameters_1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    int [||]GetGoo(int i)
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodWithParameters_2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    int [||]GetGoo(int i = 0)
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInSignature_1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    [At[||]tr]
    int GetGoo()
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInSignature_2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    int GetGoo()
    {
[||]
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUpdateGetReferenceNotInMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    int [||]GetGoo()
    {
    }

    void Bar()
    {
        var x = GetGoo();
    }
}",
@"class C
{
    int Goo
    {
        get
        {
        }
    }

    void Bar()
    {
        var x = Goo;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUpdateGetReferenceSimpleInvocation()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    int [||]GetGoo()
    {
    }

    void Bar()
    {
        var x = GetGoo();
    }
}",
@"class C
{
    int Goo
    {
        get
        {
        }
    }

    void Bar()
    {
        var x = Goo;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUpdateGetReferenceMemberAccessInvocation()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    int [||]GetGoo()
    {
    }

    void Bar()
    {
        var x = this.GetGoo();
    }
}",
@"class C
{
    int Goo
    {
        get
        {
        }
    }

    void Bar()
    {
        var x = this.Goo;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUpdateGetReferenceBindingMemberInvocation()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    int [||]GetGoo()
    {
    }

    void Bar()
    {
        C x;
        var v = x?.GetGoo();
    }
}",
@"class C
{
    int Goo
    {
        get
        {
        }
    }

    void Bar()
    {
        C x;
        var v = x?.Goo;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUpdateGetReferenceInMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    int [||]GetGoo()
    {
        return GetGoo();
    }
}",
@"class C
{
    int Goo
    {
        get
        {
            return Goo;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOverride()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    public virtual int [||]GetGoo()
    {
    }
}

class D : C
{
    public override int GetGoo()
    {
    }
}",
@"class C
{
    public virtual int Goo
    {
        get
        {
        }
    }
}

class D : C
{
    public override int Goo
    {
        get
        {
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUpdateGetReference_NonInvoked()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"using System;

class C
{
    int [||]GetGoo()
    {
    }

    void Bar()
    {
        Action<int> i = GetGoo;
    }
}",
@"using System;

class C
{
    int Goo
    {
        get
        {
        }
    }

    void Bar()
    {
        Action<int> i = {|Conflict:Goo|};
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUpdateGetReference_ImplicitReference()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"using System.Collections;

class C
{
    public IEnumerator [||]GetEnumerator()
    {
    }

    void Bar()
    {
        foreach (var x in this)
        {
        }
    }
}",
@"using System.Collections;

class C
{
    public IEnumerator Enumerator
    {
        get
        {
        }
    }

    void Bar()
    {
        {|Conflict:foreach (var x in this)
        {
        }|}
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUpdateGetSet()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"using System;

class C
{
    int [||]GetGoo()
    {
    }

    void SetGoo(int i)
    {
    }
}",
@"using System;

class C
{
    int Goo
    {
        get
        {
        }

        set
        {
        }
    }
}",
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUpdateGetSetReference_NonInvoked()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"using System;

class C
{
    int [||]GetGoo()
    {
    }

    void SetGoo(int i)
    {
    }

    void Bar()
    {
        Action<int> i = SetGoo;
    }
}",
@"using System;

class C
{
    int Goo
    {
        get
        {
        }

        set
        {
        }
    }

    void Bar()
    {
        Action<int> i = {|Conflict:Goo|};
    }
}",
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUpdateGetSet_SetterAccessibility()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"using System;

class C
{
    public int [||]GetGoo()
    {
    }

    private void SetGoo(int i)
    {
    }
}",
@"using System;

class C
{
    public int Goo
    {
        get
        {
        }

        private set
        {
        }
    }
}",
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUpdateGetSet_ExpressionBodies()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"using System;

class C
{
    int [||]GetGoo() => 0;
    void SetGoo(int i) => Bar();
}",
@"using System;

class C
{
    int Goo
    {
        get
        {
            return 0;
        }

        set
        {
            Bar();
        }
    }
}",
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUpdateGetSet_GetInSetReference()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"using System;

class C
{
    int [||]GetGoo()
    {
    }

    void SetGoo(int i)
    {
    }

    void Bar()
    {
        SetGoo(GetGoo() + 1);
    }
}",
@"using System;

class C
{
    int Goo
    {
        get
        {
        }

        set
        {
        }
    }

    void Bar()
    {
        Goo = Goo + 1;
    }
}",
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUpdateGetSet_UpdateSetParameterName_1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"using System;

class C
{
    int [||]GetGoo()
    {
    }

    void SetGoo(int i)
    {
        v = i;
    }
}",
@"using System;

class C
{
    int Goo
    {
        get
        {
        }

        set
        {
            v = value;
        }
    }
}",
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUpdateGetSet_UpdateSetParameterName_2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"using System;

class C
{
    int [||]GetGoo()
    {
    }

    void SetGoo(int value)
    {
        v = value;
    }
}",
@"using System;

class C
{
    int Goo
    {
        get
        {
        }

        set
        {
            v = value;
        }
    }
}",
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUpdateGetSet_SetReferenceInSetter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"using System;

class C
{
    int [||]GetGoo()
    {
    }

    void SetGoo(int i)
    {
        SetGoo(i - 1);
    }
}",
@"using System;

class C
{
    int Goo
    {
        get
        {
        }

        set
        {
            Goo = value - 1;
        }
    }
}",
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestVirtualGetWithOverride_1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    protected virtual int [||]GetGoo()
    {
    }
}

class D : C
{
    protected override int GetGoo()
    {
    }
}",
@"class C
{
    protected virtual int Goo
    {
        get
        {
        }
    }
}

class D : C
{
    protected override int Goo
    {
        get
        {
        }
    }
}",
index: 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestVirtualGetWithOverride_2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    protected virtual int [||]GetGoo()
    {
    }
}

class D : C
{
    protected override int GetGoo()
    {
        base.GetGoo();
    }
}",
@"class C
{
    protected virtual int Goo
    {
        get
        {
        }
    }
}

class D : C
{
    protected override int Goo
    {
        get
        {
            base.Goo;
        }
    }
}",
index: 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGetWithInterface()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"interface I
{
    int [||]GetGoo();
}

class C : I
{
    public int GetGoo()
    {
    }
}",
@"interface I
{
    int Goo { get; }
}

class C : I
{
    public int Goo
    {
        get
        {
        }
    }
}",
index: 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithPartialClasses()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"partial class C
{
    int [||]GetGoo()
    {
    }
}

partial class C
{
    void SetGoo(int i)
    {
    }
}",
@"partial class C
{
    int Goo
    {
        get
        {
        }

        set
        {
        }
    }
}

partial class C
{
}",
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUpdateGetSetCaseInsensitive()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"using System;

class C
{
    int [||]getGoo()
    {
    }

    void setGoo(int i)
    {
    }
}",
@"using System;

class C
{
    int Goo
    {
        get
        {
        }

        set
        {
        }
    }
}",
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Tuple()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    (int, string) [||]GetGoo()
    {
    }
}",
@"class C
{
    (int, string) Goo
    {
        get
        {
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Tuple_GetAndSet()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"using System;

class C
{
    (int, string) [||]getGoo()
    {
    }

    void setGoo((int, string) i)
    {
    }
}" + TestResources.NetFX.ValueTuple.tuplelib_cs,
@"using System;

class C
{
    (int, string) Goo
    {
        get
        {
        }

        set
        {
        }
    }
}" + TestResources.NetFX.ValueTuple.tuplelib_cs,
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleWithNames_GetAndSet()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"using System;

class C
{
    (int a, string b) [||]getGoo()
    {
    }

    void setGoo((int a, string b) i)
    {
    }
}" + TestResources.NetFX.ValueTuple.tuplelib_cs,
@"using System;

class C
{
    (int a, string b) Goo
    {
        get
        {
        }

        set
        {
        }
    }
}" + TestResources.NetFX.ValueTuple.tuplelib_cs,
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleWithDifferentNames_GetAndSet()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            // Cannot refactor tuples with different names together
            await TestActionCountAsync(
@"using System;

class C
{
    (int a, string b) [||]getGoo()
    {
    }

    void setGoo((int c, string d) i)
    {
    }
}",
count: 1, new TestParameters(options: AllCodeStyleOff));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOutVarDeclaration_1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    // Goo
    int [||]GetGoo()
    {
    }
    // SetGoo
    void SetGoo(out int i)
    {
    }

    void Test()
    {
        SetGoo(out int i);
    }
}",
@"class C
{
    // Goo
    int Goo
    {
        get
        {
        }
    }

    // SetGoo
    void SetGoo(out int i)
    {
    }

    void Test()
    {
        SetGoo(out int i);
    }
}",
index: 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOutVarDeclaration_2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C
{
    // Goo
    int [||]GetGoo()
    {
    }
    // SetGoo
    void SetGoo(int i)
    {
    }

    void Test()
    {
        SetGoo(out int i);
    }
}",
@"class C
{
    // Goo
    // SetGoo
    int Goo
    {
        get
        {
        }

        set
        {
        }
    }

    void Test()
    {
        {|Conflict:Goo|}(out int i);
    }
}",
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOutVarDeclaration_3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    // Goo
    int GetGoo()
    {
    }

    // SetGoo
    void [||]SetGoo(out int i)
    {
    }

    void Test()
    {
        SetGoo(out int i);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOutVarDeclaration_4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    // Goo
    int [||]GetGoo(out int i)
    {
    }

    // SetGoo
    void SetGoo(out int i, int j)
    {
    }

    void Test()
    {
        var y = GetGoo(out int i);
    }
}");
        }

        [WorkItem(14327, "https://github.com/dotnet/roslyn/issues/14327")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUpdateChainedGet1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"public class Goo
{
    public Goo()
    {
        Goo value = GetValue().GetValue();
    }

    public Goo [||]GetValue()
    {
        return this;
    }
}",
@"public class Goo
{
    public Goo()
    {
        Goo value = Value.Value;
    }

    public Goo Value
    {
        get
        {
            return this;
        }
    }
}");
        }

        [WorkItem(16980, "https://github.com/dotnet/roslyn/issues/16980")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCodeStyle1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int [||]GetGoo()
    {
        return 1;
    }
}",
@"class C
{
    int Goo { get => 1; }
}", options: PreferExpressionBodiedAccessors);
        }

        [WorkItem(16980, "https://github.com/dotnet/roslyn/issues/16980")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCodeStyle2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int [||]GetGoo()
    {
        return 1;
    }
}",
@"class C
{
    int Goo => 1;
}", options: PreferExpressionBodiedProperties);
        }

        [WorkItem(16980, "https://github.com/dotnet/roslyn/issues/16980")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCodeStyle3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int [||]GetGoo()
    {
        return 1;
    }
}",
@"class C
{
    int Goo => 1;
}", options: PreferExpressionBodiedAccessorsAndProperties);
        }

        [WorkItem(16980, "https://github.com/dotnet/roslyn/issues/16980")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCodeStyle4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int [||]GetGoo()
    {
        return 1;
    }

    void SetGoo(int i)
    {
        _i = i;
    }
}",
@"class C
{
    int Goo { get => 1; set => _i = value; }
}", 
index: 1,
options: PreferExpressionBodiedAccessors);
        }

        [WorkItem(16980, "https://github.com/dotnet/roslyn/issues/16980")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCodeStyle5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int [||]GetGoo()
    {
        return 1;
    }

    void SetGoo(int i)
    {
        _i = i;
    }
}",
@"class C
{
    int Goo
    {
        get
        {
            return 1;
        }

        set
        {
            _i = value;
        }
    }
}", 
index: 1,
options: PreferExpressionBodiedProperties);
        }

        [WorkItem(16980, "https://github.com/dotnet/roslyn/issues/16980")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCodeStyle6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int [||]GetGoo()
    {
        return 1;
    }

    void SetGoo(int i)
    {
        _i = i;
    }
}",
@"class C
{
    int Goo { get => 1; set => _i = value; }
}",
index: 1,
options: PreferExpressionBodiedAccessorsAndProperties);
        }

        [WorkItem(16980, "https://github.com/dotnet/roslyn/issues/16980")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCodeStyle7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int [||]GetGoo() => 0;
}",
@"class C
{
    int Goo => 0;
}", options: PreferExpressionBodiedProperties);
        }

        [WorkItem(16980, "https://github.com/dotnet/roslyn/issues/16980")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCodeStyle8()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int [||]GetGoo() => 0;
}",
@"class C
{
    int Goo { get => 0; }
}", options: PreferExpressionBodiedAccessors);
        }

        [WorkItem(16980, "https://github.com/dotnet/roslyn/issues/16980")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCodeStyle9()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int [||]GetGoo() => throw e;
}",
@"class C
{
    int Goo { get => throw e; }
}", options: PreferExpressionBodiedAccessors);
        }

        [WorkItem(16980, "https://github.com/dotnet/roslyn/issues/16980")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCodeStyle10()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int [||]GetGoo() { throw e; }
}",
@"class C
{
    int Goo => throw e;
}", options: PreferExpressionBodiedProperties);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUseExpressionBodyWhenOnSingleLine_AndIsSingleLine()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int [||]GetGoo() { throw e; }
}",
@"class C
{
    int Goo => throw e;
}", options: Option(CSharpCodeStyleOptions.PreferExpressionBodiedProperties, CSharpCodeStyleOptions.WhenOnSingleLineWithSilentEnforcement));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUseExpressionBodyWhenOnSingleLine_AndIsNotSingleLine()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int [||]GetGoo() { throw e +
        e; }
}",
@"class C
{
    int Goo
    {
        get
        {
            throw e +
   e;
        }
    }
}", options: OptionsSet(
    SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedProperties, CSharpCodeStyleOptions.WhenOnSingleLineWithSilentEnforcement),
    SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedAccessors, CSharpCodeStyleOptions.WhenOnSingleLineWithSilentEnforcement)));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExplicitInterfaceImplementation()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"interface IGoo
{
    int [||]GetGoo();
}

class C : IGoo
{
    int IGoo.GetGoo()
    {
        throw new System.NotImplementedException();
    }
}",
@"interface IGoo
{
    int Goo { get; }
}

class C : IGoo
{
    int IGoo.Goo
    {
        get
        {
            throw new System.NotImplementedException();
        }
    }
}");
        }

        [WorkItem(443523, "https://devdiv.visualstudio.com/DevDiv/_workitems?id=443523")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSystemObjectMetadataOverride()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    public override string [||]ToString()
    {
    }
}");
        }

        [WorkItem(443523, "https://devdiv.visualstudio.com/DevDiv/_workitems?id=443523")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsReplaceMethodWithProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMetadataOverride()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithAllCodeStyleOff(
@"class C : System.Type
{
    public override int [||]GetArrayRank()
    {
    }
}",
@"class C : System.Type
{
    public override int {|Warning:ArrayRank|}
    {
        get
        {
        }
    }
}");
        }

#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        private async Task TestWithAllCodeStyleOff(
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            string initialMarkup, string expectedMarkup, 
            ParseOptions parseOptions = null, int index = 0)
        {
            await TestAsync(
                initialMarkup, expectedMarkup, parseOptions,
                index: index,
                options: AllCodeStyleOff);
        }

        private IDictionary<OptionKey, object> AllCodeStyleOff =>
            OptionsSet(SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedAccessors, CSharpCodeStyleOptions.NeverWithSilentEnforcement),
                       SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedProperties, CSharpCodeStyleOptions.NeverWithSilentEnforcement));

        private IDictionary<OptionKey, object> PreferExpressionBodiedAccessors =>
            OptionsSet(SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedAccessors, CSharpCodeStyleOptions.WhenPossibleWithSuggestionEnforcement),
                       SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedProperties, CSharpCodeStyleOptions.NeverWithSilentEnforcement));

        private IDictionary<OptionKey, object> PreferExpressionBodiedProperties =>
            OptionsSet(SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedAccessors, CSharpCodeStyleOptions.NeverWithSilentEnforcement),
                       SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedProperties, CSharpCodeStyleOptions.WhenPossibleWithSuggestionEnforcement));

        private IDictionary<OptionKey, object> PreferExpressionBodiedAccessorsAndProperties =>
            OptionsSet(SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedAccessors, CSharpCodeStyleOptions.WhenPossibleWithSuggestionEnforcement),
                       SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedProperties, CSharpCodeStyleOptions.WhenPossibleWithSuggestionEnforcement));
    }
}
