
namespace System
{
    public interface IComparable
    {
        int CompareTo(object obj);
    }
    public struct Int32: IComparable
    {
        public Int32 op_Equals(Int32 b)
        {
            return 1;
        }
        public Boolean op_Small(Int32 b)
        {
            return 1;
        }
        public Int32 op_Assign(Int32 b)
        {
            return 1;
        }

        public Int32 op_PlusPlus(Int32 b)
        {
            return 1;
        }
    }

    public struct Boolean
    {

    }

    public class String
    {

    }

    public class Object
    {

    }

    public class Array<T>
    {
        public Array(int len) { }
    }

    public class Console
    {
        public static void WriteLine(string v);
    }

    //
    // ժҪ:
    //     
    public enum MemberTypes
    {
        //
        // ժҪ:
        //     ָ���ó�Ա��һ�����캯��
        Constructor = 1,
        //
        // ժҪ:
        //     ָ���ó�Ա��һ���¼���
        Event = 2,
        //
        // ժҪ:
        //     ָ���ó�Ա��һ���ֶΡ�
        Field = 4,
        //
        // ժҪ:
        //     ָ���ó�Ա��һ�ַ�����
        Method = 8,
        //
        // ժҪ:
        //     ָ����Ա�����ԡ�
        Property = 16,
        //
        // ժҪ:
        //     ָ���ó�Ա��һ�����͡�
        TypeInfo = 32,
        //
        // ժҪ:
        //     ָ���ó�Ա���Զ����Ա��ָ�����͡�
        Custom = 64,
        //
        // ժҪ:
        //     ָ���ó�Ա��Ƕ�׵����͡�
        NestedType = 128,
        //
        // ժҪ:
        //     ָ�����г�Ա���͡�
        All = 191
    }
}
namespace HelloWorld
{

    using System;

    public class TestGeneric<T1,T2> where T1:TestInt
    {
        public T1 a;
        //public T1 TestGenericFunction()
        //{
            //T1 a = new T1();
            //return a;
        //}
    }

    public class TestInt
    {
        public int a = 6;
    }
    enum TestE
    {
        A,
        B,
        C
    }

    public class Program
    {
        public static int a = 6;

        public TestGeneric<int, string> v;
        public int[] s;

        public static void Main()
        {
            if(true)
            {
                Console.WriteLine("Hello, World!");
            }
            else
            {
                Console.WriteLine("Hello, World!1");
                Console.WriteLine("Hello, World!2");

            }
        }

        void Print(ref HelloWorld.TestInt hello)
        {
            TestInt c = new TestInt();
            c.a = 7;
            Console.WriteLine("Print");

            MemberTypes testEnum = MemberTypes.Event;

            int[] v = new int[6];

            for (int i=0,c=6;i<3;i++)
            {

            }

            do
            {
                Console.WriteLine("DO");
            } while (true);

            while(true)
            {
                Console.WriteLine("while");
            }
            switch (5)
            {
                case 1:
                    break;
                case 2:
                case 3:
                    break;
            }

            //switch ("5")
            //{
            //    case  "haha":
            //        break;
            //    case TestE.B:
            //    case TestE.C:
            //        break;
            //}

            TestGeneric<int, string> temp = new TestGeneric<int, string>();
        }
    }
}