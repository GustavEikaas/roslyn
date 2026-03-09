// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeStyle;
using Microsoft.CodeAnalysis.CSharp.Diagnostics.Analyzers;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.PreferFrameworkType;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics.PreferFrameworkType
{
    public partial class PreferFrameworkTypeTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace) 
            => (new CSharpPreferFrameworkTypeDiagnosticAnalyzer(), new PreferFrameworkTypeCodeFixProvider());

        private readonly CodeStyleOption<bool> onWithInfo = new CodeStyleOption<bool>(true, NotificationOption.Suggestion);
        private readonly CodeStyleOption<bool> offWithInfo = new CodeStyleOption<bool>(false, NotificationOption.Suggestion);

        private IDictionary<OptionKey, object> NoFrameworkType => OptionsSet(
            SingleOption(CodeStyleOptions.PreferIntrinsicPredefinedTypeKeywordInDeclaration, true, NotificationOption.Suggestion),
            SingleOption(CodeStyleOptions.PreferIntrinsicPredefinedTypeKeywordInMemberAccess, onWithInfo, GetLanguage()));

        private IDictionary<OptionKey, object> FrameworkTypeEverywhere => OptionsSet(
            SingleOption(CodeStyleOptions.PreferIntrinsicPredefinedTypeKeywordInDeclaration, false, NotificationOption.Suggestion),
            SingleOption(CodeStyleOptions.PreferIntrinsicPredefinedTypeKeywordInMemberAccess, offWithInfo, GetLanguage()));

        private IDictionary<OptionKey, object> FrameworkTypeInDeclaration => OptionsSet(
            SingleOption(CodeStyleOptions.PreferIntrinsicPredefinedTypeKeywordInDeclaration, false, NotificationOption.Suggestion),
            SingleOption(CodeStyleOptions.PreferIntrinsicPredefinedTypeKeywordInMemberAccess, onWithInfo, GetLanguage()));

        private IDictionary<OptionKey, object> FrameworkTypeInMemberAccess => OptionsSet(
            SingleOption(CodeStyleOptions.PreferIntrinsicPredefinedTypeKeywordInDeclaration, true, NotificationOption.Suggestion),
            SingleOption(CodeStyleOptions.PreferIntrinsicPredefinedTypeKeywordInMemberAccess, offWithInfo, GetLanguage()));

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotWhenOptionsAreNotSet()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Method()
    {
        [|int|] x = 1;
    }
}", new TestParameters(options: NoFrameworkType));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
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
}", new TestParameters(options: FrameworkTypeInDeclaration));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotOnSystemVoid()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    [|void|] Method()
    {
    }
}", new TestParameters(options: FrameworkTypeEverywhere));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotOnUserdefinedType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Method()
    {
        [|Program|] p;
    }
}", new TestParameters(options: FrameworkTypeEverywhere));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotOnFrameworkType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Method()
    {
        [|Int32|] p;
    }
}", new TestParameters(options: FrameworkTypeInDeclaration));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotOnQualifiedTypeSyntax()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    void Method()
    {
        [|System.Int32|] p;
    }
}", new TestParameters(options: FrameworkTypeInDeclaration));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotOnFrameworkTypeWithNoPredefinedKeywordEquivalent()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Method()
    {
        [|List|]<int> p;
    }
}", new TestParameters(options: FrameworkTypeInDeclaration));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotOnIdentifierThatIsNotTypeSyntax()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Program
{
    void Method()
    {
        int [|p|];
    }
}", new TestParameters(options: FrameworkTypeInDeclaration));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task QualifiedReplacementWhenNoUsingFound()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"class Program
{
    [|string|] _myfield = 5;
}";

            var expected =
