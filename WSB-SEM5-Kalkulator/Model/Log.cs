using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSB_SEM5_Kalkulator.Model
{
    public class Log
    {
        public static void Write(string Message)
        {

            string TimestampString = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff");


            Console.WriteLine(string.Format("{0} | {1}", TimestampString, Message));
        }

        public static void Write(Type TypeOfClass)
        {
            if (TypeOfClass != null)
            {
                string NameOfClass = TypeOfClass.Name;

                string TextDoDisplay = string.Format("{0}::{0}()", NameOfClass);

                Write(TextDoDisplay);

            }
        }

        public static void Write(Type TypeOfClass, string NameOfMethod, string Message = null)
        {
            if (TypeOfClass != null && !string.IsNullOrEmpty(NameOfMethod))
            {
                string NameOfClass = TypeOfClass.Name;

                string TextDoDisplay = string.Format("{0}::{1}()", NameOfClass, NameOfMethod);

                if (!string.IsNullOrEmpty(Message))
                {
                    TextDoDisplay += string.Format("-> {0}", Message);
                }

                Write(TextDoDisplay);
            }
        }


        public static void Write(Type TypeOfClass, string NameOfMethod, string Message, params object[] Params)
        {
            if (TypeOfClass != null && !string.IsNullOrEmpty(NameOfMethod) && !string.IsNullOrEmpty(Message))
            {
                string TextDoDisplay = string.Format(Message, Params);

                Write(TypeOfClass, NameOfMethod, TextDoDisplay);
            }
        }
    }
}
