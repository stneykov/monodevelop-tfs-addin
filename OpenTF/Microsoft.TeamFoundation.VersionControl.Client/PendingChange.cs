//
// Microsoft.TeamFoundation.VersionControl.Client.PendingChange
//
// Authors:
//	Joel Reed (joelwreed@gmail.com)
//  Ventsislav Mladenov (ventsislav.mladenov@gmail.com)
//
// Copyright (C) 2013 Joel Reed, Ventsislav Mladenov
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Xml.Linq;
using Microsoft.TeamFoundation.Common;

namespace Microsoft.TeamFoundation.VersionControl.Client
{
    //<s:complexType name="PendingChange">
    //    <s:sequence>
    //        <s:element minOccurs="0" maxOccurs="1" name="MergeSources" type="tns:ArrayOfMergeSource"/>
    //        <s:element minOccurs="0" maxOccurs="1" name="PropertyValues" type="tns:ArrayOfPropertyValue"/>
    //    </s:sequence>
    //    <s:attribute default="0" name="chgEx" type="s:int"/>
    //    <s:attribute default="None" name="chg" type="tns:ChangeType"/>
    //    <s:attribute name="date" type="s:dateTime" use="required"/>
    //    <s:attribute default="0" name="did" type="s:int"/>
    //    <s:attribute default="Any" name="type" type="tns:ItemType"/>
    //    <s:attribute default="-2" name="enc" type="s:int"/>
    //    <s:attribute default="0" name="itemid" type="s:int"/>
    //    <s:attribute name="local" type="s:string"/>
    //    <s:attribute default="None" name="lock" type="tns:LockLevel"/>
    //    <s:attribute name="item" type="s:string"/>
    //    <s:attribute name="srclocal" type="s:string"/>
    //    <s:attribute name="srcitem" type="s:string"/>
    //    <s:attribute default="0" name="svrfm" type="s:int"/>
    //    <s:attribute default="0" name="sdi" type="s:int"/>
    //    <s:attribute default="0" name="ver" type="s:int"/>
    //    <s:attribute name="hash" type="s:base64Binary"/>
    //    <s:attribute default="-1" name="len" type="s:long"/>
    //    <s:attribute name="uhash" type="s:base64Binary"/>
    //    <s:attribute default="0" name="pcid" type="s:int"/>
    //    <s:attribute name="durl" type="s:string"/>
    //    <s:attribute name="shelvedurl" type="s:string"/>
    //    <s:attribute name="ct" type="s:int" use="required"/>
    //</s:complexType>
    public class PendingChange
    {
        //		<PendingChange chg="Add Edit Encoding" hash="" uhash="" pcid="-339254" />
        internal static PendingChange FromXml(XElement element)
        {
            PendingChange change = new PendingChange
            {
                ChangeType = ChangeType.None,
                ItemType = ItemType.Any,
                Encoding = -2,
            };

            change.ServerItem = element.GetAttribute("item");
            change.LocalItem = TfsPath.ToPlatformPath(element.GetAttribute("local"));
            if (!string.IsNullOrEmpty(element.GetAttribute("itemid")))
                change.ItemId = Convert.ToInt32(element.GetAttribute("itemid"));
            if (!string.IsNullOrEmpty(element.GetAttribute("enc")))
                change.Encoding = Convert.ToInt32(element.GetAttribute("enc"));
            if (!string.IsNullOrEmpty(element.GetAttribute("ver")))
                change.Version = Convert.ToInt32(element.GetAttribute("ver"));
            change.CreationDate = DateTime.Parse(element.GetAttribute("date"));

            if (!string.IsNullOrEmpty(element.GetAttribute("hash")))
                change.Hash = Convert.FromBase64String(element.GetAttribute("hash"));

            if (!string.IsNullOrEmpty(element.GetAttribute("uhash")))
                change.uploadHashValue = Convert.FromBase64String(element.GetAttribute("uhash"));

            if (!string.IsNullOrEmpty(element.GetAttribute("type")))
                change.ItemType = (ItemType)Enum.Parse(typeof(ItemType), element.GetAttribute("type"), true);

            change.DownloadUrl = element.GetAttribute("durl");
			
            if (!string.IsNullOrEmpty(element.GetAttribute("chg")))
            {
                string chgAttr = element.GetAttribute("chg");
                change.ChangeType = (ChangeType)Enum.Parse(typeof(ChangeType), chgAttr.Replace(" ", ","), true);
                if (change.ChangeType == ChangeType.Edit)
                    change.ItemType = ItemType.File;
            }
            return change;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("PendingChange instance ");
            sb.Append(GetHashCode());

            sb.Append("\n	 ServerItem: ");
            sb.Append(ServerItem);

            sb.Append("\n	 LocalItem: ");
            sb.Append(LocalItem);

            sb.Append("\n	 ItemId: ");
            sb.Append(ItemId);

            sb.Append("\n	 Encoding: ");
            sb.Append(Encoding);

            sb.Append("\n	 Creation Date: ");
            sb.Append(CreationDate);

            sb.Append("\n	 ChangeType: ");
            sb.Append(ChangeType);

            sb.Append("\n	 ItemType: ");
            sb.Append(ItemType);

            sb.Append("\n	 Download URL: ");
            sb.Append(DownloadUrl);

            return sb.ToString();
        }

