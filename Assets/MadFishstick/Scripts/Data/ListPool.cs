using System;
using System.Collections.Generic;

namespace MF
{
    public class ListPool<T>
    {
        static Stack<List<T>>[] stacks = new Stack<List<T>>[Environment.ProcessorCount];
        static Stack<List<T>> stack = new Stack<List<T>>();

        public static void InitThread()
        {
            for (int a = 0; a < stacks.Length; a++)
            {
                stacks[a] = new Stack<List<T>>();
            }
        }

        public static List<T> Get()
        {
            if (stack.Count > 0)
            {
                return stack.Pop();
            }

            return new List<T>();
        }

        public static List<T> GetThread(
            int threadId)
        {
            if (stacks[threadId].Count > 0)
            {
                return stacks[threadId].Pop();
            }

            return new List<T>();
        }

        public static void Add(
            List<T> list)
        {
            list.Clear();
            stack.Push(list);
        }

        public static void AddThread(
            int threadId,
            List<T> list)
        {
            list.Clear();
            stacks[threadId].Push(list);
        }
    }
}
