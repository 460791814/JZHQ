﻿using System;
namespace Model
{
	/// <summary>
	/// T_Soft:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class E_Soft
	{
		public E_Soft()
		{}
		#region Model
		private int _id;
		private string _hash;
		private string _name;
        private string _length;
		private int? _hit;
		private int? _monthhit;
        private int? _fileCount;
		private int _softtype;
		private string _details;
		private int? _area;
		private string _publisher;
		private DateTime _updatetime;
		/// <summary>
		/// 主键
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// hash码
		/// </summary>
		public string Hash
		{
			set{ _hash=value;}
			get{return _hash;}
		}
		/// <summary>
		/// 种子名称
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 种子大小 
		/// </summary>
        public string Length
		{
			set{ _length=value;}
			get{return _length;}
		}
		/// <summary>
		/// 总点击
		/// </summary>
		public int? Hit
		{
			set{ _hit=value;}
			get{return _hit;}
		}
		/// <summary>
		/// 月点击
		/// </summary>
		public int? MonthHit
		{
			set{ _monthhit=value;}
			get{return _monthhit;}
		}
		/// <summary>
		/// 文件数量
		/// </summary>
        public int? FileCount
		{
            set { _fileCount = value; }
            get { return _fileCount; }
		}
		/// <summary>
        /// 0未知类型，1，图片，2，电影
		/// </summary>
		public int SoftType
		{
			set{ _softtype=value;}
			get{return _softtype;}
		}
		/// <summary>
		/// 关键字
		/// </summary>
		public string Details
		{
			set{ _details=value;}
			get{return _details;}
		}
		/// <summary>
		/// 地区
		/// </summary>
		public int? Area
		{
			set{ _area=value;}
			get{return _area;}
		}
		/// <summary>
		/// 出版
		/// </summary>
		public string Publisher
		{
			set{ _publisher=value;}
			get{return _publisher;}
		}
		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime UpdateTime
		{
			set{ _updatetime=value;}
			get{return _updatetime;}
		}
		#endregion Model
        public string URL
        {
            get;
            set;
        }
        public int PageSize
        {
            get;
            set;
        }
        public int CurrentPage
        {
            get;
            set;
        }
        public int TotalCount
        {
            get;
            set;
        }
        public string KeyWord
        {
            get;
            set;
        }
	}
}

