// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Diagnostics.AddBraces;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.AddBraces
{
    public partial class AddBracesTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace)
            => (new CSharpAddBracesDiagnosticAnalyzer(), new CSharpAddBracesCodeFixProvider());

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DoNotFireForIfWithBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    static void Main()
    {
        [|if|] (true)
        {
            return;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DoNotFireForElseWithBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    static void Main()
    {
        if (true)
        {
            return;
        }
        [|else|]
        {
            return;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DoNotFireForElseWithChildIf()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    static void Main()
    {
        if (true)
            return;
        [|else|] if (false)
            return;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DoNotFireForForWithBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    static void Main()
    {
        [|for|] (var i = 0; i < 5; i++)
        {
            return;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DoNotFireForForEachWithBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    static void Main()
    {
        [|foreach|] (var c in ""test"")
        {
            return;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DoNotFireForWhileWithBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    static void Main()
    {
        [|while|] (true)
        {
            return;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DoNotFireForDoWhileWithBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    static void Main()
    {
        [|do|]
        {
            return;
        }
        while (true);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DoNotFireForUsingWithBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    static void Main()
    {
        [|using|] (var f = new Fizz())
        {
            return;
        }
    }
}

class Fizz : IDisposable
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DoNotFireForUsingWithChildUsing()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    static void Main()
    {
        [|using|] (var f = new Fizz())
        using (var b = new Buzz())
            return;
    }
}

class Fizz : IDisposable
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}

class Buzz : IDisposable
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DoNotFireForLockWithBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    static void Main()
    {
        var str = ""test"";
        [|lock|] (str)
        {
            return;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DoNotFireForLockWithChildLock()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    static void Main()
    {
        var str1 = ""test"";
        var str2 = ""test"";
        [|lock|] (str1)
            lock (str2)
                return;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DoNotFireForFixedWithChildFixed()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    unsafe static void Main()
    {
        [|fixed|] (int* p = null)
        fixed (int* q = null)
        {
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FireForFixedWithoutBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    unsafe static void Main()
    {
        fixed (int* p = null)
        [|fixed|] (int* q = null)
            return;
    }
}",
@"class Program
{
    unsafe static void Main()
    {
        fixed (int* p = null)
        fixed (int* q = null)
        {
            return;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FireForIfWithoutBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
   @"
class Program
{
    static void Main()
    {
        [|if|] (true) return;
    }
}",

   @"
class Program
{
    static void Main()
    {
        if (true)
        {
            return;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FireForElseWithoutBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
            @"
class Program
{
    static void Main()
    {
        if (true) { return; }
        [|else|] return;
    }
}",

   @"
class Program
{
    static void Main()
    {
        if (true) { return; }
        else
        {
            return;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FireForIfNestedInElseWithoutBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
            @"
class Program
{
    static void Main()
    {
        if (true) return;
        else [|if|] (false) return;
    }
}",

   @"
class Program
{
    static void Main()
    {
        if (true) return;
        else if (false)
        {
            return;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FireForForWithoutBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
            @"
class Program
{
    static void Main()
    {
        [|for|] (var i = 0; i < 5; i++) return;
    }
}",

   @"
class Program
{
    static void Main()
    {
        for (var i = 0; i < 5; i++)
        {
            return;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FireForForEachWithoutBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
            @"
class Program
{
    static void Main()
    {
        [|foreach|] (var c in ""test"") return;
    }
}",

   @"
class Program
{
    static void Main()
    {
        foreach (var c in ""test"")
        {
            return;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FireForWhileWithoutBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
            @"
class Program
{
    static void Main()
    {
        [|while|] (true) return;
    }
}",

   @"
class Program
{
    static void Main()
    {
        while (true)
        {
            return;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FireForDoWhileWithoutBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
            @"
class Program
{
    static void Main()
    {
        [|do|] return; while (true);
    }
}",

   @"
class Program
{
    static void Main()
    {
        do
        {
            return;
        }
        while (true);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FireForUsingWithoutBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
            @"
class Program
{
    static void Main()
    {
        [|using|] (var f = new Fizz())
            return;
    }
}

class Fizz : IDisposable
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}",

   @"
class Program
{
    static void Main()
    {
        using (var f = new Fizz())
        {
            return;
        }
    }
}

class Fizz : IDisposable
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FireForUsingWithoutBracesNestedInUsing()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
            @"
class Program
{
    static void Main()
    {
        using (var f = new Fizz())
        [|using|] (var b = new Buzz())
            return;
    }
}

class Fizz : IDisposable
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}

class Buzz : IDisposable
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}",

   @"
class Program
{
    static void Main()
    {
        using (var f = new Fizz())
        using (var b = new Buzz())
        {
            return;
        }
    }
}

class Fizz : IDisposable
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}

class Buzz : IDisposable
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FireForLockWithoutBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
            @"
class Program
{
    static void Main()
    {
        var str = ""test"";
        [|lock|] (str)
            return;
    }
}",

   @"
class Program
{
    static void Main()
    {
        var str = ""test"";
        lock (str)
        {
            return;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddBraces)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FireForLockWithoutBracesNestedInLock()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
            @"
class Program
{
    static void Main()
    {
        var str1 = ""test"";
        var str2 = ""test"";

        lock (str1)
        [|lock|] (str2) // VS thinks this should be indented one more level
            return;
    }
}",

   @"
class Program
{
    static void Main()
    {
        var str1 = ""test"";
        var str2 = ""test"";

        lock (str1)
        lock (str2) // VS thinks this should be indented one more level
            {
                return;
            }
    }
}");
        }
    }
}
