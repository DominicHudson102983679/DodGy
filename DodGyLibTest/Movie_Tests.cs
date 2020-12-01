using System;
using Xunit;
using DodGy;

namespace DodGyLibTest
{
    public class Movie_Tests
    {

        /*

        Test run for C:\Users\User\Documents\GitHub\DodGy\DodGy\DodGyLibTest\bin\Debug\netcoreapp3.1\DodGyLibTest.dll(.NETCoreApp,Version=v3.1)
        Microsoft (R) Test Execution Command Line Tool Version 16.7.0
        Copyright (c) Microsoft Corporation.  All rights reserved.
        Starting test execution, please wait...
        A total of 1 test files matched the specified pattern.

        Test Run Successful.
        Total tests: 2
        Passed: 2
        Total time: 14.5307 Seconds

        */

        [Fact]
        public void TestNumActors() {

            

            Movie m1 = new Movie(122, "The Lord of the Rings: The Return of the King", 2003, 201);
            Movie m2 = new Movie(209112, "Batman v Superman: Dawn of Justice", 2016, 151);
            Movie m3 = new Movie(293660, "Deadpool", 2016, 108);
            Movie m4 = new Movie(24428, "The Avengers", 2012, 143);
            Movie m5 = new Movie(1726, "Iron Man", 2008, 126);

            Assert.Equal(20, m1.NumActors(m1.MovieNo));
            Assert.Equal(10, m2.NumActors(m2.MovieNo));
            Assert.Equal(2, m3.NumActors(m3.MovieNo));
            Assert.Equal(14, m4.NumActors(m4.MovieNo));
            Assert.Equal(8, m5.NumActors(m5.MovieNo));
        }

        [Fact]
        public void TestGetAge() {
            Movie m1 = new Movie(1541, "Thelma & Louise", 1991, 130);
            Movie m2 = new Movie(680, "Pulp Fiction", 1994, 154);
            Movie m3 = new Movie(9824, "Mystery Men", 1999, 121);
            Movie m4 = new Movie(275, "Fargo", 1996, 98);
            Movie m5 = new Movie(44214, "Black Swan", 2010, 108);

            Assert.Equal(29, m1.GetAge(m1.MovieNo));
            Assert.Equal(26, m2.GetAge(m2.MovieNo));
            Assert.Equal(21, m3.GetAge(m3.MovieNo));
            Assert.Equal(24, m4.GetAge(m4.MovieNo));
            Assert.Equal(10, m5.GetAge(m5.MovieNo));
        }
    }
}