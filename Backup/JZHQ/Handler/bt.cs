
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Collections;

namespace JZHQ.Handler
{
    /// 种子文件解析类
    /// 作者：浊酒
    /// </summary>
    public class Torrent
    {
        /// <summary>
        /// 编码方式
        /// </summary>
        public Encoding _Code { get { return Encoding.UTF8; } }

        private int _Isprivate = 1;

        /// <summary>
        /// 是否为私有种子，默认为true
        /// </summary>
        public bool IsPrivate
        {
            get { if (_Isprivate == 0) return false; else  return true; }
            set { if (value) _Isprivate = 1; else  _Isprivate = 0; }
        }

        /// <summary>
        /// 多文件时创建file键
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> build_file()
        {
            Dictionary<string, object> file = new Dictionary<string, object>();
            file.Add("name", null);
            file.Add("length", (Int64)0);
            file.Add("md5sum", null);
            file.Add("path", null);
            return file;
        }

        /// <summary>
        /// 创建info键
        /// </summary>
        /// <param name="IsDir">是否多文件（文件夹）</param>
        /// <returns></returns>
        private Dictionary<string, object> build_info(bool IsDir)
        {
            Dictionary<string, object> info = new Dictionary<string, object>();
            info.Add("private", (Int64)_Isprivate);
            info.Add("source", null);
            info.Add("piece length", null);
            info.Add("pieces", null);

            if (IsDir)
            {
                info.Add("name", null);
                ArrayList files = new ArrayList();
                info.Add("files", files);
            }
            else
            {
                info.Add("name", null);
                info.Add("length", null);
                info.Add("md5sum", null);
            }
            return info;
        }

        /// <summary>
        /// 默认key列表
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> build_torrent()
        {
            Dictionary<string, object> list = new Dictionary<string, object>();
            list.Add("announce", null);
            list.Add("announce-list", null);
            list.Add("creation date", null);
            list.Add("comment", null);
            list.Add("created by", null);
            list.Add("encoding", this._Code.GetBytes(_Code.BodyName.ToUpper()));
            list.Add("info", null);

            return list;
        }
        private Dictionary<string, object> _torrent = null;

        /// <summary>
        /// 无参的构造函数，适合创建torrent
        /// </summary>
        public Torrent()
        {
            _torrent = build_torrent();
        }

        /// <summary>
        /// 从字节流中读取torrent信息
        /// </summary>
        /// <param name="bytes">字节流</param>
        /// <returns>torrent文件中的全部信息</returns>
        public Dictionary<string, object> ReadAll(Stream s)
        {
            Bencoding benc = new Bencoding(this._Code);
            Dictionary<string, object> dic = benc.DecodeDic(s);
            return dic;
        }

        /// <summary>
        /// 从byte[]中读取
        /// </summary>
        /// <param name="s">字节流</param>
        public Torrent(Stream s)
        {
            _torrent = this.ReadAll(s);
        }

        public Torrent(string FilePath)
        {
            using (FileStream file = new FileStream(FilePath, FileMode.Open))
            {
                _torrent = this.ReadAll(file);
            }
        }

        /// <summary>
        /// 从文件流中读取
        /// </summary>
        /// <param name="file">可读的文件流</param>
        /// <param name="AutoClose">读取完成后是否自动关闭，默认不关闭</param>
        public Torrent(FileStream file, bool AutoClose = false)
        {
            if (file.CanRead == false)
                throw new Exception("Can't Read FileStream");
            _torrent = this.ReadAll(file);
        }

        /// <summary>
        /// 保存torrent为文件
        /// </summary>
        /// <param name="FilePath">文件目录和文件名</param>
        /// <param name="Throw_Exception">保存失败是否抛出异常</param>
        /// <returns>成功返回true否则为false(抛出异常）</returns>
        public bool SaveToFile(string FilePath, bool Throw_Exception = true)
        {
            try
            {
                using (FileStream file = new FileStream(FilePath, FileMode.CreateNew))
                {
                    Bencoding benc = new Bencoding(this._Code);
                    byte[] b = benc.EncodeObj(_torrent);
                    file.Write(b, 0, b.Length);
                    file.Flush();
                    file.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                if (Throw_Exception)
                    throw e;
                else
                    return false;
            }
        }

        /// <summary>
        /// （索引器）种子参数,只读
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key] { get { return _torrent[key]; } }

