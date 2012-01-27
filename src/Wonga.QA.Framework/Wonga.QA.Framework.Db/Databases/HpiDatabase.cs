#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17020
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wonga.QA.Framework.Db.Hpi
{
	using Wonga.QA.Framework.Db;
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Hpi")]
	public partial class HpiDatabase : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertHPIActiveRequestEntity(HPIActiveRequestEntity instance);
    partial void UpdateHPIActiveRequestEntity(HPIActiveRequestEntity instance);
    partial void DeleteHPIActiveRequestEntity(HPIActiveRequestEntity instance);
    partial void InsertMSSQLDeploy(MSSQLDeploy instance);
    partial void UpdateMSSQLDeploy(MSSQLDeploy instance);
    partial void DeleteMSSQLDeploy(MSSQLDeploy instance);
    #endregion
		
		public HpiDatabase(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public HpiDatabase(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public HpiDatabase(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public HpiDatabase(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<HPIActiveRequestEntity> HPIActiveRequests
		{
			get
			{
				return this.GetTable<HPIActiveRequestEntity>().SetTable<HPIActiveRequestEntity>();
			}
		}
		
		public System.Data.Linq.Table<MSSQLDeploy> MSSQLDeploys
		{
			get
			{
				return this.GetTable<MSSQLDeploy>().SetTable<MSSQLDeploy>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="cache.HPIActiveRequest")]
	public partial class HPIActiveRequestEntity : DbEntity<HPIActiveRequestEntity>, INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ActiveRequestId;
		
		private long _RequestHash;
		
		private string _RequestData;
		
		private string _ResponseData;
		
		private System.DateTime _RequestDate;
		
		private System.Nullable<int> _TotalMilliseconds;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnActiveRequestIdChanging(int value);
    partial void OnActiveRequestIdChanged();
    partial void OnRequestHashChanging(long value);
    partial void OnRequestHashChanged();
    partial void OnRequestDataChanging(string value);
    partial void OnRequestDataChanged();
    partial void OnResponseDataChanging(string value);
    partial void OnResponseDataChanged();
    partial void OnRequestDateChanging(System.DateTime value);
    partial void OnRequestDateChanged();
    partial void OnTotalMillisecondsChanging(System.Nullable<int> value);
    partial void OnTotalMillisecondsChanged();
    #endregion
		
		public HPIActiveRequestEntity()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ActiveRequestId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ActiveRequestId
		{
			get
			{
				return this._ActiveRequestId;
			}
			set
			{
				if ((this._ActiveRequestId != value))
				{
					this.OnActiveRequestIdChanging(value);
					this.SendPropertyChanging();
					this._ActiveRequestId = value;
					this.SendPropertyChanged("ActiveRequestId");
					this.OnActiveRequestIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RequestHash", DbType="BigInt NOT NULL")]
		public long RequestHash
		{
			get
			{
				return this._RequestHash;
			}
			set
			{
				if ((this._RequestHash != value))
				{
					this.OnRequestHashChanging(value);
					this.SendPropertyChanging();
					this._RequestHash = value;
					this.SendPropertyChanged("RequestHash");
					this.OnRequestHashChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RequestData", DbType="NVarChar(MAX) NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string RequestData
		{
			get
			{
				return this._RequestData;
			}
			set
			{
				if ((this._RequestData != value))
				{
					this.OnRequestDataChanging(value);
					this.SendPropertyChanging();
					this._RequestData = value;
					this.SendPropertyChanged("RequestData");
					this.OnRequestDataChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ResponseData", DbType="NVarChar(MAX) NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string ResponseData
		{
			get
			{
				return this._ResponseData;
			}
			set
			{
				if ((this._ResponseData != value))
				{
					this.OnResponseDataChanging(value);
					this.SendPropertyChanging();
					this._ResponseData = value;
					this.SendPropertyChanged("ResponseData");
					this.OnResponseDataChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RequestDate", DbType="DateTime NOT NULL")]
		public System.DateTime RequestDate
		{
			get
			{
				return this._RequestDate;
			}
			set
			{
				if ((this._RequestDate != value))
				{
					this.OnRequestDateChanging(value);
					this.SendPropertyChanging();
					this._RequestDate = value;
					this.SendPropertyChanged("RequestDate");
					this.OnRequestDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TotalMilliseconds", DbType="Int")]
		public System.Nullable<int> TotalMilliseconds
		{
			get
			{
				return this._TotalMilliseconds;
			}
			set
			{
				if ((this._TotalMilliseconds != value))
				{
					this.OnTotalMillisecondsChanging(value);
					this.SendPropertyChanging();
					this._TotalMilliseconds = value;
					this.SendPropertyChanged("TotalMilliseconds");
					this.OnTotalMillisecondsChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.MSSQLDeploy")]
	public partial class MSSQLDeploy : DbEntity<MSSQLDeploy>, INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private System.DateTime _Date;
		
		private string _Name;
		
		private string _MD5;
		
		private System.Nullable<int> _Revision;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnDateChanging(System.DateTime value);
    partial void OnDateChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnMD5Changing(string value);
    partial void OnMD5Changed();
    partial void OnRevisionChanging(System.Nullable<int> value);
    partial void OnRevisionChanged();
    #endregion
		
		public MSSQLDeploy()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Date", DbType="DateTime NOT NULL")]
		public System.DateTime Date
		{
			get
			{
				return this._Date;
			}
			set
			{
				if ((this._Date != value))
				{
					this.OnDateChanging(value);
					this.SendPropertyChanging();
					this._Date = value;
					this.SendPropertyChanged("Date");
					this.OnDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(255) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MD5", DbType="VarChar(32) NOT NULL", CanBeNull=false)]
		public string MD5
		{
			get
			{
				return this._MD5;
			}
			set
			{
				if ((this._MD5 != value))
				{
					this.OnMD5Changing(value);
					this.SendPropertyChanging();
					this._MD5 = value;
					this.SendPropertyChanged("MD5");
					this.OnMD5Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Revision", DbType="Int")]
		public System.Nullable<int> Revision
		{
			get
			{
				return this._Revision;
			}
			set
			{
				if ((this._Revision != value))
				{
					this.OnRevisionChanging(value);
					this.SendPropertyChanging();
					this._Revision = value;
					this.SendPropertyChanged("Revision");
					this.OnRevisionChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
