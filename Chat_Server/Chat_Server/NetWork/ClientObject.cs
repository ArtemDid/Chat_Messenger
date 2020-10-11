using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.Net;
using System.Threading;
using Chat_Server.DBFirst;
using Chat_Server.DataLayer;

namespace Chat_Server.NetWork
{
    class ClientObject
    {
        protected internal string Id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        string userName;
        TcpClient client;
        ServerObject server; // объект сервера

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        public void Process()
        {
            try
            {

                Stream = client.GetStream();
                // получаем имя пользователя
                string message = GetMessage();
                userName = message;

                message = userName + ": вошел в чат";
                // посылаем сообщение о входе в чат всем подключенным пользователям
                server.BroadcastMessage(message, this.Id);
                Console.WriteLine(message);
                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        message = GetMessage();

                        if (message == "Добавить")
                        {
                            //var db = new necrochat();
                            //Role user = new Role(Roless.user);
                            //Role admin = new Role(Roless.admin);
                            //Role owner = new Role(Roless.owner);

                            //db.roles.Add(user);
                            //db.roles.Add(admin);
                            //db.roles.Add(owner);

                            //db.SaveChanges();

                            stp_UserById_Result newUser = new stp_UserById_Result()
                            {
                                FirstName = "Artem",
                                Login_ = "aaa",
                                Password_ = "123",
                                Role_ = 1,
                                Ava = 2
                            };

                            newUser.Id = DataLayer._DL.Add(newUser);
                            message += $" {newUser.Id}";
                        }

                        if (message == null)
                            throw new ArgumentNullException();
                        else
                        {
                            message = String.Format("{0}: {1}", userName, message);
                            Console.WriteLine(message);
                            server.BroadcastMessage(message, this.Id);
                        }
                    }
                    catch (Exception)
                    {
                        message = String.Format("{0}: покинул чат", userName);
                        Console.WriteLine(message);
                        server.BroadcastMessage(message, this.Id);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.RemoveConnection(this.Id);
                Close();
            }
        }

        private string GetMessage()
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);
            if (bytes != 0)
                return builder.ToString();
            else
                return null;
        }

        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }


    }
}