        public Dictionary<string, object> All
        {
            get { return _torrent; }
        }

        /// <summary>
        /// comment(可选):备注(字符串)
        /// </summary>
        public string Comment
        {
            set
            {
                if (value == null)
                    _torrent["comment"] = null;
                else
                    _torrent["comment"] = this._Code.GetBytes(value);
            }
            get
            {
                if (!_torrent.ContainsKey("created by"))
                    return null;
                if (_torrent["comment"] == null)
                    return null;
                else
                    return this._Code.GetString((byte[])_torrent["comment"]);
            }
        }

        /// <summary>
        /// (必选）tracker服务器的URL(字符串)
        /// </summary>
        public string Announce
        {
            set
            {
                if (value == null)
                    throw new Exception("announce不能设置null");
                else
                    _torrent["announce"] = this._Code.GetBytes(value);
            }
            get
            {
                try
                {
                    return this._Code.GetString((byte[])_torrent["announce"]);
                }
                catch
                {
                    throw new Exception("announce 为空");
                }
            }
        }

        /// <summary>
        /// (可选)备用tracker服务器列表(列表)
        /// </summary>
        public List<string> AnnounceList
        {
            set
            {
                if (value == null)
                    _torrent["announce-list"] = null;
                else
                {
                    ArrayList list = new ArrayList();
                    foreach (string a in value)
                    {
                        list.Add(this._Code.GetBytes(a));
                    }
                    _torrent["announce-list"] = list;
                }
            }
            get
            {
                if (!_torrent.ContainsKey("announce-list"))
                    return null;
                if (_torrent["announce-list"] == null)
                    return null;
                else
                {
                    List<string> result = new List<string>();
                    foreach (ArrayList a in (ArrayList)_torrent["announce-list"])
                    {
                        foreach (byte[] b in a)
                        {
                            result.Add(this._Code.GetString((byte[])b));
                        }
                    }
                    return result;
                }

            }
        }

        /// <summary>
        /// （可选）种子创建的时间，Unix标准时间格式，
        /// 从1970 1月1日 00:00:00到创建时间的秒数(整数)
        /// </summary>
        public Int64? CreationDate
        {
            set
            {
                if (value == 0)
                    _torrent["creation date"] = null;
                else
                    _torrent["creation date"] = value;
            }
            get
            {
                if (!_torrent.ContainsKey("created by"))
                    return null;
                return (Int64)_torrent["creation date"];
            }
        }

        /// <summary>
        /// (可选):创建人或创建程序的信息(字符串)
        /// </summary>
        public string CreatedBy
        {
            set
            {
                if (value == null)
                    _torrent["created by"] = null;
                else
                    _torrent["created by"] = this._Code.GetBytes(value);
            }
            get
            {
                if (!_torrent.ContainsKey("created by"))
                    return null;
                if (_torrent["created by"] == null)
                    return null;
                else
                    return this._Code.GetString((byte[])_torrent["created by"]);
            }
        }
        /// <summary>
        /// (可选).torrent元文件中包含一个info dictionary，
        /// 当该dictionary过大时，就需要对其分片(piece)，
        /// 该编码就是用来生成分片的。(字符串类型)
        /// </summary>
        public string TorrentEncoding
        {
            set
            {
                if (value == null)
                    _torrent["encoding"] = null;
                else
                    _torrent["encoding"] = this._Code.GetBytes(value);
            }
            get
            {
                if (_torrent["encoding"] == null)
                    return string.Empty;
                else
                    return this._Code.GetString((byte[])_torrent["encoding"]);
            }
        }

        /// <summary>
        /// （必选）info信息
        /// </summary>
        public Dictionary<string, object> Info
        {
            set
            {
                if (value == null)
                    throw new Exception("info 不能设置null");
                else
                    _torrent["info"] = value;
            }
            get
            {
                return (Dictionary<string, object>)_torrent["info"];
            }
        }

