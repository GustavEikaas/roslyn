// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics.InvokeDelegateWithConditionalAccess
{
    public partial class InvokeDelegateWithConditionalAccessTests
    {
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvokeDelegateWithConditionalAccess)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAllInDocument1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    System.Action a;

    void Goo()
    {
        {|FixAllInDocument:var|} v = a;
        if (v != null)
        {
            v();
        }

        var x = a;
        if (x != null)
        {
            x();
        }
    }
}",
@"class C
{
    System.Action a;

    void Goo()
    {
        a?.Invoke();

        a?.Invoke();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvokeDelegateWithConditionalAccess)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAllInDocument2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    System.Action a;

    void Goo()
    {
        var v = a;
        {|FixAllInDocument:if|} (v != null)
        {
            v();
        }

        var x = a;
        if (x != null)
        {
            x();
        }
    }
}",
@"class C
{
    System.Action a;

    void Goo()
    {
        a?.Invoke();

        a?.Invoke();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvokeDelegateWithConditionalAccess)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAllInDocument3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    System.Action a;

    void Goo()
    {
        var v = a;
        if (v != null)
        {
            {|FixAllInDocument:v|}();
        }

        var x = a;
        if (x != null)
        {
            x();
        }
    }
}",
@"class C
{
    System.Action a;

    void Goo()
    {
        a?.Invoke();

        a?.Invoke();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvokeDelegateWithConditionalAccess)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAllInDocument4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    System.Action a;

    void Goo()
    {
        var v = a;
        if (v != null)
        {
            v();
        }

        {|FixAllInDocument:var|} x = a;
        if (x != null)
        {
            x();
        }
    }
}",
@"class C
{
    System.Action a;

    void Goo()
    {
        a?.Invoke();

        a?.Invoke();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvokeDelegateWithConditionalAccess)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAllInDocument5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    System.Action a;

    void Goo()
    {
        var v = a;
        if (v != null)
        {
            v();
        }

        var x = a;
        {|FixAllInDocument:if|} (x != null)
        {
            x();
        }
    }
}",
@"class C
{
    System.Action a;

    void Goo()
    {
        a?.Invoke();

        a?.Invoke();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvokeDelegateWithConditionalAccess)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAllInDocument6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    System.Action a;

    void Goo()
    {
        var v = a;
        if (v != null)
        {
            v();
        }

        var x = a;
        if (x != null)
        {
            {|FixAllInDocument:x|}();
        }
    }
}",
@"class C
{
    System.Action a;

    void Goo()
    {
        a?.Invoke();

        a?.Invoke();
    }
}");
        }
    }
}
