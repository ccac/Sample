///////////////////////////////////////////////////////////////////////////////
//
//  LiveReference.cs
//
// 
// © 2008 Microsoft Corporation. All Rights Reserved.
//
// This file is licensed as part of the Silverlight 2 SDK, for details look here: 
// http://go.microsoft.com/fwlink/?LinkID=111970&clcid=0x409
//
///////////////////////////////////////////////////////////////////////////////

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;

namespace System.Windows.Controls.Test
{
    /// <summary>
    /// LiveReference is used to track elements that are added to the testing
    /// surface canvas so they can automatically be removed once the test is
    /// complete.
    /// </summary>
    public sealed class LiveReference : IDisposable
    {
        private LiveReference() { }

        private SilverlightTest _testBase;

        /// <summary>
        /// Element added to the testing surface canvas.
        /// </summary>
        public UIElement Element { get; private set; }

        /// <summary>
        /// Initializes a new instance of the LiveReference class.
        /// </summary>
        /// <param name="element">
        /// Element to add to the testing surface canvas.
        /// </param>
        public LiveReference(SilverlightTest silverlightTest, UIElement element)
        {
            _testBase = silverlightTest;

            Assert.IsNotNull(_testBase);
            Assert.IsNotNull(_testBase.Silverlight);
            Assert.IsNotNull(_testBase.Silverlight.TestSurface);

            Element = element;
            //Silverlight.TestSurface.Children.Add(Element);
            _testBase.Silverlight.TestSurface.Children.Add(Element);
        }

        /// <summary>
        /// Remove the element from the testing surface canvas when finished
        /// </summary>
        public void Dispose()
        {
            Assert.IsNotNull(Element);
            Assert.IsNotNull(_testBase);
            Assert.IsNotNull(_testBase.Silverlight);
            Assert.IsNotNull(_testBase.Silverlight.TestSurface);
            bool removed = _testBase.Silverlight.TestSurface.Children.Remove(Element);
            Assert.IsTrue(removed);
        }
    }
}