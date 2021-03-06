#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.261
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wonga.QA.Framework.Db.TimeZone
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="TimeZone")]
	public partial class TimeZoneDatabase : DbDatabase<TimeZoneDatabase>
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertMSSQLDeploy(MSSQLDeploy instance);
    partial void UpdateMSSQLDeploy(MSSQLDeploy instance);
    partial void DeleteMSSQLDeploy(MSSQLDeploy instance);
    partial void InsertTimezoneEntity(TimezoneEntity instance);
    partial void UpdateTimezoneEntity(TimezoneEntity instance);
    partial void DeleteTimezoneEntity(TimezoneEntity instance);
    #endregion
		
		public TimeZoneDatabase(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public TimeZoneDatabase(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public TimeZoneDatabase(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public TimeZoneDatabase(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<MSSQLDeploy> MSSQLDeploys
		{
			get
			{
				return this.GetTable<MSSQLDeploy>();
			}
		}
		
		public System.Data.Linq.Table<TimezoneEntity> Timezones
		{
			get
			{
				return this.GetTable<TimezoneEntity>();
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="timezone.Timezones")]
	public partial class TimezoneEntity : DbEntity<TimezoneEntity>, INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _AddressHash;
		
		private int _Timezone;
		
		private System.DateTime _CreatedOn;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnAddressHashChanging(string value);
    partial void OnAddressHashChanged();
    partial void OnTimezoneChanging(int value);
    partial void OnTimezoneChanged();
    partial void OnCreatedOnChanging(System.DateTime value);
    partial void OnCreatedOnChanged();
    #endregion
		
		public TimezoneEntity()
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AddressHash", DbType="NVarChar(12) NOT NULL", CanBeNull=false)]
		public string AddressHash
		{
			get
			{
				return this._AddressHash;
			}
			set
			{
				if ((this._AddressHash != value))
				{
					this.OnAddressHashChanging(value);
					this.SendPropertyChanging();
					this._AddressHash = value;
					this.SendPropertyChanged("AddressHash");
					this.OnAddressHashChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Timezone", DbType="Int NOT NULL")]
		public int Timezone
		{
			get
			{
				return this._Timezone;
			}
			set
			{
				if ((this._Timezone != value))
				{
					this.OnTimezoneChanging(value);
					this.SendPropertyChanging();
					this._Timezone = value;
					this.SendPropertyChanged("Timezone");
					this.OnTimezoneChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatedOn", DbType="DateTime NOT NULL")]
		public System.DateTime CreatedOn
		{
			get
			{
				return this._CreatedOn;
			}
			set
			{
				if ((this._CreatedOn != value))
				{
					this.OnCreatedOnChanging(value);
					this.SendPropertyChanging();
					this._CreatedOn = value;
					this.SendPropertyChanged("CreatedOn");
					this.OnCreatedOnChanged();
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