@"class Program
{
    System.String _myfield = 5;
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = 
@"using System;
class Program
{
    [|int|] _myfield;
}";

            var expected =
@"using System;
class Program
{
    Int32 _myfield;
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldDeclarationWithInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    [|string|] _myfield = 5;
}";

            var expected =
@"using System;
class Program
{
    String _myfield = 5;
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DelegateDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    public delegate [|int|] PerformCalculation(int x, int y);
}";

            var expected =
@"using System;
class Program
{
    public delegate Int32 PerformCalculation(int x, int y);
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PropertyDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    public [|long|] MyProperty { get; set; }
}";

            var expected =
@"using System;
class Program
{
    public Int64 MyProperty { get; set; }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenericPropertyDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
using System.Collections.Generic;
class Program
{
    public List<[|long|]> MyProperty { get; set; }
}";

            var expected =
@"using System;
using System.Collections.Generic;
class Program
{
    public List<Int64> MyProperty { get; set; }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task QualifiedReplacementInGenericTypeParameter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System.Collections.Generic;
class Program
{
    public List<[|long|]> MyProperty { get; set; }
}";

            var expected =
@"using System.Collections.Generic;
class Program
{
    public List<System.Int64> MyProperty { get; set; }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MethodDeclarationReturnType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    public [|long|] Method() { }
}";

            var expected =
@"using System;
class Program
{
    public Int64 Method() { }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MethodDeclarationParameters()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    public void Method([|double|] d) { }
}";

            var expected =
@"using System;
class Program
{
    public void Method(Double d) { }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenericMethodInvocation()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    public void Method<T>() { }
    public void Test() { Method<[|int|]>(); }
}";

            var expected =
@"using System;
class Program
{
    public void Method<T>() { }
    public void Test() { Method<Int32>(); }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task LocalDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    void Method()
    {
        [|int|] f = 5;
    }
}";

            var expected =
@"using System;
class Program
{
    void Method()
    {
        Int32 f = 5;
    }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MemberAccess()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    void Method()
    {
        Console.Write([|int|].MaxValue);
    }
}";

            var expected =
@"using System;
class Program
{
    void Method()
    {
        Console.Write(Int32.MaxValue);
    }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInMemberAccess);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MemberAccess2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    void Method()
    {
        var x = [|int|].Parse(""1"");
    }
}";

            var expected =
@"using System;
class Program
{
    void Method()
    {
        var x = Int32.Parse(""1"");
    }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInMemberAccess);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DocCommentTriviaCrefExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    /// <see cref=""[|int|].MaxValue""/>
    void Method()
    {
    }
}";

            var expected =
@"using System;
class Program
{
    /// <see cref=""Int32.MaxValue""/>
    void Method()
    {
    }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInMemberAccess);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DefaultExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    void Method()
    {
        var v = default([|int|]);
    }
}";

            var expected =
@"using System;
class Program
{
    void Method()
    {
        var v = default(Int32);
    }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TypeOfExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    void Method()
    {
        var v = typeof([|int|]);
    }
}";

            var expected =
@"using System;
class Program
{
    void Method()
    {
        var v = typeof(Int32);
    }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NameOfExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    void Method()
    {
        var v = nameof([|int|]);
    }
}";

            var expected =
@"using System;
class Program
{
    void Method()
    {
        var v = nameof(Int32);
    }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FormalParametersWithinLambdaExression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    void Method()
    {
        Func<int, int> func3 = ([|int|] z) => z + 1;
    }
}";

            var expected =
@"using System;
class Program
{
    void Method()
    {
        Func<int, int> func3 = (Int32 z) => z + 1;
    }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DelegateMethodExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    void Method()
    {
        Func<int, int> func7 = delegate ([|int|] dx) { return dx + 1; };
    }
}";

            var expected =
@"using System;
class Program
{
    void Method()
    {
        Func<int, int> func7 = delegate (Int32 dx) { return dx + 1; };
    }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ObjectCreationExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    void Method()
    {
        string s2 = new [|string|]('c', 1);
    }
}";

            var expected =
@"using System;
class Program
{
    void Method()
    {
        string s2 = new String('c', 1);
    }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ArrayDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    void Method()
    {
        [|int|][] k = new int[4];
    }
}";

            var expected =
@"using System;
class Program
{
    void Method()
    {
        Int32[] k = new int[4];
    }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ArrayInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    void Method()
    {
        int[] k = new [|int|][] { 1, 2, 3 };
    }
}";

            var expected =
@"using System;
class Program
{
    void Method()
    {
        int[] k = new Int32[] { 1, 2, 3 };
    }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MultiDimentionalArrayAsGenericTypeParameter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
using System.Collections.Generic;
class Program
{
    void Method()
    {
        List<[|string|][][,][,,,]> a;
    }
}";

            var expected =
@"using System;
using System.Collections.Generic;
class Program
{
    void Method()
    {
        List<String[][,][,,,]> a;
    }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ForStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    void Method()
    {
        for ([|int|] j = 0; j < 4; j++) { }
    }
}";

            var expected =
@"using System;
class Program
{
    void Method()
    {
        for (Int32 j = 0; j < 4; j++) { }
    }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ForeachStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    void Method()
    {
        foreach ([|int|] item in new int[] { 1, 2, 3 }) { }
    }
}";

            var expected =
@"using System;
class Program
{
    void Method()
    {
        foreach (Int32 item in new int[] { 1, 2, 3 }) { }
    }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task LeadingTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    void Method()
    {
        // this is a comment
        [|int|] x = 5;
    }
}";

            var expected =
@"using System;
class Program
{
    void Method()
    {
        // this is a comment
        Int32 x = 5;
    }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseFrameworkType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TrailingTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"using System;
class Program
{
    void Method()
    {
        [|int|] /* 2 */ x = 5;
    }
}";

            var expected =
@"using System;
class Program
{
    void Method()
    {
        Int32 /* 2 */ x = 5;
    }
}";
            await TestInRegularAndScriptAsync(code, expected, options: FrameworkTypeInDeclaration);
        }
    }
}
