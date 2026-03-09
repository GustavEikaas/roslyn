// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.AddRequiredParentheses;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeStyle;
using Microsoft.CodeAnalysis.CSharp.AddRequiredParentheses;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.AddRequiredParentheses
{
    public partial class AddRequiredParenthesesTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace)
            => (new CSharpAddRequiredParenthesesDiagnosticAnalyzer(), new AddRequiredParenthesesCodeFixProvider());
        
        private Task TestMissingAsync(string initialMarkup, IDictionary<OptionKey, object> options)
            => TestMissingInRegularAndScriptAsync(initialMarkup, new TestParameters(options: options));

        private Task TestAsync(string initialMarkup, string expected, IDictionary<OptionKey, object> options)
            => TestInRegularAndScript1Async(initialMarkup, expected, parameters: new TestParameters(options: options));

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestArithmeticPrecedence()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        int x = 1 + 2 $$* 3;
    }
}",
@"class C
{
    void M()
    {
        int x = 1 + (2 * 3);
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNoArithmeticOnLowerPrecedence()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = 1 $$+ 2 * 3;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotIfArithmeticPrecedenceStaysTheSame()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = 1 + 2 $$+ 3;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotIfArithmeticPrecedenceIsNotEnforced1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = 1 + 2 $$+ 3;
    }
}", RequireOtherBinaryParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotIfArithmeticPrecedenceIsNotEnforced2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = 1 + 2 $$* 3;
    }
}", RequireOtherBinaryParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestRelationalPrecedence()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        int x = a $$> b == c;
    }
}",
@"class C
{
    void M()
    {
        int x = (a > b) == c;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalPrecedence()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        int x = a || b $$&& c;
    }
}",
@"class C
{
    void M()
    {
        int x = a || (b && c);
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNoLogicalOnLowerPrecedence()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = a $$|| b && c;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotIfLogicalPrecedenceStaysTheSame()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = a || b $$|| c;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotIfLogicalPrecedenceIsNotEnforced()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = a || b $$|| c;
    }
}", RequireArithmeticBinaryParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMixedArithmeticAndLogical()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = a == b $$&& c == d;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalPrecedenceMultipleEqualPrecedenceParts1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        int x = a || b $$&& c && d;
    }
}",
@"class C
{
    void M()
    {
        int x = a || (b && c && d);
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalPrecedenceMultipleEqualPrecedenceParts2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        int x = a || b && c $$&& d;
    }
}",
@"class C
{
    void M()
    {
        int x = a || (b && c && d);
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestShiftPrecedence1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        int x = 1 $$+ 2 << 3;
    }
}",
@"class C
{
    void M()
    {
        int x = (1 + 2) << 3;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestShiftPrecedence2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        int x = 1 $$+ 2 << 3;
    }
}",
@"class C
{
    void M()
    {
        int x = (1 + 2) << 3;
    }
}", RequireArithmeticBinaryParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestShiftPrecedence3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = 1 $$+ 2 << 3;
    }
}", RequireOtherBinaryParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotIfShiftPrecedenceStaysTheSame1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = 1 $$<< 2 << 3;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotIfShiftPrecedenceStaysTheSame2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = 1 << 2 $$<< 3;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEqualityPrecedence1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = 1 $$+ 2 == 2 + 3;
    }
}", RequireOtherBinaryParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEqualityPrecedence2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = 1 + 2 == 2 $$+ 3;
    }
}", RequireOtherBinaryParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEqualityPrecedence3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = 1 $$+ 2 == 2 + 3;
    }
}", RequireRelationalBinaryParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEqualityPrecedence4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = 1 + 2 == 2 $$+ 3;
    }
}", RequireRelationalBinaryParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCoalescePrecedence1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = a $$+ b ?? c;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCoalescePrecedence2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = a $$?? b ?? c;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCoalescePrecedence3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = a ?? b $$?? c;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestBitwisePrecedence1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        int x = 1 $$+ 2 & 3;
    }
}",
@"class C
{
    void M()
    {
        int x = (1 + 2) & 3;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestBitwisePrecedence2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = a $$| b | c;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestBitwisePrecedence3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        int x = a | b $$& c;
    }
}",
@"class C
{
    void M()
    {
        int x = a | (b & c);
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestBitwisePrecedence4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = a $$| b & c;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotForEqualityAfterEquals()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = 1 $$== 2;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotForAssignmentEqualsAfterLocal()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M(int a)
    {
        int x = a $$+= 2;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForAssignmentAndEquality1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M(bool x, bool y, bool z)
    {
        x $$= y == z;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingForAssignmentAndEquality2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M(bool x, bool y, bool z)
    {
        x = y $$== z;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnclearCast1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = (int)$$-y;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnclearCast_NotOfferedWithIgnore()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = (int)$$-y;
    }
}", IgnoreAllParentheses);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnclearCast_NotOfferedWithRemoveForClarity()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = (int)$$-y;
    }
}", RemoveAllUnnecessaryParentheses);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnclearCast2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = (int)$$+y;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnclearCast3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = (int)$$&y;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnclearCast4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = (int)$$*y;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotForPrimary()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = (int)$$y;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotForMemberAccess()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = (int)$$y.z;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotForCastOfCast()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = (int)$$(y);
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotForNonAmbiguousUnary()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        int x = (int)$$!y;
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAll1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        if (0 {|FixAllInDocument:>=|} 3 * 2 + 4)
        {
        }
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAll2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        if (3 * 2 + 4 >= 3 {|FixAllInDocument:*|} 2 + 4)
        {
        }
    }
}",
@"class C
{
    void M()
    {
        if ((3 * 2) + 4 >= (3 * 2) + 4)
        {
        }
    }
}", options: RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAll3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    void M()
    {
        if (3 * 2 + 4 >= 3 * 2 {|FixAllInDocument:+|} 4)
        {
        }
    }
}", RequireAllParenthesesForClarity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddRequiredParentheses)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSeams1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        int x = 1 + 2 {|FixAllInDocument:*|} 3 == 1 + 2 * 3;
    }
}",
@"class C
{
    void M()
    {
        int x = 1 + (2 * 3) == 1 + (2 * 3);
    }
}", options: RequireAllParenthesesForClarity);
        }
    }
}
