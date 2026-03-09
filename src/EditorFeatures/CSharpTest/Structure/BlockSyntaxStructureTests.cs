// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Structure;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Structure;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Structure
{
    public class BlockSyntaxStructureTests : AbstractCSharpSyntaxNodeStructureTests<BlockSyntax>
    {
        internal override AbstractSyntaxStructureProvider CreateProvider() => new BlockSyntaxStructureProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTryBlock1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        {|hint:try{|textspan:
        {$$
        }
        catch 
        {
        }
        finally
        {
        }|}|}
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnsafe1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        {|hint:unsafe{|textspan:
        {$$
        }|}|}
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixed1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        {|hint:fixed(int* i = &j){|textspan:
        {$$
        }|}|}
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUsing1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        {|hint:using (goo){|textspan:
        {$$
        }|}|}
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLock1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        {|hint:lock (goo){|textspan:
        {$$
        }|}|}
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForStatement1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        {|hint:for (;;){|textspan:
        {$$
        }|}|}
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForEachStatement1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        {|hint:foreach (var v in e){|textspan:
        {$$
        }|}|}
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCompoundForEachStatement1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        {|hint:foreach ((var v, var x) in e){|textspan:
        {$$
        }|}|}
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWhileStatement1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        {|hint:while (true){|textspan:
        {$$
        }|}|}
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDoStatement1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        {|hint:do{|textspan:
        {$$
        }
        while (true);|}|}
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfStatement1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        {|hint:if (true){|textspan:
        {$$
        }|}|}
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfStatement2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        {|hint:if (true){|textspan:
        {$$
        }|}|}
        else
        {
        }
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfStatement3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        {|hint:if (true){|textspan:
        {$$
        }|}|}
        else
            return;
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestElseClause1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        if (true)
        {
        }
        {|hint:else{|textspan:
        {$$
        }|}|}
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIfElse1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        if (true)
        {
        }
        else {|hint:if (false){|textspan:
        {$$
        }|}|}
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedBlock()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        {|hint:{|textspan:{$$

        }|}|}
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedBlockInSwitchSection1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        switch (e)
        {
            case 0:
                {|hint:{|textspan:{$$

                }|}|}
        }
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedBlockInSwitchSection2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        switch (e)
        {
        case 0:
            int i = 0;
            {|hint:{|textspan:{$$

            }|}|}
        }
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }
    }
}
