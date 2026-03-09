using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;
using static Microsoft.CodeAnalysis.CSharp.Completion.Providers.DeclarationNameCompletionProvider;
using static Microsoft.CodeAnalysis.Diagnostics.Analyzers.NamingStyles.SymbolSpecification;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.DeclarationInfoTests
{
    [UseExportProvider]
    public class DeclarationNameCompletion_ContextTests
    {
        protected CSharpTestWorkspaceFixture fixture = new CSharpTestWorkspaceFixture();

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterTypeInClass1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    int $$
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Field),
                new SymbolKindOrTypeKind(SymbolKind.Property),
                new SymbolKindOrTypeKind(MethodKind.Ordinary));
            await VerifyNoModifiers(markup);
            await VerifyTypeName(markup, "int");
            await VerifyAccessibility(markup, null);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterTypeInClassWithAccessibility()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    public int $$
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Field),
                new SymbolKindOrTypeKind(SymbolKind.Property),
                new SymbolKindOrTypeKind(MethodKind.Ordinary));
            await VerifyNoModifiers(markup);
            await VerifyTypeName(markup, "int");
            await VerifyAccessibility(markup, Accessibility.Public);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterTypeInClassVirtual()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    public virtual int $$
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Property),
                new SymbolKindOrTypeKind(MethodKind.Ordinary));
            await VerifyModifiers(markup, new DeclarationModifiers(isVirtual: true));
            await VerifyTypeName(markup, "int");
            await VerifyAccessibility(markup, Accessibility.Public);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterTypeInClassStatic()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    private static int $$
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Field),
                new SymbolKindOrTypeKind(SymbolKind.Property),
                new SymbolKindOrTypeKind(MethodKind.Ordinary));
            await VerifyModifiers(markup, new DeclarationModifiers(isStatic: true));
            await VerifyTypeName(markup, "int");
            await VerifyAccessibility(markup, Accessibility.Private);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterTypeInClassConst()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    private const int $$
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Field));
            await VerifyModifiers(markup, new DeclarationModifiers(isConst: true));
            await VerifyTypeName(markup, "int");
            await VerifyAccessibility(markup, Accessibility.Private);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task VariableDeclaration1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    void goo()
    {
        int $$
    }
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Local),
                new SymbolKindOrTypeKind(MethodKind.LocalFunction));
            await VerifyModifiers(markup, new DeclarationModifiers());
            await VerifyTypeName(markup, "int");
            await VerifyAccessibility(markup, null);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task VariableDeclaration2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    void goo()
    {
        int c1, $$
    }
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Local));
            await VerifyModifiers(markup, new DeclarationModifiers());
            await VerifyTypeName(markup, "int");
            await VerifyAccessibility(markup, null);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ReadonlyVariableDeclaration1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    void goo()
    {
        readonly int $$
    }
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Local),
                new SymbolKindOrTypeKind(MethodKind.LocalFunction));
            await VerifyModifiers(markup, new DeclarationModifiers(isReadOnly: true));
            await VerifyTypeName(markup, "int");
            await VerifyAccessibility(markup, null);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ReadonlyVariableDeclaration2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    void goo()
    {
        readonly int c1, $$
    }
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Local));
            await VerifyModifiers(markup, new DeclarationModifiers(isReadOnly: true));
            await VerifyTypeName(markup, "int");
            await VerifyAccessibility(markup, null);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task UsingVariableDeclaration1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    void M()
    {
        using (int i$$
    }
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Local));
            await VerifyModifiers(markup, new DeclarationModifiers());
            await VerifyTypeName(markup, "int");
            await VerifyAccessibility(markup, null);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task UsingVariableDeclaration2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    void M()
    {
        using (int i1, $$
    }
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Local));
            await VerifyModifiers(markup, new DeclarationModifiers());
            await VerifyTypeName(markup, "int");
            await VerifyAccessibility(markup, null);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ForVariableDeclaration1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    void M()
    {
        for (int i$$
    }
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Local));
            await VerifyModifiers(markup, new DeclarationModifiers());
            await VerifyTypeName(markup, "int");
            await VerifyAccessibility(markup, null);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ForVariableDeclaration2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    void M()
    {
        for (int i1, $$
    }
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Local));
            await VerifyModifiers(markup, new DeclarationModifiers());
            await VerifyTypeName(markup, "int");
            await VerifyAccessibility(markup, null);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ForEachVariableDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    void M()
    {
        foreach (int $$
    }
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Local));
            await VerifyModifiers(markup, new DeclarationModifiers());
            await VerifyTypeName(markup, "int");
            await VerifyAccessibility(markup, null);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Parameter1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    void goo(C $$
    }
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Parameter));
            await VerifyModifiers(markup, new DeclarationModifiers());
            await VerifyTypeName(markup, "global::C");
            await VerifyAccessibility(markup, null);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Parameter2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    void goo(C c1, C $$
    }
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Parameter));
            await VerifyModifiers(markup, new DeclarationModifiers());
            await VerifyTypeName(markup, "global::C");
            await VerifyAccessibility(markup, null);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ParameterAfterPredefinedType1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    void goo(string $$
    }
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Parameter));
            await VerifyModifiers(markup, new DeclarationModifiers());
            await VerifyTypeName(markup, "string");
            await VerifyAccessibility(markup, null);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ParameterAfterPredefinedType2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    void goo(C c1, string $$
    }
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Parameter));
            await VerifyModifiers(markup, new DeclarationModifiers());
            await VerifyTypeName(markup, "string");
            await VerifyAccessibility(markup, null);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ParameterAfterGeneric()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System.Collections.Generic;
