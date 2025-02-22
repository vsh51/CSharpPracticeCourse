using System;
using System.Text.RegularExpressions;

namespace Pract2
{
    internal class MailAddress
    {
        public string mail;
        private string username;
        private string domain;

        private static string pattern = @"^([a-zA-Z0-9._-]+)@([a-zA-Z0-9.-]+\.[a-zA-Z]{2,})$";
        Regex rg = new Regex(pattern);
          
        public MailAddress(string mail)
        {
            parse(mail);
        }

        private bool parse(string mail)
        {
            Match disassembled = Regex.Match(mail, pattern);

            if (disassembled.Success)
            {
                this.mail = mail;
                this.username = disassembled.Groups[1].Value;
                this.domain = disassembled.Groups[2].Value;
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return this.mail;
        }
    }
}
