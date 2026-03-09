// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Shared.TestHooks;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.Editor.UnitTests
{
    internal class TestForegroundNotificationService : IForegroundNotificationService
    {
        private readonly object _gate = new object();
        private readonly List<Task> _tasks = new List<Task>();
        private readonly SimpleTaskQueue _queue = new SimpleTaskQueue(TaskScheduler.Default);

        public void RegisterNotification(Func<bool> action, IAsyncToken asyncToken, CancellationToken cancellationToken = default(CancellationToken))
        {
            RegisterNotification(action, 0, asyncToken, cancellationToken);
        }

        public void RegisterNotification(Func<bool> action, int delayInMS, IAsyncToken asyncToken, CancellationToken cancellationToken = default(CancellationToken))
        {
            Task task;
            lock (_gate)
            {
                task = _queue.ScheduleTask(() => Execute_NoLock(action, asyncToken, cancellationToken), cancellationToken);
                _tasks.Add(task);
            }

#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
            task.Wait(cancellationToken);
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
        }

        private void Execute_NoLock(Func<bool> action, IAsyncToken asyncToken, CancellationToken cancellationToken)
        {
            if (action())
            {
                asyncToken.Dispose();
            }
            else
            {
                _tasks.Add(_queue.ScheduleTask(() => Execute_NoLock(action, asyncToken, cancellationToken), cancellationToken));
            }
        }

        public void RegisterNotification(Action action, IAsyncToken asyncToken, CancellationToken cancellationToken = default(CancellationToken))
        {
            RegisterNotification(action, 0, asyncToken, cancellationToken);
        }

        public void RegisterNotification(Action action, int delayInMS, IAsyncToken asyncToken, CancellationToken cancellationToken = default(CancellationToken))
        {
            Task task;
            lock (_gate)
            {
                task = _queue.ScheduleTask(() =>
                {
                    action();
                }, cancellationToken).CompletesAsyncOperation(asyncToken);

                _tasks.Add(task);
            }

#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
            task.Wait(cancellationToken);
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
        }
    }
}