        /// <summary>
        /// info的hash值，一般作为整个种子的标识
        /// </summary>
        public byte[] InfoHash
        {
            get
            {
                var benc = new Bencoding();
                byte[] b = benc.EncodeDic((Dictionary<string, object>)this.All["info"]);
                return System.Security.Cryptography.SHA1.Create().ComputeHash(b);
            }
        }

        /// <summary>
        /// 总大小，如果是目录就表示所有文件的总大小
        /// </summary>
        public UInt64 Length
        {
            get
            {
                UInt64 len = 0;
                if (Info.ContainsKey("files"))
                {
                    foreach (Dictionary<string, object> file in (ArrayList)Info["files"])
                    {
                        len += (UInt64)((IConvertible)file["length"]).ToInt64(null);
                    }
                }
                else
                {
                    len = (UInt64)((IConvertible)Info["length"]).ToInt64(null);
                }
                return len;
            }
        }
        /// <summary>
        /// 文件（夹）名
        /// </summary>
        public string Name
        {
            get
            {
                return _Code.GetString((byte[])Info["name"]);
            }
        }

        /// <summary>
        /// 文件列表，当为单文件时，返回Name
        /// </summary>
        public IList<string> Files
        {
            get
            {
                IList<string> files = new List<string>();
                if (Info.ContainsKey("files"))
                {
                    foreach (Dictionary<string, object> file in ((ArrayList)Info["files"]))
                    {
                        string path = string.Empty;
                        foreach (byte[] b in (ArrayList)file["path"])
                        {
                            path += "/" + _Code.GetString(b);
                        }
                        files.Add(path.Substring(1));
                    }
                }
                else
                {
                    files.Add(this.Name);
                }
                return files;
            }
        }
    }
    ///<summary>
    /// bencode的实现类
    /// 作者：浊酒
    /// </summary>
    public class Bencoding
    {
        private Encoding _Code = Encoding.UTF8;

        /// <summary>
        /// 构造函数，指定benc的编码,默认编码为UTF8
        /// </summary>
        /// <param name="code"></param>
        public Bencoding(Encoding code = null)
        {
            if (code != null)
                _Code = code;
        }

        /// <summary>
        /// byte数组的拷贝
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="dst_start"></param>
        /// <param name="length"></param>
        private void CopToyByte(byte[] dst, byte[] src, Int64 dst_start, Int64 length)
        {
            if (src.Length > dst.Length)
                throw new Exception("目标byte数组比原数组小");
            for (Int64 i = dst_start; i - dst_start < length; i++)
            {
                dst[i] = src[i - dst_start];
            }
        }

        #region encode
        /// <summary>
        /// benc整型
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public byte[] EncodeInt(Int64 i)
        {
            return this._Code.GetBytes("i" + i.ToString() + "e");
        }

        /// <summary>
        /// benc字符串(byte数组)
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public byte[] EncodeStr(byte[] b)
        {
            if (b.Length == 0)
                return null;
            byte[] len = this._Code.GetBytes(b.Length.ToString() + ':');
            byte[] result = new byte[b.Length + len.Length];
            CopToyByte(result, len, 0, len.Length);
            CopToyByte(result, b, len.Length, b.Length);
            return result;

        }

        /// <summary>
        /// benc列表
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public byte[] EncodeList(ArrayList list)
        {
            if (list.Count == 0)
                return null;
            MemoryStream ms = new MemoryStream();
            ms.WriteByte((byte)'l');
            foreach (object o in list)
            {
                byte[] b = EncodeObj(o);
                ms.Write(b, 0, b.Length);
            }
            ms.WriteByte((byte)'e');

            return ms.ToArray();

        }
        /// <summary>
        /// benc字典
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public byte[] EncodeDic(Dictionary<string, object> dic)
        {
            if (dic.Count == 0)
                return null;
            MemoryStream ms = new MemoryStream();
            ms.WriteByte((byte)'d');
            foreach (KeyValuePair<string, object> pair in dic)
            {
                if (pair.Value == null)
                    continue;
                byte[] key = EncodeStr(this._Code.GetBytes(pair.Key));
                byte[] value = EncodeObj(pair.Value);
                ms.Write(key, 0, key.Length);
                ms.Write(value, 0, value.Length);
            }
            ms.WriteByte((byte)'e');
            return ms.ToArray();
        }

