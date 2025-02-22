using System;

namespace Pract2
{
    internal class Client
    {
        private string name, surname;
        MailAddress mail_address;

        public Client(string name, string surname, string mail_address)
        {
            this.name = name;
            this.surname = surname;
            this.mail_address = new MailAddress(mail_address);
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, Surname: {1}, Mail address: {2}", this.name, this.surname, this.mail_address);
        }
    }
}
