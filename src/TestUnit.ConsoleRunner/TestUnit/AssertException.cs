using System;

namespace TestUnit {
    public class AssertException : Exception {
        public AssertException(string message) : base(message) { }
    }
}
