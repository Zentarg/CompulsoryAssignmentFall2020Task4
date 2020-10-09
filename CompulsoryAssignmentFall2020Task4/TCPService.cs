using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Assignment1;

namespace CompulsoryAssignmentFall2020Task4
{
    public class TCPService
    {

        private static List<Book> bookList = new List<Book>()
        {
            new Book{Author = "AuthorTest1", Isbn13 = "1234567890123", PageNumber = 100, Title = "TitleTest1"},
            new Book{Author = "AuthorTest2", Isbn13 = "2345678901234", PageNumber = 100, Title = "TitleTest2"},
            new Book{Author = "AuthorTest3", Isbn13 = "3456789012345", PageNumber = 100, Title = "TitleTest3"},
            new Book{Author = "AuthorTest4", Isbn13 = "4567890123456", PageNumber = 100, Title = "TitleTest4"},
            new Book{Author = "AuthorTest5", Isbn13 = "5678901234567", PageNumber = 100, Title = "TitleTest5"}

        };

        private TcpClient connectionSocket;

        public TCPService(TcpClient connectionSocket)
        {
            this.connectionSocket = connectionSocket;
        }

        private Book GetBook(string isbn13)
        {
            return bookList.FirstOrDefault(b => b.Isbn13 == isbn13);
        }

        private List<Book> GetBooks()
        {
            return bookList;
        }

        private void PutBook(Book newBook)
        {
            bookList.Add(newBook);
        }

        internal void StartService()
        {
            Stream ns = connectionSocket.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true; // enable automatic flushing

            string message = sr.ReadLine();
            string answer = "";
            while (!string.IsNullOrEmpty(message))
            {
                string[] request = message.Split("|");
                
                switch (request[0])
                {
                    case "get":
                        answer = JsonSerializer.Serialize(GetBook(request[1]));
                        break;
                    case "getAll":
                        answer = JsonSerializer.Serialize(GetBooks());
                        break;
                    case "put":
                        PutBook(JsonSerializer.Deserialize<Book>(request[1]));
                        break;
                    default:
                        answer = "Desired request does not exist.";
                        break;
                }

                Console.WriteLine("Client request: " + request[0]);
                sw.WriteLine(answer);
                message = sr.ReadLine();

            }
            ns.Close();
            connectionSocket.Close();
        }

    }
}
