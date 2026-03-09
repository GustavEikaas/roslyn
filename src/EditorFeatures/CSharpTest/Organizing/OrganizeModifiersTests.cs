// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Organizing
{
    public class OrganizeModifiersTests : AbstractOrganizerTests
    {
        [Fact, Trait(Traits.Feature, Traits.Features.Organizing)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTypes1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var initial =
@"static public class C {
}";
            var final =
@"public static class C {
}";

            await CheckAsync(initial, final);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Organizing)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTypes2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var initial =
@"public static class D {
}";
            var final =
@"public static class D {
}";

            await CheckAsync(initial, final);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Organizing)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTypes3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var initial =
@"public static partial class E {
}";
            var final =
@"public static partial class E {
}";

            await CheckAsync(initial, final);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Organizing)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTypes4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var initial =
@"static public partial class F {
}";
            var final =
@"public static partial class F {
}";

            await CheckAsync(initial, final);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Organizing)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTypes5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var initial =
@"unsafe public static class F {
}";
            var final =
@"public static unsafe class F {
}";

            await CheckAsync(initial, final);
        }
    }
}
