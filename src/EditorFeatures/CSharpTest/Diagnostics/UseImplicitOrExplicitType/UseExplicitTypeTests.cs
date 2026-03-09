// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeStyle;
using Microsoft.CodeAnalysis.CSharp.CodeStyle;
using Microsoft.CodeAnalysis.CSharp.Diagnostics.TypeStyle;
using Microsoft.CodeAnalysis.CSharp.TypeStyle;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics.UseExplicitType
{
    public partial class UseExplicitTypeTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace)
            => (new CSharpUseExplicitTypeDiagnosticAnalyzer(), new UseExplicitTypeCodeFixProvider());

        private readonly CodeStyleOption<bool> onWithSilent = new CodeStyleOption<bool>(true, NotificationOption.Silent);
        private readonly CodeStyleOption<bool> offWithSilent = new CodeStyleOption<bool>(false, NotificationOption.Silent);
        private readonly CodeStyleOption<bool> onWithInfo = new CodeStyleOption<bool>(true, NotificationOption.Suggestion);
        private readonly CodeStyleOption<bool> offWithInfo = new CodeStyleOption<bool>(false, NotificationOption.Suggestion);
        private readonly CodeStyleOption<bool> onWithWarning = new CodeStyleOption<bool>(true, NotificationOption.Warning);
        private readonly CodeStyleOption<bool> offWithWarning = new CodeStyleOption<bool>(false, NotificationOption.Warning);
        private readonly CodeStyleOption<bool> onWithError = new CodeStyleOption<bool>(true, NotificationOption.Error);
        private readonly CodeStyleOption<bool> offWithError = new CodeStyleOption<bool>(false, NotificationOption.Error);

        // specify all options explicitly to override defaults.
        private IDictionary<OptionKey, object> ExplicitTypeEverywhere() => OptionsSet(
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeWherePossible, offWithInfo),
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeWhereApparent, offWithInfo),
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeForIntrinsicTypes, offWithInfo));

        private IDictionary<OptionKey, object> ExplicitTypeExceptWhereApparent() => OptionsSet(
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeWherePossible, offWithInfo),
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeWhereApparent, onWithInfo),
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeForIntrinsicTypes, offWithInfo));

        private IDictionary<OptionKey, object> ExplicitTypeForBuiltInTypesOnly() => OptionsSet(
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeWherePossible, onWithInfo),
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeWhereApparent, onWithInfo),
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeForIntrinsicTypes, offWithInfo));

        private IDictionary<OptionKey, object> ExplicitTypeEnforcements() => OptionsSet(
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeWherePossible, offWithWarning),
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeWhereApparent, offWithError),
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeForIntrinsicTypes, offWithInfo));

        private IDictionary<OptionKey, object> ExplicitTypeSilentEnforcement() => OptionsSet(
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeWherePossible, offWithSilent),
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeWhereApparent, offWithSilent),
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeForIntrinsicTypes, offWithSilent));

        private IDictionary<OptionKey, object> Options(OptionKey option, object value)
        {
            var options = new Dictionary<OptionKey, object>();
            options.Add(option, value);
            return options;
        }

        #region Error Cases

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotOnFieldDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    [|var|] _myfield = 5;
}", new TestParameters(options: ExplicitTypeEverywhere()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotOnFieldLikeEvents()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    public event [|var|] _myevent;
}", new TestParameters(options: ExplicitTypeEverywhere()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotOnAnonymousMethodExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Method()
    {
        [|var|] comparer = delegate (string value) {
            return value != ""0"";
        };
    }
}", new TestParameters(options: ExplicitTypeEverywhere()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotOnLambdaExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Method()
    {
        [|var|] x = y => y * y;
    }
}", new TestParameters(options: ExplicitTypeEverywhere()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotOnDeclarationWithMultipleDeclarators()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Method()
    {
        [|var|] x = 5, y = x;
    }
}", new TestParameters(options: ExplicitTypeEverywhere()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotOnDeclarationWithoutInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Method()
    {
        [|var|] x;
    }
}", new TestParameters(options: ExplicitTypeEverywhere()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotDuringConflicts()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Method()
    {
        [|var|] p = new var();
    }

    class var
    {
    }
}", new TestParameters(options: ExplicitTypeEverywhere()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotIfAlreadyExplicitlyTyped()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Method()
    {
        [|Program|] p = new Program();
    }
}", new TestParameters(options: ExplicitTypeEverywhere()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
        [WorkItem(27221, "https://github.com/dotnet/roslyn/issues/27221")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotIfRefTypeAlreadyExplicitlyTyped()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

struct Program
{
    void Method()
    {
        ref [|Program|] p = Ref();
    }
    ref Program Ref() => throw null;
}", new TestParameters(options: ExplicitTypeEverywhere()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotOnRHS()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class C
{
    void M()
    {
        var c = new [|var|]();
    }
}

class var
{
}");
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotOnErrorSymbol()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Method()
    {
        [|var|] x = new Goo();
    }
}", new TestParameters(options: ExplicitTypeEverywhere()));
        }

        #endregion

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
        [WorkItem(23907, "https://github.com/dotnet/roslyn/issues/23907")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InArrayType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var before = @"
class Program
{
    void Method()
    {
        [|var|] x = new Program[0];
    }
}";
            var after = @"
class Program
{
    void Method()
    {
        Program[] x = new Program[0];
    }
}";
            // The type is apparent and not intrinsic
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeEverywhere());
            await TestMissingInRegularAndScriptAsync(before, new TestParameters(options: ExplicitTypeForBuiltInTypesOnly()));
            await TestMissingInRegularAndScriptAsync(before, new TestParameters(options: ExplicitTypeExceptWhereApparent()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
        [WorkItem(23907, "https://github.com/dotnet/roslyn/issues/23907")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InArrayTypeWithIntrinsicType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var before = @"
class Program
{
    void Method()
    {
        [|var|] x = new int[0];
    }
}";
            var after = @"
class Program
{
    void Method()
    {
        int[] x = new int[0];
    }
}";
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeEverywhere());
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeForBuiltInTypesOnly());
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeExceptWhereApparent()); // preference for builtin types dominates
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
        [WorkItem(23907, "https://github.com/dotnet/roslyn/issues/23907")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InNullableIntrinsicType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var before = @"
class Program
{
    void Method(int? x)
    {
        [|var|] y = x;
    }
}";
            var after = @"
class Program
{
    void Method(int? x)
    {
        int? y = x;
    }
}";
            // The type is intrinsic and not apparent
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeEverywhere());
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeForBuiltInTypesOnly());
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeExceptWhereApparent());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
        [WorkItem(27221, "https://github.com/dotnet/roslyn/issues/27221")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WithRefIntrinsicType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var before = @"
class Program
{
    void Method()
    {
        ref [|var|] y = Ref();
    }
    ref int Ref() => throw null;
}";
            var after = @"
class Program
{
    void Method()
    {
        ref int y = Ref();
    }
    ref int Ref() => throw null;
}";
            // The type is intrinsic and not apparent
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeEverywhere());
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeForBuiltInTypesOnly());
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeExceptWhereApparent());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
        [WorkItem(27221, "https://github.com/dotnet/roslyn/issues/27221")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WithRefIntrinsicTypeInForeach()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var before = @"
class E
{
    public ref int Current => throw null;
    public bool MoveNext() => throw null;
    public E GetEnumerator() => throw null;

    void M()
    {
        foreach (ref [|var|] x in this) { }
    }
}";
            var after = @"
class E
{
    public ref int Current => throw null;
    public bool MoveNext() => throw null;
    public E GetEnumerator() => throw null;

    void M()
    {
        foreach (ref int x in this) { }
    }
}";
            // The type is intrinsic and not apparent
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeEverywhere());
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeForBuiltInTypesOnly());
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeExceptWhereApparent());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
        [WorkItem(23907, "https://github.com/dotnet/roslyn/issues/23907")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InArrayOfNullableIntrinsicType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var before = @"
class Program
{
    void Method(int?[] x)
    {
        [|var|] y = x;
    }
}";
            var after = @"
class Program
{
    void Method(int?[] x)
    {
        int?[] y = x;
    }
}";
            // The type is intrinsic and not apparent
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeEverywhere());
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeForBuiltInTypesOnly());
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeExceptWhereApparent());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
        [WorkItem(23907, "https://github.com/dotnet/roslyn/issues/23907")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InNullableCustomType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var before = @"
struct Program
{
    void Method(Program? x)
    {
        [|var|] y = x;
    }
}";
            var after = @"
struct Program
{
    void Method(Program? x)
    {
        Program? y = x;
    }
}";
            // The type is not intrinsic and not apparent
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeEverywhere());
            await TestMissingInRegularAndScriptAsync(before, new TestParameters(options: ExplicitTypeForBuiltInTypesOnly()));
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeExceptWhereApparent());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
        [WorkItem(23907, "https://github.com/dotnet/roslyn/issues/23907")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InPointerTypeWithIntrinsicType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var before = @"
unsafe class Program
{
    void Method(int* y)
    {
        [|var|] x = y;
    }
}";
            var after = @"
unsafe class Program
{
    void Method(int* y)
    {
        int* x = y;
    }
}";
            // The type is intrinsic and not apparent
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeEverywhere());
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeForBuiltInTypesOnly());
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeExceptWhereApparent()); // preference for builtin types dominates
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
        [WorkItem(23907, "https://github.com/dotnet/roslyn/issues/23907")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InPointerTypeWithCustomType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var before = @"
unsafe class Program
{
    void Method(Program* y)
    {
        [|var|] x = y;
    }
}";
            var after = @"
unsafe class Program
{
    void Method(Program* y)
    {
        Program* x = y;
    }
}";
            // The type is not intrinsic and not apparent
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeEverywhere());
            await TestMissingInRegularAndScriptAsync(before, new TestParameters(options: ExplicitTypeForBuiltInTypesOnly()));
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeExceptWhereApparent());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
        [WorkItem(23893, "https://github.com/dotnet/roslyn/issues/23893")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InOutParameter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var before = @"
class Program
{
    void Method(out int x)
    {
        Method(out [|var|] x);
    }
}";
            var after = @"
class Program
{
    void Method(out int x)
    {
        Method(out int x);
    }
}";
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeEverywhere());
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeForBuiltInTypesOnly());
            await TestInRegularAndScriptAsync(before, after, options: ExplicitTypeExceptWhereApparent());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotOnDynamic()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Method()
    {
        [|dynamic|] x = 1;
    }
}", new TestParameters(options: ExplicitTypeEverywhere()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotOnForEachVarWithAnonymousType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;
using System.Linq;