        internal void UpdateUploadHashValue()
        {
            using (FileStream stream = new FileStream(LocalItem, FileMode.Open, FileAccess.Read))
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                md5.ComputeHash(stream);
                uploadHashValue = md5.Hash;
            }
        }

        public void DownloadBaseFile(string localFileName)
        {
            throw new NotImplementedException();
//            if (itemType == ItemType.Folder)
//                return;
//            Uri artifactUri = new Uri(String.Format("{0}?{1}", VersionControlServer.Repository.ItemUrl, downloadUrl));
//            Client.DownloadFile.WriteTo(localFileName, VersionControlServer.Repository,
//                artifactUri);
        }

        public byte[] Hash { get; set; }

        private byte[] uploadHashValue;

        public byte[] UploadHashValue
        {
            get
            {
                if (uploadHashValue == null)
                    UpdateUploadHashValue();
                return uploadHashValue; 
            }
        }

        public DateTime CreationDate { get; private set; }

        public int Encoding { get; private set; }

        public string LocalItem { get; private set; }

        public int ItemId { get; private set; }

        public ItemType ItemType { get; private set; }

        public int Version { get; private set; }

        public bool IsAdd
        {
            get { return (ChangeType & ChangeType.Add) == ChangeType.Add; }
        }

        public bool IsBranch
        {
            get { return (ChangeType & ChangeType.Branch) == ChangeType.Branch; }
        }

        public bool IsDelete
        {
            get { return (ChangeType & ChangeType.Delete) == ChangeType.Delete; }
        }

        public bool IsEdit
        {
            get { return (ChangeType & ChangeType.Edit) == ChangeType.Edit; }
        }

        public bool IsEncoding
        {
            get { return (ChangeType & ChangeType.Encoding) == ChangeType.Encoding; }
        }

        public bool IsLock
        {
            get { return (ChangeType & ChangeType.Lock) == ChangeType.Lock; }
        }

        public bool IsMerge
        {
            get { return (ChangeType & ChangeType.Merge) == ChangeType.Merge; }
        }

        public bool IsRename
        {
            get { return (ChangeType & ChangeType.Rename) == ChangeType.Rename; }
        }

        public ChangeType ChangeType { get; private set; }

        public string ServerItem { get; private set; }

        public string DownloadUrl { get; set; }

        static public string GetLocalizedStringForChangeType(ChangeType changeType)
        {
            return changeType.ToString();
        }
    }
}

