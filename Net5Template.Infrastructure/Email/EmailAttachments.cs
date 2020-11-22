using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Infrastructure.Email
{
    public class EmailAttachments
    {
        internal AttachmentCollection Attachments { get; set; } = new AttachmentCollection();

        public EmailAttachments(string fileName, Stream stream, ContentType contentType)
        {
            Attachments.Add(fileName, stream, contentType);
        }
        public EmailAttachments(string fileName, byte[] data)
        {
            Attachments.Add(fileName, data);
        }
        public EmailAttachments(string fileName, Stream stream)
        {
            Attachments.Add(fileName, stream);
        }
        public EmailAttachments(string fileName, ContentType contentType)
        {
            Attachments.Add(fileName, contentType);
        }
        public EmailAttachments(string fileName)
        {
            Attachments.Add(fileName);
        }
        public EmailAttachments(string fileName, byte[] data, ContentType contentType)
        {
            Attachments.Add(fileName, data, contentType);
        }
    }
}
