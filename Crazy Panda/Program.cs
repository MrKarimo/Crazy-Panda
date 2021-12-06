using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
 
 Дан List<uint> list и некое число ulong sum. Ожидаемое количество элементов в list - несколько миллионов.
Необходимо написать метод FindElementsForSum, который сможет найти наименьшие индексы start и end такие,
что сумма элементов list начиная с индекса start включительно и до индекса end не включительно будет в точности равна sum.
Если таких start и end нельзя найти, то установить start и end равными 0. Решение предоставить в виде метода.
Сигнаруту и название метода менять нельзя, только тело.

public void FindElementsForSum(List<uint> list, ulong sum, out int start, out int end)
{
	// your code here
}
  */

namespace Crazy_Panda
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                int start;
                int end;
                Console.WriteLine("Должно быть:\nstart будет равен 5 и end 7");
                FindElementsForSum(new List<uint> { 0, 1, 2, 3, 4, 5, 6, 7 }, 11, out start, out end); //start будет равен 5 и end 7;
                Console.WriteLine("start: {0}, end: {1}\n", start, end);

                start = 0; end = 0;
                Console.WriteLine("Должно быть:\nstart будет равен 1 и end 4");
                FindElementsForSum(new List<uint> { 4, 5, 6, 7 }, 18, out start, out end); //start будет равен 1 и end 4;
                Console.WriteLine("start: {0}, end: {1}\n", start, end);

                start = 0; end = 0;
                Console.WriteLine("Должно быть:\nstart будет равен 0 и end 0");
                FindElementsForSum(new List<uint> { 0, 1, 2, 3, 4, 5, 6, 7 }, 88, out start, out end); //start будет равен 0 и end 0;
                Console.WriteLine("start: {0}, end: {1}\n", start, end);

                //-------------------------
                //Проверка на числа больше uint.max
                Console.WriteLine("Проверка на числа больше uint.max");
                start = 0; end = 0;
                Console.WriteLine("Должно быть:\nstart будет равен 2 и end 6");
                FindElementsForSum(new List<uint> { 4294967295, 4294967231, 845967295, 845967295, 4294737195, 4294924975, 50967295, 1323967295 }, 10281596760, out start, out end); //start будет равен 2 и end 6;
                Console.WriteLine("start: {0}, end: {1}\n", start, end);

                //Проверка на сумму обратных последовательностей
                Console.WriteLine("Проверка на сумму обратных последовательностей");
                start = 0; end = 0;
                Console.WriteLine("Должно быть:\nstart будет равен 0 и end 6, с учетом ноля");
                FindElementsForSum(new List<uint> { 0, 1, 2, 3, 4, 5, 6, 7, 6, 5, 4, 3, 2, 1, 0 }, 15, out start, out end); //start будет равен 0 и end 6 с учетом ноля;
                Console.WriteLine("start: {0}, end: {1}\n", start, end);

                //Проверка на сумму одного числа
                Console.WriteLine("Проверка на сумму одного числа");
                start = 0; end = 0;
                Console.WriteLine("Должно быть:\nstart будет равен 6 и end 7");
                FindElementsForSum(new List<uint> { 0, 0, 2, 1, 0, 6, 100, 4, 2, 3, 3, 4, 0, 0, 0 }, 100, out start, out end); //start будет равен 6 и end 7;
                Console.WriteLine("start: {0}, end: {1}\n", start, end);

                //Проверка на границы
                Console.WriteLine("Проверка на границы");
                start = 0; end = 0;
                Console.WriteLine("Должно быть:\nstart будет равен 3 и end 11");
                FindElementsForSum(new List<uint> { 0, 0, 2, 1, 0, 0, 0, 4, 2, 3, 3, 4, 0, 0, 0 }, 13, out start, out end); //start будет равен 3 и end 11;
                Console.WriteLine("start: {0}, end: {1}\n", start, end);

                //Проверка на время выполнения
                Console.WriteLine("Проверка на время выполнения");
                start = 0; end = 0;
                Random random = new Random();
                var listCount = random.Next(100, 900);
                List<uint> list = new List<uint>();
                for (int i = 0; i < listCount; ++i) list.Add((uint)random.Next(1, 1000));
                var sum = Sum(list, 0, list.Count - 1);
                Console.WriteLine("Должно быть:\nstart будет равен {0} и end {1}", 0, list.Count);
                Stopwatch sWatch = new Stopwatch();
                Console.WriteLine("FindElementsForSum");
                sWatch.Start();
                FindElementsForSum(list, sum, out start, out end);
                sWatch.Stop();
                Console.WriteLine("Результат:\nstart: {0}, end: {1}", start, end);
                Console.WriteLine("Время выполнения:" + sWatch.ElapsedMilliseconds.ToString() + "ms");
                Console.WriteLine("FindElementsForSum_NotOptimal");
                start = 0; end = 0;
                sWatch.Start();
                FindElementsForSum_NotOptimal(list, sum, out start, out end);
                sWatch.Stop();
                Console.WriteLine("Результат:\nstart: {0}, end: {1}", start, end);
                Console.WriteLine("Время выполнения:" + sWatch.ElapsedMilliseconds.ToString() + "ms\n");
            } while (string.IsNullOrEmpty(Console.ReadLine()));
        }

        static public void FindElementsForSum(List<uint> list, ulong sum, out int start, out int end)
        {
            start = 0;
            ulong _sum = 0;
            for (end = 0; end < list.Count; ++end)
            {
                _sum += list[end];
                while (_sum > sum)
                {
                    _sum -= list[start];
                    ++start;
                }
                if (_sum == sum)
                {
                    ++end;//Не известно, что делать если индекс вышел за границы массива. По условию задачи [end] не должен быть включен в последовательность
                    break;
                }
            }

            if (_sum != sum)
            {
                start = 0;
                end = 0;
            }
        }

        static private ulong Sum(List<uint> list, int start, int end)
        {
            ulong sum = 0;
            foreach (var item in list.GetRange(start, end - start + 1))
                sum += item;
            return sum;
        }

        static public void FindElementsForSum_NotOptimal(List<uint> list, ulong sum, out int start, out int end)
        {
           for(int range = 0; range <= list.Count; ++range)
            {
                start = 0;
                end = start + range;

                while(end < list.Count)
                {
                    if (sum == Sum(list, start, end))
                    {
                        ++end;
                        return;
                    }
                    ++start;
                    ++end;
                }

            }
            start = 0;
            end = 0;
        }
    }
}
