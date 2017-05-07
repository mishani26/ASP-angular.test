using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using SPA_test.Models;
using System.Xml.Linq;

namespace SPA_test.XML_Helpers
{
    public static class XML_Helpers
    {
        public static XmlDocument ReadXML(string FileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(HttpContext.Current.Server.MapPath(FileName));
            return doc;
        }

        public static LoginViewModel GetCurrentUser()
        {
            LoginViewModel User = new LoginViewModel();
            XmlDocument doc = ReadXML("\\CurrentUser.xml");           
            User.Id= Int32.Parse(doc.GetElementsByTagName("id").Item(0).InnerText);
            User.Email = doc.GetElementsByTagName("mail").Item(0).InnerText;
            User.Password = doc.GetElementsByTagName("password").Item(0).InnerText;
            return User;
        }

        public static List<MassageViewModel> GetCurrentUserMessages()
        {
            List<MassageViewModel> MessagesList = new List<MassageViewModel>();
            XmlDocument doc = ReadXML("\\Messages.xml");
            XmlNodeList messages = doc.GetElementsByTagName("message");
            foreach (XmlNode message in messages)
            {
                if(message.Attributes["id"].Value== GetCurrentUser().Id.ToString())
                {
                    MassageViewModel MessageModel = new MassageViewModel();
                    MessageModel.Id = Int32.Parse(message.Attributes["id"].Value);
                    MessageModel.Date = message.Attributes.GetNamedItem("date").Value;
                    MessageModel.Message = message.InnerText;
                    MessagesList.Add(MessageModel);
                }

            }
            return MessagesList;
        }
        public static void SendMessage(string messageVal)
        {
            XmlDocument doc = ReadXML("\\Messages.xml");
            XmlNode userNode = doc.CreateElement("message");
            doc.LastChild.AppendChild(userNode);
            XmlAttribute Date = doc.CreateAttribute("date");
            Date.Value = DateTime.Now.ToString();
            userNode.Attributes.Append(Date);
            XmlAttribute id = doc.CreateAttribute("id");
            id.Value = GetCurrentUser().Id.ToString();
            userNode.Attributes.Append(id);
            userNode.InnerText = messageVal;
            doc.Save(HttpContext.Current.Server.MapPath("\\Messages.xml"));
        }

        public static List<MassageViewModel> GetAllMessages()
        {
            List<MassageViewModel> MessagesList = new List<MassageViewModel>();
            XmlDocument doc = ReadXML("\\Messages.xml");
            XmlNodeList messages = doc.GetElementsByTagName("message");
            foreach (XmlNode message in messages)
            {
                MassageViewModel MessageModel = new MassageViewModel();
                MessageModel.Id = Int32.Parse(message.Attributes["id"].Value);
                MessageModel.Date = message.Attributes.GetNamedItem("date").Value;
                MessageModel.Message = message.InnerText;
                MessagesList.Add(MessageModel);
            }
            return MessagesList;
        }

        public static void RegisterUser(RegisterViewModel model)
        {
            XmlDocument doc = ReadXML("\\Users.xml");
            XmlNode userNode = doc.CreateElement("user");
            doc.LastChild.AppendChild(userNode);
            XmlAttribute email = doc.CreateAttribute("email");
            email.Value = model.Email;
            userNode.Attributes.Append(email);
            XmlAttribute id = doc.CreateAttribute("id");
            id.Value = model.Id.ToString();
            userNode.Attributes.Append(id);
            XmlAttribute password = doc.CreateAttribute("password");
            password.Value = model.Password;
            userNode.Attributes.Append(password);
            doc.Save(HttpContext.Current.Server.MapPath("\\Users.xml"));
        }

        public static bool LoginUserWithXML(LoginViewModel model)
        {
            XmlDocument doc = ReadXML("\\Users.xml");
            string user = doc.InnerXml.ToString();
            XDocument doc1 = XDocument.Parse(user);
            XElement selectedElement = doc1.Descendants()
                .Where(x => (string)x.Attribute("email") == model.Email && (string)x.Attribute("password") == model.Password).FirstOrDefault();
            if (selectedElement != null)
            {
                XmlDocument Userdoc = ReadXML("\\CurrentUser.xml");
                XmlNode mail=Userdoc.GetElementsByTagName("mail").Item(0);
                mail.InnerText = model.Email;
                XmlNode Password = Userdoc.GetElementsByTagName("password").Item(0);
                Password.InnerText = model.Password;
                XmlNode Id = Userdoc.GetElementsByTagName("id").Item(0);
                Id.InnerText = selectedElement.Attribute("id").Value;
                Userdoc.Save(HttpContext.Current.Server.MapPath("\\CurrentUser.xml"));
                return true;
            }
            //XmlNode userNode = doc.CreateElement("user");
            //doc.LastChild.AppendChild(userNode);
            //XmlAttribute email = doc.CreateAttribute("email");
            //email.Value = model.Email;
            //userNode.Attributes.Append(email);
            //XmlAttribute id = doc.CreateAttribute("id");
            //id.Value = model.Id.ToString();
            //userNode.Attributes.Append(id);
            //XmlAttribute password = doc.CreateAttribute("password");
            //password.Value = model.Password;
            //userNode.Attributes.Append(password);
            //doc.Save(HttpContext.Current.Server.MapPath("\\Users.xml"));
            return false;
        }
    }
}