class Program
{
    void Method()
    {
        var values = Enumerable.Range(1, 5).Select(i => new { Value = i });

        foreach ([|var|] value in values)
        {
            Console.WriteLine(value.Value);
        }
    }
}", new TestParameters(options: ExplicitTypeEverywhere()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
        [WorkItem(23752, "https://github.com/dotnet/roslyn/issues/23752")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OnDeconstructionVarParens()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
class Program
{
    void M()
    {
        [|var|] (x, y) = new Program();
    }
    void Deconstruct(out int i, out string s) { i = 1; s = ""hello""; }
}", @"using System;
class Program
{
    void M()
    {
        (int x, string y) = new Program();
    }
    void Deconstruct(out int i, out string s) { i = 1; s = ""hello""; }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OnDeconstructionVar()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
class Program
{
    void M()
    {
        ([|var|] x, var y) = new Program();
    }
    void Deconstruct(out int i, out string s) { i = 1; s = ""hello""; }
}", @"using System;
class Program
{
    void M()
    {
        (int x, var y) = new Program();
    }
    void Deconstruct(out int i, out string s) { i = 1; s = ""hello""; }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
        [WorkItem(23752, "https://github.com/dotnet/roslyn/issues/23752")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OnNestedDeconstructionVar()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
class Program
{
    void M()
    {
        [|var|] (x, (y, z)) = new Program();
    }
    void Deconstruct(out int i, out Program s) { i = 1; s = null; }
}", @"using System;
class Program
{
    void M()
    {
        (int x, (int y, Program z)) = new Program();
    }
    void Deconstruct(out int i, out Program s) { i = 1; s = null; }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
        [WorkItem(23752, "https://github.com/dotnet/roslyn/issues/23752")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OnBadlyFormattedNestedDeconstructionVar()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
class Program
{
    void M()
    {
        [|var|](x,(y,z)) = new Program();
    }
    void Deconstruct(out int i, out Program s) { i = 1; s = null; }
}", @"using System;
class Program
{
    void M()
    {
        (int x, (int y, Program z)) = new Program();
    }
    void Deconstruct(out int i, out Program s) { i = 1; s = null; }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
        [WorkItem(23752, "https://github.com/dotnet/roslyn/issues/23752")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OnForeachNestedDeconstructionVar()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
class Program
{
    void M()
    {
        foreach ([|var|] (x, (y, z)) in new[] { new Program() } { }
    }
    void Deconstruct(out int i, out Program s) { i = 1; s = null; }
}", @"using System;
class Program
{
    void M()
    {
        foreach ((int x, (int y, Program z)) in new[] { new Program() } { }
    }
    void Deconstruct(out int i, out Program s) { i = 1; s = null; }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
        [WorkItem(23752, "https://github.com/dotnet/roslyn/issues/23752")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OnNestedDeconstructionVarWithTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
class Program
{
    void M()
    {
        /*before*/[|var|]/*after*/ (/*x1*/x/*x2*/, /*yz1*/(/*y1*/y/*y2*/, /*z1*/z/*z2*/)/*yz2*/) /*end*/ = new Program();
    }
    void Deconstruct(out int i, out Program s) { i = 1; s = null; }
}", @"using System;
class Program
{
    void M()
    {
        /*before*//*after*/(/*x1*/int x/*x2*/, /*yz1*/(/*y1*/int y/*y2*/, /*z1*/Program z/*z2*/)/*yz2*/) /*end*/ = new Program();
    }
    void Deconstruct(out int i, out Program s) { i = 1; s = null; }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
        [WorkItem(23752, "https://github.com/dotnet/roslyn/issues/23752")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OnDeconstructionVarWithDiscard()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
class Program
{
    void M()
    {
        [|var|] (x, _) = new Program();
    }
    void Deconstruct(out int i, out string s) { i = 1; s = ""hello""; }
}", @"using System;
class Program
{
    void M()
    {
        (int x, string _) = new Program();
    }
    void Deconstruct(out int i, out string s) { i = 1; s = ""hello""; }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
        [WorkItem(23752, "https://github.com/dotnet/roslyn/issues/23752")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OnDeconstructionVarWithErrorType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
class Program
{
    void M()
    {
        [|var|] (x, y) = new Program();
    }
    void Deconstruct(out int i, out Error s) { i = 1; s = null; }
}", @"using System;
class Program
{
    void M()
    {
        (int x, Error y) = new Program();
    }
    void Deconstruct(out int i, out Error s) { i = 1; s = null; }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OnForEachVarWithExplicitType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
using System.Linq;

class Program
{
    void Method()
    {
        var values = Enumerable.Range(1, 5);

        foreach ([|var|] value in values)
        {
            Console.WriteLine(value.Value);
        }
    }
}",
@"using System;
using System.Linq;

class Program
{
    void Method()
    {
        var values = Enumerable.Range(1, 5);

        foreach (int value in values)
        {
            Console.WriteLine(value.Value);
        }
    }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotOnAnonymousType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Method()
    {
        [|var|] x = new { Amount = 108, Message = ""Hello"" };
    }
}", new TestParameters(options: ExplicitTypeEverywhere()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotOnArrayOfAnonymousType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Method()
    {
        [|var|] x = new[] { new { name = ""apple"", diam = 4 }, new { name = ""grape"", diam = 1 } };
    }
}", new TestParameters(options: ExplicitTypeEverywhere()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotOnEnumerableOfAnonymousTypeFromAQueryExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    void Method()
    {
        var products = new List<Product>();
        [|var|] productQuery = from prod in products
                           select new { prod.Color, prod.Price };
    }
}

class Product
{
    public ConsoleColor Color { get; set; }
    public int Price { get; set; }
}");
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnLocalWithIntrinsicTypeString()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    static void M()
    {
        [|var|] s = ""hello"";
    }
}",
@"using System;

class C
{
    static void M()
    {
        string s = ""hello"";
    }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnIntrinsicType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    static void M()
    {
        [|var|] s = 5;
    }
}",
@"using System;

class C
{
    static void M()
    {
        int s = 5;
    }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnFrameworkType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class C
{
    static void M()
    {
        [|var|] c = new List<int>();
    }
}",
@"using System.Collections.Generic;

class C
{
    static void M()
    {
        List<int> c = new List<int>();
    }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnUserDefinedType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    void M()
    {
        [|var|] c = new C();
    }
}",
@"using System;

class C
{
    void M()
    {
        C c = new C();
    }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnGenericType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C<T>
{
    static void M()
    {
        [|var|] c = new C<int>();
    }
}",
@"using System;

class C<T>
{
    static void M()
    {
        C<int> c = new C<int>();
    }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnSingleDimensionalArrayTypeWithNewOperator()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    static void M()
    {
        [|var|] n1 = new int[4] { 2, 4, 6, 8 };
    }
}",
@"using System;

class C
{
    static void M()
    {
        int[] n1 = new int[4] { 2, 4, 6, 8 };
    }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnSingleDimensionalArrayTypeWithNewOperator2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    static void M()
    {
        [|var|] n1 = new[] { 2, 4, 6, 8 };
    }
}",
@"using System;

class C
{
    static void M()
    {
        int[] n1 = new[] { 2, 4, 6, 8 };
    }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnSingleDimensionalJaggedArrayType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    static void M()
    {
        [|var|] cs = new[] {
            new[] { 1, 2, 3, 4 },
            new[] { 5, 6, 7, 8 }
        };
    }
}",
@"using System;

class C
{
    static void M()
    {
        int[][] cs = new[] {
            new[] { 1, 2, 3, 4 },
            new[] { 5, 6, 7, 8 }
        };
    }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnDeclarationWithObjectInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    static void M()
    {
        [|var|] cc = new Customer { City = ""Madras"" };
    }

    private class Customer
    {
        public string City { get; set; }
    }
}",
@"using System;

class C
{
    static void M()
    {
        Customer cc = new Customer { City = ""Madras"" };
    }

    private class Customer
    {
        public string City { get; set; }
    }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnDeclarationWithCollectionInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
using System.Collections.Generic;

class C
{
    static void M()
    {
        [|var|] digits = new List<int> { 1, 2, 3 };
    }
}",
@"using System;
using System.Collections.Generic;

class C
{
    static void M()
    {
        List<int> digits = new List<int> { 1, 2, 3 };
    }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnDeclarationWithCollectionAndObjectInitializers()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
using System.Collections.Generic;

class C
{
    static void M()
    {
        [|var|] cs = new List<Customer>
        {
            new Customer { City = ""Madras"" }
        };
    }

    private class Customer
    {
        public string City { get; set; }
    }
}",
@"using System;
using System.Collections.Generic;

class C
{
    static void M()
    {
        List<Customer> cs = new List<Customer>
        {
            new Customer { City = ""Madras"" }
        };
    }

    private class Customer
    {
        public string City { get; set; }
    }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnForStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    static void M()
    {
        for ([|var|] i = 0; i < 5; i++)
        {
        }
    }
}",
@"using System;

class C
{
    static void M()
    {
        for (int i = 0; i < 5; i++)
        {
        }
    }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnForeachStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
using System.Collections.Generic;

class C
{
    static void M()
    {
        var l = new List<int> { 1, 3, 5 };
        foreach ([|var|] item in l)
        {
        }
    }
}",
@"using System;
using System.Collections.Generic;

class C
{
    static void M()
    {
        var l = new List<int> { 1, 3, 5 };
        foreach (int item in l)
        {
        }
    }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnQueryExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
using System.Collections.Generic;
using System.Linq;

class C
{
    static void M()
    {
        var customers = new List<Customer>();
        [|var|] expr = from c in customers
                   where c.City == ""London""
                   select c;
    }

    private class Customer
    {
        public string City { get; set; }
    }
}
}",
@"using System;
using System.Collections.Generic;
using System.Linq;

class C
{
    static void M()
    {
        var customers = new List<Customer>();
        IEnumerable<Customer> expr = from c in customers
                   where c.City == ""London""
                   select c;
    }

    private class Customer
    {
        public string City { get; set; }
    }
}
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeInUsingStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    static void M()
    {
        using ([|var|] r = new Res())
        {
        }
    }

    private class Res : IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}",
@"using System;

class C
{
    static void M()
    {
        using (Res r = new Res())
        {
        }
    }

    private class Res : IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnInterpolatedString()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class Program
{
    void Method()
    {
        [|var|] s = $""Hello, {name}""
    }
}",
@"using System;

class Program
{
    void Method()
    {
        string s = $""Hello, {name}""
    }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnExplicitConversion()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    static void M()
    {
        double x = 1234.7;
        [|var|] a = (int)x;
    }
}",
@"using System;

class C
{
    static void M()
    {
        double x = 1234.7;
        int a = (int)x;
    }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnConditionalAccessExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    static void M()
    {
        C obj = new C();
        [|var|] anotherObj = obj?.Test();
    }

    C Test()
    {
        return this;
    }
}",
@"using System;

class C
{
    static void M()
    {
        C obj = new C();
        C anotherObj = obj?.Test();
    }

    C Test()
    {
        return this;
    }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeInCheckedExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    static void M()
    {
        long number1 = int.MaxValue + 20L;
        [|var|] intNumber = checked((int)number1);
    }
}",
@"using System;

class C
{
    static void M()
    {
        long number1 = int.MaxValue + 20L;
        int intNumber = checked((int)number1);
    }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeInAwaitExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
using System.Threading.Tasks;

class C
{
    public async void ProcessRead()
    {
        [|var|] text = await ReadTextAsync(null);
    }

    private async Task<string> ReadTextAsync(string filePath)
    {
        return string.Empty;
    }
}",
@"using System;
using System.Threading.Tasks;

class C
{
    public async void ProcessRead()
    {
        string text = await ReadTextAsync(null);
    }

    private async Task<string> ReadTextAsync(string filePath)
    {
        return string.Empty;
    }
}", options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeInBuiltInNumericType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    public void ProcessRead()
    {
        [|var|] text = 1;
    }
}",
@"using System;

class C
{
    public void ProcessRead()
    {
        int text = 1;
    }
}", options: ExplicitTypeForBuiltInTypesOnly());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeInBuiltInCharType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    public void ProcessRead()
    {
        [|var|] text = GetChar();
    }

    public char GetChar() => 'c';
}",
@"using System;

class C
{
    public void ProcessRead()
    {
        char text = GetChar();
    }

    public char GetChar() => 'c';
}", options: ExplicitTypeForBuiltInTypesOnly());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeInBuiltInType_string()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            // though string isn't an intrinsic type per the compiler
            // we in the IDE treat it as an intrinsic type for this feature.
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    public void ProcessRead()
    {
        [|var|] text = string.Empty;
    }
}",
@"using System;

class C
{
    public void ProcessRead()
    {
        string text = string.Empty;
    }
}", options: ExplicitTypeForBuiltInTypesOnly());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeInBuiltInType_object()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            // object isn't an intrinsic type per the compiler
            // we in the IDE treat it as an intrinsic type for this feature.
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    public void ProcessRead()
    {
        object j = new C();
        [|var|] text = j;
    }
}",
@"using System;

class C
{
    public void ProcessRead()
    {
        object j = new C();
        object text = j;
    }
}", options: ExplicitTypeForBuiltInTypesOnly());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeNotificationLevelSilent()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var source =
@"using System;
class C
{
    static void M()
    {
        [|var|] n1 = new C();
    }
}";
            await TestDiagnosticInfoAsync(source,
                options: ExplicitTypeSilentEnforcement(),
                diagnosticId: IDEDiagnosticIds.UseExplicitTypeDiagnosticId,
                diagnosticSeverity: DiagnosticSeverity.Hidden);
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeNotificationLevelInfo()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var source =
@"using System;
class C
{
    static void M()
    {
        [|var|] s = 5;
    }
}";
            await TestDiagnosticInfoAsync(source,
                options: ExplicitTypeEnforcements(),
                diagnosticId: IDEDiagnosticIds.UseExplicitTypeDiagnosticId,
                diagnosticSeverity: DiagnosticSeverity.Info);
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
        [WorkItem(23907, "https://github.com/dotnet/roslyn/issues/23907")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeNotificationLevelWarning()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var source =
@"using System;
class C
{
    static void M()
    {
        [|var|] n1 = new[] { new C() }; // type not apparent and not intrinsic
    }
}";
            await TestDiagnosticInfoAsync(source,
                options: ExplicitTypeEnforcements(),
                diagnosticId: IDEDiagnosticIds.UseExplicitTypeDiagnosticId,
                diagnosticSeverity: DiagnosticSeverity.Warning);
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeNotificationLevelError()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var source =
@"using System;
class C
{
    static void M()
    {
        [|var|] n1 = new C();
    }
}";
            await TestDiagnosticInfoAsync(source,
                options: ExplicitTypeEnforcements(),
                diagnosticId: IDEDiagnosticIds.UseExplicitTypeDiagnosticId,
                diagnosticSeverity: DiagnosticSeverity.Error);
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnLocalWithIntrinsicTypeTuple()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    static void M()
    {
        [|var|] s = (1, ""hello"");
    }
}",
@"class C
{
    static void M()
    {
        (int, string) s = (1, ""hello"");
    }
}",
options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnLocalWithIntrinsicTypeTupleWithNames()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    static void M()
    {
        [|var|] s = (a: 1, b: ""hello"");
    }
}",
@"class C
{
    static void M()
    {
        (int a, string b) s = (a: 1, b: ""hello"");
    }
}",
options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnLocalWithIntrinsicTypeTupleWithOneName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    static void M()
    {
        [|var|] s = (a: 1, ""hello"");
    }
}",
@"class C
{
    static void M()
    {
        (int a, string) s = (a: 1, ""hello"");
    }
}",
options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
        [WorkItem(20437, "https://github.com/dotnet/roslyn/issues/20437")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SuggestExplicitTypeOnDeclarationExpressionSyntax()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class C
{
    static void M()
    {
        DateTime.TryParse(string.Empty, [|out var|] date);
    }
}",
@"using System;

class C
{
    static void M()
    {
        DateTime.TryParse(string.Empty, out DateTime date);
    }
}",
options: ExplicitTypeEverywhere());
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
        [WorkItem(20244, "https://github.com/dotnet/roslyn/issues/20244")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExplicitTypeOnPredefinedTypesByTheirMetadataNames1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Method()
    {
        [|String|] test = new String(' ', 4);
    }
}", new TestParameters(options: ExplicitTypeForBuiltInTypesOnly()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
        [WorkItem(20244, "https://github.com/dotnet/roslyn/issues/20244")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExplicitTypeOnPredefinedTypesByTheirMetadataNames2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Main()
    {
        foreach ([|String|] test in new String[] { ""test1"", ""test2"" })
        {
        }
    }
}", new TestParameters(options: ExplicitTypeForBuiltInTypesOnly()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
        [WorkItem(20244, "https://github.com/dotnet/roslyn/issues/20244")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExplicitTypeOnPredefinedTypesByTheirMetadataNames3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Main()
    {
        [|Int32[]|] array = new[] { 1, 2, 3 };
    }
}", new TestParameters(options: ExplicitTypeForBuiltInTypesOnly()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
        [WorkItem(20244, "https://github.com/dotnet/roslyn/issues/20244")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExplicitTypeOnPredefinedTypesByTheirMetadataNames4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Main()
    {
        [|Int32[][]|] a = new Int32[][]
        {
            new[] { 1, 2 },
            new[] { 3, 4 }
        };
    }
}", new TestParameters(options: ExplicitTypeForBuiltInTypesOnly()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
        [WorkItem(20244, "https://github.com/dotnet/roslyn/issues/20244")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExplicitTypeOnPredefinedTypesByTheirMetadataNames5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;
using System.Collections.Generic;

class Program
{
    void Main()
    {
        [|IEnumerable<Int32>|] a = new List<Int32> { 1, 2 }.Where(x => x > 1);
    }
}", new TestParameters(options: ExplicitTypeForBuiltInTypesOnly()));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
        [WorkItem(20244, "https://github.com/dotnet/roslyn/issues/20244")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExplicitTypeOnPredefinedTypesByTheirMetadataNames6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Main()
    {
        String name = ""name"";
        [|String|] s = $""Hello, {name}""
    }
}", new TestParameters(options: ExplicitTypeForBuiltInTypesOnly()));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
        [WorkItem(20244, "https://github.com/dotnet/roslyn/issues/20244")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExplicitTypeOnPredefinedTypesByTheirMetadataNames7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Main()
    {
        Object name = ""name"";
        [|String|] s = (String) name;
    }
}", new TestParameters(options: ExplicitTypeForBuiltInTypesOnly()));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
        [WorkItem(20244, "https://github.com/dotnet/roslyn/issues/20244")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExplicitTypeOnPredefinedTypesByTheirMetadataNames8()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;
using System.Threading.Tasks;

class C
{
    public async void ProcessRead()
    {
        [|String|] text = await ReadTextAsync(null);
    }

    private async Task<string> ReadTextAsync(string filePath)
    {
        return String.Empty;
    }
}", new TestParameters(options: ExplicitTypeForBuiltInTypesOnly()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
        [WorkItem(20244, "https://github.com/dotnet/roslyn/issues/20244")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExplicitTypeOnPredefinedTypesByTheirMetadataNames9()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Main()
    {
        String number = ""12"";
        Int32.TryParse(name, out [|Int32|] number)
    }
}", new TestParameters(options: ExplicitTypeForBuiltInTypesOnly()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
        [WorkItem(20244, "https://github.com/dotnet/roslyn/issues/20244")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExplicitTypeOnPredefinedTypesByTheirMetadataNames10()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Main()
    {
        for ([|Int32|] i = 0; i < 5; i++)
        {
        }
    }
}", new TestParameters(options: ExplicitTypeForBuiltInTypesOnly()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseImplicitType)]
        [WorkItem(20244, "https://github.com/dotnet/roslyn/issues/20244")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExplicitTypeOnPredefinedTypesByTheirMetadataNames11()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;
using System.Collections.Generic;

class Program
{
    void Main()
    {
        [|List<Int32>|] a = new List<Int32> { 1, 2 };
    }
}", new TestParameters(options: ExplicitTypeForBuiltInTypesOnly()));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExplicitType)]
        [WorkItem(26923, "https://github.com/dotnet/roslyn/issues/26923")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoSuggestionOnForeachCollectionExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;
using System.Collections.Generic;

class Program
{
    void Method(List<int> var)
    {
        foreach (int value in [|var|])
        {
            Console.WriteLine(value.Value);
        }
    }
}", new TestParameters(options: ExplicitTypeEverywhere()));
        }
    }
}
