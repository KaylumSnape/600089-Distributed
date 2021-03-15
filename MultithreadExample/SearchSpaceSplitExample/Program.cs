﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchSpaceSplitExample
{
    // 10 threads, each increment by 10 to split it up.
    // 1 - 11 - 21 etc..
    // 2 - 12 - 22 ...
    // 3 - 13 - 33 ...
    class Program
    {
        static Dictionary<int, double> squareRoots = new Dictionary<int, double>();
        static void Main(string[] args)
        {
            Console.WriteLine("Please wait while square roots are calculated...");
            int numberToCalculate = 10000000;
            
            for (int i = 0; i < numberToCalculate; i++)
            {
                squareRoots.Add(i, 0);
            }

            int step = 10; // Add on 10 and work out what the square root of that is.
            Task[] tasks = new Task[step];
            for (int i = 0; i < step; i++)
            {
                // This has the potential to be a 'gotcha'
                // As Task.Run requires a thread to be allocated from the pool, etc. i may be updated by the loop by the time the invoke is called
                // Remove taskStartValue and pass i to CalculateSqrt to see what I mean...
                int taskStartValue = i;
                // New threads start working on this work, while we are still going round building the rest, caution.
                tasks[i] = Task.Run(() => CalculateSqrt(taskStartValue, step, numberToCalculate));
            }

            Task.WaitAll(tasks);
            
            while(true)
            {
                Console.WriteLine("Enter an integer (0-{0}) to find its square root...", numberToCalculate);
                Console.WriteLine(squareRoots[int.Parse(Console.ReadLine())]);
            }
        }

        public static void CalculateSqrt(int start, int step, int stop)
        {
            // Split the search based on a step size
            for (int i = start; i < stop; i += step)
            {
                squareRoots[i] = SqrtByAlogorithm(i);
            }
        }

        // From: http://www.softwareandfinance.com/CSharp/Find_SQRT_Algorithm.html
        public static double SqrtByAlogorithm(double x)
        {
            long numeric = (long)x;
            long n = numeric;
            long fraction = (long)((x - numeric) * 1000000); // 6 digits
            long f = fraction;
            int numdigits = 0, fnumdigits = 0, currdigits = 0;
            int tempresult = 0;
            int bOdd = 0, part = 0, tens = 1;
            int fractioncount = 0;
            double result = 0;
            int k, f1, f2, i, num, temp, quotient;

            for (numdigits = 0; n >= 10; numdigits++)
            {
                n = (n / 10);
            }
            numdigits++;
            for (fnumdigits = 0; f >= 10; fnumdigits++)
            {
                f = (f / 10);
            }
            fnumdigits++;
            if ((numdigits % 2) == 1)
            {
                bOdd = 1;
            }
            while (true)
            {
                tens = 1;
                currdigits = (bOdd == 1) ? (numdigits - 1) : (numdigits - 2);
                for (k = 0; k < currdigits; k++)
                {
                    tens *= 10;
                }
                part = (int)numeric / tens;
                num = part;
                quotient = tempresult * 2;
                i = 0;
                temp = 0;
                for (i = 1; ; i++)
                {
                    if (quotient == 0)
                    {
                        if (num - i * i < 0)
                        {
                            tempresult = (i - 1);
                            break;
                        }
                    }
                    else
                    {
                        temp = quotient * 10 + i;
                        if (num - i * temp < 0)
                        {
                            tempresult = quotient / 2 * 10 + i - 1;
                            break;
                        }
                    }
                }
                f1 = tempresult / 10;
                f2 = tempresult % 10;
                if (f1 == 0)
                {
                    numeric = numeric - (tempresult * tempresult * tens);
                }
                else
                {
                    numeric = numeric - ((f1 * 2 * 10 + f2) * f2 * tens);
                }
                if (numeric == 0 && fraction == 0)
                {
                    if (currdigits > 0)
                    {
                        // Handle the Zero case
                        tens = 1;
                        currdigits = currdigits / 2;
                        for (k = 0; k < currdigits; k++)
                        {
                            tens *= 10;
                        }
                        tempresult *= tens;
                    }
                    break;
                }
                if (bOdd == 1)
                {
                    numdigits -= 1;
                    bOdd = 0;
                }
                else
                {
                    numdigits -= 2;
                }
                if (numdigits <= 0)
                {
                    if (numeric > 0 || fraction > 0)
                    {
                        if (fractioncount >= 5)
                        {
                            break;
                        }
                        fractioncount++;
                        numeric *= 100;
                        if (fraction > 0)
                        {
                            // Handle the fraction part for real numbers
                            fnumdigits -= 2;
                            tens = 1;
                            for (k = 0; k < fnumdigits; k++)
                            {
                                tens *= 10;
                            }
                            numeric += fraction / tens;
                            fraction = fraction % tens;
                        }
                        numdigits += 2;
                    }
                    else
                        break;
                }
            }
            if (fractioncount == 0)
            {
                result = tempresult;
            }
            else
            {
                tens = 1;
                for (k = 0; k < fractioncount; k++)
                {
                    tens *= 10;
                }
                result = (double)tempresult / tens;
            }
            return result;
        }
    }
}
