using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyJwtTokenAuthentication.Interfaces
{
    public interface IJwtTokenGenerator
    {
        /*
         Interfaces can't have static members and static methods can not be used as implementation of interface methods.
        */
        //public static string GenerateToken(string username, string role, int expirationMinutes = 30);//This is a compile error

        /*
         * you can define  static method as below          
         */
        static string GetHello() => "Default Hello from IJwtTokenGenerator interface";
        static void WriteWorld() => Console.WriteLine("Writing World from IJwtTokenGenerator interface");


        /*
         * C# 8 Allows Static Members on Interfaces as below
         */
        public static string Something = "something";

        /*
        Starting from C# 11 and .NET 7, it is possible to have static members in interfaces without having to provide a default implementation.
        see below example
        */
       //public static abstract string GenerateToken(string username, string role, int expirationMinutes = 30);
       public string GenerateToken(string username, string role, int expirationMinutes = 30);
    }
}
