// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.CodeFixes.FullyQualify;
using Microsoft.CodeAnalysis.CSharp.Diagnostics;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.FullyQualify
{
    public class FullyQualifyUnboundIdentifierTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace)
            => (new CSharpUnboundIdentifiersDiagnosticAnalyzer(), new CSharpFullyQualifyCodeFixProvider());

        protected override ImmutableArray<CodeAction> MassageActions(ImmutableArray<CodeAction> actions)
            => FlattenActions(actions);

        [WorkItem(26887, "https://github.com/dotnet/roslyn/issues/26887")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFullyQualifyUnboundIdentifier1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"public class Program
{
    public class Inner
    {
    }
}

class Test
{
    [|Inner|]
}",
@"public class Program
{
    public class Inner
    {
    }
}

class Test
{
    Program.Inner
}");
        }

        [WorkItem(26887, "https://github.com/dotnet/roslyn/issues/26887")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFullyQualifyUnboundIdentifier2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"public class Program
{
    public class Inner
    {
    }
}

class Test
{
    public [|Inner|]
}",
@"public class Program
{
    public class Inner
    {
    }
}

class Test
{
    public Program.Inner
}");
        }
    }
}
