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
        stp_UserById_Result newUser = null;

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
                string message = "";

                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        message = GetMessage();
                                            
                        if (message == null)
                            throw new ArgumentNullException();
                        else
                        {
                            string[] str = message.Split(new string[] { "\n" }, StringSplitOptions.None);
                            

                            if (str[0] == "Регистрация")
                            {
                                userName = str[1];
                                newUser = new stp_UserById_Result()
                                {
                                    FirstName = str[1],
                                    Login_ = str[2],
                                    Password_ = str[3],
                                    Role_ = 1,
                                    Ava = 2
                                };

                                newUser.Id = DataLayer._DL.Add(newUser);
                                server.RegistrMessage(newUser.Id.ToString(), this.Id);
                                //message += $" {newUser.Id}";
                            }


                            else if (str[0] == "Вход")
                            {
                                userName = str[1];
                                Console.WriteLine(message);
                                server.BroadcastMessage(str[1], this.Id);
                            }
                            else
                            {
                                message = String.Format("{0}: {1}", userName, str[0]);
                                Console.WriteLine(message);
                                server.BroadcastMessage(message, this.Id);                              
                            }
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