class C
{
    void goo(C c1, List<string> $$
    }
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Parameter));
            await VerifyModifiers(markup, new DeclarationModifiers());
            await VerifyTypeName(markup, "global::System.Collections.Generic.List<string>");
            await VerifyAccessibility(markup, null);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ClassTypeParameter1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C<$$
{
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.TypeParameter));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ClassTypeParameter2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C<T1, $$
{
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.TypeParameter));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ModifierExclusion1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    readonly int $$
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Field));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ModifierExclusion2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    const int $$
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Field));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ModifierExclusion3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    abstract int $$
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Property),
                new SymbolKindOrTypeKind(MethodKind.Ordinary));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ModifierExclusion4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    virtual int $$
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Property),
                new SymbolKindOrTypeKind(MethodKind.Ordinary));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ModifierExclusion5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    sealed int $$
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Property),
                new SymbolKindOrTypeKind(MethodKind.Ordinary));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ModifierExclusion6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    override int $$
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Property),
                new SymbolKindOrTypeKind(MethodKind.Ordinary));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ModifierExclusion7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    async int $$
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(MethodKind.Ordinary));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ModifierExclusion8()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            // Note that the async is not included in the incomplete member syntax
            var markup = @"
class C
{
    partial int $$
}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Field),
                new SymbolKindOrTypeKind(SymbolKind.Property),
                new SymbolKindOrTypeKind(MethodKind.Ordinary));
        }

        [Theory, Trait(Traits.Feature, Traits.Features.Completion)]
        [InlineData("int")]
        [InlineData("C")]
        [InlineData("List<string>")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ModifierExclusionInsideMethod_Const(string type)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = $@"
using System.Collections.Generic;
class C
{{
    void M()
    {{
        const {type} $$
    }}
}}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Local));
        }

        [Theory, Trait(Traits.Feature, Traits.Features.Completion)]
        [InlineData("int")]
        [InlineData("C")]
        [InlineData("List<string>")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ModifierExclusionInsideMethod_ConstLocalDeclaration(string type)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = $@"
using System.Collections.Generic;
class C
{{
    void M()
    {{
        const {type} v$$ = default;
    }}
}}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Local));
        }

        [Theory, Trait(Traits.Feature, Traits.Features.Completion)]
        [InlineData("int")]
        [InlineData("C")]
        [InlineData("List<string>")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ModifierExclusionInsideMethod_ConstLocalFunction(string type)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = $@"
using System.Collections.Generic;
class C
{{
    void M()
    {{
        const {type} v$$()
        {{
        }}
    }}
}}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Local));
        }

        [Theory, Trait(Traits.Feature, Traits.Features.Completion)]
        [InlineData("int")]
        [InlineData("C")]
        [InlineData("List<string>")]
        public async Task ModifierExclusionInsideMethod_Async(string type)
        {
            // This only works with a partially written name.
            // Because async is not a keyword, the syntax tree when the name is missing is completely broken
            // in that there can be multiple statements full of missing and skipped tokens depending on the type syntax.
            var markup = $@"
using System.Collections.Generic;
class C
{{
    void M()
    {{
        async {type} v$$
    }}
}}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(MethodKind.LocalFunction));
        }

        [Theory, Trait(Traits.Feature, Traits.Features.Completion)]
        [InlineData("int")]
        [InlineData("C")]
        [InlineData("List<string>")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ModifierExclusionInsideMethod_AsyncLocalDeclaration(string type)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = $@"
using System.Collections.Generic;
class C
{{
    void M()
    {{
        async {type} v$$ = default;
    }}
}}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(MethodKind.LocalFunction));
        }

        [Theory, Trait(Traits.Feature, Traits.Features.Completion)]
        [InlineData("int")]
        [InlineData("C")]
        [InlineData("List<string>")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ModifierExclusionInsideMethod_AsyncLocalFunction(string type)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = $@"
using System.Collections.Generic;
class C
{{
    void M()
    {{
        async {type} v$$()
        {{
        }}
    }}
}}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(MethodKind.LocalFunction));
        }

        [Theory, Trait(Traits.Feature, Traits.Features.Completion)]
        [InlineData("int")]
        [InlineData("C")]
        [InlineData("List<string>")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ModifierExclusionInsideMethod_Unsafe(string type)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = $@"
using System.Collections.Generic;
class C
{{
    void M()
    {{
        unsafe {type} $$
    }}
}}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(MethodKind.LocalFunction));
        }

        [Theory, Trait(Traits.Feature, Traits.Features.Completion)]
        [InlineData("int")]
        [InlineData("C")]
        [InlineData("List<string>")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ModifierExclusionInsideMethod_UnsafeLocalDeclaration(string type)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = $@"
using System.Collections.Generic;
class C
{{
    void M()
    {{
        unsafe {type} v$$ = default;
    }}
}}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(MethodKind.LocalFunction));
        }

        [Theory, Trait(Traits.Feature, Traits.Features.Completion)]
        [InlineData("int")]
        [InlineData("C")]
        [InlineData("List<string>")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ModifierExclusionInsideMethod_UnsafeLocalFunction(string type)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = $@"
using System.Collections.Generic;
class C
{{
    void M()
    {{
        unsafe {type} v$$()
        {{
        }}
    }}
}}
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(MethodKind.LocalFunction));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task LocalInsideMethod1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
namespace ConsoleApp1
{
    class ReallyLongClassName { }
    class Program
    {
        static void Main(string[] args)
        {
            ReallyLongClassName $$
        }
    }
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Local),
                new SymbolKindOrTypeKind(MethodKind.LocalFunction));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task LocalInsideMethod2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
