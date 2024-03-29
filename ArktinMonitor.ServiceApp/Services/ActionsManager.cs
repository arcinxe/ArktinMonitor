﻿using ArktinMonitor.Helpers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ArktinMonitor.ServiceApp.Services
{
    public static class ActionsManager
    {
        private static readonly Queue<Action> ActionsQueue = new Queue<Action>();
        private static bool _active;
        private static Task _task;

        /// <summary>
        ///     Adds the action to the end of the queue.
        /// </summary>
        /// <param name="action">Action to be called</param>
        public static void EnqueuNewAction(Action action)
        {
            ActionsQueue.Enqueue(action);
        }

        /// <summary>
        ///     Starts the execution of the actions in the ActionsQueue.
        /// </summary>
        public static void Start()
        {
            _active = true;
            _task = Task.Run(() => InvokeActions());
        }

        /// <summary>
        ///     Stops the execution of the actions in the ActionsQueue.
        /// </summary>
        public static void Stop()
        {
            _active = false;
            _task.Wait();
            _task.Dispose();
        }

        private static void InvokeActions()
        {
            while (_active)
            {
                Thread.Sleep(1000);
                if (ActionsQueue.Count <= 0) continue;
                try
                {
                    var action = ActionsQueue.Dequeue();
                    action?.Invoke();
                    //LocalLogger.Log($"{ActionsQueue.Count} actions waiting in queue..");
                }
                catch (Exception e)
                {
                    LocalLogger.Log($"{nameof(ActionsManager)} > {nameof(InvokeActions)}", e);
                    _active = false;
                }
            }
        }
    }
}