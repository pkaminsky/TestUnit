using System;
using System.Linq.Expressions;

namespace TestUnit.ConsoleRunner {
    class Program {
        static void Main(string[] args) {

            bool @true = true;
            bool @false = false;
            int a = 5;
            int b = 4;

            Test(() => @true);
            Test(() => @false);
            Test(() => a == 5);
            Test(() => a == b);
            Test(() => ComputeStrValue1().Equals(ComputeStrValue0()));

            Console.ReadLine();
        }

        static string ComputeStrValue0() => "myopia";
        static string ComputeStrValue1() => "astigmatism";

        static void Test(Expression<Func<bool>> test) {
            try {
                Assert.True(test);
                Console.WriteLine("Test Passed");
            }
            catch (Exception ex) {
                Console.WriteLine("Test failed with exception: " + ex.Message);
            }
        }
    }
}