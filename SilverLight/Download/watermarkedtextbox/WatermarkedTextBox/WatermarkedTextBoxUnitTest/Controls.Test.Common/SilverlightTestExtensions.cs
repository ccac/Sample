///////////////////////////////////////////////////////////////////////////////
//
//  SilverlightTestExtensions.cs
//
// 
// © 2008 Microsoft Corporation. All Rights Reserved.
//
// This file is licensed as part of the Silverlight 2 SDK, for details look here: 
// http://go.microsoft.com/fwlink/?LinkID=111970&clcid=0x409
//
///////////////////////////////////////////////////////////////////////////////

using Microsoft.Silverlight.Testing;

namespace System.Windows.Controls.Test
{
    public static class SilverlightTestExtensions
    {
        /// <summary>
        /// Enqueue to step allow (for example) event handlers a chance to execute.
        /// </summary>
        /// <param name="test"></param>
        public static void EnqueueYieldThread(this SilverlightTest test)
        {
            test.EnqueueSleep(0);
        }
    }
}
