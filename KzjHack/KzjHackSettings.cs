#region Copyright
// Copyright (c) 2019 TonesNotes
// Distributed under the Open BSV software license, see the accompanying file LICENSE.
#endregion
using KZJ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace KzjHack
{
    public class KzjHackSettings : ICloneBySerialization
    {
        [Category("Blocks")]
        [DisplayName("Disk Storage Folder")]
        [Description(@"File system path to root folder use to store previously retrieved block data. For example: 'D:\KzBitcoinSVdata\RawBlocks'.")]
        [DefaultValue(@"D:\KzBitcoinSVdata\RawBlocks")]
        public string BlocksFolder { get; set; }

        [Category("Blocks")]
        [DisplayName("Since When")]
        [Description(@"How far back in time to go. For example: '2019-05-01'.")]
        [DefaultValue(@"2019-05-01")]
        public string XmlBlocksSince { get => BlocksSince.ToString("yyyy-MM-dd"); set => BlocksSince = DateTime.ParseExact(value, "yyyy-MM-dd", null); }

        [Browsable(false)]
        [XmlIgnore]
        public DateTime BlocksSince { get; set; }

        [Category("Blocks")]
        [DisplayName("Headers Only")]
        [Description(@"True if only interested in block headers. False if complete blocks are needed. For example: 'true'.")]
        [DefaultValue(true)]
        public bool BlocksHeadersOnly { get; set; }

        [Category("Blocks")]
        [DisplayName("Check All")]
        [Description(@"True if blocks saved to disk need checking. False will stop checking once first block on disk is found. For example: 'false'.")]
        [DefaultValue(false)]
        public bool BlocksCheckAll { get; set; }

        [Category("Data Services")]
        [DisplayName("BSV ZMQ Address")]
        [Description(@"Set to address of a Bitcoin SV node for ZMQ data services. For example: 'tcp://192.168.0.101:28332'.")]
        [DefaultValue("tcp://127.0.0.1:28332")]
        public string BitcoinSvZmqAddress { get; set; }

        [Category("Data Services")]
        [DisplayName("BSV RPC Address")]
        [Description(@"Set to address of a Bitcoin SV node for JSON-RPC data services. For example: 'http://192.168.0.101:8332'.")]
        [DefaultValue("http://127.0.0.1:8332")]
        public string BitcoinSvRpcAddress { get; set; }

        [Category("Data Services")]
        [DisplayName("BSV RPC Username")]
        [Description(@"Set to username of a Bitcoin SV node for JSON-RPC data services. For example: 'root'.")]
        [DefaultValue("root")]
        public string BitcoinSvRpcUsername { get; set; }

        [Category("Data Services")]
        [DisplayName("BSV RPC Password")]
        [Description(@"Set to password of a Bitcoin SV node for JSON-RPC data services. For example: 'bitcoin'.")]
        [DefaultValue("bitcoin")]
        public string BitcoinSvRpcPassword { get; set; }

        public KzjHackSettings()
        {
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(this)) {
                DefaultValueAttribute a = pd.Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;
                if (a != null && !pd.IsReadOnly) {
                    object d = a.Value;
                    pd.SetValue(this, d);
                }
            }
        }

        public static string DefaultFilename {
            get {
                return Path.Combine(Application.UserAppDataPath, "Settings.xml");
            }
        }

        public void Save() { Save(DefaultFilename); }

        public static KzjHackSettings Load()
        {
            if (!File.Exists(DefaultFilename)) new KzjHackSettings().Save();
            return Load(DefaultFilename);
        }

        public void Save(string filename, bool saveAs = false)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filename));
            using (StreamWriter sw = new StreamWriter(filename)) {
                Save(sw.BaseStream);
            }
            // Not working...
            //File.Encrypt(filename);
        }

        public static KzjHackSettings Load(string filename)
        {
            try {
                // Not working...
                //File.Decrypt(filename);
            } catch (Exception ex) {
                var m = ex.Message;
            }
            var job = (KzjHackSettings)null;
            using (StreamReader sr = new StreamReader(filename)) {
                job = Load(sr.BaseStream);
            }
            return job;
        }

        static XmlSerializer _ser = null;
        static object serLock = new object();
        static XmlSerializer ser {
            get {
                if (_ser == null) { lock (serLock) { if (_ser == null) { _ser = new XmlSerializer(typeof(KzjHackSettings)); } } }
                return _ser;
            }
        }

        public void Save(Stream s)
        {
            lock (serLock) {
                ser.Serialize(s, this);
            }
        }

        public static KzjHackSettings Load(Stream s)
        {
            lock (serLock) {
                var o = ser.Deserialize(s) as KzjHackSettings;
                return o;
            }
        }

        public object CloneBySerialization() { return Clone(); }

        public KzjHackSettings Clone()
        {
            XmlSerializer s = new XmlSerializer(this.GetType());
            using (MemoryStream stream = new MemoryStream()) {
                s.Serialize(stream, this);
                stream.Position = 0;
                return s.Deserialize(stream) as KzjHackSettings;
            }
        }

        public override string ToString()
        {
            return "<KzjHack Settings>";
        }
    }

    public class KzjHackUiSettings : ICloneBySerialization
    {
        public List<XmlUiLayoutRoot> UiLayouts { get; set; }

        public static string DefaultFilename {
            get {
                return Path.Combine(Application.UserAppDataPath, "UiSettings.xml");
            }
        }

        public static string DefaultSettingsFilename {
            get {
                return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Controls\\DefaultUiSettings.xml");
            }
        }

        public void Save() { Save(DefaultFilename); }

        public static KzjHackUiSettings Load()
        {
            if (!File.Exists(DefaultFilename)) {
                File.Copy(DefaultSettingsFilename, DefaultFilename);
            }
            return Load(DefaultFilename);
        }

        public void Save(string filename, bool saveAs = false)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filename));
            using (StreamWriter sw = new StreamWriter(filename)) {
                Save(sw.BaseStream);
            }
        }

        public static KzjHackUiSettings Load(string filename)
        {
            var job = (KzjHackUiSettings)null;
            using (StreamReader sr = new StreamReader(filename)) {
                job = Load(sr.BaseStream);
            }
            return job;
        }

        static XmlSerializer _ser = null;
        static object serLock = new object();
        static XmlSerializer ser {
            get {
                if (_ser == null) { lock (serLock) { if (_ser == null) { _ser = new XmlSerializer(typeof(KzjHackUiSettings)); } } }
                return _ser;
            }
        }

        public void Save(Stream s)
        {
            lock (serLock) {
                ser.Serialize(s, this);
            }
        }

        public static KzjHackUiSettings Load(Stream s)
        {
            lock (serLock) {
                var o = ser.Deserialize(s) as KzjHackUiSettings;
                return o;
            }
        }

        public object CloneBySerialization() { return Clone(); }

        public KzjHackUiSettings Clone()
        {
            XmlSerializer s = new XmlSerializer(this.GetType());
            using (MemoryStream stream = new MemoryStream()) {
                s.Serialize(stream, this);
                stream.Position = 0;
                return s.Deserialize(stream) as KzjHackUiSettings;
            }
        }

        public override string ToString()
        {
            return "<KzjHackUiSettings>";
        }
    }
}
