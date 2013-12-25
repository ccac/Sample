///////////////////////////////////////////////////////////////////////////////
//
//  SilverlightControlTest.cs
//
// 
// © 2008 Microsoft Corporation. All Rights Reserved.
//
// This file is licensed as part of the Silverlight 2 SDK, for details look here: 
// http://go.microsoft.com/fwlink/?LinkID=111970&clcid=0x409
//
///////////////////////////////////////////////////////////////////////////////

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Windows.Controls.Test
{
    /// <summary>
    /// Unit tests for Silverlight controls.
    /// </summary>
    public abstract class SilverlightControlTest : SilverlightTest
    {
        /// <summary>
        /// Number of milliseconds to wait between actions in CreateAsyncTest.
        /// </summary>
        public static int VisualDelayInMilliseconds = 100;

        /// <summary>
        /// Add an element to the test surface and perform a series of test
        /// actions with a pause in between each allowing the test surface to be
        /// updated.  This task does not complete the async test and a call to
        /// EnqueueTestCompleted is still required.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <param name="actions">Test actions.</param>
        /// <remarks>
        /// CreateAsyncTask should only be invoked via test methods with the
        /// AsynchronousAttribute applied.
        /// </remarks>
        protected void CreateAsyncTask(FrameworkElement element, params Action[] actions)
        {
            Assert.IsNotNull(element);
            actions = actions ?? new Action[] { };

            // Add a handler to determine when the element is loaded
            bool isLoaded = false;
            element.Loaded += delegate { isLoaded = true; };

            // Add the element to the test surface and wait until it's loaded
            Silverlight.TestSurface.Children.Add(element);
            EnqueueConditional(() => isLoaded);

            // Perform the test actions
            foreach (Action action in actions)
            {
                Action capturedAction = action;
                EnqueueCallback(() => capturedAction());
                EnqueueSleep(VisualDelayInMilliseconds);
            }

            // Remove the element from the test surface and finish the test
            EnqueueCallback(() => Silverlight.TestSurface.Children.Remove(element));
        }

        /// <summary>
        /// Add an element to the test surface and perform a series of test
        /// actions with a pause in between each allowing the test surface to be
        /// updated.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <param name="actions">Test actions.</param>
        /// <remarks>
        /// CreateAsyncTest should only be invoked via test methods with the
        /// AsynchronousAttribute applied.
        /// </remarks>
        protected void CreateAsyncTest(FrameworkElement element, params Action[] actions)
        {
            CreateAsyncTask(element, actions);
            EnqueueTestComplete();
        }
    }
}