        /// <summary>
        /// benc泛型
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public byte[] EncodeObj(object x)
        {
            if (x is byte[])
                return EncodeStr((byte[])x);
            else if (x is string)
                return EncodeStr(_Code.GetBytes((string)x));
            else if ((x is Int32) || (x is Int64) || (x is Int16))
                return EncodeInt(((IConvertible)x).ToInt64(null));
            else if (x is ArrayList)
                return EncodeList((ArrayList)x);
            else if (x is Dictionary<string, object>)
                return EncodeDic((Dictionary<string, object>)x);
            else
                throw new Exception("类型错误");
        }
        #endregion encode

        #region decode
        /// <summary>
        /// decode整型
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Int64 DecodeInt(Stream ms)
        {
            if (ms.ReadByte() != (int)'i')
                throw new Exception("not int");
            string s = string.Empty;
            do
            {
                char c = (char)ms.ReadByte();
                if (c == 'e' || c < '0' || c > '9')
                    break;
                s += c;
            } while (true);
            return Int64.Parse(s);
        }

        /// <summary>
        /// decode字符串
        /// </summary>
        /// <param name="ms">字节流</param>
        /// <returns></returns>
        public byte[] DecodeStr(Stream ms)
        {
            char c = (char)ms.ReadByte();
            if (c < '0' || c > '9')
                throw new Exception("not string");
            string s = string.Empty;
            do
            {
                s += c;
                c = (char)ms.ReadByte();
                if (c == ':' || c < '0' || c > '9')
                    break;
            } while (true);
            int length = int.Parse(s);
            byte[] b = new byte[length];
            ms.Read(b, 0, length);
            return b;
        }

        /// <summary>
        /// Decode字典
        /// </summary>
        /// <param name="ms">字节流</param>
        /// <returns></returns>
        public Dictionary<string, object> DecodeDic(Stream ms)
        {
            if ((char)ms.ReadByte() != 'd')
                throw new Exception("not dic");
            Dictionary<string, object> result = new Dictionary<string, object>();
            while (true)
            {
                char c = (char)ms.ReadByte();
                if (c == 'e')
                    break;
                ms.Seek(-1, SeekOrigin.Current);
                byte[] key = DecodeStr(ms);
                c = (char)ms.ReadByte();
                ms.Seek(-1, SeekOrigin.Current);
                switch (c)//get value
                {
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        byte[] a = DecodeStr(ms);
                        result.Add(_Code.GetString(key), a);
                        continue;
                    case 'i':
                        Int64 i = DecodeInt(ms);
                        result.Add(_Code.GetString(key), i);
                        continue;
                    case 'l':
                        ArrayList arr = DecodeList(ms);
                        result.Add(_Code.GetString(key), arr);
                        continue;
                    case 'd':
                        Dictionary<string, object> dic = DecodeDic(ms);
                        result.Add(_Code.GetString(key), dic);
                        continue;
                    case 'e':
                        break;
                    default:
                        throw new Exception("字典读取错误");
                }
            }
            return result;
        }

        /// <summary>
        /// Decode列表
        /// </summary>
        /// <param name="ms">字节流</param>
        /// <returns></returns>
        public ArrayList DecodeList(Stream ms)
        {
            if ((char)ms.ReadByte() != 'l')
                throw new Exception("not list");
            ArrayList result = new ArrayList();
            while (true)
            {
                char c = (char)ms.ReadByte();
                if (c == 'e')
                    break;
                ms.Seek(-1, SeekOrigin.Current);
                switch (c)
                {
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        byte[] a = DecodeStr(ms);
                        result.Add(a);
                        continue;
                    case 'i':
                        Int64 i = DecodeInt(ms);
                        result.Add(i);
                        continue;
                    case 'l':
                        ArrayList arr = DecodeList(ms);
                        result.Add(arr);
                        continue;
                    case 'd':
                        Dictionary<string, object> dic = DecodeDic(ms);
                        result.Add(dic);
                        continue;
                    case 'e':
                        break;
                    default:
                        throw new Exception("列表读取错误");
                }
            }
            return result;
        }
        #endregion decode

    }
}