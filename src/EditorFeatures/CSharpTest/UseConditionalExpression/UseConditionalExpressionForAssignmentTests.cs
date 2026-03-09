// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeStyle;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.CodeStyle;
using Microsoft.CodeAnalysis.CSharp.UseConditionalExpression;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.UseConditionalExpression
{
    public partial class UseConditionalExpressionForAssignmentTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace)
            => (new CSharpUseConditionalExpressionForAssignmentDiagnosticAnalyzer(),
                new CSharpUseConditionalExpressionForAssignmentCodeRefactoringProvider());

        private static readonly Dictionary<OptionKey, object> s_preferImplicitTypeAlways = new Dictionary<OptionKey, object>
        {
            { CSharpCodeStyleOptions.UseImplicitTypeWhereApparent, CodeStyleOptions.TrueWithSilentEnforcement },
            { CSharpCodeStyleOptions.UseImplicitTypeWherePossible, CodeStyleOptions.TrueWithSilentEnforcement },
            { CSharpCodeStyleOptions.UseImplicitTypeForIntrinsicTypes, CodeStyleOptions.TrueWithSilentEnforcement },
        };

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnSimpleAssignment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M(int i)
    {
        [||]if (true)
        {
            i = 0;
        }
        else
        {
            i = 1;
        }
    }
}",
@"
class C
{
    void M(int i)
    {
        i = true ? 0 : 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnSimpleAssignmentNoBlocks()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M(int i)
    {
        [||]if (true)
            i = 0;
        else
            i = 1;
    }
}",
@"
class C
{
    void M(int i)
    {
        i = true ? 0 : 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnSimpleAssignmentNoBlocks_NotInBlock()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M(int i)
    {
        if (true)
            [||]if (true)
                i = 0;
            else
                i = 1;
    }
}",
@"
class C
{
    void M(int i)
    {
        if (true)
            i = true ? 0 : 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnSimpleAssignmentToDifferentTargets()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"
class C
{
    void M(int i, int j)
    {
        [||]if (true)
        {
            i = 0;
        }
        else
        {
            j = 1;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnAssignmentToUndefinedField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        [||]if (true)
        {
            this.i = 0;
        }
        else
        {
            this.i = 1;
        }
    }
}",
@"
class C
{
    void M()
    {
        this.i = true ? 0 : 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnNonUniformTargetSyntax()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        [||]if (true)
        {
            this.i = 0;
        }
        else
        {
            this . i = 1;
        }
    }
}",
@"
class C
{
    void M()
    {
        this.i = true ? 0 : 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnAssignmentToDefinedField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    int i;

    void M()
    {
        [||]if (true)
        {
            this.i = 0;
        }
        else
        {
            this.i = 1;
        }
    }
}",
@"
class C
{
    int i;

    void M()
    {
        this.i = true ? 0 : 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnAssignmentToAboveLocalNoInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        int i;
        [||]if (true)
        {
            i = 0;
        }
        else
        {
            i = 1;
        }
    }
}",
@"
class C
{
    void M()
    {
        int i = true ? 0 : 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnAssignmentToAboveLocalLiteralInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        int i = 0;
        [||]if (true)
        {
            i = 0;
        }
        else
        {
            i = 1;
        }
    }
}",
@"
class C
{
    void M()
    {
        int i = true ? 0 : 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnAssignmentToAboveLocalDefaultLiteralInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
class C
{
    void M()
    {
        int i = default;
        [||]if (true)
        {
            i = 0;
        }
        else
        {
            i = 1;
        }
    }
}",
@"
class C
{
    void M()
    {
        int i = true ? 0 : 1;
    }
}", parseOptions: CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnAssignmentToAboveLocalDefaultExpressionInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        int i = default(int);
        [||]if (true)
        {
            i = 0;
        }
        else
        {
            i = 1;
        }
    }
}",
@"
class C
{
    void M()
    {
        int i = true ? 0 : 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDoNotMergeAssignmentToAboveLocalWithComplexInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        int i = Foo();
        [||]if (true)
        {
            i = 0;
        }
        else
        {
            i = 1;
        }
    }
}",
@"
class C
{
    void M()
    {
        int i = Foo();
        i = true ? 0 : 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDoNotMergeAssignmentToAboveLocalIfIntermediaryStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        int i = 0;
        Console.WriteLine();
        [||]if (true)
        {
            i = 0;
        }
        else
        {
            i = 1;
        }
    }
}",
@"
class C
{
    void M()
    {
        int i = 0;
        Console.WriteLine();
        i = true ? 0 : 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDoNotMergeAssignmentToAboveIfLocalUsedInIfCondition()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        int i = 0;
        [||]if (Bar(i))
        {
            i = 0;
        }
        else
        {
            i = 1;
        }
    }
}",
@"
class C
{
    void M()
    {
        int i = 0;
        i = Bar(i) ? 0 : 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDoNotMergeAssignmentToAboveIfMultiDecl()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        int i = 0, j = 0;
        [||]if (true)
        {
            i = 0;
        }
        else
        {
            i = 1;
        }
    }
}",
@"
class C
{
    void M()
    {
        int i = 0, j = 0;
        i = true ? 0 : 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUseImplicitTypeForIntrinsicTypes()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        int i = 0;
        [||]if (true)
        {
            i = 0;
        }
        else
        {
            i = 1;
        }
    }
}",
@"
class C
{
    void M()
    {
        var i = true ? 0 : 1;
    }
}", options: new Dictionary<OptionKey, object> {
    {  CSharpCodeStyleOptions.UseImplicitTypeForIntrinsicTypes, CodeStyleOptions.TrueWithSilentEnforcement }
});
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUseImplicitTypeWhereApparent()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        int i = 0;
        [||]if (true)
        {
            i = 0;
        }
        else
        {
            i = 1;
        }
    }
}",
@"
class C
{
    void M()
    {
        int i = true ? 0 : 1;
    }
}", options: new Dictionary<OptionKey, object> {
    {  CSharpCodeStyleOptions.UseImplicitTypeWhereApparent, CodeStyleOptions.TrueWithSilentEnforcement }
});
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUseImplicitTypeWherePossible()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        int i = 0;
        [||]if (true)
        {
            i = 0;
        }
        else
        {
            i = 1;
        }
    }
}",
@"
class C
{
    void M()
    {
        int i = true ? 0 : 1;
    }
}", options: new Dictionary<OptionKey, object> {
    {  CSharpCodeStyleOptions.UseImplicitTypeWherePossible, CodeStyleOptions.TrueWithSilentEnforcement }
});
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingWithoutElse()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
class C
{
    void M(int i)
    {
        [||]if (true)
        {
            i = 0;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingWithoutElseWithStatementAfterwards()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
class C
{
    void M(int i)
    {
        [||]if (true)
        {
            i = 0;
        }

        i = 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConversionWithUseVarForAll_CastInsertedToKeepTypeSame()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        // cast will be necessary, otherwise 'var' would get the type 'string'.
        object o;
        [||]if (true)
        {
            o = ""a"";
        }
        else
        {
            o = ""b"";
        }
    }
}",
@"
class C
{
    void M()
    {
        // cast will be necessary, otherwise 'var' would get the type 'string'.
        var o = true ? ""a"" : (object)""b"";
    }
}", options: s_preferImplicitTypeAlways);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConversionWithUseVarForAll_CanUseVarBecauseConditionalTypeMatches()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        string s;
        [||]if (true)
        {
            s = ""a"";
        }
        else
        {
            s = null;
        }
    }
}",
@"
class C
{
    void M()
    {
        var s = true ? ""a"" : null;
    }
}", options: s_preferImplicitTypeAlways);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConversionWithUseVarForAll_CanUseVarButRequiresCastOfConditionalBranch()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        string s;
        [||]if (true)
        {
            s = null;
        }
        else
        {
            s = null;
        }
    }
}",
@"
class C
{
    void M()
    {
        var s = true ? null : (string)null;
    }
}", options: s_preferImplicitTypeAlways);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestKeepTriviaAroundIf()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M(int i)
    {
        // leading
        [||]if (true)
        {
            i = 0;
        }
        else
        {
            i = 1;
        } // trailing
    }
}",
@"
class C
{
    void M(int i)
    {
        // leading
        i = true ? 0 : 1; // trailing
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAll1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M(int i)
    {
        {|FixAllInDocument:if|} (true)
        {
            i = 0;
        }
        else
        {
            i = 1;
        }

        string s;
        if (true)
        {
            s = ""a"";
        }
        else
        {
            s = ""b"";
        }
    }
}",
@"
class C
{
    void M(int i)
    {
        i = true ? 0 : 1;

        string s = true ? ""a"" : ""b"";
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMultiLine1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M(int i)
    {
        [||]if (true)
        {
            i = Foo(
                1, 2, 3);
        }
        else
        {
            i = 1;
        }
    }
}",
@"
class C
{
    void M(int i)
    {
        i = true
            ? Foo(
                1, 2, 3)
            : 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMultiLine2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M(int i)
    {
        [||]if (true)
        {
            i = 0;
        }
        else
        {
            i = Foo(
                1, 2, 3);
        }
    }
}",
@"
class C
{
    void M(int i)
    {
        i = true
            ? 0
            : Foo(
                1, 2, 3);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMultiLine3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M(int i)
    {
        [||]if (true)
        {
            i = Foo(
                1, 2, 3);
        }
        else
        {
            i = Foo(
                4, 5, 6);
        }
    }
}",
@"
class C
{
    void M(int i)
    {
        i = true
            ? Foo(
                1, 2, 3)
            : Foo(
                4, 5, 6);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestElseIfWithBlock()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M(int i)
    {
        if (true)
        {
        }
        else [||]if (false)
        {
            i = 1;
        }
        else
        {
            i = 0;
        }
    }
}",
@"
class C
{
    void M(int i)
    {
        if (true)
        {
        }
        else
        {
            i = false ? 1 : 0;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestElseIfWithoutBlock()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M(int i)
    {
        if (true) i = 2;
        else [||]if (false) i = 1;
        else i = 0;
    }
}",
@"
class C
{
    void M(int i)
    {
        if (true) i = 2;
        else i = false ? 1 : 0;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseConditionalExpression)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestRefAssignment1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    void M(ref int i, ref int j)
    {
        ref int x = ref i;
        [||]if (true)
        {
            x = ref i;
        }
        else
        {
            x = ref j
        }
    }
}",
@"
class C
{
    void M(ref int i, ref int j)
    {
        ref int x = ref i;
        x = ref true ? ref i : ref j;
    }
}");
        }
    }
}