namespace ConsoleApp1
{
    class ReallyLongClassName<T> { }
    class Program
    {
        static void Main(string[] args)
        {
            ReallyLongClassName<int> $$
        }
    }
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Local),
                new SymbolKindOrTypeKind(MethodKind.LocalFunction));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task LocalInsideMethodAfterPredefinedTypeKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
namespace ConsoleApp1
{
    class ReallyLongClassName { }
    class Program
    {
        static void Main(string[] args)
        {
            string $$
        }
    }
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Local),
                new SymbolKindOrTypeKind(MethodKind.LocalFunction));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task LocalInsideMethodAfterArray()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] $$
        }
    }
";
            await VerifySymbolKinds(markup,
                new SymbolKindOrTypeKind(SymbolKind.Local),
                new SymbolKindOrTypeKind(MethodKind.LocalFunction));
        }

#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        private async Task VerifyNoType(string markup)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var result = await GetResultsAsync(markup);
            Assert.Null(result.Type);
        }

#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        private async Task VerifyTypeName(string markup, string typeName)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var result = await GetResultsAsync(markup);
            Assert.Equal(typeName, result.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
        }

#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        private async Task VerifyNoModifiers(string markup)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var result = await GetResultsAsync(markup);
            Assert.Equal(default(DeclarationModifiers), result.Modifiers);
        }

#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        private async Task VerifySymbolKinds(string markup, params SymbolKindOrTypeKind[] expectedSymbolKinds)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var result = await GetResultsAsync(markup);
            Assert.True(expectedSymbolKinds.SequenceEqual(result.PossibleSymbolKinds));
        }

#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        private async Task VerifyModifiers(string markup, DeclarationModifiers modifiers)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var result = await GetResultsAsync(markup);
            Assert.Equal(modifiers, result.Modifiers);
        }

#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        private async Task VerifyAccessibility(string markup, Accessibility? accessibility)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var result = await GetResultsAsync(markup);
            Assert.Equal(accessibility, result.DeclaredAccessibility);
        }

        private async Task<NameDeclarationInfo> GetResultsAsync(string markup)
        {
            var (document, position) = ApplyChangesToFixture(markup);
            var result = await NameDeclarationInfo.GetDeclarationInfo(document, position, CancellationToken.None);
            return result;
        }

        private (Document, int) ApplyChangesToFixture(string markup)
        {
            MarkupTestFile.GetPosition(markup, out var text, out int position);
            return (fixture.UpdateDocument(text, SourceCodeKind.Regular), position);
        }
    }
}
